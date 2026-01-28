using JoyModels.API.Setups.DatabaseSeed.Helpers;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class ModelSeeder
{
    public static async Task SeedModels(
        JoyModelsDbContext context,
        ModelImageSettingsDetails modelImageSettings,
        ModelSettingsDetails modelSettings,
        ILogger logger)
    {
        var existingModelsCount = await context.Models.CountAsync();
        if (existingModelsCount > 0)
        {
            logger.LogInformation("Database already contains {Count} models. Skipping model seeding.",
                existingModelsCount);
            return;
        }

        logger.LogInformation("Starting model seeding...");

        var adminUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.Root)
                        || u.UserRoleUu.RoleName == nameof(UserRoleEnum.Admin))
            .ToListAsync();

        var publicAvailability =
            await context.ModelAvailabilities.FirstAsync(a =>
                a.AvailabilityName == nameof(ModelAvailabilityEnum.Public));
        var categories = await context.Categories.ToListAsync();

        logger.LogInformation("Generating 100 shared model preview images...");
        var sharedImageFileNames = GenerateSharedModelImages(modelImageSettings, 100);

        var minimalObjContent = GenerateMinimalObjFile();

        var models = new List<Model>();
        var modelCategories = new List<ModelCategory>();
        var modelPictures = new List<ModelPicture>();
        var usedNames = new HashSet<string>();
        var createdFolders = new List<string>();

        var modelAssignments = DistributionUtilities.DistributeModelsAmongCreators(adminUsers.Count, 100, 5, 20);
        var modelIndex = 0;

        for (var creatorIndex = 0; creatorIndex < adminUsers.Count; creatorIndex++)
        {
            var creator = adminUsers[creatorIndex];
            var modelsForThisCreator = modelAssignments[creatorIndex];

            logger.LogInformation("Creator {Nickname} will create {Count} models.", creator.NickName,
                modelsForThisCreator);

            for (var j = 0; j < modelsForThisCreator; j++)
            {
                var modelUuid = Guid.NewGuid();

                var modelName = GenerateUniqueModelName(usedNames, modelIndex);
                usedNames.Add(modelName);

                var modelFileName = $"model-{Guid.NewGuid()}.obj";
                var modelFolderPath = Path.Combine(modelSettings.SavePath, "models", modelUuid.ToString());
                Directory.CreateDirectory(modelFolderPath);
                createdFolders.Add(modelFolderPath);
                var modelFilePath = Path.Combine(modelFolderPath, modelFileName);
                await File.WriteAllTextAsync(modelFilePath, minimalObjContent);

                var pictureFolderPath = Path.Combine(modelImageSettings.SavePath, "models", modelUuid.ToString());
                Directory.CreateDirectory(pictureFolderPath);
                createdFolders.Add(pictureFolderPath);

                var pictureCount = SeedDataConstants.Random.Next(3, 6);
                var selectedImages = sharedImageFileNames.OrderBy(_ => SeedDataConstants.Random.Next())
                    .Take(pictureCount).ToList();

                foreach (var sourceImageName in selectedImages)
                {
                    var sourcePath = Path.Combine(modelImageSettings.SavePath, "seed-images", sourceImageName);
                    var destFileName = $"model-picture-{Guid.NewGuid()}.jpg";
                    var destPath = Path.Combine(pictureFolderPath, destFileName);
                    File.Copy(sourcePath, destPath);

                    modelPictures.Add(new ModelPicture
                    {
                        Uuid = Guid.NewGuid(),
                        ModelUuid = modelUuid,
                        PictureLocation = destFileName,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                var model = new Model
                {
                    Uuid = modelUuid,
                    Name = modelName,
                    UserUuid = creator.Uuid,
                    Description =
                        SeedDataConstants.ModelDescriptions[modelIndex % SeedDataConstants.ModelDescriptions.Length],
                    Price = Math.Round((decimal)(SeedDataConstants.Random.NextDouble() * 99 + 1), 2),
                    LocationPath = modelFilePath,
                    ModelAvailabilityUuid = publicAvailability.Uuid,
                    CreatedAt = DateTime.UtcNow.AddDays(-365 + modelIndex)
                };
                models.Add(model);

                var categoryCount = SeedDataConstants.Random.Next(1, 4);
                var selectedCategories = categories.OrderBy(_ => SeedDataConstants.Random.Next()).Take(categoryCount)
                    .ToList();
                foreach (var category in selectedCategories)
                {
                    modelCategories.Add(new ModelCategory
                    {
                        Uuid = Guid.NewGuid(),
                        ModelUuid = modelUuid,
                        CategoryUuid = category.Uuid
                    });
                }

                creator.UserModelsCount++;
                modelIndex++;

                logger.LogInformation("Created model: {ModelName} by {Creator}", modelName, creator.NickName);
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.Models.AddRangeAsync(models);
            await context.SaveChangesAsync();

            await context.ModelCategories.AddRangeAsync(modelCategories);
            await context.SaveChangesAsync();

            await context.ModelPictures.AddRangeAsync(modelPictures);
            await context.SaveChangesAsync();

            context.Users.UpdateRange(adminUsers);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("Model seeding completed. Created {Count} models.", models.Count);

            var seedImagesFolder = Path.Combine(modelImageSettings.SavePath, "seed-images");
            if (Directory.Exists(seedImagesFolder))
            {
                Directory.Delete(seedImagesFolder, true);
                logger.LogInformation("Cleaned up seed-images folder.");
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

            var seedImagesFolder = Path.Combine(modelImageSettings.SavePath, "seed-images");
            if (Directory.Exists(seedImagesFolder))
                Directory.Delete(seedImagesFolder, true);

            logger.LogError(ex, "Model seeding failed. Rolling back transaction and cleaning up folders.");
            throw;
        }
    }

    private static string GenerateUniqueModelName(HashSet<string> usedNames, int index)
    {
        string name;
        var attempts = 0;

        do
        {
            var prefix =
                SeedDataConstants.ModelNamePrefixes[
                    SeedDataConstants.Random.Next(SeedDataConstants.ModelNamePrefixes.Length)];
            var noun = SeedDataConstants.ModelNameNouns[
                SeedDataConstants.Random.Next(SeedDataConstants.ModelNameNouns.Length)];
            var suffix = attempts > 0 ? $" {index + 1}" : "";
            name = $"{prefix} {noun}{suffix}";
            attempts++;
        } while (usedNames.Contains(name) && attempts < 100);

        return name;
    }

    private static List<string> GenerateSharedModelImages(ModelImageSettingsDetails settings, int count)
    {
        var seedImagesFolder = Path.Combine(settings.SavePath, "seed-images");
        Directory.CreateDirectory(seedImagesFolder);

        var fileNames = new List<string>();

        for (var i = 0; i < count; i++)
        {
            var fileName = $"seed-model-{i + 1}.jpg";
            var filePath = Path.Combine(seedImagesFolder, fileName);

            if (!File.Exists(filePath))
            {
                var colorIndex = i % SeedDataConstants.ModelColors.Length;
                GenerateModelPreviewImage(filePath, SeedDataConstants.ModelColors[colorIndex], i);
            }

            fileNames.Add(fileName);
        }

        return fileNames;
    }

    private static void GenerateModelPreviewImage(string filePath, string bgColorHex, int seed)
    {
        const int size = 512;
        const float center = size / 2f;

        var bgColor = Color.ParseHex(bgColorHex);
        var gridColor = ColorUtilities.LightenColor(bgColor, 0.15f);
        var wireframeColor = ColorUtilities.LightenColor(bgColor, 0.5f);
        var accentColor = Color.ParseHex(SeedDataConstants.AvatarColors[seed % SeedDataConstants.AvatarColors.Length]);

        using var image = new Image<Rgba32>(size, size);

        image.Mutate(ctx =>
        {
            ctx.BackgroundColor(bgColor);

            for (var j = 0; j < size; j += 32)
            {
                ctx.DrawLine(gridColor, 1f, new PointF(j, 0), new PointF(j, size));
                ctx.DrawLine(gridColor, 1f, new PointF(0, j), new PointF(size, j));
            }

            var shapeType = seed % 4;

            switch (shapeType)
            {
                case 0:
                    ImageGenerators.DrawWireframeCube(ctx, center, center, 150, wireframeColor, accentColor, seed);
                    break;
                case 1:
                    ImageGenerators.DrawWireframeSphere(ctx, center, center, 120, wireframeColor, accentColor);
                    break;
                case 2:
                    ImageGenerators.DrawWireframePyramid(ctx, center, center + 30, 180, wireframeColor, accentColor);
                    break;
                case 3:
                    ImageGenerators.DrawWireframeTorus(ctx, center, center, 100, 40, wireframeColor, accentColor);
                    break;
            }

            var highlightGradient = new RadialGradientBrush(
                new PointF(size * 0.3f, size * 0.3f),
                size * 0.8f,
                GradientRepetitionMode.None,
                new ColorStop(0, Color.FromRgba(255, 255, 255, 30)),
                new ColorStop(1, Color.FromRgba(255, 255, 255, 0)));
            ctx.Fill(highlightGradient, new EllipsePolygon(size * 0.3f, size * 0.3f, size * 0.5f));
        });

        image.SaveAsJpeg(filePath);
    }

    private static string GenerateMinimalObjFile()
    {
        return """
               # Minimal OBJ file - Simple Cube
               # Generated by JoyModels Seeder

               # Vertices
               v -1.0 -1.0  1.0
               v  1.0 -1.0  1.0
               v  1.0  1.0  1.0
               v -1.0  1.0  1.0
               v -1.0 -1.0 -1.0
               v  1.0 -1.0 -1.0
               v  1.0  1.0 -1.0
               v -1.0  1.0 -1.0

               # Texture coordinates
               vt 0.0 0.0
               vt 1.0 0.0
               vt 1.0 1.0
               vt 0.0 1.0

               # Normals
               vn  0.0  0.0  1.0
               vn  0.0  0.0 -1.0
               vn  0.0  1.0  0.0
               vn  0.0 -1.0  0.0
               vn  1.0  0.0  0.0
               vn -1.0  0.0  0.0

               # Faces
               f 1/1/1 2/2/1 3/3/1 4/4/1
               f 5/1/2 8/2/2 7/3/2 6/4/2
               f 4/1/3 3/2/3 7/3/3 8/4/3
               f 1/1/4 5/2/4 6/3/4 2/4/4
               f 2/1/5 6/2/5 7/3/5 3/4/5
               f 1/1/6 4/2/6 8/3/6 5/4/6
               """;
    }
}