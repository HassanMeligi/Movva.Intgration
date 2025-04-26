using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Movva.Data.Extensions;
using Movva.Data.Interceptors;

namespace Moova.Integration.Infrastructure.Data;

public class MoovAIntegrationDbContext(DbContextOptions<MoovAIntegrationDbContext> options) : DbContext(options)
{
    private readonly AuditInterceptor _auditInterceptor = new();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _auditInterceptor.OnBeforeSaving(ChangeTracker);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _auditInterceptor.OnBeforeSaving(ChangeTracker);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyGlobalQueryFilters();
        base.OnModelCreating(builder);
    }
}
