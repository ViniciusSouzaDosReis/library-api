using library.Application.UseCases.Reservations.Request;
using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;
using Moq;

namespace UseCases.Test.Reservations;

public class RequestReservationUseCaseTest
{
    private readonly IRequestBookUseCase _useCase;
    private readonly Mock<IReservationsUtils> _reservationsUtils;

    public RequestReservationUseCaseTest()
    {
        _reservationsUtils = new Mock<IReservationsUtils>();
        _useCase = new RequestBookUseCase(_reservationsUtils.Object);
    }

    [Fact]
    public async Task ShouldChangeStatusToRequested()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.Requested), Times.Once);
    }

    [Fact]
    public async Task ShouldChangeStatusToRequestedWhenReservationIsAlreadyRequested()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _useCase.Execute(id);

        // Assert
        _reservationsUtils.Verify(x => x.ChangStatusReservation(id, StatusType.Requested), Times.Once);
    }
}
