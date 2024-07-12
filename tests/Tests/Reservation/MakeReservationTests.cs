using API.Features.Reservation.Endpoints.FinishReservation;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using Tests.SharedUtils;
using API.Features.Reservation.Endpoints.MakeReservation;
using Microsoft.EntityFrameworkCore;

namespace Tests.Stock
{
    public class MakeReservationTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/reservation/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidParameters_WhenMakingReservation_ThenReturnsOkResultWithReservation()
        {
            // Arrange
            var client = (await _autoFixture.AddClientsToDb(DbContext, 1)).Single();
            var books = await _autoFixture.AddBooksOnDb(DbContext, 2);

            var req = new MakeReservationRequest
            {
                ClientId = client.ClientId,
                Books = books.ConvertAll(x => x.BookId)
            };

            // Act
            var rsp = await AnonymousUser.PostAsJsonAsync(Path, req);

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.Created, await rsp.Content.ReadAsStringAsync());
            var res = await rsp.Content.ReadFromJsonAsync<MakeReservationResponse>();

            res.Should().NotBeNull();

            var reservationDb = await DbContext.Reservations
                .AsNoTracking()
                .Include(x => x.Books)
                .Include(x => x.Client)
                .SingleAsync(x => x.ReservationId == res!.ReservationId);

            reservationDb.Should().NotBeNull();

            reservationDb!.IsReturned.Should().BeFalse();
            reservationDb!.ReturnDate.Should().Be(reservationDb.ReservationDate.AddMonths(1));
            reservationDb!.ClientId.Should().Be(client.ClientId);

            reservationDb!.Books.Should().HaveCount(books.Count);

            reservationDb!.Books.AsEnumerable().Should().BeEquivalentTo(books);
        }
    }
}
