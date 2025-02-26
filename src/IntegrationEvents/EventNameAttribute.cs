namespace CraftersCloud.Core.IntegrationEvents;

/// <summary>
/// Attribute to specify a custom event name for integration events.
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public class EventNameAttribute : Attribute
{
    /// <summary>
    /// The custom event name.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventNameAttribute"/> class with the specified event name.
    /// </summary>
    /// <param name="eventName">The custom event name.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="eventName"/> is null or empty.</exception>
    public EventNameAttribute(string eventName)
    {
        ArgumentException.ThrowIfNullOrEmpty(eventName);
        EventName = eventName;
    }
}