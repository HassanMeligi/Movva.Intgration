using Moova.Integration.Application.DTOs;
using Moova.Integration.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Moova.Integration.Application.Mapping
{
    public static class ConfigurationMapping
    {
        public static readonly Expression<Func<Configuration, ConfigurationDto>> ToConfigurationDto =
            x => new ConfigurationDto
            {
                Id = x.Id,
                ConfigurationKey = x.ConfigurationKey,
                ConfigurationValue = x.ConfigurationValue
            };
    }
}
