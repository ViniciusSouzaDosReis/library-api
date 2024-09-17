using library.Communication.Requests;
using library.Communication.Responses;
using library.Domain.Repositories;
using library.Domain.Repositories.Tokens;
using library.Exception.ExceptionBase;
using library.Exception.Token;
using System.Net;

namespace library.Application.UseCases.Tokens.Logout;

public class LogoutTokenUseCase : ILogoutTokenUseCase
{
    private readonly ITokenReadOnlyRepositories _tokenReadRepository;
    private readonly ITokenWriteOnlyRepositories _tokenWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutTokenUseCase(ITokenReadOnlyRepositories tokenReadRepository, 
        ITokenWriteOnlyRepositories tokenWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenReadRepository = tokenReadRepository;
        _tokenWriteRepository = tokenWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Execute(RequestLogoutJson request)
    {
        string requestToken = request.Token ?? throw new ErrorOnValidationException([ResourceErrorMessage.TOKEN_REQUIRED]);

        var response = await _tokenReadRepository.GetByToken(requestToken) ?? throw new NotFoundException(ResourceErrorMessage.TOKEN_NOT_FOUND);

        response.IsActived = false;
        _tokenWriteRepository.Update(response);
        await _unitOfWork.Commit();

        return ApiResponse.CreateSuccesResponse((int)HttpStatusCode.OK);
    }
}
