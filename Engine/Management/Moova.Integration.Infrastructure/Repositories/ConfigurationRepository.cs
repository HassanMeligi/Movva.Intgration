using Moova.Integration.Domain.Entities;
using Moova.Integration.Domain.Repositories;
using Moova.Integration.Infrastructure.Data;

namespace Moova.Integration.Infrastructure.Repositories;

public class ConfigurationRepository(MoovAIntegrationDbContext context) : IConfigurationRepository
{
    public async Task<List<Configuration>> GetAllConfigurationsAsync()
    {
        return await context.Configuration
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }
}

}