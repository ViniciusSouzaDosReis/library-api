using library.Domain.Configurations;

namespace library.Infrastructure.Configuration;

public class AwsConfiguration : IAwsConfiguration
{
    public string S3BucketName { get; set; } = string.Empty;
    public string S3BucketKey { get; set; } = string.Empty;
    public string AccessKey { get ; set; } = string.Empty;
    public string Secret { get ; set ; } = string.Empty;
    public string Profile { get; set ; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}
