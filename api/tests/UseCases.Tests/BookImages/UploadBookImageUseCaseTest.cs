using Amazon.S3;
using Amazon.S3.Model;
using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.BookImages.UploadImage;
using library.Application.UseCases.Utils.Image;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Domain.Repositories.Images;
using library.Exception.ExceptionBase;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;

namespace UseCases.Test.BookImages;

public class UploadBookImageUseCaseTests
{
    private readonly Mock<IImageUtils> _imageUtils;
    private readonly Mock<IBookUpdateOnlyRepositores> _bookRepository;
    private readonly Mock<IImageWriteOnlyRepositories> _imageRepositories;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IAmazonS3> _s3Client;
    private readonly UploadBookImageUseCase _useCase;

    public UploadBookImageUseCaseTests()
    {
        _imageUtils = new Mock<IImageUtils>();
        _bookRepository = new Mock<IBookUpdateOnlyRepositores>();
        _imageRepositories = new Mock<IImageWriteOnlyRepositories>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _s3Client = new Mock<IAmazonS3>();

        _useCase = new UploadBookImageUseCase(_imageUtils.Object, _bookRepository.Object, _imageRepositories.Object, _unitOfWork.Object, _s3Client.Object);
    }

    [Fact]
    public async Task ShouldUploadImageSuccessfully()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        var file = new Mock<IFormFile>();
        file.Setup(x => x.FileName).Returns("image.png");
        var putRequest = new PutObjectRequest { BucketName = "bucket", Key = "image_key", ContentType = "image/png" };

        _bookRepository.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);
        _imageUtils.Setup(x => x.UploadImage(book.Id, file.Object)).Returns(putRequest);
        var putObjectResponse = new PutObjectResponse();

        _s3Client
            .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(putObjectResponse);

        // Act
        var response = await _useCase.Execute(book.Id, file.Object);

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        _imageRepositories.Verify(x => x.Add(It.IsAny<BookImage>()), Times.Once);
        _unitOfWork.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenBookNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var file = new Mock<IFormFile>();
        file.Setup(x => x.FileName).Returns("image.png");

        _bookRepository.Setup(x => x.GetById(id)).ReturnsAsync(() => null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(id, file.Object));
    }

    [Fact]
    public async Task ShouldThrowErrorImageExceptionWhenUploadFails()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        var file = new Mock<IFormFile>();
        file.Setup(x => x.FileName).Returns("image.png");
        var putRequest = new PutObjectRequest { BucketName = "bucket", Key = "image_key", ContentType = "image/png" };

        _bookRepository.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);
        _imageUtils.Setup(x => x.UploadImage(book.Id, file.Object)).Returns(putRequest);
        _s3Client.Setup(x => x.PutObjectAsync(putRequest, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("S3 error"));

        // Act & Assert
        await Assert.ThrowsAsync<ErrorImageException>(() => _useCase.Execute(book.Id, file.Object));
    }

    [Fact]
    public async Task ShouldCommitAfterImageUpload()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        var file = new Mock<IFormFile>();
        file.Setup(x => x.FileName).Returns("image.png");
        var putRequest = new PutObjectRequest { BucketName = "bucket", Key = "image_key", ContentType = "image/png" };

        _bookRepository.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);
        _imageUtils.Setup(x => x.UploadImage(book.Id, file.Object)).Returns(putRequest);

        var putObjectResponse = new PutObjectResponse();
        _s3Client
            .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(putObjectResponse);


        // Act
        await _useCase.Execute(book.Id, file.Object);

        // Assert
        _unitOfWork.Verify(x => x.Commit(), Times.Once);
    }
}
