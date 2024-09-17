using library.Domain.Entities;

namespace library.Domain.Repositories.Tokens;

public interface ITokenReadOnlyRepositories
{
    bool Verify(string token);
    Token? GetLastByUserId(Guid userId);
    Task<Token?> GetByToken(string token);
}
