using System.ComponentModel.DataAnnotations;

namespace library.Domain.Entities;

public class Token
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string TokenCode { get; set; } = string.Empty;
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public bool IsActived { get; set; }
}
