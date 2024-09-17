using library.Communication.Requests;

namespace library.Application.UseCases.Books.Update;
public interface IUpdateBookUseCase
{
    Task Execute(Guid id, RequestBookJson book);
}
