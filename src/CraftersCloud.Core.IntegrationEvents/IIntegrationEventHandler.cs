using JetBrains.Annotations;

namespace CraftersCloud.Core.IntegrationEvents;

[PublicAPI]
public interface IIntegrationEventHandler
{
}

[PublicAPI]
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}