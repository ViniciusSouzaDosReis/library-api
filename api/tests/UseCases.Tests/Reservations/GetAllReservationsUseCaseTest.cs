using CommonTestUtilities;
using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.Reservations.GetAll;
using library.Communication.Enums;
using library.Domain.Authentication;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Reservations;
using Moq;

namespace UseCases.Test.Reservations;

public class GetAllReservationsUseCaseTest
{
    private readonly IGetAllReservationsUseCase _useCase;
    private readonly Mock<IReservationReadOnlyRepositories> _reservationReadOnlyRepositories;
    private readonly Mock<ICurrentUser> _currentUser;

    public GetAllReservationsUseCaseTest()
    {
        _reservationReadOnlyRepositories = new Mock<IReservationReadOnlyRepositories>();
        _currentUser = new Mock<ICurrentUser>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var mapper = GenerateMapMock.GenerateMock();

        _useCase = new GetAllReservationsUseCase(
            _reservationReadOnlyRepositories.Object,
            mapper, _currentUser.Object);
    }

    [Fact]
    public async Task ShouldReturnAllReservations()
    {
        // Arrange
        var reservations = ReservationBuilder.Builder(10);
        _reservationReadOnlyRepositories.Setup(x => x.GetAll()).ReturnsAsync(reservations);
        _currentUser.Setup(x => x.GetId()).Returns(Guid.NewGuid());
        _currentUser.Setup(x => x.GetRole()).Returns(RoleType.Admin.ToString());

        // Act
        var response = await _useCase.Execute();

        // Assert
        response.Data.Count.Should().Be(10);
    }

    [Fact]
    public async Task ShouldReturnReservationsForSpecificUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var reservations = ReservationBuilder.Builder(5);
        _reservationReadOnlyRepositories.Setup(x => x.GetAllByUserId(userId)).ReturnsAsync(reservations);
        _currentUser.Setup(x => x.GetId()).Returns(userId);
        _currentUser.Setup(x => x.GetRole()).Returns(RoleType.User.ToString());

        // Act
        var response = await _useCase.Execute();

        // Assert
        response.Data.Count.Should().Be(5);
    }

    [Fact]
    public async Task ShouldReturnEmptyWhenNoReservationsForAdmin()
    {
        // Arrange
        var reservations = new List<Reservation>();
        _reservationReadOnlyRepositories.Setup(x => x.GetAll()).ReturnsAsync(reservations);
        _currentUser.Setup(x => x.GetRole()).Returns(RoleType.Admin.ToString());

        // Act
        var response = await _useCase.Execute();

        // Assert
        response.Data.Should().BeEmpty(); 
    }

    [Fact]
    public async Task ShouldReturnEmptyWhenNoReservationsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var reservations = new List<Reservation>(); 
        _reservationReadOnlyRepositories.Setup(x => x.GetAllByUserId(userId)).ReturnsAsync(reservations);
        _currentUser.Setup(x => x.GetId()).Returns(userId);
        _currentUser.Setup(x => x.GetRole()).Returns(RoleType.User.ToString());

        // Act
        var response = await _useCase.Execute();

        // Assert
        response.Data.Should().BeEmpty();
    }

}
