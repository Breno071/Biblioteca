using Domain.Models.Entities;

namespace Domain.Events
{
    public class ReservationCreatedEventArgs(Reservation reservation) : EventArgs
    {
        public Reservation Reservation { get; } = reservation;
    }
}
