using JoyModels.API.Setups.DatabaseSeed.Helpers;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class CommunityPostSeeder
{
    public static async Task SeedCommunityPosts(
        JoyModelsDbContext context,
        ModelImageSettingsDetails modelImageSettings,
        ILogger logger)
    {
        var existingPostsCount = await context.CommunityPosts.CountAsync();
        if (existingPostsCount > 0)
        {
            logger.LogInformation("Database already contains {Count} community posts. Skipping community post seeding.",
                existingPostsCount);
            return;
        }

        logger.LogInformation("Starting CommunityPost seeding...");

        var allUsers = await context.Users.ToListAsync();
        var postTypes = await context.CommunityPostTypes.ToListAsync();

        logger.LogInformation("Generating 50 shared community post images...");
        var sharedImageFileNames = GenerateSharedCommunityPostImages(modelImageSettings, 50);

        var posts = new List<CommunityPost>();
        var postPictures = new List<CommunityPostPicture>();
        var usedTitles = new HashSet<string>();
        var createdFolders = new List<string>();

        const int totalPosts = 80;
        var youtubePostCount = (int)(totalPosts * 0.4);

        for (var i = 0; i < totalPosts; i++)
        {
            var postUuid = Guid.NewGuid();
            var creator = allUsers[SeedDataConstants.Random.Next(allUsers.Count)];
            var postType = postTypes[SeedDataConstants.Random.Next(postTypes.Count)];

            var title = GetUniqueCommunityPostTitle(usedTitles, i);
            usedTitles.Add(title);

            string? youtubeLink = null;
            if (i < youtubePostCount)
            {
                youtubeLink = SeedDataConstants.YoutubeVideoLinks[i % SeedDataConstants.YoutubeVideoLinks.Length];
            }

            var post = new CommunityPost
            {
                Uuid = postUuid,
                UserUuid = creator.Uuid,
                Title = title,
                Description =
                    SeedDataConstants.CommunityPostDescriptions[i % SeedDataConstants.CommunityPostDescriptions.Length],
                PostTypeUuid = postType.Uuid,
                YoutubeVideoLink = youtubeLink,
                CreatedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 180)),
                CommunityPostLikes = 0,
                CommunityPostDislikes = 0,
                CommunityPostCommentCount = 0
            };
            posts.Add(post);

            var pictureFolderPath = Path.Combine(modelImageSettings.SavePath, "community-posts", postUuid.ToString());
            Directory.CreateDirectory(pictureFolderPath);
            createdFolders.Add(pictureFolderPath);

            var pictureCount = SeedDataConstants.Random.Next(0, 5);
            var selectedImages = sharedImageFileNames.OrderBy(_ => SeedDataConstants.Random.Next()).Take(pictureCount)
                .ToList();

            foreach (var sourceImageName in selectedImages)
            {
                var sourcePath = Path.Combine(modelImageSettings.SavePath, "community-post-seed-images",
                    sourceImageName);
                var destFileName = $"community-post-picture-{Guid.NewGuid()}.jpg";
                var destPath = Path.Combine(pictureFolderPath, destFileName);
                File.Copy(sourcePath, destPath);

                postPictures.Add(new CommunityPostPicture
                {
                    Uuid = Guid.NewGuid(),
                    CommunityPostUuid = postUuid,
                    PictureLocation = destFileName,
                    CreatedAt = DateTime.UtcNow
                });
            }

            logger.LogInformation("Created community post: {Title} by {Creator}", title, creator.NickName);
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.CommunityPosts.AddRangeAsync(posts);
            await context.SaveChangesAsync();

            await context.CommunityPostPictures.AddRangeAsync(postPictures);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("CommunityPost seeding completed. Created {Posts} posts and {Pictures} pictures.",
                posts.Count, postPictures.Count);

            var seedImagesFolder = Path.Combine(modelImageSettings.SavePath, "community-post-seed-images");
            if (Directory.Exists(seedImagesFolder))
            {
                Directory.Delete(seedImagesFolder, true);
                logger.LogInformation("Cleaned up community-post-seed-images folder.");
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            foreach (var folder in createdFolders)
            {
                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);
            }

            var seedImagesFolder = Path.Combine(modelImageSettings.SavePath, "community-post-seed-images");
            if (Directory.Exists(seedImagesFolder))
                Directory.Delete(seedImagesFolder, true);

            logger.LogError(ex, "CommunityPost seeding failed. Rolling back transaction and cleaning up folders.");
            throw;
        }
    }

    private static string GetUniqueCommunityPostTitle(HashSet<string> usedTitles, int index)
    {
        var baseTitle = SeedDataConstants.CommunityPostTitles[index % SeedDataConstants.CommunityPostTitles.Length];
        if (!usedTitles.Contains(baseTitle))
            return baseTitle;

        var suffix = 2;
        while (usedTitles.Contains($"{baseTitle} #{suffix}"))
            suffix++;

        return $"{baseTitle} #{suffix}";
    }

    private static List<string> GenerateSharedCommunityPostImages(ModelImageSettingsDetails settings, int count)
    {
        var seedImagesFolder = Path.Combine(settings.SavePath, "community-post-seed-images");
        Directory.CreateDirectory(seedImagesFolder);

        var fileNames = new List<string>();

        for (var i = 0; i < count; i++)
        {
            var fileName = $"seed-community-{i + 1}.jpg";
            var filePath = Path.Combine(seedImagesFolder, fileName);

            if (!File.Exists(filePath))
            {
                var colorIndex = i % SeedDataConstants.CommunityPostColors.Length;
                GenerateCommunityPostImage(filePath, SeedDataConstants.CommunityPostColors[colorIndex], i);
            }

            fileNames.Add(fileName);
        }

        return fileNames;
    }

    private static void GenerateCommunityPostImage(string filePath, string bgColorHex, int seed)
    {
        const int size = 512;
        const float center = size / 2f;

        var bgColor = Color.ParseHex(bgColorHex);
        var gridColor = ColorUtilities.LightenColor(bgColor, 0.1f);
        var primaryColor = ColorUtilities.LightenColor(bgColor, 0.4f);
        var accentColor = Color.ParseHex(SeedDataConstants.AvatarColors[seed % SeedDataConstants.AvatarColors.Length]);

        using var image = new Image<Rgba32>(size, size);

        image.Mutate(ctx =>
        {
            ctx.BackgroundColor(bgColor);

            for (var j = 0; j < size; j += 40)
            {
                ctx.DrawLine(gridColor, 0.5f, new PointF(j, 0), new PointF(j, size));
                ctx.DrawLine(gridColor, 0.5f, new PointF(0, j), new PointF(size, j));
            }

            var iconType = seed % 5;

            switch (iconType)
            {
                case 0:
                    ImageGenerators.DrawGameController(ctx, center, center, 100, primaryColor, accentColor);
                    break;
                case 1:
                    ImageGenerators.DrawCodeBrackets(ctx, center, center, 120, primaryColor, accentColor);
                    break;
                case 2:
                    ImageGenerators.DrawPixelHeart(ctx, center, center, 80, primaryColor, accentColor);
                    break;
                case 3:
                    ImageGenerators.DrawLightbulb(ctx, center, center, 100, primaryColor, accentColor);
                    break;
                case 4:
                    ImageGenerators.DrawPencil(ctx, center, center, 120, primaryColor, accentColor);
                    break;
            }

            var highlightGradient = new RadialGradientBrush(
                new PointF(size * 0.25f, size * 0.25f),
                size * 0.6f,
                GradientRepetitionMode.None,
                new ColorStop(0, Color.FromRgba(255, 255, 255, 20)),
                new ColorStop(1, Color.FromRgba(255, 255, 255, 0)));
            ctx.Fill(highlightGradient, new EllipsePolygon(size * 0.25f, size * 0.25f, size * 0.4f));
        });

        image.SaveAsJpeg(filePath);
    }
}