using library.Communication.Requests;
using library.Communication.Responses;
using library.Communication.Responses.User;

namespace library.Application.UseCases.Tokens.Generate;
public interface IGenerateTokenUseCase
{
    Task<ApiResponse<ResponseLoginJson>> Execute(RequestLoginJson userRequest);
}
