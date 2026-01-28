using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class UserFollowerSeeder
{
    public static async Task SeedUserFollowers(JoyModelsDbContext context, ILogger logger)
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
            var followerCount = Math.Min(60 + SeedDataConstants.Random.Next(30), allUsers.Count - 1);
            var potentialFollowers = allUsers
                .Where(u => u.Uuid != creator.Uuid)
                .OrderBy(_ => SeedDataConstants.Random.Next())
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
                        FollowedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 180))
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
                .OrderBy(_ => SeedDataConstants.Random.Next())
                .Take(SeedDataConstants.Random.Next(3, Math.Min(8, otherCreators.Count + 1)))
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
                        FollowedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 180))
                    });
                    creator.UserFollowingCount++;
                    target.UserFollowerCount++;
                }
            }
        }

        foreach (var user in regularUsers)
        {
            if (SeedDataConstants.Random.NextDouble() > 0.3)
                continue;

            var usersToFollow = regularUsers
                .Where(u => u.Uuid != user.Uuid)
                .OrderBy(_ => SeedDataConstants.Random.Next())
                .Take(SeedDataConstants.Random.Next(1, 5))
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
                        FollowedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 180))
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
}