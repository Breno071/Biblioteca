using Domain.Models.Entities;

namespace Domain.Events
{
    public class ReservationFinishedEventArgs(Reservation reservation) : EventArgs
    {
        public Reservation Reservation { get; } = reservation;
    }
}
