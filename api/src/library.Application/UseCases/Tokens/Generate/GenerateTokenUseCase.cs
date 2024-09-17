using library.Communication.Requests;
using library.Domain.Authentication;
using library.Domain.Repositories;
using library.Domain.Repositories.Tokens;
using library.Domain.Repositories.Users;
using library.Exception.Authentication;
using library.Domain.Entities;
using library.Communication.Responses;
using library.Communication.Responses.User;
using library.Exception.ExceptionBase;
using System.Net;
using library.Application.Helpers.Cryptography;

namespace library.Application.UseCases.Tokens.Generate;

public class GenerateTokenUseCase : IGenerateTokenUseCase
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserReadOnlyRepositories _userRepository;
    private readonly ITokenWriteOnlyRepositories _tokenWriteRepository;
    private readonly ITokenReadOnlyRepositories _tokenReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptography _cryptography;

    public GenerateTokenUseCase(
        ITokenGenerator tokenGenerator,
        IUserReadOnlyRepositories userRepository,
        ITokenWriteOnlyRepositories tokenWriteRepository,
        IUnitOfWork unitOfWork,
        ITokenReadOnlyRepositories tokenReadRepository,
        ICryptography cryptography)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _tokenWriteRepository = tokenWriteRepository;
        _unitOfWork = unitOfWork;
        _tokenReadRepository = tokenReadRepository;
        _cryptography = cryptography;
    }

    public async Task<ApiResponse<ResponseLoginJson>> Execute(RequestLoginJson userRequest)
    {
        Validate(userRequest);

        var user = await _userRepository.GetByEmail(userRequest.Email);

        if (user is null || !_cryptography.VerifyHash(userRequest.Password, user.Password))
        {
            throw new ErrorOnValidationException([ResourceErrorMessage.INVALID_CREDENTIAL]);
        }

        string tokenCode = _tokenGenerator.Generator(user);
        var token = new Token()
        {
            CreatedDate = DateTime.Now,
            IsActived = true,
            TokenCode = tokenCode,
            UserId = user.Id
        };

        var lastToken =  _tokenReadRepository.GetLastByUserId(user.Id);

        if(lastToken != null && lastToken.IsActived)
        {
            lastToken.IsActived = false;
            _tokenWriteRepository.Update(lastToken);
        }

        await _tokenWriteRepository.Add(token);
        await _unitOfWork.Commit();

        var response = ApiResponse.CreateSuccesResponseWithData(new ResponseLoginJson
        {
            Token = tokenCode
        },
        (int)HttpStatusCode.OK);

        return response;
    }

    private void Validate(RequestLoginJson request)
    {
        var validate = new LoginValidate();

        var result = validate.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
