using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Configuration.Mappers;

public class AutoMapperConfigurationBuilder
{
    public static void ConfigureAutoMapper(IServiceCollection services, Type type) 
        => services.AddAutoMapper(type);
}
