using FluentAssertions;
using library.Application.UseCases.Reservations.Delete;
using library.Domain.Repositories;
using library.Domain.Repositories.Reservations;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Reservations;

public class DeleteReservationUseCaseTest
{
    private readonly IDeleteReservationUseCase _useCase;
    private readonly Mock<IReservationWriteOnlyRepositories> _bookWriteOnlyRepositories;

    public DeleteReservationUseCaseTest()
    {
        _bookWriteOnlyRepositories = new Mock<IReservationWriteOnlyRepositories>();
        var unitOfWork = new Mock<IUnitOfWork>();

        _useCase = new DeleteReservationUseCase(_bookWriteOnlyRepositories.Object, unitOfWork.Object);
    }

    [Fact]
    public async Task ShouldDeleteReservation()
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
    public async Task ShouldThrowNotFoundExceptionWhenReservationNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _bookWriteOnlyRepositories.Setup(x => x.Delete(id)).ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(id));

        // Assert
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
