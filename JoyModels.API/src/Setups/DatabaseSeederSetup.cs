using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
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
    private static readonly Random Random = new();

    private static readonly string[] AvatarColors =
    [
        "#3498db", "#e74c3c", "#2ecc71", "#9b59b6", "#f39c12", "#1abc9c"
    ];

    private static readonly string[] ModelColors =
    [
        "#1a1a2e", "#16213e", "#0f3460", "#1b262c", "#2d4059", "#222831",
        "#1e3163", "#0d1b2a", "#1b2838", "#171717", "#212529", "#343a40",
        "#2c3e50", "#34495e", "#1c2833", "#17202a", "#1a237e", "#0d47a1",
        "#01579b", "#006064", "#004d40", "#1b5e20", "#33691e", "#827717"
    ];

    private static readonly string[] FirstNames =
    [
        "Alex", "Jordan", "Taylor", "Morgan", "Casey", "Riley", "Quinn", "Avery", "Peyton", "Cameron",
        "Skyler", "Dakota", "Reese", "Finley", "Sage", "Rowan", "Emery", "Phoenix", "Harley", "Blake",
        "Drew", "Hayden", "Jamie", "Logan", "Parker", "Emma", "Liam", "Olivia", "Noah", "Ava",
        "Ethan", "Sophia", "Mason", "Isabella", "Lucas", "Mia", "Oliver", "Charlotte", "Elijah", "Amelia",
        "James", "Harper", "Benjamin", "Evelyn", "Henry", "Abigail", "Sebastian", "Emily", "Jack", "Elizabeth",
        "Aiden", "Sofia", "Owen", "Ella", "Samuel", "Scarlett", "Ryan", "Grace", "Nathan", "Chloe",
        "Caleb", "Victoria", "Christian", "Aria", "Dylan", "Lily", "Isaac", "Zoey", "Joshua", "Penelope",
        "Andrew", "Layla", "Daniel", "Nora", "Matthew", "Riley", "Joseph", "Zoe", "David", "Hannah",
        "Carter", "Hazel", "Luke", "Luna", "Gabriel", "Savannah", "Anthony", "Audrey", "Lincoln", "Brooklyn",
        "Jaxon", "Bella", "Asher", "Claire", "Christopher", "Skylar", "Ezra", "Lucy", "Theodore", "Paisley"
    ];

    private static readonly string[] LastNames =
    [
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Wilson", "Anderson", "Taylor", "Thomas", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White",
        "Harris", "Clark", "Lewis", "Robinson", "Walker", "Hall", "Allen", "Young", "King", "Wright",
        "Scott", "Green", "Baker", "Adams", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner",
        "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers",
        "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox",
        "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks", "Kelly",
        "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins", "Perry",
        "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler", "Simmons", "Foster", "Gonzales",
        "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes", "Myers", "Ford", "Hamilton", "Graham"
    ];

    private static readonly string[] ModelNamePrefixes =
    [
        "Ancient", "Modern", "Futuristic", "Vintage", "Classic", "Epic", "Mystic",
        "Crystal", "Shadow", "Golden", "Silver", "Dark", "Light", "Royal", "Noble"
    ];

    private static readonly string[] ModelNameNouns =
    [
        "Dragon", "Knight", "Castle", "Sword", "Shield", "Tower", "Temple",
        "Warrior", "Mage", "Throne", "Crown", "Armor", "Helmet", "Fortress",
        "Phoenix", "Griffin", "Unicorn", "Golem", "Titan", "Sphinx"
    ];

    private static readonly string[] ModelDescriptions =
    [
        "A highly detailed 3D model perfect for game development and visualization projects.",
        "Meticulously crafted with attention to every polygon and texture detail.",
        "Optimized for real-time rendering while maintaining visual fidelity.",
        "Created using industry-standard techniques for maximum compatibility.",
        "Features clean topology suitable for animation and rigging.",
        "Includes multiple LOD versions for performance optimization.",
        "Hand-sculpted details combined with procedural texturing.",
        "Perfect for architectural visualization and product rendering."
    ];

    private static readonly string[] FaqQuestions =
    [
        "What software was used to create this model?",
        "Is this model rigged for animation?",
        "What file formats are included in the download?",
        "Can I use this model for commercial projects?",
        "What is the polygon count of this model?",
        "Are textures included with this model?",
        "Is this model suitable for 3D printing?",
        "Can you provide a lower poly version?",
        "What rendering engine was used for the preview images?",
        "Does this model include UV mapping?",
        "Are there any known issues with the model?",
        "Can I request modifications to this model?",
        "What scale is this model created in?",
        "Is the model optimized for game engines?",
        "Do you offer support after purchase?"
    ];

    private static readonly string[] FaqAnswers =
    [
        "This model was created using Blender and ZBrush for high-detail sculpting.",
        "Yes, the model is fully rigged and ready for animation in most 3D software.",
        "The download includes OBJ, FBX, and GLTF formats for maximum compatibility.",
        "Yes, this model comes with a commercial license for use in any project.",
        "The model has approximately 50,000 polygons in the high-poly version.",
        "Yes, all textures are included in 4K resolution (diffuse, normal, roughness).",
        "The model can be 3D printed but may require some cleanup for best results.",
        "I can provide a lower poly version upon request, just send me a message.",
        "The preview images were rendered in Cycles with HDRI lighting.",
        "Yes, the model includes properly unwrapped UV maps for easy texturing.",
        "No known issues. If you find any problems, please let me know!",
        "Yes, I offer custom modifications for an additional fee. Contact me for details.",
        "The model is created at real-world scale (1 unit = 1 meter).",
        "Yes, it's optimized for Unity and Unreal Engine with LOD support.",
        "Absolutely! I provide support for any issues you might encounter.",
        "Thank you for your interest! Feel free to reach out with any questions.",
        "Great question! The answer depends on your specific use case.",
        "I appreciate your feedback and will consider this for future updates.",
        "Please check the product description for more detailed specifications.",
        "I'm happy to help! Let me know if you need anything else."
    ];

    private static readonly string[] PositiveReviewTexts =
    [
        "Excellent quality model! The details are amazing and it works perfectly in my project.",
        "Very impressed with this model. Clean topology and great textures included.",
        "Best purchase I've made. The model is exactly as described and easy to use.",
        "Fantastic work! The artist clearly put a lot of effort into this piece.",
        "Highly recommend this model. Perfect for game development projects.",
        "Outstanding quality for the price. Will definitely buy from this creator again.",
        "The model exceeded my expectations. Great attention to detail throughout.",
        "Perfect for my needs. The file formats provided made integration seamless.",
        "Amazing model with professional quality. The creator was also very helpful.",
        "Five stars! This model saved me hours of work on my project."
    ];

    private static readonly string[] NegativeReviewTexts =
    [
        "The model quality doesn't match the preview images. Disappointed with this purchase.",
        "Had some issues with the topology. Not ideal for animation purposes.",
        "The textures were lower resolution than expected. Needs improvement.",
        "Model required significant cleanup before I could use it in my project.",
        "Not worth the price. I've seen better models for less money.",
        "The file formats provided were outdated. Had compatibility issues.",
        "Description was misleading. The model lacks many advertised features.",
        "Polygon count was much higher than stated, causing performance issues.",
        "UV mapping needs work. Had to redo most of it myself.",
        "Customer support was slow to respond when I had questions."
    ];

    public static IApplicationBuilder RegisterDatabaseSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<JoyModelsDbContext>();
        var userImageSettings = scope.ServiceProvider.GetRequiredService<UserImageSettingsDetails>();
        var modelImageSettings = scope.ServiceProvider.GetRequiredService<ModelImageSettingsDetails>();
        var modelSettings = scope.ServiceProvider.GetRequiredService<ModelSettingsDetails>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<JoyModelsDbContext>>();

        SeedUsers(context, userImageSettings, logger).GetAwaiter().GetResult();
        SeedModels(context, modelImageSettings, modelSettings, logger).GetAwaiter().GetResult();
        SeedModelFaqSections(context, logger).GetAwaiter().GetResult();
        SeedUserModelLikes(context, logger).GetAwaiter().GetResult();
        SeedOrdersAndLibrary(context, logger).GetAwaiter().GetResult();
        SeedShoppingCart(context, logger).GetAwaiter().GetResult();
        SeedModelReviews(context, logger).GetAwaiter().GetResult();

        return app;
    }

    #region User Seeding

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

        logger.LogInformation("Starting user seeding...");

        var rootRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Root));
        var adminRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.Admin));
        var userRole = await context.UserRoles.FirstAsync(r => r.RoleName == nameof(UserRoleEnum.User));

        const string defaultPassword = "strinG1!";
        var users = new List<User>();

        for (var i = 0; i < 100; i++)
        {
            var userUuid = Guid.NewGuid();
            var colorIndex = i % AvatarColors.Length;
            var pictureFileName = $"avatar-{userUuid}.jpg";

            var role = i switch
            {
                < 5 => rootRole,
                < 10 => adminRole,
                _ => userRole
            };

            var firstName = FirstNames[i % FirstNames.Length];
            var lastName = LastNames[i % LastNames.Length];
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

            GenerateAndSaveAvatar(userUuid, pictureFileName, AvatarColors[colorIndex], userImageSettings);

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
            logger.LogError(ex, "User seeding failed. Rolling back transaction.");
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

    #endregion

    #region Model Seeding

    private static async Task SeedModels(
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

        var modelAssignments = DistributeModelsAmongCreators(adminUsers.Count, 100, 5, 20);
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
                var modelFilePath = Path.Combine(modelFolderPath, modelFileName);
                await File.WriteAllTextAsync(modelFilePath, minimalObjContent);

                var pictureFolderPath = Path.Combine(modelImageSettings.SavePath, "models", modelUuid.ToString());
                Directory.CreateDirectory(pictureFolderPath);

                var pictureCount = Random.Next(3, 6);
                var selectedImages = sharedImageFileNames.OrderBy(_ => Random.Next()).Take(pictureCount).ToList();

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
                    Description = ModelDescriptions[modelIndex % ModelDescriptions.Length],
                    Price = Math.Round((decimal)(Random.NextDouble() * 99 + 1), 2),
                    LocationPath = modelFilePath,
                    ModelAvailabilityUuid = publicAvailability.Uuid,
                    CreatedAt = DateTime.UtcNow.AddDays(-365 + modelIndex)
                };
                models.Add(model);

                var categoryCount = Random.Next(1, 4);
                var selectedCategories = categories.OrderBy(_ => Random.Next()).Take(categoryCount).ToList();
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
            logger.LogError(ex, "Model seeding failed. Rolling back transaction.");
            throw;
        }
    }

    private static string GenerateUniqueModelName(HashSet<string> usedNames, int index)
    {
        string name;
        var attempts = 0;

        do
        {
            var prefix = ModelNamePrefixes[Random.Next(ModelNamePrefixes.Length)];
            var noun = ModelNameNouns[Random.Next(ModelNameNouns.Length)];
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
                var colorIndex = i % ModelColors.Length;
                GenerateModelPreviewImage(filePath, ModelColors[colorIndex], i);
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
        var gridColor = LightenColor(bgColor, 0.15f);
        var wireframeColor = LightenColor(bgColor, 0.5f);
        var accentColor = Color.ParseHex(AvatarColors[seed % AvatarColors.Length]);

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
                    DrawWireframeCube(ctx, center, center, 150, wireframeColor, accentColor, seed);
                    break;
                case 1:
                    DrawWireframeSphere(ctx, center, center, 120, wireframeColor, accentColor);
                    break;
                case 2:
                    DrawWireframePyramid(ctx, center, center + 30, 180, wireframeColor, accentColor);
                    break;
                case 3:
                    DrawWireframeTorus(ctx, center, center, 100, 40, wireframeColor, accentColor);
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

    private static void DrawWireframeCube(IImageProcessingContext ctx, float cx, float cy, float cubeSize,
        Color lineColor, Color accentColor, int rotation)
    {
        var halfSize = cubeSize / 2;
        var offset = cubeSize * 0.3f;
        var rotOffset = (rotation % 10) * 5f;

        var frontTopLeft = new PointF(cx - halfSize + rotOffset, cy - halfSize);
        var frontTopRight = new PointF(cx + halfSize + rotOffset, cy - halfSize);
        var frontBottomLeft = new PointF(cx - halfSize, cy + halfSize * 0.6f);
        var frontBottomRight = new PointF(cx + halfSize, cy + halfSize * 0.6f);

        var backTopLeft = new PointF(cx - halfSize + offset + rotOffset, cy - halfSize - offset);
        var backTopRight = new PointF(cx + halfSize + offset + rotOffset, cy - halfSize - offset);
        var backBottomLeft = new PointF(cx - halfSize + offset, cy + halfSize * 0.6f - offset);
        var backBottomRight = new PointF(cx + halfSize + offset, cy + halfSize * 0.6f - offset);

        ctx.DrawLine(lineColor, 2f, frontTopLeft, frontTopRight);
        ctx.DrawLine(lineColor, 2f, frontTopRight, frontBottomRight);
        ctx.DrawLine(lineColor, 2f, frontBottomRight, frontBottomLeft);
        ctx.DrawLine(lineColor, 2f, frontBottomLeft, frontTopLeft);

        ctx.DrawLine(lineColor, 1.5f, backTopLeft, backTopRight);
        ctx.DrawLine(lineColor, 1.5f, backTopRight, backBottomRight);
        ctx.DrawLine(lineColor, 1.5f, backBottomRight, backBottomLeft);
        ctx.DrawLine(lineColor, 1.5f, backBottomLeft, backTopLeft);

        ctx.DrawLine(accentColor, 2f, frontTopLeft, backTopLeft);
        ctx.DrawLine(accentColor, 2f, frontTopRight, backTopRight);
        ctx.DrawLine(lineColor, 1.5f, frontBottomLeft, backBottomLeft);
        ctx.DrawLine(lineColor, 1.5f, frontBottomRight, backBottomRight);
    }

    private static void DrawWireframeSphere(IImageProcessingContext ctx, float cx, float cy, float radius,
        Color lineColor, Color accentColor)
    {
        ctx.Draw(accentColor, 2.5f, new EllipsePolygon(cx, cy, radius, radius));

        ctx.Draw(lineColor, 1.5f, new EllipsePolygon(cx, cy, radius, radius * 0.4f));

        ctx.Draw(lineColor, 1.5f, new EllipsePolygon(cx, cy, radius * 0.4f, radius));

        for (var i = 1; i < 4; i++)
        {
            var r = radius * (i / 4f);
            ctx.Draw(DarkenColor(lineColor, 0.2f * i), 1f, new EllipsePolygon(cx, cy, r, r * 0.4f));
        }
    }

    private static void DrawWireframePyramid(IImageProcessingContext ctx, float cx, float cy, float pyramidSize,
        Color lineColor, Color accentColor)
    {
        var halfBase = pyramidSize / 2;
        var height = pyramidSize * 0.8f;

        var apex = new PointF(cx, cy - height);
        var baseLeft = new PointF(cx - halfBase, cy);
        var baseRight = new PointF(cx + halfBase, cy);
        var baseBack = new PointF(cx, cy - halfBase * 0.5f);

        ctx.DrawLine(accentColor, 2.5f, apex, baseLeft);
        ctx.DrawLine(accentColor, 2.5f, apex, baseRight);
        ctx.DrawLine(lineColor, 2f, apex, baseBack);

        ctx.DrawLine(lineColor, 2f, baseLeft, baseRight);
        ctx.DrawLine(lineColor, 1.5f, baseRight, baseBack);
        ctx.DrawLine(lineColor, 1.5f, baseBack, baseLeft);
    }

    private static void DrawWireframeTorus(IImageProcessingContext ctx, float cx, float cy, float majorRadius,
        float minorRadius, Color lineColor, Color accentColor)
    {
        ctx.Draw(accentColor, 2.5f,
            new EllipsePolygon(cx, cy, majorRadius + minorRadius, (majorRadius + minorRadius) * 0.4f));
        ctx.Draw(lineColor, 2f,
            new EllipsePolygon(cx, cy, majorRadius - minorRadius, (majorRadius - minorRadius) * 0.4f));

        for (var i = 0; i < 8; i++)
        {
            var angle = i * MathF.PI / 4;
            var x = cx + majorRadius * MathF.Cos(angle);
            var y = cy + majorRadius * 0.4f * MathF.Sin(angle);
            ctx.Draw(lineColor, 1f, new EllipsePolygon(x, y, minorRadius * 0.3f, minorRadius));
        }
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

    #endregion

    #region ModelFaqSection Seeding

    private static async Task SeedModelFaqSections(JoyModelsDbContext context, ILogger logger)
    {
        var existingFaqCount = await context.ModelFaqSections.CountAsync();
        if (existingFaqCount > 0)
        {
            logger.LogInformation("Database already contains {Count} FAQ entries. Skipping FAQ seeding.",
                existingFaqCount);
            return;
        }

        logger.LogInformation("Starting ModelFaqSection seeding...");

        var models = await context.Models.ToListAsync();
        var allUsers = await context.Users.ToListAsync();

        var faqSections = new List<ModelFaqSection>();

        foreach (var model in models)
        {
            var modelOwner = allUsers.First(u => u.Uuid == model.UserUuid);
            var otherUsers = allUsers.Where(u => u.Uuid != model.UserUuid).ToList();

            var questionCount = Random.Next(5, 9);
            var selectedQuestions = FaqQuestions.OrderBy(_ => Random.Next()).Take(questionCount).ToList();

            foreach (var questionText in selectedQuestions)
            {
                var asker = otherUsers[Random.Next(otherUsers.Count)];
                var questionUuid = Guid.NewGuid();
                var questionDate = model.CreatedAt.AddDays(Random.Next(1, 30));

                var question = new ModelFaqSection
                {
                    Uuid = questionUuid,
                    ParentMessageUuid = null,
                    ModelUuid = model.Uuid,
                    UserUuid = asker.Uuid,
                    MessageText = questionText,
                    CreatedAt = questionDate
                };
                faqSections.Add(question);

                var answerCount = Random.Next(2, 4);
                var lastAnswerDate = questionDate;

                for (var i = 0; i < answerCount; i++)
                {
                    var isOwnerAnswer = i == 0 || Random.NextDouble() > 0.5;
                    var answerer = isOwnerAnswer ? modelOwner : otherUsers[Random.Next(otherUsers.Count)];

                    lastAnswerDate = lastAnswerDate.AddHours(Random.Next(1, 48));

                    var answer = new ModelFaqSection
                    {
                        Uuid = Guid.NewGuid(),
                        ParentMessageUuid = questionUuid,
                        ModelUuid = model.Uuid,
                        UserUuid = answerer.Uuid,
                        MessageText = FaqAnswers[Random.Next(FaqAnswers.Length)],
                        CreatedAt = lastAnswerDate
                    };
                    faqSections.Add(answer);
                }
            }

            logger.LogInformation("Created {Count} FAQ entries for model: {ModelName}",
                questionCount + faqSections.Count(f => f.ParentMessageUuid != null && f.ModelUuid == model.Uuid),
                model.Name);
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.ModelFaqSections.AddRangeAsync(faqSections);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            var totalQuestions = faqSections.Count(f => f.ParentMessageUuid == null);
            var totalAnswers = faqSections.Count(f => f.ParentMessageUuid != null);
            logger.LogInformation(
                "ModelFaqSection seeding completed. Created {Questions} questions and {Answers} answers.",
                totalQuestions, totalAnswers);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "ModelFaqSection seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region UserModelLikes Seeding

    private static async Task SeedUserModelLikes(JoyModelsDbContext context, ILogger logger)
    {
        var existingLikesCount = await context.UserModelLikes.CountAsync();
        if (existingLikesCount > 0)
        {
            logger.LogInformation("Database already contains {Count} likes. Skipping likes seeding.",
                existingLikesCount);
            return;
        }

        logger.LogInformation("Starting UserModelLikes seeding...");

        var allUsers = await context.Users.OrderBy(u => u.CreatedAt).ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var likes = new List<UserModelLike>();
        var likedPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = allUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < allUsers.Count; userIndex++)
        {
            var user = allUsers[userIndex];
            var userGroup = userIndex / usersPerGroup;
            if (userGroup >= groupCount) userGroup = groupCount - 1;

            for (var modelIndex = 0; modelIndex < models.Count; modelIndex++)
            {
                var model = models[modelIndex];
                if (model.UserUuid == user.Uuid)
                    continue;

                var modelGroup = modelIndex / modelsPerGroup;
                if (modelGroup >= groupCount) modelGroup = groupCount - 1;

                double likeChance;
                var groupDistance = Math.Abs(userGroup - modelGroup);

                likeChance = groupDistance switch
                {
                    0 => 0.8,
                    1 => 0.4,
                    2 => 0.15,
                    _ => 0.05
                };

                if (Random.NextDouble() < likeChance)
                {
                    if (likedPairs.Add((user.Uuid, model.Uuid)))
                    {
                        likes.Add(new UserModelLike
                        {
                            Uuid = Guid.NewGuid(),
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid
                        });
                    }
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.UserModelLikes.AddRangeAsync(likes);
            await context.SaveChangesAsync();

            foreach (var userGroup in likes.GroupBy(l => l.UserUuid))
            {
                var user = allUsers.First(u => u.Uuid == userGroup.Key);
                user.UserLikedModelsCount += userGroup.Count();
            }

            context.Users.UpdateRange(allUsers);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("UserModelLikes seeding completed. Created {Count} likes.", likes.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "UserModelLikes seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region ShoppingCart Seeding

    private static async Task SeedShoppingCart(JoyModelsDbContext context, ILogger logger)
    {
        var existingCartCount = await context.ShoppingCartItems.CountAsync();
        if (existingCartCount > 0)
        {
            logger.LogInformation("Database already contains {Count} cart items. Skipping cart seeding.",
                existingCartCount);
            return;
        }

        logger.LogInformation("Starting ShoppingCart seeding...");

        var allUsers = await context.Users.OrderBy(u => u.CreatedAt).ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var ownedModels = await context.Libraries
            .Select(l => new { l.UserUuid, l.ModelUuid })
            .ToListAsync();
        var ownedSet = ownedModels.ToHashSet();

        var cartItems = new List<ShoppingCart>();
        var cartPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = allUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < allUsers.Count; userIndex++)
        {
            var user = allUsers[userIndex];
            var userGroup = userIndex / usersPerGroup;
            if (userGroup >= groupCount) userGroup = groupCount - 1;

            for (var modelIndex = 0; modelIndex < models.Count; modelIndex++)
            {
                var model = models[modelIndex];
                if (model.UserUuid == user.Uuid)
                    continue;

                if (ownedSet.Contains(new { UserUuid = user.Uuid, ModelUuid = model.Uuid }))
                    continue;

                var modelGroup = modelIndex / modelsPerGroup;
                if (modelGroup >= groupCount) modelGroup = groupCount - 1;

                var groupDistance = Math.Abs(userGroup - modelGroup);

                double cartChance = groupDistance switch
                {
                    0 => 0.08,
                    1 => 0.03,
                    _ => 0.005
                };

                if (Random.NextDouble() < cartChance)
                {
                    if (cartPairs.Add((user.Uuid, model.Uuid)))
                    {
                        cartItems.Add(new ShoppingCart
                        {
                            Uuid = Guid.NewGuid(),
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 30))
                        });
                    }
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.ShoppingCartItems.AddRangeAsync(cartItems);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("ShoppingCart seeding completed. Created {Count} cart items.", cartItems.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "ShoppingCart seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region Orders and Library Seeding

    private static async Task SeedOrdersAndLibrary(JoyModelsDbContext context, ILogger logger)
    {
        var existingOrdersCount = await context.Orders.CountAsync();
        if (existingOrdersCount > 0)
        {
            logger.LogInformation("Database already contains {Count} orders. Skipping orders seeding.",
                existingOrdersCount);
            return;
        }

        logger.LogInformation("Starting Orders and Library seeding...");

        var allUsers = await context.Users.OrderBy(u => u.CreatedAt).ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var orders = new List<Order>();
        var libraryEntries = new List<Library>();
        var purchasedPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = allUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < allUsers.Count; userIndex++)
        {
            var user = allUsers[userIndex];
            var userGroup = userIndex / usersPerGroup;
            if (userGroup >= groupCount) userGroup = groupCount - 1;

            for (var modelIndex = 0; modelIndex < models.Count; modelIndex++)
            {
                var model = models[modelIndex];
                if (model.UserUuid == user.Uuid)
                    continue;

                var modelGroup = modelIndex / modelsPerGroup;
                if (modelGroup >= groupCount) modelGroup = groupCount - 1;

                var groupDistance = Math.Abs(userGroup - modelGroup);

                double purchaseChance = groupDistance switch
                {
                    0 => 0.25,
                    1 => 0.10,
                    _ => 0.02
                };

                if (Random.NextDouble() < purchaseChance)
                {
                    if (purchasedPairs.Add((user.Uuid, model.Uuid)))
                    {
                        var orderUuid = Guid.NewGuid();
                        var purchaseDate = DateTime.UtcNow.AddDays(-Random.Next(1, 180));

                        orders.Add(new Order
                        {
                            Uuid = orderUuid,
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            Amount = model.Price,
                            Status = nameof(OrderStatus.Completed),
                            StripePaymentIntentId = $"seed_pi_{Guid.NewGuid()}",
                            CreatedAt = purchaseDate,
                            UpdatedAt = purchaseDate
                        });

                        libraryEntries.Add(new Library
                        {
                            Uuid = Guid.NewGuid(),
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            OrderUuid = orderUuid,
                            AcquiredAt = purchaseDate
                        });
                    }
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            await context.Libraries.AddRangeAsync(libraryEntries);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation(
                "Orders and Library seeding completed. Created {Orders} orders and {Libraries} library entries.",
                orders.Count, libraryEntries.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Orders and Library seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region ModelReviews Seeding

    private static async Task SeedModelReviews(JoyModelsDbContext context, ILogger logger)
    {
        var existingReviewsCount = await context.ModelReviews.CountAsync();
        if (existingReviewsCount > 0)
        {
            logger.LogInformation("Database already contains {Count} reviews. Skipping reviews seeding.",
                existingReviewsCount);
            return;
        }

        logger.LogInformation("Starting ModelReviews seeding...");

        var libraryEntries = await context.Libraries
            .Include(l => l.User)
            .Include(l => l.Model)
            .ToListAsync();

        var positiveReviewType = await context.ModelReviewTypes
            .FirstAsync(r => r.ReviewName == nameof(ModelReviewEnum.Positive));
        var negativeReviewType = await context.ModelReviewTypes
            .FirstAsync(r => r.ReviewName == nameof(ModelReviewEnum.Negative));

        var reviews = new List<ModelReview>();
        var reviewedPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        foreach (var library in libraryEntries)
        {
            if (library.Model.UserUuid == library.UserUuid)
                continue;

            if (reviewedPairs.Contains((library.UserUuid, library.ModelUuid)))
                continue;

            if (Random.NextDouble() > 0.75)
                continue;

            var isPositive = Random.NextDouble() > 0.2;
            var reviewType = isPositive ? positiveReviewType : negativeReviewType;
            var reviewTexts = isPositive ? PositiveReviewTexts : NegativeReviewTexts;

            var review = new ModelReview
            {
                Uuid = Guid.NewGuid(),
                ModelUuid = library.ModelUuid,
                UserUuid = library.UserUuid,
                ReviewTypeUuid = reviewType.Uuid,
                ReviewText = reviewTexts[Random.Next(reviewTexts.Length)],
                CreatedAt = library.AcquiredAt.AddDays(Random.Next(1, 30))
            };
            reviews.Add(review);
            reviewedPairs.Add((library.UserUuid, library.ModelUuid));
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.ModelReviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            var positiveCount = reviews.Count(r => r.ReviewTypeUuid == positiveReviewType.Uuid);
            var negativeCount = reviews.Count(r => r.ReviewTypeUuid == negativeReviewType.Uuid);
            logger.LogInformation(
                "ModelReviews seeding completed. Created {Total} reviews ({Positive} positive, {Negative} negative).",
                reviews.Count, positiveCount, negativeCount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "ModelReviews seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region Distribution Utilities

    private static List<int> DistributeModelsAmongCreators(int creatorCount, int totalModels, int minPerCreator,
        int maxPerCreator)
    {
        var assignments = new List<int>();

        for (var i = 0; i < creatorCount; i++)
            assignments.Add(minPerCreator);

        var remaining = totalModels - (creatorCount * minPerCreator);

        while (remaining > 0)
        {
            var creatorIndex = Random.Next(creatorCount);

            if (assignments[creatorIndex] < maxPerCreator)
            {
                assignments[creatorIndex]++;
                remaining--;
            }
        }

        return assignments;
    }

    #endregion

    #region Color Utilities

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

    #endregion
}