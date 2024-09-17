using library.Domain.Entities;
using library.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.DataAccess.Repositories;

public class UsersRepository : IUserReadOnlyRepositories , IUserWriteOnlyRepositories
{
    private readonly LibraryDbContext _context;

    public UsersRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> Delete(Guid id)
    {
       var response = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (response is null) return false;

        _context.Users.Remove(response);

        return true;
    }
}
