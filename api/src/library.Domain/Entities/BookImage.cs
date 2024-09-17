using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library.Domain.Entities;

public class BookImage
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [ForeignKey("Book")]
    public Guid BookId { get; set; }
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string S3Key { get; set; } = string.Empty;
    [Required]
    public string ContentType { get; set; } = string.Empty;
    public virtual required Book Book { get; set; }
}
