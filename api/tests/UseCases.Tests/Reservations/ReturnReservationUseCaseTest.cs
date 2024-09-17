using library.Application.UseCases.Reservations.Return;
using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;
using Moq;

namespace UseCases.Test.Reservations;

public class ReturnReservationUseCaseTest
{
    private readonly IReturnReservationUseCase _useCase;
    private readonly Mock<IReservationsUtils> _reservationsUtils;

    public ReturnReservationUseCaseTest()
    {
        _reservationsUtils = new Mock<IReservationsUtils>();
        _useCase = new ReturnReservationUseCase(_reservationsUtils.Object);
    }

    [Fact]
    public async Task ShouldChangeStatusToReturned()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.Returned), Times.Once);
    }

    [Fact]
    public async Task ShouldChangeStatusToReturnedWhenReservationIsAlreadyReturned()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.Returned), Times.Once);
    }
}
