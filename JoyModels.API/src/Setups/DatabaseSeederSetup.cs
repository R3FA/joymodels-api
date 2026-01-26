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

        logger.LogInformation("Generating 60 shared model preview images...");
        var sharedImageFileNames = GenerateSharedModelImages(modelImageSettings, 60);

        var minimalObjContent = GenerateMinimalObjFile();

        var models = new List<Model>();
        var modelCategories = new List<ModelCategory>();
        var modelPictures = new List<ModelPicture>();
        var usedNames = new HashSet<string>();

        for (var i = 0; i < 60; i++)
        {
            var modelUuid = Guid.NewGuid();
            var creator = adminUsers[i % adminUsers.Count];

            var modelName = GenerateUniqueModelName(usedNames, i);
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
                Description = ModelDescriptions[i % ModelDescriptions.Length],
                Price = Math.Round((decimal)(Random.NextDouble() * 99 + 1), 2),
                LocationPath = modelFilePath,
                ModelAvailabilityUuid = publicAvailability.Uuid,
                CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 365))
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

            logger.LogInformation("Created model: {ModelName} by {Creator}", modelName, creator.NickName);
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