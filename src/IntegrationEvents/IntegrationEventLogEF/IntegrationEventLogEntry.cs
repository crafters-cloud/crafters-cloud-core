namespace CraftersCloud.Core.IntegrationEvents.IntegrationEventLogEF;

[PublicAPI]
public class IntegrationEventLogEntry
{
    private IntegrationEventLogEntry() { }

    public IntegrationEventLogEntry(IntegrationEvent @event, Func<IntegrationEvent, string> serializer)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName!;
        Content = serializer(@event);
        State = EventState.NotPublished;
        TimesSent = 0;
    }

    public Guid EventId { get; private set; }
    public string EventTypeName { get; private set; } = string.Empty;
    public EventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTimeOffset CreationTime { get; private set; }
    public string Content { get; private set; } = string.Empty;
}