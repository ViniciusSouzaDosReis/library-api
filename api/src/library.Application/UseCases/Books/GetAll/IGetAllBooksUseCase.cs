using library.Communication.Responses;
using library.Communication.Responses.Book;

namespace library.Application.UseCases.Books.GetAll;
public interface IGetAllBooksUseCase
{
    Task<ApiResponse<ICollection<ResponseBookJson>>> Execute();
}
