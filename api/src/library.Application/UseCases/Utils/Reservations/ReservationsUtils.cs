using library.Domain.Enums;
using library.Domain.Repositories;
using library.Domain.Repositories.Reservations;
using library.Exception.ExceptionBase;
using library.Exception.Reservation;

namespace library.Application.UseCases.Utils.Reservations;

public class ReservationsUtils : IReservationsUtils
{
    private readonly IReservationUpdateOnlyRepositores _reservationRepositories;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationsUtils(IReservationUpdateOnlyRepositores reservationRepositories, IUnitOfWork unitOfWork)
    {
        _reservationRepositories = reservationRepositories;
        _unitOfWork = unitOfWork;
    }

    public async Task ChangStatusReservation(Guid id, StatusType statusType)
    {
        var reservation = await _reservationRepositories.GetById(id) ?? throw new NotFoundException(ResourceErrorMessage.RESERVATION_NOT_FOUND);

        if (reservation.Status == StatusType.Returned && statusType == StatusType.Returned)
        {
            throw new ErrorOnValidationException([ResourceErrorMessage.RESERVATION_ALREADY_RETURNED]);
        }

        if (reservation.Status == StatusType.InUse && statusType == StatusType.InUse)
        {
            throw new ErrorOnValidationException([ResourceErrorMessage.RESERVATION_ALREADY_PICKED]);
        }

        if (reservation.Status == StatusType.Requested && statusType == StatusType.Requested)
        {
            throw new ErrorOnValidationException([ResourceErrorMessage.RESERVATION_ALREADY_REQUESTED]);
        }

        reservation.Status = statusType;

        _reservationRepositories.Update(reservation);

        await _unitOfWork.Commit();
    }
}
