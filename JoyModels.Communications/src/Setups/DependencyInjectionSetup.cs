using JoyModels.Communications.Services;
using JoyModels.Communications.Services.BackgroundServices;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;

namespace JoyModels.Communications.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(RabbitMqSetup.RegisterRabbitMqDetails(configuration));
        services.AddTransient<IMessageConsumer, MessageConsumer>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddHostedService<EmailBackgroundService>();

        return services;
    }
}