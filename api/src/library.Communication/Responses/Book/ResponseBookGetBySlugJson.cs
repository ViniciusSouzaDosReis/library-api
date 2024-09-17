using library.Communication.Enums;

namespace library.Communication.Responses.Book;

public class ResponseBookGetBySlugJson
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string[] Genres { get; set; } = [];
    public string Synopsis { get; set; } = string.Empty;
    public DateTime Published { get; set; }
    public string Language { get; set; } = string.Empty;
    public StatusType Status { get; set; }
}
