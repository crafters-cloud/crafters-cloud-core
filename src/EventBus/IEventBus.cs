using CraftersCloud.Core.IntegrationEvents;

namespace CraftersCloud.Core.EventBus;

[PublicAPI]
public interface IEventBus
{
    void Publish(IntegrationEvent @event);

    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    void Unsubscribe<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationEvent;

    /// <summary>
    ///     After all subscriptions have been registered, call this method to start receiving messages
    /// </summary>
    void StartReceivingMessages();
}