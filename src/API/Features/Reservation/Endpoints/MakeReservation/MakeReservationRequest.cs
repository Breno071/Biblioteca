using API.Features.Book.Validators;
using API.Features.Client.Validators;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Features.Reservation.Endpoints.MakeReservation
{
    public class MakeReservationRequest
    {
        [Required]
        [FromRoute]
        public Guid ClientId { get; set; }
        public List<Guid> Books { get; set; }
    }

    public class MakeReservationRequestValidator : Validator<MakeReservationRequest>
    {
        public MakeReservationRequestValidator(IServiceScopeFactory serviceScopeFactory)
        {
            RuleFor(x => x.ClientId).ClientShouldExist(serviceScopeFactory);
            RuleForEach(x => x.Books).SetAsyncValidator(new BookShouldExistValidator<MakeReservationRequest>(serviceScopeFactory));
            RuleForEach(x => x.Books).SetAsyncValidator(new BookShouldHaveStock<MakeReservationRequest>(serviceScopeFactory));
        }
    }
}
