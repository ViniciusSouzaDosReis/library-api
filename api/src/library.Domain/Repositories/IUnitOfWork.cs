namespace library.Domain.Repositories;
public interface IUnitOfWork
{
    Task Commit();
}
