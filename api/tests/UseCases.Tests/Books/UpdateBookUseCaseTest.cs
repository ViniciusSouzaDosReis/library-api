using AutoMapper;
using CommonTestUtilities;
using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.UseCases.Books.Update;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Books;

public class UpdateBookUseCaseTest
{
    private readonly Mock<IBookUpdateOnlyRepositores> _bookRepository;
    private readonly IUpdateBookUseCase _useCase;
    private readonly IMapper _mapper;

    public UpdateBookUseCaseTest()
    {
        _bookRepository = new Mock<IBookUpdateOnlyRepositores>();
        var mapper = GenerateMapMock.GenerateMock();
        var unitOfWork = new Mock<IUnitOfWork>();
        _mapper = mapper;

        _useCase = new UpdateBookUseCase(_bookRepository.Object, mapper, unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldUpdateBook()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = RequestBookJsonBuilder.Builder();
        var book = _mapper.Map<Book>(request);
        _bookRepository.Setup(x => x.GetById(id)).ReturnsAsync(book);

        // Act
        await _useCase.Execute(id, request);

        // Assert
        _bookRepository.Verify(x => x.Update(book), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookRepositoryThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = RequestBookJsonBuilder.Builder();
        _bookRepository.Setup(x => x.GetById(id)).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(id, request));

        // Assert
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = RequestBookJsonBuilder.Builder();
        _bookRepository.Setup(x => x.GetById(id)).ReturnsAsync(() => null);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(id, request));

        // Assert
        exception.Should().NotBeNull();
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
