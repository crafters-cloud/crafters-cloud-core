using System.Text;
using System.Text.Json;
using Autofac;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using CraftersCloud.Core.EventBus.ServiceBus.Rules;
using CraftersCloud.Core.Helpers;
using CraftersCloud.Core.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EventBus.ServiceBus;

internal class EventBusServiceBus : IEventBus, IDisposable
{
    private const string AutofacScopeName = "event_bus_service_bus";
    internal const string IntegrationEventSuffix = "IntegrationEvent";
    private readonly ILifetimeScope _autofac;
    private readonly ILogger<EventBusServiceBus> _logger;
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusSessionProcessor _processor;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly string _subscriptionClientName;
    private readonly IServiceBusRulesProvider _rulesProvider;
    private readonly string _topicName;

    // ReSharper disable once ConvertToConstant.Local
    private readonly ServiceBusReceiveMode ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete;
    private bool _processingStarted;
    private readonly ServiceBusAdministrationClient _administrationClient;

    public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
        ILogger<EventBusServiceBus> logger, IEventBusSubscriptionsManager subsManager,
        string subscriptionClientName, ILifetimeScope autofac, IServiceBusRulesProvider rulesProvider)
    {
        _logger = logger;
        _subsManager = subsManager;
        _subscriptionClientName = subscriptionClientName;
        _autofac = autofac;
        _rulesProvider = rulesProvider;

        if (string.IsNullOrEmpty(serviceBusPersisterConnection.TopicName))
        {
            throw new InvalidOperationException("TopicName is missing.");
        }

        _topicName = serviceBusPersisterConnection.TopicName;
        _sender = serviceBusPersisterConnection.TopicClient.CreateSender(_topicName);
        var options = new ServiceBusSessionProcessorOptions { ReceiveMode = ReceiveMode };
        _processor =
            serviceBusPersisterConnection.TopicClient.CreateSessionProcessor(_topicName, _subscriptionClientName,
                options);

        _administrationClient = serviceBusPersisterConnection.AdministrationClient;
        RemoveRule(RuleProperties.DefaultRuleName);
    }

    public void Publish(IntegrationEvent @event)
    {
        var eventName = GetEventName(@event);
        var jsonMessage = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var message = new ServiceBusMessage
        {
            MessageId = Guid.NewGuid().ToString(),
            Body = new BinaryData(body),
            Subject = eventName,
            SessionId = _subscriptionClientName
        };

        //add subscription client name as the sender 
        message.ApplicationProperties.Add(MessagePropertiesKeys.MessageSenderOriginator, _subscriptionClientName);

        _sender.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
    }

    public void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName,
            typeof(TH).Name);

        _subsManager.AddDynamicSubscription<TH>(eventName);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventName<T>();

        var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
        if (!containsKey)
        {
            var rules = _rulesProvider.GetRulesForEvent<TH>(eventName);
            AddOrUpdateRules(rules);
        }

        _subsManager.AddSubscription<T, TH>();
    }

    private void AddOrUpdateRules(IEnumerable<CreateRuleOptions> ruleOptions)
    {
        foreach (var ruleDescription in ruleOptions)
        {
            AddOrUpdateRule(ruleDescription);
        }
    }

    private void AddOrUpdateRule(CreateRuleOptions ruleOptions)
    {
        try
        {
            _logger.LogInformation("Adding or updating rule: {RuleName}.", ruleOptions.Name);
            var ruleExists =
                _administrationClient.RuleExistsAsync(_topicName,
                    _subscriptionClientName, ruleOptions.Name).GetAwaiter().GetResult();

            if (ruleExists)
            {
                _logger.LogDebug("Rule exists.");
                var ruleProperties =
                    _administrationClient.GetRuleAsync(_topicName,
                        _subscriptionClientName, ruleOptions.Name).GetAwaiter().GetResult().Value;
                ruleProperties.Filter = ruleOptions.Filter;
                _administrationClient
                    .UpdateRuleAsync(_topicName, _subscriptionClientName, ruleProperties).GetAwaiter().GetResult();
            }
            else
            {
                _logger.LogDebug("Rule does not exist.");
                _administrationClient
                    .CreateRuleAsync(_topicName, _subscriptionClientName, ruleOptions).GetAwaiter().GetResult();
            }
        }
        catch (ServiceBusException ex)
        {
            _logger.LogError(ex, "Failed on adding the rule: {RuleName}", ruleOptions.Name);
        }
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventName<T>();

        RemoveRule(eventName);

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler =>
        _subsManager.RemoveDynamicSubscription<TH>(eventName);

    public void Dispose()
    {
        _subsManager.Clear();
        _processor.CloseAsync().GetAwaiter().GetResult();
    }

    public void StartReceivingMessages()
    {
        if (_processingStarted)
        {
            throw new InvalidOperationException("Processing messages already started");
        }

        _processingStarted = true;
        RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
    }

    private async Task RegisterSubscriptionClientMessageHandlerAsync()
    {
        _processor.ProcessMessageAsync +=
            async args =>
            {
                var eventName = $"{args.Message.Subject}{IntegrationEventSuffix}";
                var messageData = args.Message.Body.ToString();

                // Complete the message so that it is not received again.
                if (await ProcessEvent(eventName, messageData))
                {
                    if (ReceiveMode == ServiceBusReceiveMode.PeekLock)
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                }
            };

        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        var ex = args.Exception;
        var context = args.ErrorSource;

        _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}",
            ex.Message, context);

        return Task.CompletedTask;
    }

    private async Task<bool> ProcessEvent(string eventName, string message)
    {
        var processed = false;
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            await using (var scope = _autofac.BeginLifetimeScope(AutofacScopeName))
            {
                var subscriptions =
                    _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        var handler =
                            scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                        if (handler == null)
                        {
                            continue;
                        }

                        dynamic eventData = JsonDocument.Parse(message).RootElement;
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null)
                        {
                            _logger.LogDebug("Can not resolve handler, Handler type: {HandlerType}",
                                subscription.HandlerType);
                            continue;
                        }

                        var eventType = _subsManager.GetEventTypeByName(eventName)!;
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType)!;
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        var handleMethod = concreteType.GetMethod("Handle")!;
                        await ((Task?) handleMethod.Invoke(handler, [integrationEvent]))!;
                    }
                }
            }

            processed = true;
        }

        return processed;
    }

    private void RemoveRule(string ruleName)
    {
        try
        {
            // if we try to remove rule without checking for existence we will run into exception
            // Azure.RequestFailedException: Resource Conflict Occurred. Another conflicting operation may be in progress.
            var ruleExists = _administrationClient.RuleExistsAsync(_topicName, _subscriptionClientName, ruleName)
                .GetAwaiter()
                .GetResult();
            if (ruleExists)
            {
                _administrationClient
                    .DeleteRuleAsync(_topicName, _subscriptionClientName, ruleName)
                    .GetAwaiter()
                    .GetResult();
            }
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogWarning("The messaging entity {RuleName} Could not be found.",
                ruleName);
        }
    }

    private static string GetEventName(IntegrationEvent @event) => GetEventName(@event.GetType());

    private static string GetEventName<T>() where T : IntegrationEvent => GetEventName(typeof(T));

    private static string GetEventName(Type eventType)
    {
        var eventNameAttribute = eventType.FindAttribute<EventNameAttribute>();
        var eventName = eventNameAttribute != null
            ? eventNameAttribute.EventName
            : eventType.Name;

        return eventName.Replace(IntegrationEventSuffix, "");
    }
}