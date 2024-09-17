using library.Communication.Enums;
using library.Communication.Responses.BookImage;
using library.Communication.Responses.Reservations;

namespace library.Communication.Responses.Book;

public class ResponseBookJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string[] Genres { get; set; } = [];
    public string Synopsis { get; set; } = string.Empty;
    public DateTime Published { get; set; }
    public string Language { get; set; } = string.Empty;
    public StatusType Status { get; set; }
    public required ResponseBookImageJson BookImage { get; set; }
    public List<ResponseReservationJson> Reservations { get; set; } = [];
}
