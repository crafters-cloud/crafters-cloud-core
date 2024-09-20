using Azure.Messaging.ServiceBus.Administration;
using CraftersCloud.Core.IntegrationEvents;

namespace CraftersCloud.Core.EventBus;

public interface IServiceBusRulesProvider
{
    IEnumerable<CreateRuleOptions> GetRulesForEvent<T>(string eventName) where T : IIntegrationEventHandler;
}