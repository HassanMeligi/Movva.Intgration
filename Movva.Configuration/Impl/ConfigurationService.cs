using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movva.Configuration.DTO;
using Movva.Configuration.Interfaces;

namespace Movva.Configuration.Impl
{
    public class ConfigurationService : IConfigurationService
    {
        private static readonly object ConfigLockObject = new();
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
            LoadAllConfigurationAsync();
        }

        public IEnumerable<ConfigurationdDto> GetConfigurations(List<string> keys)
        {
            var confList = new List<ConfigurationdDto>();
            keys.ForEach(c =>
            {
                var propInfo = GetType().GetProperty(c);
                if (propInfo == null)
                    return;

                ConfigurationdDto conf = new()
                {
                    Id = 1,
                    ConfigurationKey = propInfo.Name,
                    ConfigurationValue = GetType().GetProperty(propInfo.Name).GetValue(this, null).ToString()
                };
                confList.Add(conf);
            });
            return confList;
        }

    

        private void LoadAllConfigurationAsync()
        {
            List<Configuration> conf;
            var options = new DbContextOptionsBuilder<MoovAIntegrationDbContext>();
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            using MoovAIntegrationDbContext context = new(options.Options);

            lock (ConfigLockObject)
            {
                conf = context.Configuration.Where(x => !x.IsDeleted).ToList();
            }

            conf.ForEach(c =>
            {
                var propInfo = GetType().GetProperty(c.ConfigurationKey);
                if (propInfo == null)
                    return;

                var propertyType = propInfo.PropertyType;
                var propertyVal = Convert.ChangeType(c.ConfigurationValue, propertyType);
                propInfo.SetValue(this, propertyVal, null);
            });
        }

        public string CustomerQueueName { get; private set; }
        public int CustomerFetchCount { get; private set; }
        public decimal ServiceTax { get; private set; }

    }
}