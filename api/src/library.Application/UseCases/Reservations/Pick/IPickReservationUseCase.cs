namespace library.Application.UseCases.Reservations.Pick;
public interface IPickReservationUseCase
{
    Task Execute(Guid id);
}
