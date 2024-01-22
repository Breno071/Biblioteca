using Domain.Models.Entities;

namespace Domain.Interfaces
{
    public interface IReservationService
    {
        Task AddReservation(Reservation reservation);
    }
}
