namespace CraftersCloud.Core.EventBus;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData);
}