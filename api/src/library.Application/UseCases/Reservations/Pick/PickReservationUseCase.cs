
using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;

namespace library.Application.UseCases.Reservations.Pick;

public class PickReservationUseCase : IPickReservationUseCase
{
    private readonly IReservationsUtils _reservationsUtils;

    public PickReservationUseCase(IReservationsUtils reservationsUtils)
    {
        _reservationsUtils = reservationsUtils;
    }

    public async Task Execute(Guid id)
    {
        await _reservationsUtils.ChangStatusReservation(id, StatusType.InUse);
    }
}
