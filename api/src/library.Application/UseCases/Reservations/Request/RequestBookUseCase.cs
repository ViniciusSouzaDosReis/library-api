
using library.Application.UseCases.Utils.Reservations;
using library.Domain.Enums;

namespace library.Application.UseCases.Reservations.Request;

public class RequestBookUseCase : IRequestBookUseCase
{
    private readonly IReservationsUtils _reservationsUtils;

    public RequestBookUseCase(IReservationsUtils reservationsUtils)
    {
        _reservationsUtils = reservationsUtils;
    }

    public async Task Execute(Guid id)
    {
        await _reservationsUtils.ChangStatusReservation(id, StatusType.Requested);
    }
}
