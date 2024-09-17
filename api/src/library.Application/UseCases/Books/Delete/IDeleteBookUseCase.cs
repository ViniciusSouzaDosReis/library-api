namespace library.Application.UseCases.Books.Delete;

public interface IDeleteBookUseCase
{
    Task Execute(Guid id);
}
