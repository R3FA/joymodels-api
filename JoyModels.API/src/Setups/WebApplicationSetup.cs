namespace JoyModels.API.Setups;

public static class WebApplicationSetup
{
    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}