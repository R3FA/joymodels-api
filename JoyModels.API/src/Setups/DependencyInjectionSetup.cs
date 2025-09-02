using JoyModels.API.Handlers;
using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.API.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection InitializeDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mariaDbDetails = configuration.GetSection("Connection:MariaDB");
        var mariaDbConnectionString = $"Server={mariaDbDetails["Server"]};" +
                                      $"Port={mariaDbDetails["Port"]};" +
                                      $"Database={mariaDbDetails["Database"]};" +
                                      $"User={mariaDbDetails["User"]};" +
                                      $"Password={mariaDbDetails["Password"]};";
        var mariaDbVersion = ServerVersion.AutoDetect(mariaDbConnectionString);

        services.AddDbContext<JoyModelsDbContext>(options => options.UseMySql(mariaDbConnectionString, mariaDbVersion));

        return services;
    }

    public static IServiceCollection InitializeDependencyInjectionServices(
        this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddControllers();

        return services;
    }
}