using JoyModels.API.Setups;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDatabaseServices(builder.Configuration);
builder.Services.RegisterDependencyInjectionServices();

var app = builder.Build().RegisterAppServices();
app.RegisterSwaggerServices();
app.Run();