using JoyModels.API.Setups;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.RegisterDatabaseServices(builder.Configuration);
builder.Services.RegisterDependencyInjectionServices(builder.Configuration);
builder.Services.RegisterJwtServices(builder.Configuration);

var app = builder.Build().RegisterAppServices();
app.Run();