using JoyModels.API.Handlers;
using JoyModels.Services.Services.Sso;

namespace JoyModels.API.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
        this IServiceCollection services)
    {
        // GlobalExceptionHandler DI
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Swagger DI
        services.AddSwaggerGen();

        // Custom Defined Services
        services.AddTransient<ISsoService, SsoService>();

        services.AddControllers();

        return services;
    }
}