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

    private static readonly
        List<(Guid Uuid, string Email, string Nickname, string FirstName, string LastName, string Role)> FixedUsers =
        [
            (Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), "root1@joymodels.com", "root1", "Root", "One",
                nameof(UserRoleEnum.Root)),
            (Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"), "root2@joymodels.com", "root2", "Root", "Two",
                nameof(UserRoleEnum.Root)),
            (Guid.Parse("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f"), "admin1@joymodels.com", "admin1", "Admin", "One",
                nameof(UserRoleEnum.Admin)),
            (Guid.Parse("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8a"), "admin2@joymodels.com", "admin2", "Admin", "Two",
                nameof(UserRoleEnum.Admin)),
            (Guid.Parse("e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8a9b"), "user1@joymodels.com", "user1", "User", "One",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("f6a7b8c9-d0e1-4f2a-3b4c-5d6e7f8a9b0c"), "user2@joymodels.com", "user2", "User", "Two",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("a7b8c9d0-e1f2-4a3b-4c5d-6e7f8a9b0c1d"), "user3@joymodels.com", "user3", "User", "Three",
                nameof(UserRoleEnum.User)),
            (Guid.Parse("b8c9d0e1-f2a3-4b4c-5d6e-7f8a9b0c1d2e"), "user4@joymodels.com", "user4", "User", "Four",
                nameof(UserRoleEnum.User))
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

    private static readonly string[] CommunityPostColors =
    [
        "#6B2D5C", "#4A1942", "#2D132C", "#801336", "#C72C41",
        "#0B3D91", "#1E5128", "#4E3524", "#2C3E50", "#1A1A2E",
        "#FF6B35", "#004E89", "#7B2D26", "#3D5A80", "#293241"
    ];

    private static readonly string[] CommunityPostTitles =
    [
        "My first game dev project - feedback welcome!",
        "Tutorial: Creating realistic water shaders",
        "Showcase: Low-poly character I made this week",
        "Tips for optimizing Unity projects",
        "How I improved my Blender workflow",
        "Question about PBR texturing",
        "My indie game progress update",
        "Best practices for game UI design",
        "Procedural generation techniques I learned",
        "Retro pixel art style guide",
        "Animation tips for beginners",
        "Level design principles that work",
        "My journey learning Unreal Engine",
        "Shader Graph tutorial for beginners",
        "Game jam experience and lessons learned",
        "How to create stylized environments",
        "VFX breakdown of my latest effect",
        "Character rigging workflow tips",
        "Sound design basics for games",
        "Post-processing effects guide",
        "Mobile game optimization tricks",
        "Creating immersive game worlds",
        "Substance Painter workflow tips",
        "Making modular game assets",
        "AI in games: My experiments"
    ];

    private static readonly string[] CommunityPostDescriptions =
    [
        "I've been working on this project for the past few months and would love to get some feedback from the community. Any suggestions for improvement are welcome!",
        "In this guide, I'll walk you through the process step by step. I've learned a lot from this community and wanted to give back by sharing what I know.",
        "Here's something I've been working on lately. It took me about a week to complete and I'm pretty happy with how it turned out. Let me know what you think!",
        "After struggling with this for a while, I finally figured out a workflow that works for me. Sharing it here in case it helps someone else.",
        "I've compiled some tips and tricks that have significantly improved my productivity. Hope this helps other developers in their journey!",
        "Been experimenting with different techniques and wanted to share my results. The difference in quality is quite noticeable.",
        "This is my progress so far on my indie game project. Still a lot of work to do but I'm excited about the direction it's going.",
        "I've gathered some resources and techniques that have helped me improve significantly. Sharing them with the community!",
        "After watching countless tutorials and practicing for months, here's what I've learned. These tips really made a difference for me.",
        "Wanted to document my learning journey and share it with others who might be starting out. We all start somewhere!",
        "Here's a breakdown of my creative process for this project. I hope it gives some insight into how I approach these challenges.",
        "I've been following this community for a while and finally decided to share my own work. Feedback is greatly appreciated!",
        "This tutorial covers everything from the basics to more advanced techniques. Perfect for those looking to level up their skills.",
        "Sharing my latest creation with the community. It's been a challenging but rewarding experience putting this together.",
        "I've put together a comprehensive guide based on my experience. Hope it helps others avoid the mistakes I made early on."
    ];

    private static readonly string[] CommunityPostComments =
    [
        "This is really impressive work! How long did it take you to learn these techniques?",
        "Great tutorial! I've been looking for something like this for a while.",
        "Love the art style! What software did you use for this?",
        "Thanks for sharing! This will definitely help with my current project.",
        "Awesome work! Any plans to create more tutorials like this?",
        "This is exactly what I needed. Bookmarking for later!",
        "Really well explained. Even a beginner like me could follow along.",
        "The attention to detail here is incredible. Well done!",
        "I tried this technique and it worked great. Thanks for the tip!",
        "Would love to see more content like this from you.",
        "How do you handle the performance impact of this approach?",
        "This inspired me to try something similar. Great work!",
        "Clean and professional looking. What's your workflow?",
        "I've been struggling with this exact problem. Thanks for the solution!",
        "Mind sharing the settings you used for this effect?"
    ];

    private static readonly string[] CommunityPostAnswers =
    [
        "Thanks for the kind words! It took me about 6 months of practice to get to this level.",
        "Glad you found it helpful! I used Blender for the modeling and Substance Painter for textures.",
        "I'm planning to create more tutorials in the future. Stay tuned!",
        "The performance impact is minimal if you follow the optimization tips I mentioned.",
        "My workflow involves planning everything out first, then executing in stages.",
        "Feel free to reach out if you have any questions about the process!",
        "I'll definitely share more settings and configurations in my next post.",
        "Thanks! I spent a lot of time on the small details to make it look right.",
        "Happy to help! Let me know if you need any clarification on the steps.",
        "I appreciate the feedback! It motivates me to create more content."
    ];

    private static readonly string[] YoutubeVideoLinks =
    [
        "https://www.youtube.com/watch?v=i_UEP_Mkiqs",
        "https://www.youtube.com/watch?v=TPrnSACiTJ4",
        "https://www.youtube.com/watch?v=IKqJPuCfvmo",
        "https://www.youtube.com/watch?v=gB1F9G0JXOo",
        "https://www.youtube.com/watch?v=pwZpJzpE2lQ",
        "https://www.youtube.com/watch?v=_nRzoTzeyxU",
        "https://www.youtube.com/watch?v=QMU0qR_GGFw",
        "https://www.youtube.com/watch?v=RqRoXLLwJ8g",
        "https://www.youtube.com/watch?v=aircAruvnKk",
        "https://www.youtube.com/watch?v=Z1qyvQsjK5Y"
    ];

    private static readonly string[] ReportDescriptions =
    [
        "This content violates community guidelines and should be reviewed immediately.",
        "I've seen this user posting similar content multiple times.",
        "This appears to be automated spam behavior.",
        "The content contains misleading information.",
        "This user has been harassing other community members.",
        "The content appears to be stolen from another creator.",
        "Multiple users have complained about this content.",
        "This violates the terms of service.",
        "The behavior is disruptive to the community.",
        "Please review this content as soon as possible."
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
        SeedUserFollowers(context, logger).GetAwaiter().GetResult();
        SeedCommunityPosts(context, modelImageSettings, logger).GetAwaiter().GetResult();
        SeedCommunityPostQuestionSections(context, logger).GetAwaiter().GetResult();
        SeedCommunityPostUserReviews(context, logger).GetAwaiter().GetResult();
        SeedReports(context, logger).GetAwaiter().GetResult();

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
        var createdFolders = new List<string>();

        foreach (var (uuid, email, nickname, firstName, lastName, roleName) in FixedUsers)
        {
            var role = roleName switch
            {
                nameof(UserRoleEnum.Root) => rootRole,
                nameof(UserRoleEnum.Admin) => adminRole,
                _ => userRole
            };

            var colorIndex = users.Count % AvatarColors.Length;
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
            GenerateAndSaveAvatar(uuid, pictureFileName, AvatarColors[colorIndex], userImageSettings);

            users.Add(user);

            logger.LogInformation("Created fixed user: {Nickname} ({Role})", user.NickName, role.RoleName);
        }

        for (var i = 0; i < 92; i++)
        {
            var userUuid = Guid.NewGuid();
            var colorIndex = (i + FixedUsers.Count) % AvatarColors.Length;
            var pictureFileName = $"avatar-{userUuid}.jpg";

            var role = i switch
            {
                < 3 => rootRole,
                < 6 => adminRole,
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

            var userFolder = Path.Combine(userImageSettings.SavePath, "users", userUuid.ToString());
            createdFolders.Add(userFolder);
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
        var createdFolders = new List<string>();

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
                createdFolders.Add(modelFolderPath);
                var modelFilePath = Path.Combine(modelFolderPath, modelFileName);
                await File.WriteAllTextAsync(modelFilePath, minimalObjContent);

                var pictureFolderPath = Path.Combine(modelImageSettings.SavePath, "models", modelUuid.ToString());
                Directory.CreateDirectory(pictureFolderPath);
                createdFolders.Add(pictureFolderPath);

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

        var regularUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.User))
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var ownedModels = await context.Libraries
            .Select(l => new { l.UserUuid, l.ModelUuid })
            .ToListAsync();
        var ownedSet = ownedModels.ToHashSet();

        var cartItems = new List<ShoppingCart>();
        var cartPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = regularUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < regularUsers.Count; userIndex++)
        {
            var user = regularUsers[userIndex];
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

        var regularUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.User))
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var orders = new List<Order>();
        var libraryEntries = new List<Library>();
        var purchasedPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = regularUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < regularUsers.Count; userIndex++)
        {
            var user = regularUsers[userIndex];
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

            var isPositive = Random.NextDouble() < 0.65;
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

    #region UserFollowers Seeding

    private static async Task SeedUserFollowers(JoyModelsDbContext context, ILogger logger)
    {
        var existingFollowersCount = await context.UserFollowers.CountAsync();
        if (existingFollowersCount > 0)
        {
            logger.LogInformation("Database already contains {Count} followers. Skipping followers seeding.",
                existingFollowersCount);
            return;
        }

        logger.LogInformation("Starting UserFollowers seeding...");

        var allUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .ToListAsync();

        var modelCreators = allUsers
            .Where(u => u.UserModelsCount > 0)
            .OrderByDescending(u => u.UserModelsCount)
            .ToList();

        var regularUsers = allUsers
            .Where(u => u.UserModelsCount == 0)
            .ToList();

        var followers = new List<UserFollower>();
        var followPairs = new HashSet<(Guid Origin, Guid Target)>();

        foreach (var creator in modelCreators)
        {
            var followerCount = Math.Min(60 + Random.Next(30), allUsers.Count - 1);
            var potentialFollowers = allUsers
                .Where(u => u.Uuid != creator.Uuid)
                .OrderBy(_ => Random.Next())
                .Take(followerCount)
                .ToList();

            foreach (var follower in potentialFollowers)
            {
                if (followPairs.Add((follower.Uuid, creator.Uuid)))
                {
                    followers.Add(new UserFollower
                    {
                        Uuid = Guid.NewGuid(),
                        UserOriginUuid = follower.Uuid,
                        UserTargetUuid = creator.Uuid,
                        FollowedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 180))
                    });
                    follower.UserFollowingCount++;
                    creator.UserFollowerCount++;
                }
            }
        }

        foreach (var creator in modelCreators)
        {
            var otherCreators = modelCreators.Where(c => c.Uuid != creator.Uuid).ToList();
            var creatorsToFollow = otherCreators
                .OrderBy(_ => Random.Next())
                .Take(Random.Next(3, Math.Min(8, otherCreators.Count + 1)))
                .ToList();

            foreach (var target in creatorsToFollow)
            {
                if (followPairs.Add((creator.Uuid, target.Uuid)))
                {
                    followers.Add(new UserFollower
                    {
                        Uuid = Guid.NewGuid(),
                        UserOriginUuid = creator.Uuid,
                        UserTargetUuid = target.Uuid,
                        FollowedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 180))
                    });
                    creator.UserFollowingCount++;
                    target.UserFollowerCount++;
                }
            }
        }

        foreach (var user in regularUsers)
        {
            if (Random.NextDouble() > 0.3)
                continue;

            var usersToFollow = regularUsers
                .Where(u => u.Uuid != user.Uuid)
                .OrderBy(_ => Random.Next())
                .Take(Random.Next(1, 5))
                .ToList();

            foreach (var target in usersToFollow)
            {
                if (followPairs.Add((user.Uuid, target.Uuid)))
                {
                    followers.Add(new UserFollower
                    {
                        Uuid = Guid.NewGuid(),
                        UserOriginUuid = user.Uuid,
                        UserTargetUuid = target.Uuid,
                        FollowedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 180))
                    });
                    user.UserFollowingCount++;
                    target.UserFollowerCount++;
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.UserFollowers.AddRangeAsync(followers);
            await context.SaveChangesAsync();

            context.Users.UpdateRange(allUsers);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation("UserFollowers seeding completed. Created {Count} follow relationships.",
                followers.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "UserFollowers seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region CommunityPost Seeding

    private static async Task SeedCommunityPosts(
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
            var creator = allUsers[Random.Next(allUsers.Count)];
            var postType = postTypes[Random.Next(postTypes.Count)];

            var title = GetUniqueCommunityPostTitle(usedTitles, i);
            usedTitles.Add(title);

            string? youtubeLink = null;
            if (i < youtubePostCount)
            {
                youtubeLink = YoutubeVideoLinks[i % YoutubeVideoLinks.Length];
            }

            var post = new CommunityPost
            {
                Uuid = postUuid,
                UserUuid = creator.Uuid,
                Title = title,
                Description = CommunityPostDescriptions[i % CommunityPostDescriptions.Length],
                PostTypeUuid = postType.Uuid,
                YoutubeVideoLink = youtubeLink,
                CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 180)),
                CommunityPostLikes = 0,
                CommunityPostDislikes = 0,
                CommunityPostCommentCount = 0
            };
            posts.Add(post);

            var pictureFolderPath = Path.Combine(modelImageSettings.SavePath, "community-posts", postUuid.ToString());
            Directory.CreateDirectory(pictureFolderPath);
            createdFolders.Add(pictureFolderPath);

            var pictureCount = Random.Next(0, 5);
            var selectedImages = sharedImageFileNames.OrderBy(_ => Random.Next()).Take(pictureCount).ToList();

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
        var baseTitle = CommunityPostTitles[index % CommunityPostTitles.Length];
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
                var colorIndex = i % CommunityPostColors.Length;
                GenerateCommunityPostImage(filePath, CommunityPostColors[colorIndex], i);
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
        var gridColor = LightenColor(bgColor, 0.1f);
        var primaryColor = LightenColor(bgColor, 0.4f);
        var accentColor = Color.ParseHex(AvatarColors[seed % AvatarColors.Length]);

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
                    DrawGameController(ctx, center, center, 100, primaryColor, accentColor);
                    break;
                case 1:
                    DrawCodeBrackets(ctx, center, center, 120, primaryColor, accentColor);
                    break;
                case 2:
                    DrawPixelHeart(ctx, center, center, 80, primaryColor, accentColor);
                    break;
                case 3:
                    DrawLightbulb(ctx, center, center, 100, primaryColor, accentColor);
                    break;
                case 4:
                    DrawPencil(ctx, center, center, 120, primaryColor, accentColor);
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

    private static void DrawGameController(IImageProcessingContext ctx, float cx, float cy, float controllerSize,
        Color primaryColor, Color accentColor)
    {
        var bodyWidth = controllerSize * 1.6f;
        var bodyHeight = controllerSize * 0.8f;

        ctx.Fill(primaryColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy, bodyHeight * 0.5f, bodyHeight * 0.5f));
        ctx.Fill(primaryColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy, bodyHeight * 0.5f, bodyHeight * 0.5f));
        ctx.Fill(primaryColor,
            new RectangularPolygon(cx - bodyWidth * 0.3f, cy - bodyHeight * 0.25f, bodyWidth * 0.6f,
                bodyHeight * 0.5f));

        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy - 5, 12, 12));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f - 15, cy + 5, 8, 8));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f + 15, cy + 5, 8, 8));
        ctx.Fill(accentColor, new EllipsePolygon(cx - bodyWidth * 0.3f, cy + 15, 8, 8));

        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f - 12, cy, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f + 12, cy, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy - 12, 10, 10));
        ctx.Fill(accentColor, new EllipsePolygon(cx + bodyWidth * 0.3f, cy + 12, 10, 10));
    }

    private static void DrawCodeBrackets(IImageProcessingContext ctx, float cx, float cy, float bracketSize,
        Color primaryColor, Color accentColor)
    {
        var thickness = 8f;

        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.4f, cy - bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.6f, cy - bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.6f, cy - bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.6f, cy + bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx - bracketSize * 0.6f, cy + bracketSize * 0.5f),
            new PointF(cx - bracketSize * 0.4f, cy + bracketSize * 0.5f));

        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.4f, cy - bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.6f, cy - bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.6f, cy - bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.6f, cy + bracketSize * 0.5f));
        ctx.DrawLine(primaryColor, thickness, new PointF(cx + bracketSize * 0.6f, cy + bracketSize * 0.5f),
            new PointF(cx + bracketSize * 0.4f, cy + bracketSize * 0.5f));

        ctx.DrawLine(accentColor, thickness * 0.8f, new PointF(cx - bracketSize * 0.15f, cy - bracketSize * 0.3f),
            new PointF(cx + bracketSize * 0.15f, cy + bracketSize * 0.3f));
    }

    private static void DrawPixelHeart(IImageProcessingContext ctx, float cx, float cy, float heartSize,
        Color primaryColor, Color accentColor)
    {
        var pixelSize = heartSize / 6f;

        int[,] heartPattern =
        {
            { 0, 1, 1, 0, 0, 1, 1, 0 },
            { 1, 2, 2, 1, 1, 2, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 1 },
            { 0, 1, 2, 2, 2, 2, 1, 0 },
            { 0, 0, 1, 2, 2, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 }
        };

        var startX = cx - (heartPattern.GetLength(1) * pixelSize) / 2;
        var startY = cy - (heartPattern.GetLength(0) * pixelSize) / 2;

        for (var row = 0; row < heartPattern.GetLength(0); row++)
        {
            for (var col = 0; col < heartPattern.GetLength(1); col++)
            {
                if (heartPattern[row, col] == 0) continue;

                var color = heartPattern[row, col] == 1 ? primaryColor : accentColor;
                var x = startX + col * pixelSize;
                var y = startY + row * pixelSize;
                ctx.Fill(color, new RectangularPolygon(x, y, pixelSize - 1, pixelSize - 1));
            }
        }
    }

    private static void DrawLightbulb(IImageProcessingContext ctx, float cx, float cy, float bulbSize,
        Color primaryColor, Color accentColor)
    {
        ctx.Fill(accentColor, new EllipsePolygon(cx, cy - bulbSize * 0.15f, bulbSize * 0.4f, bulbSize * 0.45f));

        ctx.Fill(primaryColor,
            new RectangularPolygon(cx - bulbSize * 0.15f, cy + bulbSize * 0.25f, bulbSize * 0.3f, bulbSize * 0.25f));

        for (var i = 0; i < 3; i++)
        {
            var y = cy + bulbSize * 0.3f + i * 8;
            ctx.DrawLine(DarkenColor(primaryColor, 0.2f), 2f, new PointF(cx - bulbSize * 0.12f, y),
                new PointF(cx + bulbSize * 0.12f, y));
        }

        ctx.DrawLine(accentColor, 3f, new PointF(cx - bulbSize * 0.6f, cy - bulbSize * 0.15f),
            new PointF(cx - bulbSize * 0.45f, cy - bulbSize * 0.15f));
        ctx.DrawLine(accentColor, 3f, new PointF(cx + bulbSize * 0.6f, cy - bulbSize * 0.15f),
            new PointF(cx + bulbSize * 0.45f, cy - bulbSize * 0.15f));
        ctx.DrawLine(accentColor, 3f, new PointF(cx, cy - bulbSize * 0.7f),
            new PointF(cx, cy - bulbSize * 0.55f));
    }

    private static void DrawPencil(IImageProcessingContext ctx, float cx, float cy, float pencilSize,
        Color primaryColor, Color accentColor)
    {
        var angle = -45f * MathF.PI / 180f;
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);

        var bodyLength = pencilSize * 0.8f;
        var bodyWidth = pencilSize * 0.15f;

        var bodyPoints = new PointF[]
        {
            new(cx - bodyLength * 0.5f * cos - bodyWidth * 0.5f * sin,
                cy - bodyLength * 0.5f * sin + bodyWidth * 0.5f * cos),
            new(cx - bodyLength * 0.5f * cos + bodyWidth * 0.5f * sin,
                cy - bodyLength * 0.5f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos + bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos - bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin + bodyWidth * 0.5f * cos)
        };
        ctx.FillPolygon(primaryColor, bodyPoints);

        var tipPoints = new PointF[]
        {
            new(cx + bodyLength * 0.3f * cos - bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin + bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.3f * cos + bodyWidth * 0.5f * sin,
                cy + bodyLength * 0.3f * sin - bodyWidth * 0.5f * cos),
            new(cx + bodyLength * 0.5f * cos, cy + bodyLength * 0.5f * sin)
        };
        ctx.FillPolygon(accentColor, tipPoints);

        ctx.Fill(DarkenColor(primaryColor, 0.3f), new EllipsePolygon(
            cx - bodyLength * 0.5f * cos,
            cy - bodyLength * 0.5f * sin,
            bodyWidth * 0.6f, bodyWidth * 0.4f));
    }

    #endregion

    #region CommunityPostQuestionSection Seeding

    private static async Task SeedCommunityPostQuestionSections(JoyModelsDbContext context, ILogger logger)
    {
        var existingCommentsCount = await context.CommunityPostQuestionSections.CountAsync();
        if (existingCommentsCount > 0)
        {
            logger.LogInformation(
                "Database already contains {Count} community post comments. Skipping comment seeding.",
                existingCommentsCount);
            return;
        }

        logger.LogInformation("Starting CommunityPostQuestionSection seeding...");

        var posts = await context.CommunityPosts.ToListAsync();
        var allUsers = await context.Users.ToListAsync();

        var comments = new List<CommunityPostQuestionSection>();

        foreach (var post in posts)
        {
            var postOwner = allUsers.First(u => u.Uuid == post.UserUuid);
            var otherUsers = allUsers.Where(u => u.Uuid != post.UserUuid).ToList();

            var questionCount = Random.Next(3, 8);

            for (var i = 0; i < questionCount; i++)
            {
                var commenter = otherUsers[Random.Next(otherUsers.Count)];
                var questionUuid = Guid.NewGuid();
                var questionDate = post.CreatedAt.AddDays(Random.Next(1, 30));

                var question = new CommunityPostQuestionSection
                {
                    Uuid = questionUuid,
                    ParentMessageUuid = null,
                    CommunityPostUuid = post.Uuid,
                    UserUuid = commenter.Uuid,
                    MessageText = CommunityPostComments[Random.Next(CommunityPostComments.Length)],
                    CreatedAt = questionDate
                };
                comments.Add(question);
                post.CommunityPostCommentCount++;

                var answerCount = Random.Next(1, 4);
                var lastAnswerDate = questionDate;

                for (var j = 0; j < answerCount; j++)
                {
                    var isOwnerAnswer = j == 0 || Random.NextDouble() > 0.5;
                    var answerer = isOwnerAnswer ? postOwner : otherUsers[Random.Next(otherUsers.Count)];

                    lastAnswerDate = lastAnswerDate.AddHours(Random.Next(1, 48));

                    var answer = new CommunityPostQuestionSection
                    {
                        Uuid = Guid.NewGuid(),
                        ParentMessageUuid = questionUuid,
                        CommunityPostUuid = post.Uuid,
                        UserUuid = answerer.Uuid,
                        MessageText = CommunityPostAnswers[Random.Next(CommunityPostAnswers.Length)],
                        CreatedAt = lastAnswerDate
                    };
                    comments.Add(answer);
                    post.CommunityPostCommentCount++;
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.CommunityPostQuestionSections.AddRangeAsync(comments);
            await context.SaveChangesAsync();

            context.CommunityPosts.UpdateRange(posts);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            var totalQuestions = comments.Count(c => c.ParentMessageUuid == null);
            var totalAnswers = comments.Count(c => c.ParentMessageUuid != null);
            logger.LogInformation(
                "CommunityPostQuestionSection seeding completed. Created {Questions} questions and {Answers} answers.",
                totalQuestions, totalAnswers);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "CommunityPostQuestionSection seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region CommunityPostUserReview Seeding

    private static async Task SeedCommunityPostUserReviews(JoyModelsDbContext context, ILogger logger)
    {
        var existingReviewsCount = await context.CommunityPostUserReviews.CountAsync();
        if (existingReviewsCount > 0)
        {
            logger.LogInformation("Database already contains {Count} community post reviews. Skipping review seeding.",
                existingReviewsCount);
            return;
        }

        logger.LogInformation("Starting CommunityPostUserReview seeding...");

        var posts = await context.CommunityPosts.ToListAsync();
        var allUsers = await context.Users.ToListAsync();

        var positiveReviewType = await context.CommunityPostReviewTypes
            .FirstAsync(r => r.ReviewName == "Positive");
        var negativeReviewType = await context.CommunityPostReviewTypes
            .FirstAsync(r => r.ReviewName == "Negative");

        var reviews = new List<CommunityPostUserReview>();
        var reviewedPairs = new HashSet<(Guid UserUuid, Guid PostUuid)>();

        foreach (var post in posts)
        {
            var otherUsers = allUsers.Where(u => u.Uuid != post.UserUuid).ToList();

            var reviewerCount = Random.Next(10, 40);
            var selectedReviewers = otherUsers.OrderBy(_ => Random.Next()).Take(reviewerCount).ToList();

            foreach (var reviewer in selectedReviewers)
            {
                if (reviewedPairs.Contains((reviewer.Uuid, post.Uuid)))
                    continue;

                var isPositive = Random.NextDouble() < 0.65;
                var reviewType = isPositive ? positiveReviewType : negativeReviewType;

                reviews.Add(new CommunityPostUserReview
                {
                    Uuid = Guid.NewGuid(),
                    UserUuid = reviewer.Uuid,
                    CommunityPostUuid = post.Uuid,
                    ReviewTypeUuid = reviewType.Uuid
                });
                reviewedPairs.Add((reviewer.Uuid, post.Uuid));

                if (isPositive)
                    post.CommunityPostLikes++;
                else
                    post.CommunityPostDislikes++;
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.CommunityPostUserReviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            context.CommunityPosts.UpdateRange(posts);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            var positiveCount = reviews.Count(r => r.ReviewTypeUuid == positiveReviewType.Uuid);
            var negativeCount = reviews.Count(r => r.ReviewTypeUuid == negativeReviewType.Uuid);
            logger.LogInformation(
                "CommunityPostUserReview seeding completed. Created {Total} reviews ({Positive} likes, {Negative} dislikes).",
                reviews.Count, positiveCount, negativeCount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "CommunityPostUserReview seeding failed. Rolling back transaction.");
            throw;
        }
    }

    #endregion

    #region Report Seeding

    private static async Task SeedReports(JoyModelsDbContext context, ILogger logger)
    {
        var existingReportsCount = await context.Reports.CountAsync();
        if (existingReportsCount > 0)
        {
            logger.LogInformation("Database already contains {Count} reports. Skipping report seeding.",
                existingReportsCount);
            return;
        }

        logger.LogInformation("Starting Report seeding...");

        var regularUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.User))
            .ToListAsync();

        var communityPosts = await context.CommunityPosts.ToListAsync();
        var communityPostComments = await context.CommunityPostQuestionSections
            .Where(x => x.ParentMessageUuid == null)
            .ToListAsync();
        var modelReviews = await context.ModelReviews.ToListAsync();
        var modelFaqQuestions = await context.ModelFaqSections
            .Where(x => x.ParentMessageUuid == null)
            .ToListAsync();

        var adminUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.Root)
                        || u.UserRoleUu.RoleName == nameof(UserRoleEnum.Admin))
            .ToListAsync();

        var reports = new List<Report>();
        var reportedPairs = new HashSet<(Guid ReporterUuid, Guid ReportedEntityUuid)>();

        var reportReasons = Enum.GetNames<ReportReason>();
        var reportStatuses = Enum.GetNames<ReportStatus>();

        var userReportCount = Random.Next(10, 16);
        for (var i = 0; i < userReportCount; i++)
        {
            var reportedUser = regularUsers[Random.Next(regularUsers.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != reportedUser.Uuid).ToList();
            var reporter = availableReporters[Random.Next(availableReporters.Count)];

            if (reportedPairs.Contains((reporter.Uuid, reportedUser.Uuid)))
                continue;

            var report = CreateReport(
                reporter.Uuid,
                nameof(ReportedEntityType.User),
                reportedUser.Uuid,
                reportReasons,
                reportStatuses,
                adminUsers);

            reports.Add(report);
            reportedPairs.Add((reporter.Uuid, reportedUser.Uuid));

            logger.LogInformation("Created User report: {Reporter} reported {Reported}",
                reporter.NickName, reportedUser.NickName);
        }

        var postReportCount = Random.Next(15, 26);
        for (var i = 0; i < postReportCount && i < communityPosts.Count; i++)
        {
            var post = communityPosts[Random.Next(communityPosts.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != post.UserUuid).ToList();
            var reporter = availableReporters[Random.Next(availableReporters.Count)];

            if (reportedPairs.Contains((reporter.Uuid, post.Uuid)))
                continue;

            var report = CreateReport(
                reporter.Uuid,
                nameof(ReportedEntityType.CommunityPost),
                post.Uuid,
                reportReasons,
                reportStatuses,
                adminUsers);

            reports.Add(report);
            reportedPairs.Add((reporter.Uuid, post.Uuid));

            logger.LogInformation("Created CommunityPost report for post: {PostTitle}", post.Title);
        }

        var commentReportCount = Random.Next(10, 21);
        for (var i = 0; i < commentReportCount && i < communityPostComments.Count; i++)
        {
            var comment = communityPostComments[Random.Next(communityPostComments.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != comment.UserUuid).ToList();
            var reporter = availableReporters[Random.Next(availableReporters.Count)];

            if (reportedPairs.Contains((reporter.Uuid, comment.Uuid)))
                continue;

            var report = CreateReport(
                reporter.Uuid,
                nameof(ReportedEntityType.CommunityPostComment),
                comment.Uuid,
                reportReasons,
                reportStatuses,
                adminUsers);

            reports.Add(report);
            reportedPairs.Add((reporter.Uuid, comment.Uuid));

            logger.LogInformation("Created CommunityPostComment report");
        }

        var reviewReportCount = Random.Next(10, 16);
        for (var i = 0; i < reviewReportCount && i < modelReviews.Count; i++)
        {
            var review = modelReviews[Random.Next(modelReviews.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != review.UserUuid).ToList();
            var reporter = availableReporters[Random.Next(availableReporters.Count)];

            if (reportedPairs.Contains((reporter.Uuid, review.Uuid)))
                continue;

            var report = CreateReport(
                reporter.Uuid,
                nameof(ReportedEntityType.ModelReview),
                review.Uuid,
                reportReasons,
                reportStatuses,
                adminUsers);

            reports.Add(report);
            reportedPairs.Add((reporter.Uuid, review.Uuid));

            logger.LogInformation("Created ModelReview report");
        }

        var faqReportCount = Random.Next(5, 11);
        for (var i = 0; i < faqReportCount && i < modelFaqQuestions.Count; i++)
        {
            var faq = modelFaqQuestions[Random.Next(modelFaqQuestions.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != faq.UserUuid).ToList();
            var reporter = availableReporters[Random.Next(availableReporters.Count)];

            if (reportedPairs.Contains((reporter.Uuid, faq.Uuid)))
                continue;

            var report = CreateReport(
                reporter.Uuid,
                nameof(ReportedEntityType.ModelFaqQuestion),
                faq.Uuid,
                reportReasons,
                reportStatuses,
                adminUsers);

            reports.Add(report);
            reportedPairs.Add((reporter.Uuid, faq.Uuid));

            logger.LogInformation("Created ModelFaqQuestion report");
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.Reports.AddRangeAsync(reports);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            var pendingCount = reports.Count(r => r.Status == nameof(ReportStatus.Pending));
            var reviewedCount = reports.Count(r => r.Status != nameof(ReportStatus.Pending));
            logger.LogInformation(
                "Report seeding completed. Created {Total} reports ({Pending} pending, {Reviewed} reviewed).",
                reports.Count, pendingCount, reviewedCount);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Report seeding failed. Rolling back transaction.");
            throw;
        }
    }

    private static Report CreateReport(
        Guid reporterUuid,
        string entityType,
        Guid entityUuid,
        string[] reasons,
        string[] statuses,
        List<User> adminUsers)
    {
        var isPending = Random.NextDouble() < 0.6;
        var status = isPending
            ? nameof(ReportStatus.Pending)
            : statuses.Where(s => s != nameof(ReportStatus.Pending)).ToArray()[Random.Next(statuses.Length - 1)];

        var hasDescription = Random.NextDouble() < 0.4;

        var report = new Report
        {
            Uuid = Guid.NewGuid(),
            ReporterUuid = reporterUuid,
            ReportedEntityType = entityType,
            ReportedEntityUuid = entityUuid,
            Reason = reasons[Random.Next(reasons.Length)],
            Description = hasDescription ? ReportDescriptions[Random.Next(ReportDescriptions.Length)] : null,
            Status = status,
            CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(1, 60))
        };

        if (!isPending && adminUsers.Count > 0)
        {
            report.ReviewedByUuid = adminUsers[Random.Next(adminUsers.Count)].Uuid;
            report.ReviewedAt = report.CreatedAt.AddDays(Random.Next(1, 5));
        }

        return report;
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