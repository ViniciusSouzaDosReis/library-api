namespace library.Application.UseCases.Reservations.Return;

public interface IReturnReservationUseCase
{
    Task Execute(Guid id);
}
