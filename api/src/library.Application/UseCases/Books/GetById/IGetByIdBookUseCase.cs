using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Domain.Entities;

namespace library.Application.UseCases.Books.GetById;
public interface IGetByIdBookUseCase
{
    Task<ApiResponse<ResponseBookJson>> Execute(Guid id);
}
