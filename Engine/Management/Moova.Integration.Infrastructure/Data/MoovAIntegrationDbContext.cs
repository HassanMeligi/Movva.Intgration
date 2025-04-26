using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;
using Movva.Data;
using Movva.Data.Interfaces;

namespace Moova.Integration.Infrastructure.Data;

public class MoovAIntegrationDbContext(DbContextOptions<MoovAIntegrationDbContext> options) : DbContext(options)
{
    //public DbSet<Counties> Counties { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        foreach (var entry in ChangeTracker.Entries())
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreatedPropertiesForAuditableEntities(entry);
                    if (entry.Entity is not IHardDeletable) entry.CurrentValues["IsDeleted"] = false;
                    break;
                case EntityState.Modified:
                    SetUpdatedPropertiesForAuditableEntities(entry);
                    break;
                case EntityState.Deleted:
                    SetUpdatedPropertiesForAuditableEntities(entry);
                    if (entry.Entity is not IHardDeletable)
                    {
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                    }

                    break;
            }
    }

    private static void SetUpdatedPropertiesForAuditableEntities(EntityEntry entry)
    {
        if (entry.Entity is IAuditable)
        {
            ((IAuditable)entry.Entity).UpdatedAtUtc = DateTime.UtcNow;
            ((IAuditable)entry.Entity).UpdatedBy = ((IAuditable)entry.Entity).UpdatedBy ?? null;
        }
    }

    private static void SetCreatedPropertiesForAuditableEntities(EntityEntry entry)
    {
        if (entry.Entity is IAuditable)
        {
            ((IAuditable)entry.Entity).CreatedAtUtc = DateTime.UtcNow;
            ((IAuditable)entry.Entity).CreatedBy = ((IAuditable)entry.Entity).CreatedBy == null
                ? Guid.Empty
                : ((IAuditable)entry.Entity).CreatedBy;
        }
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        //add isDeleted query filter to all entities
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
        {
            var parameter = Expression.Parameter(entityType.ClrType);
            var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
            var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
            var compareExpression =
                Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
            var lambda = Expression.Lambda(compareExpression, parameter);
            builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}

 