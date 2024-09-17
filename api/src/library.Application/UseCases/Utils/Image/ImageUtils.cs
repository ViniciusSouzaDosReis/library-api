using Amazon.S3;
using Amazon.S3.Model;
using library.Domain.Configurations;
using Microsoft.AspNetCore.Http;

namespace library.Application.UseCases.Utils.Image;

public class ImageUtils : IImageUtils
{
    private readonly IAwsConfiguration _awsConfiguration;

    public ImageUtils(IAwsConfiguration awsConfiguration)
    {
        _awsConfiguration = awsConfiguration;
    }

    public PutObjectRequest UploadImage(Guid id, IFormFile file)
    {
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _awsConfiguration.S3BucketName,
            Key = $"{_awsConfiguration.S3BucketKey}/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream(),
            Metadata =
            {
                ["x-amz-meta-originalname"] = file.FileName,
                ["x-amz-meta-extension"] = Path.GetExtension(file.FileName),
                ["x-amz-meta-resized"] = false.ToString()
            }
        };

        return putObjectRequest;
    }

    public DeleteObjectRequest DeleteImage(Guid id)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _awsConfiguration.S3BucketName,
            Key = $"{_awsConfiguration.S3BucketKey}/{id}",
        };

        return deleteObjectRequest;
    }
}
