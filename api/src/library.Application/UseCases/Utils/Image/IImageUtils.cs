using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace library.Application.UseCases.Utils.Image;

public interface IImageUtils
{
    PutObjectRequest UploadImage(Guid id, IFormFile file);
    DeleteObjectRequest DeleteImage(Guid id);
}
