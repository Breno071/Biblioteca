using FluentValidation;
using FluentValidation.Validators;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Client.Validators
{
    public class ClientShouldExistValidator<T> : IAsyncPropertyValidator<T, Guid> where T : class
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ClientShouldExistValidator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public string Name => "ClientId";

        public string GetDefaultMessageTemplate(string errorCode) => "Cliente não encontrado!";

        public async Task<bool> IsValidAsync(ValidationContext<T> context, Guid value, CancellationToken cancellation)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var clientExist = await dbContext.Clients
                .AsNoTracking()
                .AnyAsync(c => c.ClientId == value, cancellationToken: cancellation);

            return clientExist;
        }
    }

    public static class ClientShouldExistValidatorExtension
    {
        public static IRuleBuilderOptions<T, Guid> ClientShouldExist<T>(this IRuleBuilderInitial<T, Guid> ruleBuilder, IServiceScopeFactory serviceScopeFactory) where T : class
        {
            return ruleBuilder.SetAsyncValidator(new ClientShouldExistValidator<T>(serviceScopeFactory));
        }
    }
}
