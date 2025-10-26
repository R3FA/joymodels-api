using JoyModels.Communications.Services;
using JoyModels.Communications.Services.BackgroundServices;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IMessageConsumer, MessageConsumer>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddHostedService<EmailBackgroundService>();
    })
    .UseConsoleLifetime()
    .Build();

builder.Run();