using library.Communication.Enums;
using library.Communication.Responses.Book;
using library.Communication.Responses.Reservations;

namespace library.Communication.Responses.User;

public class ResponseUserJson
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RoleType Role { get; set; }
    public ResponseBookJson? CurrentBook { get; set; }
    public ICollection<ResponseReservationJson>? Reservations { get; set; }
}
