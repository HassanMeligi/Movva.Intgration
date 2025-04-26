using Moova.Integration.Application.DTOs;

namespace Moova.Integration.Application.Interfaces;

public interface IConfigurationService
{
    Task<IEnumerable<ConfigurationDto>> GetConfigurationsAsync(List<string> keys);
}