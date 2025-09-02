using JoyModels.API.Setups;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitializeDatabaseServices(builder.Configuration);
builder.Services.InitializeDependencyInjectionServices();

builder.Build().Create();