using FluentValidation;
using FluentValidation.Validators;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Client.Validators
{
    public class ClientShouldNotExistValidator<T> : IAsyncPropertyValidator<T, string> where T : class
    {
        public string Name => "Email";

        public string GetDefaultMessageTemplate(string errorCode) => "Já existe um usuário com este email";

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ClientShouldNotExistValidator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> IsValidAsync(ValidationContext<T> context, string value, CancellationToken cancellation)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var clientExist = await dbContext.Clients.AsNoTracking().AnyAsync(x => x.Email == value, cancellationToken: cancellation);

            return !clientExist;
        }
    }

    public static class ClientShouldNotExistValidatorExtension
    {
        public static IRuleBuilderOptions<T, string> ClientShouldNotExist<T>(this IRuleBuilderInitial<T, string> ruleBuilder, IServiceScopeFactory serviceScopeFactory) where T : class
        {
            return ruleBuilder.SetAsyncValidator(new ClientShouldNotExistValidator<T>(serviceScopeFactory));
        }
    }
}
