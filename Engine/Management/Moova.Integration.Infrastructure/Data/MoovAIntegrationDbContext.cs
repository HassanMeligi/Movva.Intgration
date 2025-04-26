using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Moova.Integration.Domain.Entities;
using Moova.Integration.Infrastructure.Data.Extensions;

namespace Moova.Integration.Infrastructure.Data;

public class MoovAIntegrationDbContext(DbContextOptions<MoovAIntegrationDbContext> options) : DbContext(options)
{
    public DbSet<Configuration> Configuration { get; set; } 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyGlobalQueryFilters();
        base.OnModelCreating(builder);
    }
}