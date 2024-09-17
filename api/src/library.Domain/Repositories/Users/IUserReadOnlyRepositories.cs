using library.Domain.Entities;

namespace library.Domain.Repositories.Users;
public interface IUserReadOnlyRepositories
{
    public Task<List<User>> GetAll();
    public Task<User?> GetByEmail(string email);
}
