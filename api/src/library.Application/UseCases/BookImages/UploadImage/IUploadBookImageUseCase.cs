using library.Communication.Responses;
using Microsoft.AspNetCore.Http;

namespace library.Application.UseCases.BookImages.UploadImage;
public interface IUploadBookImageUseCase
{
    Task<ApiResponse> Execute(Guid id, IFormFile file);
}
