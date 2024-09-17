using library.Domain.Entities;

namespace library.Domain.Authentication;
public interface ITokenGenerator
{
    string Generator(User user);
}
