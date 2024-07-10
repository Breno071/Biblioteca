namespace API.Features.Reservation.Endpoints.MakeReservation
{
    public class MakeReservationResponse
    {
        public Guid ReservationId { get; set; }
        public Guid ClientId { get; set; }
        public ICollection<Guid> BookIds { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
