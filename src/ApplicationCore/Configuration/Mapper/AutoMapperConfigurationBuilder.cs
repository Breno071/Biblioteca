using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Configuration.Mappers;

public class AutoMapperConfigurationBuilder
{
    public static void ConfigureAutoMapper(IServiceCollection services) 
        => services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
