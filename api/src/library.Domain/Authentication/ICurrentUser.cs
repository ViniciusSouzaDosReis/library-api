namespace library.Domain.Authentication;
public interface ICurrentUser
{
    Guid GetId();
    string GetRole();
}
