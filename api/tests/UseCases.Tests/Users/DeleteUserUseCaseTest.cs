using FluentAssertions;
using library.Application.UseCases.Users.Delete;
using library.Domain.Repositories;
using library.Domain.Repositories.Users;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Users;

public class DeleteUserUseCaseTest
{
    private readonly IDeleteUserUseCase _useCase;
    private readonly Mock<IUserWriteOnlyRepositories> _userRepository;

    public DeleteUserUseCaseTest()
    {
        _userRepository = new Mock<IUserWriteOnlyRepositories>();
        var unitOfWork = new Mock<IUnitOfWork>();

        _useCase = new DeleteUserUseCase(_userRepository.Object, unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.Delete(id)).ReturnsAsync(true);

        // Act
        await _useCase.Execute(id);

        // Assert
        _userRepository.Verify(x => x.Delete(id), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenUserNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(x => x.Delete(id)).ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(id));

        // Assert
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
