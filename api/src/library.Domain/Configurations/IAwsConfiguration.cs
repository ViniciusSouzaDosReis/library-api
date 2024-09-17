namespace library.Domain.Configurations;
public interface IAwsConfiguration
{
    string S3BucketName { get; set; }
    string S3BucketKey { get; set; }
    string AccessKey { get; set; }
    string Secret { get; set; }
    string Profile { get; set; }
    string Region { get; set; }
}
