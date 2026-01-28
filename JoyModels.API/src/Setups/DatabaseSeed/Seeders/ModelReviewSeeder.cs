using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class ModelReviewSeeder
{
    public static async Task SeedModelReviews(JoyModelsDbContext context, ILogger logger)
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

            if (SeedDataConstants.Random.NextDouble() > 0.75)
                continue;

            var isPositive = SeedDataConstants.Random.NextDouble() < 0.65;
            var reviewType = isPositive ? positiveReviewType : negativeReviewType;
            var reviewTexts =
                isPositive ? SeedDataConstants.PositiveReviewTexts : SeedDataConstants.NegativeReviewTexts;

            var review = new ModelReview
            {
                Uuid = Guid.NewGuid(),
                ModelUuid = library.ModelUuid,
                UserUuid = library.UserUuid,
                ReviewTypeUuid = reviewType.Uuid,
                ReviewText = reviewTexts[SeedDataConstants.Random.Next(reviewTexts.Length)],
                CreatedAt = library.AcquiredAt.AddDays(SeedDataConstants.Random.Next(1, 30))
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
}