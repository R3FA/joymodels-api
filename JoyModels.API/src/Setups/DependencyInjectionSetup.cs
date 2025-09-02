using JoyModels.API.Handlers;

namespace JoyModels.API.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
        this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddControllers();

        return services;
    }
}