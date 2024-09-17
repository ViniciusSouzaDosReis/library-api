using System.ComponentModel.DataAnnotations;

namespace library.Domain.Entities;

public class Book
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Slug { get; set; } = string.Empty;
    [Required]
    public string[] Genres { get; set; } = [];
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public string Synopsis { get; set; } = string.Empty;
    [Required]
    public DateTime Published { get; set; }
    [Required]
    public string? Language { get; set; }
    public virtual BookImage? BookImage { get; set; }
    public virtual List<Reservation>? Reservations { get; set; }
}
