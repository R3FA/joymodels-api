using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.Sso.HelperMethods;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

namespace JoyModels.API.Setups;

public static class DatabaseSeederSetup
{
    private static readonly string[] AvatarColors =
    [
        "#3498db",
        "#e74c3c",
        "#2ecc71",
        "#9b59b6",
        "#f39c12",
        "#1abc9c"
    ];

    private static readonly string[] FirstNames =
    [
        "Alex", "Jordan", "Taylor", "Morgan", "Casey",
        "Riley", "Quinn", "Avery", "Peyton", "Cameron",
        "Skyler", "Dakota", "Reese", "Finley", "Sage",
        "Rowan", "Emery", "Phoenix", "Harley", "Blake",
        "Drew", "Hayden", "Jamie", "Logan", "Parker"
    ];

    private static readonly string[] LastNames =
    [
        "Smith", "Johnson", "Williams", "Brown", "Jones",
        "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Wilson", "Anderson", "Taylor", "Thomas", "Moore",
        "Jackson", "Martin", "Lee", "Thompson", "White",
        "Harris", "Clark", "Lewis", "Robinson", "Walker"
    ];

    public static IApplicationBuilder RegisterDatabaseSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JoyModelsDbContext>();
        var userImageSettings = scope.ServiceProvider.GetRequiredService<UserImageSettingsDetails>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<JoyModelsDbContext>>();

        SeedUsers(context, userImageSettings, logger).GetAwaiter().GetResult();

        return app;
    }

    private static async Task SeedUsers(
        JoyModelsDbContext context,
        UserImageSettingsDetails userImageSettings,
        ILogger logger)
    {
        var existingUsersCount = await context.Users.CountAsync();
        if (existingUsersCount > 0)
        {
            logger.LogInformation("Database already contains {Count} users. Skipping user seeding.",
                existingUsersCount);
            return;
        }

        logger.LogInformation("Starting database seeding...");

        var rootRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Root));
        var adminRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Admin));
        var userRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.User));

        const string defaultPassword = "strinG1!";
        var users = new List<User>();

        for (var i = 0; i < 25; i++)
        {
            var userUuid = Guid.NewGuid();
            var colorIndex = i % AvatarColors.Length;
            var pictureFileName = $"avatar-{userUuid}.jpg";

            var role = i switch
            {
                0 or 1 => rootRole,
                2 or 3 or 4 => adminRole,
                _ => userRole
            };

            var user = new User
            {
                Uuid = userUuid,
                Email = $"user{i + 1}@joymodels.com",
                NickName = $"user{i + 1}",
                FirstName = FirstNames[i],
                LastName = LastNames[i],
                PasswordHash = string.Empty,
                UserRoleUuid = role.Uuid,
                UserRoleUu = role,
                CreatedAt = DateTime.UtcNow,
                UserPictureLocation = pictureFileName,
                UserFollowerCount = 0,
                UserFollowingCount = 0,
                UserLikedModelsCount = 0,
                UserModelsCount = 0
            };

            user.PasswordHash = SsoPasswordHasher.Hash(user, defaultPassword);

            GenerateAndSaveAvatar(userUuid, pictureFileName, AvatarColors[colorIndex], userImageSettings);

            users.Add(user);

            logger.LogInformation("Created user: {Nickname} ({Role})", user.NickName, role.RoleName);
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        logger.LogInformation("Database seeding completed. Created {Count} users.", users.Count);
    }

    private static void GenerateAndSaveAvatar(
        Guid userUuid,
        string fileName,
        string hexColor,
        UserImageSettingsDetails userImageSettings)
    {
        const int size = 512;
        const int circleRadius = 120;
        const int headRadius = 80;
        var center = size / 2;

        var userFolder = Path.Combine(userImageSettings.SavePath, "users", userUuid.ToString());
        Directory.CreateDirectory(userFolder);

        var filePath = Path.Combine(userFolder, fileName);

        var backgroundColor = Color.ParseHex(hexColor);
        var darkerShade = DarkenColor(backgroundColor, 0.2f);
        var lighterShade = LightenColor(backgroundColor, 0.3f);

        using var image = new Image<Rgba32>(size, size);

        image.Mutate(ctx =>
        {
            ctx.BackgroundColor(backgroundColor);

            var bodyEllipse = new EllipsePolygon(center, size - 80, circleRadius + 40, circleRadius);
            ctx.Fill(lighterShade, bodyEllipse);

            var headEllipse = new EllipsePolygon(center, center - 40, headRadius, headRadius);
            ctx.Fill(lighterShade, headEllipse);

            var innerCircle = new EllipsePolygon(center, center - 40, headRadius - 15, headRadius - 15);
            ctx.Fill(darkerShade, innerCircle);
        });

        image.SaveAsJpeg(filePath);
    }

    private static Color DarkenColor(Color color, float amount)
    {
        var pixel = color.ToPixel<Rgba32>();
        return Color.FromRgba(
            (byte)(pixel.R * (1 - amount)),
            (byte)(pixel.G * (1 - amount)),
            (byte)(pixel.B * (1 - amount)),
            pixel.A);
    }

    private static Color LightenColor(Color color, float amount)
    {
        var pixel = color.ToPixel<Rgba32>();
        return Color.FromRgba(
            (byte)(pixel.R + (255 - pixel.R) * amount),
            (byte)(pixel.G + (255 - pixel.G) * amount),
            (byte)(pixel.B + (255 - pixel.B) * amount),
            pixel.A);
    }
}