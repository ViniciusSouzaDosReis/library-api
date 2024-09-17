using library.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library.Domain.Entities;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [ForeignKey("Book")]
    public Guid? CurrentBookId { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public RoleType Role { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public Book? CurrentBook { get; set; }
    public List<Reservation>? Reservations { get; set; }
}
