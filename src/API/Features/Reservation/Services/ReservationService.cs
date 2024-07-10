using API.Features.Reservation.Endpoints.FinishReservation;
using API.Features.Reservation.Endpoints.MakeReservation;
using Domain.Models.Entities;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Reservation.Services
{
    public interface IReservationService
    {
        Task<MakeReservationResponse> AddReservationAsync(MakeReservationRequest req, CancellationToken ct);
        Task FinishReservationAsync(FinishReservationRequest req, CancellationToken ct);
    }

    public class ReservationService : IReservationService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReservationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<MakeReservationResponse> AddReservationAsync(MakeReservationRequest req, CancellationToken ct)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var client = await dbContext.Clients.SingleAsync(c => c.Code == req.ClientId, cancellationToken: ct);
            var books = await dbContext.Books.Where(b => req.Books.Contains(b.Code)).ToListAsync(ct);

            var reservationId = Guid.NewGuid();

            var reservation = new Domain.Models.Entities.Reservation
            {
                Code = reservationId,
                Client = client,
                Books = books,
                ReservationDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddMonths(-1)
            };

            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync(ct);

            return new MakeReservationResponse
            {
                ReservationId = reservationId,
                ClientId = client.Code,
                BookIds = books.Select(b => b.Code).ToList(),
                ReservationDate = reservation.ReservationDate,
                ReturnDate = reservation.ReturnDate,
                IsReturned = reservation.IsReturned
            };
        }

        public async Task FinishReservationAsync(FinishReservationRequest req, CancellationToken ct)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var reservation = await dbContext.Reservations.SingleAsync(r => r.Code == req.ReservationId, cancellationToken: ct);

            reservation.ReturnDate = DateTime.Now;
            reservation.IsReturned = true;

            dbContext.Update(reservation);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
