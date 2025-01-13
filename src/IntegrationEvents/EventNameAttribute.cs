namespace CraftersCloud.Core.IntegrationEvents;

[PublicAPI]
// EventName attribute allows us to subscribe to events coming from unknown sources (where we cannot hold reference to actual integration event type class)
public class EventNameAttribute(string eventName) : Attribute
{
    public string EventName { get; } = eventName;
}