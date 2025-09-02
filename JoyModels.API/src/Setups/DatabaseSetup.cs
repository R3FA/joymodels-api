using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.API.Setups;

public static class DatabaseSetup
{
    public static IServiceCollection RegisterDatabaseServices(
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
}