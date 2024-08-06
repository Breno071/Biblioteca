using API.Features.Client.DTOs;
using API.Features.Reservation.Endpoints.FinishReservation;
using AutoFixture;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Net.Http.Json;
using Tests.SharedUtils;

namespace Tests.Features.Reservation
{
    public class FinishReservationTests(IntegrationTestWebApiFactory factory) : BaseIntegrationTest(factory)
    {
        private const string Path = "/web/reservation/";
        private readonly Fixture _autoFixture = new Fixture();

        [Fact]
        public async Task GivenValidReservationCode_WhenFinishingReservation_ThenReturnsOkResult()
        {
            // Arrange
            var client = (await _autoFixture.AddClientsToDb(DbContext, 1)).Single();
            var books = await _autoFixture.AddBooksOnDb(DbContext, 2);
            var reservation = await _autoFixture.AddReservationToDb(DbContext, client, books);

            // Act
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, reservation.ReservationId), new FinishReservationRequest());

            // Assert
            rsp.StatusCode.Should().Be(HttpStatusCode.OK, await rsp.Content.ReadAsStringAsync());

            var reservationDb = DbContext.Reservations.Single(x => x.ReservationId == reservation.ReservationId);

            reservationDb.Should().NotBeNull();

            reservationDb!.IsReturned.Should().BeTrue();
            reservationDb!.ReturnDate.Should().BeAfter(DateTime.MinValue);
        }

        [Fact]
        public async Task GivenNonExistentReservation_WhenFinishingReservation_ThenReturnsNotFoundResult()
        {
            // Arrange
            var reservationId = Guid.NewGuid();

            // Act
            var rsp = await AnonymousUser.PutAsJsonAsync(string.Concat(Path, reservationId), new FinishReservationRequest());
            var res = await rsp.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            // Assert
            res!.Errors["reservationId"].Should().HaveCount(1);
            res!.Errors["reservationId"].Single().Should().Be("Reserva não encontrada!");
        }
    }
}
