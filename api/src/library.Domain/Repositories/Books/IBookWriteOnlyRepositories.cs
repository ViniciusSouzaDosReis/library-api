using library.Domain.Entities;

namespace library.Domain.Repositories.Books;
public interface IBookWriteOnlyRepositories
{
    Task Add(Book book);
    Task<bool> Delete(Guid id);
}
