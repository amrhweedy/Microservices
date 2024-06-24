using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Interceptors;
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    // before applying the changes in the database this code will be executed


    // This method is executed before the changes are saved to the database.
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "amr";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "amr";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>    //The purpose of this method is to check if any owned entity (a related entity that is owned by another entity in the context of EF Core) associated with a given EntityEntry has been added or modified. It's useful for scenarios where you want to track changes to related entities that are owned by another entity.
       entry.References.Any(r =>
           r.TargetEntry != null &&
           r.TargetEntry.Metadata.IsOwned() &&
           (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
