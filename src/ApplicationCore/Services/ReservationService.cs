using Domain.Events;
using Domain.Interfaces;
using Domain.Models.Entities;
using Infraestructure.Data;

namespace ApplicationCore.Services
{
    public class ReservationService(BaseDbContext context, IProducer producer) : IReservationService
    {
        private readonly BaseDbContext _context = context;
        private readonly IProducer _producer = producer;
        private const string RESERVATIONS_QUEUE = "Reservations";
        private const string FINISHED_RESERVATIONS_QUEUE = "Finished_Reservations";

        public async Task AddReservation(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            // Config the event subscriber
            reservation.ReservationCreated += ReservationCreated;

            // Publish the event
            reservation.OnReservationCreated();

            // Removes the event subscriber
            reservation.ReservationCreated -= ReservationCreated;
        }

        private void ReservationCreated(object? sender, ReservationCreatedEventArgs e)
        {
            // Publish the message
            _producer.Send(e.Reservation, RESERVATIONS_QUEUE);
        }

        public async Task FinishReservation(Reservation reservation)
        {
            reservation.IsReturned = true;
            reservation.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // Config the event subscriber
            reservation.ReservationFinished += ReservationFinished;

            // Publish the event
            reservation.OnFinishedReservation();

            // Removes the event subscriber
            reservation.ReservationFinished -= ReservationFinished;
        }

        private void ReservationFinished(object? sender, ReservationFinishedEventArgs e)
        {
            // Publish the message
            _producer.Send(e.Reservation, FINISHED_RESERVATIONS_QUEUE);
        }
    }
}
