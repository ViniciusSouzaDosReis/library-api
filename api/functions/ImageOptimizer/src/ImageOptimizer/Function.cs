using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ImageOptimizer;
public class Function
{
    IAmazonS3 S3Client { get; set; } = new AmazonS3Client();
    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        context.Logger.LogInformation("FunctionHandler started.");
        var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();
        context.Logger.LogInformation($"Received {eventRecords.Count} event records.");
        foreach (var record in eventRecords)
        {
            var s3Event = record.S3;
            if (s3Event == null)
            {
                context.Logger.LogWarning("s3Event is null, skipping...");
                continue;
            }

            try
            {
                var response = await S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key);

                if (response.Metadata["x-amz-meta-resized"] == true.ToString())
                {
                    context.Logger.LogInformation($"Object {s3Event.Object.Key} already resized");
                    continue;
                }

                await using var imageStream = await S3Client.GetObjectStreamAsync(
                    s3Event.Bucket.Name,
                    s3Event.Object.Key,
                    new Dictionary<string, object>());

                using var outStream = new MemoryStream();
                using (var image = await Image.LoadAsync(imageStream))
                {
                    image.Mutate(x => x.Resize(400, 500, LanczosResampler.Lanczos3));
                    var originalName = response.Metadata["x-amz-meta-originalname"];
                    await image.SaveAsync(outStream, image.DetectEncoder(originalName));
                }

                await S3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = s3Event.Bucket.Name,
                    Key = s3Event.Object.Key,
                    Metadata = {
                        ["x-amz-meta-originalname"] = response.Metadata["x-amz-meta-originalname"],
                        ["x-amz-meta-extension"] = response.Metadata["x-amz-meta-extension"],
                        ["x-amz-meta-resized"] = true.ToString()
                    },
                    ContentType = response.Headers.ContentType,
                    InputStream = outStream,
                });
            }
            catch (Exception e)
            {
                context.Logger.LogError($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogError(e.Message);
                context.Logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}