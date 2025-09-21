namespace JoyModels.API.Setups;

public static class WebApplicationSetup
{
    public static WebApplication RegisterAppServices(this WebApplication app)
    {
        app.RegisterSwaggerServices();
        app.RegisterDatabaseMigrations();
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}