using CommonTestUtilities.Mocks.Entities;
using CommonTestUtilities;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using Moq;
using library.Application.UseCases.Books.GetBySlug;
using FluentAssertions;
using System.Net;

namespace UseCases.Test.Books;

public class GetBySlugBookUseCaseTest
{
    private readonly IGetBySlugBookUseCase _useCase;
    private readonly Mock<IBookReadOnlyRepositories> _bookRepository;

    public GetBySlugBookUseCaseTest()
    {
        _bookRepository = new Mock<IBookReadOnlyRepositories>();
        var mapper = GenerateMapMock.GenerateMock();

        _useCase = new GetBySlugBookUseCase(_bookRepository.Object, mapper);
    }

    [Fact]
    public async Task ShouldReturnBook()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        _bookRepository.Setup(x => x.GetBySlug(book.Slug)).ReturnsAsync(book);

        // Act
        var response = await _useCase.Execute(book.Slug);

        // Assert
        response.Should().NotBeNull();
        response.Data.Slug.Should().Be(book.Slug);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenBookNotFound()
    {
        // Arrange
        _bookRepository.Setup(x => x.GetBySlug(It.IsAny<string>())).ReturnsAsync(() => null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(It.IsAny<string>()));

        // Assert
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookRepositoryThrowException()
    {
        // Arrange
        _bookRepository.Setup(x => x.GetBySlug(It.IsAny<string>())).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(It.IsAny<string>()));

        // Assert
        exception.Should().NotBeNull();
    }
}