using System.Reflection;
using JoyModels.API.Handlers;
using JoyModels.Communications.Setups;
using JoyModels.Models.Database;
using JoyModels.Services.Services.Categories;
using JoyModels.Services.Services.ModelAvailability;
using JoyModels.Services.Services.ModelReviews;
using JoyModels.Services.Services.Models;
using JoyModels.Services.Services.Sso;
using JoyModels.Services.Services.Users;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.OpenApi.Models;

namespace JoyModels.API.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // GlobalExceptionHandler DI
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Swagger DI
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Insert JWT access token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            };
            c.AddSecurityRequirement(securityRequirement);
        });

        // IHttpContext DI
        services.AddHttpContextAccessor();

        // AutoMapper DI
        services.AddAutoMapper(cfg =>
        {
            var dataAccessAssembly = Assembly.GetAssembly(typeof(JoyModelsDbContext));
            cfg.AddMaps(dataAccessAssembly);
        });

        // JWT DI
        services.AddSingleton(JwtSetup.RegisterJwtDetails(configuration));
        services.AddTransient<UserAuthValidation>();

        // RabbitMQ DI
        services.AddSingleton(RabbitMqSetup.RegisterRabbitMqDetails(configuration));
        services.AddTransient<IMessageProducer, MessageProducer>();
        services.AddTransient<IMessageConsumer, MessageConsumer>();

        // ImageSettings DI
        services.AddSingleton(ImageSettingsSetup.RegisterModelImageSettingsDetails(configuration));
        services.AddSingleton(ImageSettingsSetup.RegisterUserImageSettingsDetails(configuration));

        // ModelSettings DI
        services.AddSingleton(ModelSettingsSetup.RegisterModelSettingsDetails(configuration));

        // Custom Defined Services
        services.AddTransient<ISsoService, SsoService>();
        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<IModelService, ModelService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IModelAvailabilityService, ModelAvailabilityService>();
        services.AddTransient<IModelReviewService, ModelReviewService>();

        services.AddControllers();

        return services;
    }
}