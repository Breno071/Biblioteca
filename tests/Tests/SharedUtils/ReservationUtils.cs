using AutoFixture;
using Infraestructure.Data;

namespace Tests.SharedUtils
{
    public static class ReservationUtils
    {
        public static async Task<Domain.Models.Entities.Reservation> AddReservationToDb(this Fixture fixture, BaseDbContext dbContext, Domain.Models.Entities.Client client, List<Domain.Models.Entities.Book> books)
        {
            var reservation = fixture
                .Build<Domain.Models.Entities.Reservation>()
                .With(x => x.Client, client)
                .With(x => x.Books, books)
                .Create();

            dbContext.Add(reservation);
            await dbContext.SaveChangesAsync();

            return reservation;
        }
    }
}
