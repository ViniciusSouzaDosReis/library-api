using library.Domain.Entities;

namespace library.Domain.Repositories.Images;
public interface IImageWriteOnlyRepositories
{
    Task Add(BookImage bookImage);
}
