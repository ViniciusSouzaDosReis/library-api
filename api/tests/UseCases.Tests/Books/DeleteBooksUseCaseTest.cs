using Amazon.S3;
using library.Application.UseCases.Books.Delete;
using library.Application.UseCases.Utils.Image;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using Moq;
using FluentAssertions;
using Amazon.S3.Model;
using System.Net;

namespace UseCases.Tests.Books;

public class DeleteBooksUseCaseTest
{
    private readonly IDeleteBookUseCase _useCase;
    private readonly Mock<IBookWriteOnlyRepositories> _bookWriteOnlyRepositories;
    private Mock<IImageUtils> _imageUtils;
    private Mock<IAmazonS3> _s3Client;

    public DeleteBooksUseCaseTest()
    {
        _bookWriteOnlyRepositories = new Mock<IBookWriteOnlyRepositories>();
        var unitOfWork = new Mock<IUnitOfWork>();
        _imageUtils = new Mock<IImageUtils>();
        _s3Client = new Mock<IAmazonS3>();

        _useCase = new DeleteBookUseCase(_bookWriteOnlyRepositories.Object, unitOfWork.Object, _imageUtils.Object, _s3Client.Object);
    }

    [Fact]
    public async Task ShouldDeleteBook()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(true);

        // Act
        await _useCase.Execute(id);

        // Assert
        _bookWriteOnlyRepositories.Verify(x => x.Delete(id), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenBookNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(id));

        // Assert
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldThrowErrorImageExceptionWhenErrorOnDeleteImage()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(true);
        var deleteObjectRequest = new DeleteObjectRequest();
        var exceptionMessage = "Error on delete image";
        var exception = new Exception(exceptionMessage);

        _imageUtils.Setup(x => x.DeleteImage(id)).Returns(deleteObjectRequest);
        _s3Client.Setup(x => x.DeleteObjectAsync(deleteObjectRequest, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var error = await Assert.ThrowsAsync<ErrorImageException>(() => _useCase.Execute(id));

        // Assert
        error.Message.Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task ShouldCommitUnitOfWork()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(true);

        // Act
        await _useCase.Execute(id);

        // Assert
        _bookWriteOnlyRepositories.Verify(x => x.Delete(id), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteImage()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(true);
        var deleteObjectRequest = new DeleteObjectRequest();

        _imageUtils.Setup(x => x.DeleteImage(id)).Returns(deleteObjectRequest);

        // Act
        await _useCase.Execute(id);

        // Assert
        _s3Client.Verify(x => x.DeleteObjectAsync(deleteObjectRequest, It.IsAny<CancellationToken>()), Times.Once);
    }
}
