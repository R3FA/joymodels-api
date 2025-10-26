using JoyModels.Communications.Services;
using JoyModels.Communications.Services.BackgroundServices;
using JoyModels.Utilities.RabbitMQ;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;

var builder = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.AddJsonFile("appsettings.json", optional: false);
        configuration.AddJsonFile("appsettings.Development.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        var config = hostContext.Configuration;
        var rabbitConnectionDetails = config.GetSection("Connection:RabbitMQ");

        RabbitMqService.Init(rabbitConnectionDetails["Host"], rabbitConnectionDetails["User"],
            rabbitConnectionDetails["Password"]);

        services.AddTransient<IMessageConsumer, MessageConsumer>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddHostedService<EmailBackgroundService>();
    })
    .UseConsoleLifetime()
    .Build();

builder.Run();