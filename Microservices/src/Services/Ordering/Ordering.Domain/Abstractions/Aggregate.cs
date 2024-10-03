
namespace Ordering.Domain.Abstractions;

// the aggregate is entity so it inherits from the entity
// and the aggregate should handle the domain events so it implements the IAggregate
public abstract class Aggregate<T> :  Entity<T> ,IAggregate<T>
{

    private readonly List<IDomainEvent> _domainEvents = new();  // private field to store the events in it
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();  // readonly property to get the _domainEvents
 
    public void AddDomainEvent (IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = _domainEvents.ToArray(); 

        _domainEvents.Clear();

        return dequeuedEvents;  // return the deleted events
    }
}
