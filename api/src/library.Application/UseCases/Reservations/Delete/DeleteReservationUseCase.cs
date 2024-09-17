
using library.Domain.Repositories;
using library.Domain.Repositories.Reservations;
using library.Exception.Reservation;
using library.Exception.ExceptionBase;

namespace library.Application.UseCases.Reservations.Delete;

public class DeleteReservationUseCase : IDeleteReservationUseCase
{
    private readonly IReservationWriteOnlyRepositories _reservationRepositories;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReservationUseCase(IReservationWriteOnlyRepositories reservationRepositories, IUnitOfWork unitOfWork)
    {
        _reservationRepositories = reservationRepositories;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var isDeleted = await _reservationRepositories.Delete(id);

        if (!isDeleted)
        {
            throw new NotFoundException(ResourceErrorMessage.RESERVATION_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
