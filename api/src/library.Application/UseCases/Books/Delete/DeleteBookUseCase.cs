using Amazon.S3;
using library.Application.UseCases.Utils.Image;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Exception.Book;
using library.Exception.ExceptionBase;

namespace library.Application.UseCases.Books.Delete;

public class DeleteBookUseCase : IDeleteBookUseCase
{
    private readonly IBookWriteOnlyRepositories _bookWriteOnlyRepositories;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUtils _imageUtils;
    private readonly IAmazonS3 _s3Client;

    public DeleteBookUseCase(
        IBookWriteOnlyRepositories bookWriteOnlyRepositories,
        IUnitOfWork unitOfWork,
        IImageUtils imageUtils,
        IAmazonS3 s3Client)
    {
        _bookWriteOnlyRepositories = bookWriteOnlyRepositories;
        _unitOfWork = unitOfWork;
        _imageUtils = imageUtils;
        _s3Client = s3Client;
    }

    public async Task Execute(Guid id)
    {
        var isDeleted = await _bookWriteOnlyRepositories.Delete(id);

        if (!isDeleted)
        {
            throw new NotFoundException(ResourceErrorMessage.BOOK_NOT_FOUND);
        }

        var deleteObjectRequest = _imageUtils.DeleteImage(id);

        try
        {
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
        catch (System.Exception e)
        {
            throw new ErrorImageException(e.Message);
        }


        await _unitOfWork.Commit();
    }
}
