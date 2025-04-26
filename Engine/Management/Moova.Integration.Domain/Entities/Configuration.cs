using Moova.Integration.Domain.Common;

namespace Moova.Integration.Domain.Entities;

public class Configuration : BaseAuditableEntity, IAuditable
{
    public string ConfigurationKey { get; set; }
    public string ConfigurationValue { get; set; }
}