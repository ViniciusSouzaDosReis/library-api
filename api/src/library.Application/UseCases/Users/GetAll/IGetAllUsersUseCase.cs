using library.Communication.Responses;
using library.Communication.Responses.User;

namespace library.Application.UseCases.Users.GetAll;

public interface IGetAllUsersUseCase
{
    public Task<ApiResponse<ICollection<ResponseUserJson>>> Execute();
}
