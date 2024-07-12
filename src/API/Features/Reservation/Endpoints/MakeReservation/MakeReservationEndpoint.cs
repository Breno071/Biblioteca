using API.Features.Reservation.DTOs;
using API.Features.Reservation.Services;
using API.Shared.Extensions;
using Domain.Models.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Reservation.Endpoints.MakeReservation
{
    public class MakeReservationEndpoint : Endpoint<MakeReservationRequest, Created<MakeReservationResponse>>
    {
        public override void Configure()
        {
            Post($"/web/reservation");
            AllowAnonymous();

            Summary(x =>
            {
                x.Summary = "Create a new reservation";
                x.Responses[201] = "Reservation created";
            });

            Tags(ReservationFeature.Tags);
            Version(ReservationFeature.Version);
        }

        public override async Task<Created<MakeReservationResponse>> ExecuteAsync(MakeReservationRequest req, CancellationToken ct)
        {
            var result = await Resolve<IReservationService>().MakeReservationAsync(req, ct);

            return TypedResults.Created(HttpContext.CreatedUri(result.ReservationId), new MakeReservationResponse
            {
                ReservationId = result.ReservationId,
                ClientId = result.ClientId,
                BookIds = result.BookIds,
                ReservationDate = result.ReservationDate,
                ReturnDate = result.ReturnDate,
                IsReturned = result.IsReturned
            });
        }
    }
}
