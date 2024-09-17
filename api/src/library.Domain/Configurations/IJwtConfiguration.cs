namespace library.Domain.Configurations;
public interface IJwtConfiguration
{
    string Secret { get; set; }
    int ExpirationInMinutes { get; set; }
}
