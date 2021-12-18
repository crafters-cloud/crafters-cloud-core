using JetBrains.Annotations;

namespace CraftersCloud.Core.IntegrationEvents;

[PublicAPI]
// EventName attribute allows us to subscribe to events coming from unknown sources (where we cannot hold reference to actual integration event type class)
public class EventNameAttribute : Attribute
{
    public EventNameAttribute(string eventName) => EventName = eventName;

    public string EventName { get; }
}