using JoyModels.API.Setups.DatabaseSeed;

namespace JoyModels.API.Setups;

public static class WebApplicationSetup
{
    public static WebApplication RegisterAppServices(this WebApplication app)
    {
        app.RegisterSwaggerServices();
        app.RegisterDatabaseMigrations();
        app.RegisterDatabaseSeeder();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");

        return app;
    }
}