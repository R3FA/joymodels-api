namespace JoyModels.API.Setups;

public static class WebApplicationSetup
{
    public static WebApplication Create(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

        return app;
    }
}