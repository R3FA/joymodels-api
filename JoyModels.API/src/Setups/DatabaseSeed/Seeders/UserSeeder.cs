using JoyModels.API.Setups.DatabaseSeed.Helpers;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.Sso.HelperMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class UserSeeder
{
    public static async Task SeedUsers(
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

        logger.LogInformation("Starting user seeding...");

        var rootRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Root));
        var adminRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Admin));
        var userRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.User));

        const string defaultPassword = "strinG1!";
        var users = new List<User>();
        var createdFolders = new List<string>();

        foreach (var (uuid, email, nickname, firstName, lastName, roleName) in SeedDataConstants.FixedUsers)
        {
            var role = roleName switch
            {
                nameof(UserRoleEnum.Root) => rootRole,
                nameof(UserRoleEnum.Admin) => adminRole,
                _ => userRole
            };

            var colorIndex = users.Count % SeedDataConstants.AvatarColors.Length;
            var pictureFileName = $"avatar-{uuid}.jpg";

            var user = new User
            {
                Uuid = uuid,
                Email = email,
                NickName = nickname,
                FirstName = firstName,
                LastName = lastName,
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

            var userFolder = Path.Combine(userImageSettings.SavePath, "users", uuid.ToString());
            createdFolders.Add(userFolder);
            GenerateAndSaveAvatar(uuid, pictureFileName, SeedDataConstants.AvatarColors[colorIndex], userImageSettings);

            users.Add(user);

            logger.LogInformation("Created fixed user: {Nickname} ({Role})", user.NickName, role.RoleName);
        }

        for (var i = 0; i < 92; i++)
        {
            var userUuid = Guid.NewGuid();
            var colorIndex = (i + SeedDataConstants.FixedUsers.Count) % SeedDataConstants.AvatarColors.Length;
            var pictureFileName = $"avatar-{userUuid}.jpg";

            var role = i switch
            {
                < 3 => rootRole,
                < 6 => adminRole,
                _ => userRole
            };

            var firstName = SeedDataConstants.FirstNames[i % SeedDataConstants.FirstNames.Length];
            var lastName = SeedDataConstants.LastNames[i % SeedDataConstants.LastNames.Length];
            var nickname = $"{firstName.ToLower()}{lastName.ToLower()[0]}{i + 1}";

            var user = new User
            {
                Uuid = userUuid,
                Email = $"{nickname}@joymodels.com",
                NickName = nickname,
                FirstName = firstName,
                LastName = lastName,
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

            var userFolder = Path.Combine(userImageSettings.SavePath, "users", userUuid.ToString());
            createdFolders.Add(userFolder);
            GenerateAndSaveAvatar(userUuid, pictureFileName, SeedDataConstants.AvatarColors[colorIndex],
                userImageSettings);

            users.Add(user);

            logger.LogInformation("Created user: {Nickname} ({Role})", user.NickName, role.RoleName);
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("User seeding completed. Created {Count} users.", users.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            foreach (var folder in createdFolders)
            {
                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);
            }

            logger.LogError(ex, "User seeding failed. Rolling back transaction and cleaning up folders.");
            throw;
        }
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
        const int center = size / 2;

        var userFolder = Path.Combine(userImageSettings.SavePath, "users", userUuid.ToString());
        Directory.CreateDirectory(userFolder);

        var filePath = Path.Combine(userFolder, fileName);

        var backgroundColor = Color.ParseHex(hexColor);
        var darkerShade = ColorUtilities.DarkenColor(backgroundColor, 0.2f);
        var lighterShade = ColorUtilities.LightenColor(backgroundColor, 0.3f);

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
}