using library.Domain.Entities;

namespace library.Domain.Repositories.Tokens;

public interface ITokenWriteOnlyRepositories
{
    Task Add(Token token);
    void Update(Token token);
}
