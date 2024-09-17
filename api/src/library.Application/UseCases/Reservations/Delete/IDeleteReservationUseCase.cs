namespace library.Application.UseCases.Reservations.Delete;
public interface IDeleteReservationUseCase
{
    Task Execute(Guid id);
}
