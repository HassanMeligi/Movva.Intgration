using Moova.Integration.Domain.Entities;

namespace Moova.Integration.Domain.Repositories;

public interface IConfigurationRepository
{
    Task<List<Configuration>> GetAllConfigurationsAsync();
}