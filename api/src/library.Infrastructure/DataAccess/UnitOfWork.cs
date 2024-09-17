using library.Domain.Repositories;

namespace library.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _dbContext;
    public UnitOfWork(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}