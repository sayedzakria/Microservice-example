

namespace Ordering.Infratstructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            UpdateEntities(eventData.Context);
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "System";
                }
                else if (entry.State == EntityState.Added||entry.State == EntityState.Modified||entry.HasChangeOwnedEntities())
                {
                    entry.Entity.LastModified = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = "System";
                }
            }


        }
    }
}

public static class Extensions
{
    public static bool HasChangeOwnedEntities(this EntityEntry entry)=>
        entry.References.Any(r => 
        r.TargetEntry != null && 
        r.TargetEntry.Metadata.IsOwned() && 
        (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

}