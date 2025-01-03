namespace CraftersCloud.Core.IntegrationEvents;

[UsedImplicitly]
public class ProcessServiceBusMessagesFromAttribute : Attribute
{
    public ProcessServiceBusMessagesFromAttribute(params string[] subscriptionClients) =>
        SubscriptionClients = subscriptionClients;

    public string[] SubscriptionClients { get; }
}