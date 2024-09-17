using CommonTestUtilities.Mocks.Entities;
using FluentAssertions;
using library.Application.UseCases.Reservations.Register;
using library.Domain.Authentication;
using library.Domain.Enums;
using library.Domain.Repositories;
using library.Domain.Repositories.Books;
using library.Domain.Repositories.Reservations;
using library.Exception.ExceptionBase;
using Moq;
using System.Net;

namespace UseCases.Test.Reservations;

public class RegisterReservationUseCaseTest
{
    private readonly IRegisterReservationUseCase _useCase;
    private readonly Mock<IReservationWriteOnlyRepositories> _reservationWriteOnlyRepositories;
    private readonly Mock<IBookUpdateOnlyRepositores> _bookUpdateOnlyRepositores;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ICurrentUser> _currentUser;

    public RegisterReservationUseCaseTest()
    {
        _reservationWriteOnlyRepositories = new Mock<IReservationWriteOnlyRepositories>();
        _bookUpdateOnlyRepositores = new Mock<IBookUpdateOnlyRepositores>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _currentUser = new Mock<ICurrentUser>();

        _useCase = new RegisterReservationUseCase(
                       _reservationWriteOnlyRepositories.Object,
                       _unitOfWork.Object,
                       _bookUpdateOnlyRepositores.Object,
                       _currentUser.Object);
    }

    [Fact]
    public async Task ShouldRegisterReservation()
    {
        // Arrange
        var book = BookBuilder.Builder(1)[0];
        _bookUpdateOnlyRepositores.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);
        _currentUser.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var response = await _useCase.Execute(book.Id);

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task ShouldNotRegisterReservationWhenBookIsReserved()
    {
        // Arrange
        var reservation = ReservationBuilder.Builder(1);
        reservation[0].Status = StatusType.Requested;
        var book = BookBuilder.Builder(1)[0];
        book.Reservations = reservation;
        _bookUpdateOnlyRepositores.Setup(x => x.GetById(book.Id)).ReturnsAsync(book);
        _currentUser.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var response = await Assert.ThrowsAsync<ErrorOnValidationException>(() => _useCase.Execute(book.Id));

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldBookNotFound()
    {
        // Arrange
        _bookUpdateOnlyRepositores.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);
        _currentUser.Setup(x => x.GetId()).Returns(Guid.NewGuid());

        // Act
        var response = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Execute(Guid.NewGuid()));

        // Assert
        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
