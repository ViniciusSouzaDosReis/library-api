using CommonTestUtilities;
using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.Books.GetById;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Books;

public class GetByIdBookUseCaseTest
{
    private readonly IGetByIdBookUseCase _useCase;
    private readonly Mock<IBookReadOnlyRepositories> _bookRepository;

    public GetByIdBookUseCaseTest()
    {
        _bookRepository = new Mock<IBookReadOnlyRepositories>();
        var mapper = GenerateMapMock.GenerateMock();

        _useCase = new GetByIdBookUseCase(_bookRepository.Object, mapper);
    }

    [Fact]
    public async Task ShouldReturnBook()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        _bookRepository.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);

        // Act
        var response = await _useCase.Execute(book.Id);

        // Assert
        response.Should().NotBeNull();
        response.Data.Id.Should().Be(book.Id);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenBookNotFound()
    {
        // Arrange
        _bookRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(It.IsAny<Guid>()));

        // Assert
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookRepositoryThrowException()
    {
        // Arrange
        _bookRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(It.IsAny<Guid>()));

        // Assert
        exception.Should().NotBeNull();
    }
}
