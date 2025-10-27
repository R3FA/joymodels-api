using JoyModels.Communications.Setups;

var builder = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.AddJsonFile("appsettings.json", optional: false);
        configuration.AddJsonFile("appsettings.Development.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.RegisterDependencyInjectionServices(hostContext.Configuration);
    })
    .UseConsoleLifetime()
    .Build();

builder.Run();