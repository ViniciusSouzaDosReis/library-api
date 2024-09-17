using AutoMapper;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using library.Communication.Responses.User;
using library.Domain.Repositories.Users;
using System.Net;

namespace library.Application.UseCases.Users.GetAll;

public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IUserReadOnlyRepositories _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersUseCase(IUserReadOnlyRepositories userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ICollection<ResponseUserJson>>> Execute()
    {
        var users = await _userRepository.GetAll();

        var response = ApiResponse.CreateSuccesResponseWithData(
            _mapper.Map<ICollection<ResponseUserJson>>(users),
            (int)HttpStatusCode.OK);
        return response;

    }
}
