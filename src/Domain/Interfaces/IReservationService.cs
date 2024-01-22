using Domain.Models.Entities;

namespace Domain.Interfaces
{
    public interface IReservationService
    {
        Task AddReservation(Reservation reservation);
        Task FinishReservation(Reservation reservation);
    }
}
