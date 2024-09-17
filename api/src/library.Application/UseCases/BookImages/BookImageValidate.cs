using FluentValidation;
using library.Exception;
using library.Exception.BookImage;
using Microsoft.AspNetCore.Http;
namespace library.Application.UseCases.BookImages;

public class BookImageValidate : AbstractValidator<IFormFile>
{
    public BookImageValidate()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(ResourceErrorMessage.IMAGE_IS_REQUIRED);
        RuleFor(x => x)
            .Must(x => x.Length <= 5 * 1024 * 1024) // 5MB
            .WithMessage(ResourceErrorMessage.IMAGE_EXCEEDED_SIZE_LIMIT);
        RuleFor(x => x)
            .Must(x => x.FileName.EndsWith(".jpg") || x.FileName.EndsWith(".jpeg") || x.FileName.EndsWith(".png"))
            .WithMessage(ResourceErrorMessage.IMAGE_MUST_BE_JPG_JPEG_OR_PNG);
    }
}
