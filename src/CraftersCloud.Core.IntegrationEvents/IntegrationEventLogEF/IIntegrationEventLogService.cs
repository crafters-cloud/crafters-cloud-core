using System.Data.Common;

namespace CraftersCloud.Core.IntegrationEvents.IntegrationEventLogEF;

public interface IIntegrationEventLogService
{
    Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
    Task MarkEventAsPublishedAsync(IntegrationEvent @event);
}