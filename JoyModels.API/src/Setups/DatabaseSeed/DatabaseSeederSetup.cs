using JoyModels.API.Setups.DatabaseSeed.Seeders;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;

namespace JoyModels.API.Setups.DatabaseSeed;

public static class DatabaseSeederSetup
{
    public static IApplicationBuilder RegisterDatabaseSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JoyModelsDbContext>();
        var userImageSettings = scope.ServiceProvider.GetRequiredService<UserImageSettingsDetails>();
        var modelImageSettings = scope.ServiceProvider.GetRequiredService<ModelImageSettingsDetails>();
        var modelSettings = scope.ServiceProvider.GetRequiredService<ModelSettingsDetails>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<JoyModelsDbContext>>();

        UserSeeder.SeedUsers(context, userImageSettings, logger).GetAwaiter().GetResult();
        ModelSeeder.SeedModels(context, modelImageSettings, modelSettings, logger).GetAwaiter().GetResult();
        ModelFaqSeeder.SeedModelFaqSections(context, logger).GetAwaiter().GetResult();
        UserModelLikeSeeder.SeedUserModelLikes(context, logger).GetAwaiter().GetResult();
        OrderAndLibrarySeeder.SeedOrdersAndLibrary(context, logger).GetAwaiter().GetResult();
        ShoppingCartSeeder.SeedShoppingCart(context, logger).GetAwaiter().GetResult();
        ModelReviewSeeder.SeedModelReviews(context, logger).GetAwaiter().GetResult();
        UserFollowerSeeder.SeedUserFollowers(context, logger).GetAwaiter().GetResult();
        CommunityPostSeeder.SeedCommunityPosts(context, modelImageSettings, logger).GetAwaiter().GetResult();
        CommunityPostCommentSeeder.SeedCommunityPostQuestionSections(context, logger).GetAwaiter().GetResult();
        CommunityPostReviewSeeder.SeedCommunityPostUserReviews(context, logger).GetAwaiter().GetResult();
        ReportSeeder.SeedReports(context, logger).GetAwaiter().GetResult();

        return app;
    }
}