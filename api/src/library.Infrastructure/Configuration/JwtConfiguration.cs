using library.Domain.Configurations;

namespace library.Infrastructure.Configuration;

public class JwtConfiguration : IJwtConfiguration
{
    public string Secret { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; }
}
