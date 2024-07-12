using Domain.Events;
using Domain.Interfaces;

namespace API.Features.Reservation.Events
{
    public interface IReservationEventService
    {
        Task MakeReservationEvent(Domain.Models.Entities.Reservation reservation);
        Task AddFinishReservationEvent(Domain.Models.Entities.Reservation reservation);
    }

    public class ReservationEventService(IProducer producer) : IReservationEventService
    {
        private readonly IProducer _producer = producer;
        private const string RESERVATIONS_QUEUE = "Reservations";
        private const string FINISHED_RESERVATIONS_QUEUE = "Finished_Reservations";

        public Task MakeReservationEvent(Domain.Models.Entities.Reservation reservation)
        {
            // Config the event subscriber
            reservation.ReservationCreated += ReservationCreated;

            // Publish the event
            reservation.OnReservationCreated();

            // Removes the event subscriber
            reservation.ReservationCreated -= ReservationCreated;

            return Task.CompletedTask;
        }

        public Task AddFinishReservationEvent(Domain.Models.Entities.Reservation reservation)
        {
            // Config the event subscriber
            reservation.ReservationFinished += ReservationFinished;

            // Publish the event
            reservation.OnFinishedReservation();

            // Removes the event subscriber
            reservation.ReservationFinished -= ReservationFinished;

            return Task.CompletedTask;
        }

        private void ReservationFinished(object? sender, ReservationFinishedEventArgs e)
        {
            // Publish the message
            producer.Send(e.Reservation, FINISHED_RESERVATIONS_QUEUE);
        }

        private void ReservationCreated(object? sender, ReservationCreatedEventArgs e)
        {
            // Publish the message
            _producer.Send(e.Reservation, RESERVATIONS_QUEUE);
        }
    }
}
