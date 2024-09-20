namespace CraftersCloud.Core.Entities;

public abstract class Entity
{
    // needs to be private so that EF does not map the field
    private readonly Dictionary<DomainEvent, List<DomainEvent>> _domainEvents = [];

    public IEnumerable<DomainEvent> GetDomainEvents() => _domainEvents.Values.SelectMany(v => v);

    protected void AddDomainEvent(DomainEvent eventItem) =>
        // prevents multiple events with the same data to be added
        // last event wins
        _domainEvents[eventItem] = [eventItem];

    protected void AddDomainEvents(IEnumerable<DomainEvent> eventItems)
    {
        var items = eventItems.ToList();
        if (items.Count == 0)
        {
            return;
        }

        var key = items.First();
        _domainEvents[key] = [.. items];
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}