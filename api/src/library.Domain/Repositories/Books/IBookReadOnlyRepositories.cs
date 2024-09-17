using library.Domain.Entities;

namespace library.Domain.Repositories.Books;
public interface IBookReadOnlyRepositories
{
    Task<List<Book>> GetAll();
    Task<Book?> GetById(Guid id);
    Task<Book?> GetBySlug(string slug);
}
