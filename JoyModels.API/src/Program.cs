using JoyModels.API.Setups;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.InitializeDatabaseServices(builder.Configuration);
    builder.Services.InitializeDependencyInjectionServices();

    var app = builder.Build();
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    throw new Exception("An error occured", ex);
}