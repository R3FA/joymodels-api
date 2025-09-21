using JoyModels.Models.Database;
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

    public static IApplicationBuilder RegisterDatabaseMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<JoyModelsDbContext>();

        if (context == null) throw new Exception("JoyModelsDbContext is not registered as a service!");

        context.Database.Migrate();

        return app;
    }
}