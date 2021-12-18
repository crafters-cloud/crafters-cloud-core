using Azure.Messaging.ServiceBus;
using System.Text;

namespace CraftersCloud.Core.EventBus.ServiceBus;

public static class EventBusMessageExtensions
{
    public static string GetEventName(this ServiceBusMessage message) =>
        $"{message.Subject}{EventBusServiceBus.IntegrationEventSuffix}";

    public static string GetMessageData(this ServiceBusMessage message) => Encoding.UTF8.GetString(message.Body);
}