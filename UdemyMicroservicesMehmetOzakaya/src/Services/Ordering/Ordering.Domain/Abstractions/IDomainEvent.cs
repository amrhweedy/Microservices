
namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification  // we can use the mediator handlers to handle these domain events using the mediator
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;

}

// the domain model like the order publish the event 
// any event must implement the IDomainEvent