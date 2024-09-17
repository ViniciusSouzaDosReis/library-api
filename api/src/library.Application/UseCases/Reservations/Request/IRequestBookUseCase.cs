namespace library.Application.UseCases.Reservations.Request;
public interface IRequestBookUseCase
{
    Task Execute(Guid id);
}
