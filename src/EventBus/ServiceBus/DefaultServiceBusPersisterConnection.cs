using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace CraftersCloud.Core.EventBus.ServiceBus;

[UsedImplicitly]
internal class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
{
    private readonly string _serviceBusConnectionString;
    private ServiceBusClient _topicClient;

    private bool _disposed;

    public string? TopicName { get; }

    public DefaultServiceBusPersisterConnection(string serviceBusConnectionString)
    {
        _serviceBusConnectionString = serviceBusConnectionString;
        AdministrationClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);
        _topicClient = new ServiceBusClient(_serviceBusConnectionString);
        TopicName = ExtractTopicName(serviceBusConnectionString);
    }

    internal static string? ExtractTopicName(string serviceBusConnectionString)
    {
        var connectionStringProperties = ServiceBusConnectionStringProperties.Parse(serviceBusConnectionString);
        return connectionStringProperties.EntityPath;
    }

    public ServiceBusClient TopicClient
    {
        get
        {
            if (_topicClient.IsClosed)
            {
                _topicClient = new ServiceBusClient(_serviceBusConnectionString);
            }

            return _topicClient;
        }
    }

    public ServiceBusAdministrationClient AdministrationClient { get; }

    public ServiceBusClient CreateModel()
    {
        if (_topicClient.IsClosed)
        {
            _topicClient = new ServiceBusClient(_serviceBusConnectionString);
        }

        return _topicClient;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _topicClient.DisposeAsync().GetAwaiter().GetResult();
    }
}