using library.Application.UseCases.Reservations.Pick;
using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;
using Moq;

namespace UseCases.Test.Reservations;

public class PickReservationUseCaseTest
{
    private readonly IPickReservationUseCase _useCase;
    private readonly Mock<IReservationsUtils> _reservationsUtils;

    public PickReservationUseCaseTest()
    {
        _reservationsUtils = new Mock<IReservationsUtils>();
        _useCase = new PickReservationUseCase(_reservationsUtils.Object);
    }

    [Fact]
    public async Task ShouldChangeStatusToInUse()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.InUse), Times.Once);
    }

    [Fact]
    public async Task ShouldChangeStatusToInUseWhenReservationIsAlreadyInUse()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.InUse), Times.Once);
    }
}
