using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class UserModelLikeSeeder
{
    public static async Task SeedUserModelLikes(JoyModelsDbContext context, ILogger logger)
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

                if (SeedDataConstants.Random.NextDouble() < likeChance)
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
}