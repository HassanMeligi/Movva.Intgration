using Moova.Integration.Application.DTOs;
using Moova.Integration.Application.Interfaces;
using Moova.Integration.Application.Mapping;
using Moova.Integration.Domain.Repositories;

namespace Moova.Integration.Application.Services;

public class ConfigurationService(IConfigurationRepository repository) : IConfigurationService
{
    public async Task<IEnumerable<ConfigurationDto>> GetConfigurationsAsync(List<string> keys)
    {
        var configurations = await repository.GetAllConfigurationsAsync();

        return configurations
            .AsQueryable()
            .Where(c => keys.Contains(c.ConfigurationKey))
            .Select(ConfigurationMapping.ToConfigurationDto)
            .ToList();
    }
}