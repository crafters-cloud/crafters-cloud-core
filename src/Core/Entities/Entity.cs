namespace CraftersCloud.Core.Entities;

[PublicAPI]
public abstract class Entity
{
    // needs to be private so that EF does not map the field
    private readonly Dictionary<DomainEvent, DomainEvent> _domainEvents = [];

    public IEnumerable<DomainEvent> GetDomainEvents() => _domainEvents.Values;

    protected void AddDomainEvent(DomainEvent eventItem)
    {
        ArgumentNullException.ThrowIfNull(eventItem);

        // prevents multiple events with the same data to be added
        // last event wins
        _domainEvents[eventItem] = eventItem;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}