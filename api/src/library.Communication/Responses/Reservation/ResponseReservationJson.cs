using library.Communication.Enums;

namespace library.Communication.Responses.Reservations;

public class ResponseReservationJson
{
    public Guid Id { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public StatusType Status { get; set; }
    public string BookName { get; set; } = string.Empty;
}
