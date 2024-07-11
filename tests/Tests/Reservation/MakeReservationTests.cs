using API.Features.Reservation.Endpoints.FinishReservation;
using AutoFixture;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using Tests.SharedUtils;
using API.Features.Reservation.Endpoints.MakeReservation;

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

            var reservationDb = DbContext.Reservations.Single(x => x.ReservationId == res.ReservationId);

            reservationDb.Should().NotBeNull();

            reservationDb!.IsReturned.Should().BeFalse();
            reservationDb!.ReturnDate.Should().BeNull();
            reservationDb!.Client.Should().Be(client);
            reservationDb!.Books.AsEnumerable().Should().Equal(books);
        }
    }
}
