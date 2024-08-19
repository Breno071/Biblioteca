namespace API.Features.Reservation.Events
{
    public class ReservationFinishedEvent
    {
        public Guid ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
