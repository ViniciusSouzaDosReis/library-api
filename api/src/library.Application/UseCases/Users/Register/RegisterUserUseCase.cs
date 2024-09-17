using AutoMapper;
using library.Application.Helpers.Cryptography;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Domain.Entities;
using library.Domain.Repositories;
using library.Domain.Repositories.Users;
using library.Exception.ExceptionBase;
using library.Exception.User;
using System.Net;

namespace library.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepositories _userWriteRepository;
    private readonly IUserReadOnlyRepositories _userReadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICryptography _cryptography;

    public RegisterUserUseCase(
        IUserWriteOnlyRepositories userWriteRepository,
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepositories userReadRepository,
        IMapper mapper,
        ICryptography cryptography)
    {
        _userWriteRepository = userWriteRepository;
        _unitOfWork = unitOfWork;
        _userReadRepository = userReadRepository;
        _mapper = mapper;
        _cryptography = cryptography;
    }

    public async Task<ApiResponse> Execute(RequestUserJson requestUser)
    {
        Validate(requestUser);

        var findUser = await _userReadRepository.GetByEmail(requestUser.Email);

        if(findUser != null)
        {
            throw new ErrorOnValidationException([ResourceErrorMessage.USER_ALREADY_EXISTS]);
        }

        var user = _mapper.Map<User>(requestUser);
        user.Password = _cryptography.Hash(requestUser.Password);

        await _userWriteRepository.AddUser(user);
        await _unitOfWork.Commit();

        return ApiResponse.CreateSuccesResponse((int)HttpStatusCode.Created);
    }

    private void Validate(RequestUserJson request)
    {
        var validate = new UserValidate();

        var result = validate.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
