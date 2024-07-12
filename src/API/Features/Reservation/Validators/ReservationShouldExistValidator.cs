using FluentValidation;
using FluentValidation.Validators;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Reservation.Validators
{
    public class ReservationShouldExistValidator<T> : IAsyncPropertyValidator<T, Guid> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReservationShouldExistValidator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public string Name => "ReservationId";

        public string GetDefaultMessageTemplate(string errorCode) => "Reserva não encontrada!";

        public async Task<bool> IsValidAsync(ValidationContext<T> context, Guid value, CancellationToken cancellation)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var reservationExist = await dbContext.Reservations
                .AsNoTracking()
                .AnyAsync(r => r.ReservationId == value, cancellationToken: cancellation);

            return reservationExist;
        }
    }

    public static class ReservationShouldExistValidatorExtension
    {
        public static IRuleBuilderOptions<T, Guid> ReservationShouldExistValidator<T>(this IRuleBuilderInitial<T, Guid> ruleBuilder, IServiceScopeFactory serviceScopeFactory) where T : class
        {
            return ruleBuilder.SetAsyncValidator(new ReservationShouldExistValidator<T>(serviceScopeFactory));
        }
    }
}
