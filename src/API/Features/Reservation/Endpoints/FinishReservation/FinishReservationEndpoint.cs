using API.Features.Reservation.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Reservation.Endpoints.FinishReservation
{
    public class FinishReservationEndpoint : Endpoint<FinishReservationRequest, Ok>
    {
        public override void Configure()
        {
            Put($"/web/reservation");
            AllowAnonymous();

            Summary(x =>
            {
                x.Summary = "Finish a reservation";
            });

            Tags(ReservationFeature.Tags);
            Version(ReservationFeature.Version);
        }

        public override async Task<Ok> ExecuteAsync(FinishReservationRequest req, CancellationToken ct)
        {
            await Resolve<IReservationService>().FinishReservationAsync(req, ct);

            return TypedResults.Ok();
        }
    }
}
