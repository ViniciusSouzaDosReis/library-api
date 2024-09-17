using library.Communication.Requests;
using library.Communication.Responses;

namespace library.Application.UseCases.Tokens.Logout;
public interface ILogoutTokenUseCase
{
    Task<ApiResponse> Execute(RequestLogoutJson requestToken);
}
