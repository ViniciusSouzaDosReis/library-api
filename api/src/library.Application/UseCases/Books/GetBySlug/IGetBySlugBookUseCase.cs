using library.Communication.Responses;
using library.Communication.Responses.Book;

namespace library.Application.UseCases.Books.GetBySlug;
public interface IGetBySlugBookUseCase
{
    Task<ApiResponse<ResponseBookGetBySlugJson>> Execute(string slug);
}
