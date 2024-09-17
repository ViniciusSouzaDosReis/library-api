using library.Domain.Entities;

namespace library.Domain.Repositories.Books;
public interface IBookUpdateOnlyRepositores
{
    void Update(Book book);
    Task<Book?> GetById(Guid id);
}
