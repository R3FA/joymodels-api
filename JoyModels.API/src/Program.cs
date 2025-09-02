using JoyModels.API.Setups;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitializeDatabaseServices(builder.Configuration);
builder.Services.InitializeDependencyInjectionServices();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();
app.Run();