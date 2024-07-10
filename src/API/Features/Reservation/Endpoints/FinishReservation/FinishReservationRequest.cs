using API.Features.Reservation.Validators;
using FastEndpoints;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Reservation.Endpoints.FinishReservation
{
    public class FinishReservationRequest
    {
        [Required]
        [FromRoute]
        public Guid ReservationId { get; set; }
    }

    public class FinishReservationRequestValidator : Validator<FinishReservationRequest>
    {
        public FinishReservationRequestValidator(IServiceScopeFactory serviceScopeFactory)
        {
            RuleFor(x => x.ReservationId).SetAsyncValidator(new ReservationShouldExistValidator<FinishReservationRequest>(serviceScopeFactory));
        }
    }
}
