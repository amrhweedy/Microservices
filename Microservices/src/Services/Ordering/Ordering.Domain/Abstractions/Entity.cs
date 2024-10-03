
namespace Ordering.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>   // this is the base class for all entities
{
    public T Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
