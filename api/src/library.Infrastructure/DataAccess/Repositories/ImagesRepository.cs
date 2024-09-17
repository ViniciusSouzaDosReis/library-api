using library.Domain.Entities;
using library.Domain.Repositories.Images;

namespace library.Infrastructure.DataAccess.Repositories;

public class ImagesRepository : IImageWriteOnlyRepositories
{
    private readonly LibraryDbContext _context;
    public ImagesRepository(LibraryDbContext context)
    {
        _context = context;
    }
    public async Task Add(BookImage bookImage)
    {
        await _context.BookImages.AddAsync(bookImage);
    }
}
