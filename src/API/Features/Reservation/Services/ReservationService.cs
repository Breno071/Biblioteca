using API.Features.Reservation.Endpoints.FinishReservation;
using API.Features.Reservation.Endpoints.MakeReservation;
using Core.Messaging.Services;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Reservation.Services
{
    public interface IReservationService
    {
        Task<MakeReservationResponse> MakeReservationAsync(MakeReservationRequest req, CancellationToken ct);
        Task FinishReservationAsync(FinishReservationRequest req, CancellationToken ct);
    }

    public class ReservationService : IReservationService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISendMessage _sendMessageService;

        public ReservationService(IServiceScopeFactory serviceScopeFactory, ISendMessage sendMessageService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _sendMessageService = sendMessageService;
        }

        public async Task<MakeReservationResponse> MakeReservationAsync(MakeReservationRequest req, CancellationToken ct)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var client = await dbContext.Clients.SingleAsync(c => c.ClientId == req.ClientId, cancellationToken: ct);
            var books = await dbContext.Books.Where(b => req.Books.Contains(b.BookId)).ToListAsync(ct);

            var reservationId = Guid.NewGuid();

            var today = DateTime.Now;

            var reservation = new Domain.Models.Entities.Reservation
            {
                ReservationId = reservationId,
                Client = client,
                Books = books,
                ReservationDate = today,
                ReturnDate = today.AddMonths(1)
            };

            dbContext.Reservations.Add(reservation);

            books.ForEach(b => b.Stock--);

            //Dispara evento no rabbitMQ
            _sendMessageService.SendLargeMessage(reservation, "");

            await dbContext.SaveChangesAsync(ct);            

            return new MakeReservationResponse
            {
                ReservationId = reservationId,
                ClientId = client.ClientId,
                BookIds = books.ConvertAll(b => b.BookId),
                ReservationDate = reservation.ReservationDate,
                ReturnDate = reservation.ReturnDate,
                IsReturned = reservation.IsReturned
            };
        }

        public async Task FinishReservationAsync(FinishReservationRequest req, CancellationToken ct)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var reservation = await dbContext.Reservations.SingleAsync(r => r.ReservationId == req.ReservationId, cancellationToken: ct);

            reservation.ReturnDate = DateTime.Now;
            reservation.IsReturned = true;

            dbContext.Update(reservation);
            await dbContext.SaveChangesAsync(ct);

            _sendMessageService.SendLargeMessage(reservation, "");
        }
    }
}
