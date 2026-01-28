using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class CommunityPostReviewSeeder
{
    public static async Task SeedCommunityPostUserReviews(JoyModelsDbContext context, ILogger logger)
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

            var reviewerCount = SeedDataConstants.Random.Next(10, 40);
            var selectedReviewers =
                otherUsers.OrderBy(_ => SeedDataConstants.Random.Next()).Take(reviewerCount).ToList();

            foreach (var reviewer in selectedReviewers)
            {
                if (reviewedPairs.Contains((reviewer.Uuid, post.Uuid)))
                    continue;

                var isPositive = SeedDataConstants.Random.NextDouble() < 0.65;
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
}