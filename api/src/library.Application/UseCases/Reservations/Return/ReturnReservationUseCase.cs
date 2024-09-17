using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;
namespace library.Application.UseCases.Reservations.Return;

public class ReturnReservationUseCase : IReturnReservationUseCase
{
    private readonly IReservationsUtils _reservationsUtils;

    public ReturnReservationUseCase(IReservationsUtils reservationsUtils)
    {
        _reservationsUtils = reservationsUtils;
    }

    public async Task Execute(Guid id)
    {
        await _reservationsUtils.ChangStatusReservation(id, StatusType.Returned);
    }
}
