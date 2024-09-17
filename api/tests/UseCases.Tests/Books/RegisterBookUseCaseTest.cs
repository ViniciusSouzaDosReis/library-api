using AutoMapper;
using CommonTestUtilities;
using CommonTestUtilities.Mocks.Requests;
using FluentAssertions;
using library.Application.UseCases.Books.Register;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using Moq;
using System.Net;

namespace UseCases.Test.Books;

public class RegisterBookUseCaseTest
{
    private readonly IRegisterBookUseCase _useCase;
    private readonly Mock<IBookWriteOnlyRepositories> _bookRepository;
    private readonly IMapper _mapper;

    public RegisterBookUseCaseTest()
    {
        _bookRepository = new Mock<IBookWriteOnlyRepositories>();
        var mapper = GenerateMapMock.GenerateMock();
        var unitOfWork = new Mock<IUnitOfWork>();
        _mapper = mapper;

        _useCase = new RegisterBookUseCase(_bookRepository.Object, mapper, unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldRegisterBook()
    {
        // Arrange
        var request = RequestBookJsonBuilder.Builder();
        var book = _mapper.Map<Book>(request);
        _bookRepository.Setup(x => x.Add(book));

        // Act
        var response = await _useCase.Execute(request);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenBookRepositoryThrowException()
    {
        // Arrange
        var request = RequestBookJsonBuilder.Builder();
        _bookRepository.Setup(x => x.Add(It.IsAny<Book>())).ThrowsAsync(new Exception());

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(request));

        // Assert
        exception.Should().NotBeNull();
    }
}
