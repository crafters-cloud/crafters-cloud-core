using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace CraftersCloud.Core.EventBus;

public interface IServiceBusPersisterConnection : IDisposable
{
    ServiceBusClient TopicClient { get; }
    ServiceBusAdministrationClient AdministrationClient { get; }
    string? TopicName { get; }
}