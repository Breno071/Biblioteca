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
            _producer.Send(e.Reservation);
        }
    }
}
