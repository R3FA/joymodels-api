using JoyModels.Communications.Services;
using JoyModels.Communications.Services.BackgroundServices;
using JoyModels.Models.Database;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Communications.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
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

        services.AddSingleton(RabbitMqSetup.RegisterRabbitMqDetails(configuration));
        services.AddTransient<IMessageConsumer, MessageConsumer>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddHostedService<EmailBackgroundService>();
        services.AddHostedService<NotificationBackgroundService>();

        return services;
    }
}