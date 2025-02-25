using System.Text;
using Azure.Messaging.ServiceBus.Administration;
using CraftersCloud.Core.Helpers;
using CraftersCloud.Core.IntegrationEvents;

namespace CraftersCloud.Core.EventBus.ServiceBus.Rules;

public class DefaultRulesProvider : IServiceBusRulesProvider
{
    private readonly string _subscriptionClientName;

    public DefaultRulesProvider(string subscriptionClientName) => _subscriptionClientName = subscriptionClientName;

    public IEnumerable<CreateRuleOptions> GetRulesForEvent<T>(string eventName) where T : IIntegrationEventHandler =>
        new List<CreateRuleOptions> { BuildRuleDescriptionFromEvent<T>(eventName) };

    private CreateRuleOptions BuildRuleDescriptionFromEvent<T>(string eventName) where T : IIntegrationEventHandler =>
        new() { Filter = CreateFilter<T>(eventName), Name = eventName };

    private RuleFilter CreateFilter<T>(string defaultEventName) where T : IIntegrationEventHandler
    {
        var clients = GetSubscriptionClients<T>();
        var eventName = GetEventName<T>(defaultEventName);

        var filter = new StringBuilder();
        if (!GetProcessMessagesFromSelfFlag<T>())
        {
            filter.Append(
                $"sys.Label in ('{eventName}') and user.{MessagePropertiesKeys.MessageSenderOriginator} != '{_subscriptionClientName}'");
        }
        else
        {
            filter.Append($"sys.Label in ('{eventName}')");
        }

        if (clients.Any())
        {
            var clientsNames = clients.Select(c => $"'{c}'").JoinStrings(", ");
            filter.Append($" and user.{MessagePropertiesKeys.MessageSenderOriginator} in ({clientsNames})");
        }

        return new SqlRuleFilter(filter.ToString());
    }

    private static string[] GetSubscriptionClients<T>()
    {
        var attribute = typeof(T).FindAttribute<ProcessServiceBusMessagesFromAttribute>();
        return attribute != null
            ? attribute.SubscriptionClients
            : Array.Empty<string>();
    }

    private static bool GetProcessMessagesFromSelfFlag<T>() =>
        typeof(T).FindAttribute<ProcessMessagesSentFromSelfAttribute>() != null;

    private static string GetEventName<T>(string defaultEventName) where T : IIntegrationEventHandler
    {
        var eventNameAttribute = typeof(T).FindAttribute<EventNameAttribute>();

        var eventName = eventNameAttribute != null
            ? eventNameAttribute.EventName
            : defaultEventName;
        return eventName;
    }
}