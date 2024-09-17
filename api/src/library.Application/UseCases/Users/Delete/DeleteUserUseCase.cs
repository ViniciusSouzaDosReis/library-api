
using library.Domain.Repositories;
using library.Domain.Repositories.Users;
using library.Exception.ExceptionBase;
using library.Exception.User;

namespace library.Application.UseCases.Users.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepositories _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(IUserWriteOnlyRepositories userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var response = await _userRepository.Delete(id);

        if(response == false)
        {
            throw new NotFoundException(ResourceErrorMessage.USER_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
