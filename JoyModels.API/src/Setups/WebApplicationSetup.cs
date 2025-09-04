namespace JoyModels.API.Setups;

public static class WebApplicationSetup
{
    public static WebApplication RegisterAppServices(this WebApplication app)
    {
        app.RegisterDatabaseMigrations();
        // app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}