namespace Ordering.Domain.Abstractions;

public interface IAggregate<T> : IEntity<T>, IAggregate
{

}


public interface IAggregate : IEntity     // aggregate is a special kind of entity that can handle a domain events, the aggregate should have domain event list that you can clear and add domain events into the aggregates
{
    IReadOnlyList<IDomainEvent> DomainEvents { get;  }

    IDomainEvent[] ClearDomainEvents();
}
