namespace Moova.Integration.Application.DTOs;

public class ConfigurationDto
{
    public int Id { get; set; }
    public required string ConfigurationKey { get; set; }
    public required string ConfigurationValue { get; set; }
}