using library.Domain.Entities;
using library.Domain.Repositories.Tokens;
using Microsoft.EntityFrameworkCore;

namespace library.Infrastructure.DataAccess.Repositories;

public class TokenRepository : ITokenReadOnlyRepositories, ITokenWriteOnlyRepositories
{
    private readonly LibraryDbContext _context;

    public TokenRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public bool Verify(string token)
    {
        var response = _context.Tokens.FirstOrDefault(t => t.TokenCode == token);

        return response != null && response.IsActived;
    }
    public Token? GetLastByUserId(Guid userId)
    {
        return _context.Tokens.OrderByDescending(t => t.CreatedDate).FirstOrDefault();
    }

    public async Task<Token?> GetByToken(string token)
    {
        return await _context.Tokens.FirstOrDefaultAsync(t => t.TokenCode == token);
    }

    public async Task Add(Token token)
    {
        await _context.Tokens.AddAsync(token);
    }


    public void Update(Token token)
    {
        _context.Tokens.Update(token);
    }
}
