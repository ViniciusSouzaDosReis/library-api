using library.Domain.Entities;

namespace library.Domain.Repositories.Reservations;

public interface IReservationWriteOnlyRepositories
{
    Task Add(Reservation reservation);
    Task<bool> Delete(Guid id);
}
