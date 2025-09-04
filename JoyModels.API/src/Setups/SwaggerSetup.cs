namespace JoyModels.API.Setups;

public static class SwaggerSetup
{
    public static WebApplication RegisterSwaggerServices(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}