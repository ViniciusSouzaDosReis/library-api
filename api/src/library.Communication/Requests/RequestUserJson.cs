using library.Communication.Enums;

namespace library.Communication.Requests;

public class RequestUserJson
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RoleType Role { get; set; }
}
