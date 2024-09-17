using library.Application.UseCases.Utils.Image;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using Microsoft.AspNetCore.Http;
using library.Domain.Entities;
using library.Domain.Repositories.Images;
using library.Domain.Repositories;
using library.Exception.Book;
using Amazon.S3;
using library.Communication.Responses;
using System.Net;

namespace library.Application.UseCases.BookImages.UploadImage;

public class UploadBookImageUseCase : IUploadBookImageUseCase
{
    private readonly IImageUtils _imageUtils;
    private readonly IBookUpdateOnlyRepositores _bookRepository;
    private readonly IImageWriteOnlyRepositories _imageRepositories;
    private readonly IUnitOfWork _uniOfWork;
    private readonly IAmazonS3 _s3Client;

    public UploadBookImageUseCase(
        IImageUtils imageUtils,
        IBookUpdateOnlyRepositores bookRepository,
        IImageWriteOnlyRepositories imageWriteOnlyRepositories,
        IUnitOfWork uniOfWork,
        IAmazonS3 s3Client)
    {
        _imageUtils = imageUtils;
        _bookRepository = bookRepository;
        _imageRepositories = imageWriteOnlyRepositories;
        _uniOfWork = uniOfWork;
        _s3Client = s3Client;
    }

    public async Task<ApiResponse> Execute(Guid id, IFormFile file)
    {
        Validate(file);

        var book = await _bookRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessage.BOOK_NOT_FOUND);
        var putRequest = _imageUtils.UploadImage(book.Id, file);
        try
        {
            await _s3Client.PutObjectAsync(putRequest);
        }
        catch (System.Exception e)
        {
            throw new ErrorImageException(e.Message);
        }

        var bookImage = new BookImage
        {
            BookId = book.Id,
            Id = Guid.NewGuid(),
            ContentType = putRequest.ContentType,
            Url = $"https://{putRequest.BucketName}.s3.amazonaws.com/{putRequest.Key}",
            S3Key = putRequest.Key,
            Book = book
        };

        await _imageRepositories.Add(bookImage);
        await _uniOfWork.Commit();

        return ApiResponse.CreateSuccesResponse((int)HttpStatusCode.Created);
    }

    private static void Validate(IFormFile file)
    {
        var validate = new BookImageValidate();

        var result = validate.Validate(file);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
