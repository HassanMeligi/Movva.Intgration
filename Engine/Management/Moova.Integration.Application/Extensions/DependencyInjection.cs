using Microsoft.Extensions.DependencyInjection;
using Moova.Integration.Application.Interfaces;
using Moova.Integration.Application.Services;

namespace Moova.Integration.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IConfigurationService, ConfigurationService>();
            return services;
        }
    }
}
