using library.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library.Domain.Entities;  
public class Reservation
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    [ForeignKey("Book")]
    public Guid BookId { get; set; }
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    [Required]
    public string ReservationCode { get; set; } = string.Empty;
    [Required]
    public DateTime ReservationDate { get; set; }
    [Required]
    public StatusType Status { get; set; }
    public virtual Book? Book { get; set; }
    public virtual User? User { get; set; }
}
