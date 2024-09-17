using CommonTestUtilities;
using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.Books.GetAll;
using library.Domain.Repositories.Books;
using Moq;

namespace UseCases.Test.Books;

public class GetAllBookUseCaseTest
{
    private readonly IGetAllBooksUseCase _useCase;
    private readonly Mock<IBookReadOnlyRepositories> _bookReadOnlyRepositories;

    public GetAllBookUseCaseTest()
    {
        _bookReadOnlyRepositories = new Mock<IBookReadOnlyRepositories>();

        var mapper = GenerateMapMock.GenerateMock();
        _useCase = new GetAllBooksUseCase(_bookReadOnlyRepositories.Object, mapper);
    }

    [Fact]
    public async Task ShouldReturnAllBooks()
    {
        // Arrange
        var books = BookBuilder.Builder(10);
        _bookReadOnlyRepositories.Setup(x => x.GetAll()).ReturnsAsync(books);

        // Act
        var response = await _useCase.Execute();

        // Assert
        response.Should().NotBeNull();
        response.Data.Count.Should().Be(10);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookRepositoryThrowException()
    {
        // Arrange
        _bookReadOnlyRepositories.Setup(x => x.GetAll()).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute());

        // Assert
        exception.Should().NotBeNull();
    }
}
