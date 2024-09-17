using library.Communication.Requests;
using library.Communication.Responses;

namespace library.Application.UseCases.Books.Register;

public interface IRegisterBookUseCase
{
    Task<ApiResponse> Execute(RequestBookJson request);
}
