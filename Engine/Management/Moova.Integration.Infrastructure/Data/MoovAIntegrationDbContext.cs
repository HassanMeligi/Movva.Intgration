using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Movva.Data.Extensions;

namespace Moova.Integration.Infrastructure.Data;

public class MoovAIntegrationDbContext(DbContextOptions<MoovAIntegrationDbContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyGlobalQueryFilters(); 
        base.OnModelCreating(builder);
    }
}
