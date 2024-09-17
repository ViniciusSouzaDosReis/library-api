using library.Domain.Entities;

namespace library.Domain.Repositories.Users;
public interface IUserWriteOnlyRepositories
{
    Task AddUser(User user);
    Task<bool> Delete(Guid id);
}
