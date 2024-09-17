using library.Domain.Entities;

namespace library.Domain.Repositories.Reservations;
public interface IReservationUpdateOnlyRepositores
{
    void Update(Reservation reservation);
    Task<Reservation?> GetById(Guid id);
}
