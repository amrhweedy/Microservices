
namespace Ordering.Domain.Events;
public record OrderCreatedEvent(Order order) : IDomainEvent;   // we dont implement the properties which in the interface because they have default return 

