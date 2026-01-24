using System.Reflection;
using System.Text.Json.Serialization;
using JoyModels.API.Handlers;
using JoyModels.Communications.Setups;
using JoyModels.Models.Database;
using JoyModels.Services.Services.Categories;
using JoyModels.Services.Services.CommunityPost;
using JoyModels.Services.Services.CommunityPostQuestionSection;
using JoyModels.Services.Services.CommunityPostReviewType;
using JoyModels.Services.Services.CommunityPostType;
using JoyModels.Services.Services.ModelAvailability;
using JoyModels.Services.Services.ModelFaqSection;
using JoyModels.Services.Services.ModelReviews;
using JoyModels.Services.Services.ModelReviewType;
using JoyModels.Services.Services.Models;
using JoyModels.Services.Services.Library;
using JoyModels.Services.Services.Orders;
using JoyModels.Services.Services.ShoppingCart;
using JoyModels.Services.Services.Sso;
using JoyModels.Services.Services.UserRole;
using JoyModels.Services.Services.Notification;
using JoyModels.Services.Services.Report;
using JoyModels.Services.Services.Users;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageConsumer;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.OpenApi;

namespace JoyModels.API.Setups;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDependencyInjectionServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            x.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document),
                    new List<string>()
                }
            });
        });

        services.AddHttpContextAccessor();

        services.AddAutoMapper(cfg =>
        {
            var dataAccessAssembly = Assembly.GetAssembly(typeof(JoyModelsDbContext));
            cfg.AddMaps(dataAccessAssembly);
        });

        services.AddSingleton(JwtSetup.RegisterJwtDetails(configuration));
        services.AddTransient<UserAuthValidation>();

        services.AddSingleton(RabbitMqSetup.RegisterRabbitMqDetails(configuration));
        services.AddTransient<IMessageProducer, MessageProducer>();
        services.AddTransient<IMessageConsumer, MessageConsumer>();

        services.AddSingleton(ImageSettingsSetup.RegisterModelImageSettingsDetails(configuration));
        services.AddSingleton(ImageSettingsSetup.RegisterUserImageSettingsDetails(configuration));

        services.AddSingleton(ModelSettingsSetup.RegisterModelSettingsDetails(configuration));

        services.AddSingleton(StripeSetup.RegisterStripeDetails(configuration));

        services.AddTransient<ISsoService, SsoService>();
        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<IUserRoleService, UserRoleService>();
        services.AddTransient<IModelService, ModelService>();
        services.AddTransient<IModelAvailabilityService, ModelAvailabilityService>();
        services.AddTransient<IModelReviewService, ModelReviewService>();
        services.AddTransient<IModelFaqSectionService, ModelFaqSectionService>();
        services.AddTransient<IModelReviewTypeService, ModelReviewTypeService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICommunityPostService, CommunityPostService>();
        services.AddTransient<ICommunityPostTypeService, CommunityPostTypeService>();
        services.AddTransient<ICommunityPostReviewTypeService, CommunityPostReviewTypeService>();
        services.AddTransient<ICommunityPostQuestionSectionService, CommunityPostQuestionSectionService>();
        services.AddTransient<IShoppingCartService, ShoppingCartService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ILibraryService, LibraryService>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IReportService, ReportService>();

        services
            .AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return services;
    }
}