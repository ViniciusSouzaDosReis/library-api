using library.Domain.Entities;
using library.Domain.Repositories.Books;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.DataAccess.Repositories;

internal class BooksRepository : IBookReadOnlyRepositories, IBookWriteOnlyRepositories, IBookUpdateOnlyRepositores
{
    private readonly LibraryDbContext _context;
    public BooksRepository(LibraryDbContext context)
    {
        _context = context;
    }
    public async Task<List<Book>> GetAll()
    {
        return await _context.Books.Include(b => b.BookImage).Include(b => b.Reservations).AsNoTracking().ToListAsync();
    }

    public async Task Add(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    async Task<Book?> IBookUpdateOnlyRepositores.GetById(Guid id)
    {
        return await _context.Books.Include(b => b.BookImage).Include(b => b.Reservations).FirstOrDefaultAsync(x => x.Id == id);
    }

    async Task<Book?> IBookReadOnlyRepositories.GetById(Guid id)
    {
        return await _context.Books.AsNoTracking().Include(b => b.BookImage).Include(b => b.Reservations).FirstOrDefaultAsync(x => x.Id == id);
    }

    async Task<Book?> IBookReadOnlyRepositories.GetBySlug(string slug)
    {
        return await _context.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<bool> Delete(Guid id)
    {
        var response = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

        if (response is null) return false;

        _context.Books.Remove(response);

        return true;
    }
}
