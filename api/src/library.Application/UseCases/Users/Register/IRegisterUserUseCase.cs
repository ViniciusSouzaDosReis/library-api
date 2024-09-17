using library.Communication.Requests;
using library.Communication.Responses;

namespace library.Application.UseCases.Users.Register;
public interface IRegisterUserUseCase
{
    Task<ApiResponse> Execute(RequestUserJson requestUser);
}
