namespace CraftersCloud.Core.IntegrationEvents;

[UsedImplicitly]
public class ProcessServiceBusMessagesFromAttribute(params string[] subscriptionClients) : Attribute
{
    public string[] SubscriptionClients { get; } = subscriptionClients;
}