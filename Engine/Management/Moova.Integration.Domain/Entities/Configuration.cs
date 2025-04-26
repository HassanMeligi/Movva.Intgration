using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moova.Integration.Domain.Entities
{
    public class Configuration : BaseAuditableEntity, IAuditable
    {
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
    }
}
