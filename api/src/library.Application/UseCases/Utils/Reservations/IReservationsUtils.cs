using library.Domain.Enums;

namespace library.Application.UseCases.Utils.Reservations;
public interface IReservationsUtils
{
    Task ChangStatusReservation(Guid id, StatusType statusType);
}
