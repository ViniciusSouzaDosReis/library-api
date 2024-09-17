using library.Domain.Entities;

namespace library.Domain.Repositories.Reservations;

public interface IReservationReadOnlyRepositories
{
    Task<List<Reservation>> GetAll();
    Task<List<Reservation>> GetAllByUserId(Guid userId);
}
