namespace CraftersCloud.Core.IntegrationEvents;

public interface IIntegrationEventService
{
    Task PublishThroughEventBusAsync<T>(T evt) where T : IntegrationEvent;
}