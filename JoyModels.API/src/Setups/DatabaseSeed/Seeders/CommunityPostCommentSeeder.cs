using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class CommunityPostCommentSeeder
{
    public static async Task SeedCommunityPostQuestionSections(JoyModelsDbContext context, ILogger logger)
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

            var questionCount = SeedDataConstants.Random.Next(3, 8);

            for (var i = 0; i < questionCount; i++)
            {
                var commenter = otherUsers[SeedDataConstants.Random.Next(otherUsers.Count)];
                var questionUuid = Guid.NewGuid();
                var questionDate = post.CreatedAt.AddDays(SeedDataConstants.Random.Next(1, 30));

                var question = new CommunityPostQuestionSection
                {
                    Uuid = questionUuid,
                    ParentMessageUuid = null,
                    CommunityPostUuid = post.Uuid,
                    UserUuid = commenter.Uuid,
                    MessageText =
                        SeedDataConstants.CommunityPostComments[
                            SeedDataConstants.Random.Next(SeedDataConstants.CommunityPostComments.Length)],
                    CreatedAt = questionDate
                };
                comments.Add(question);
                post.CommunityPostCommentCount++;

                var answerCount = SeedDataConstants.Random.Next(1, 4);
                var lastAnswerDate = questionDate;

                for (var j = 0; j < answerCount; j++)
                {
                    var isOwnerAnswer = j == 0 || SeedDataConstants.Random.NextDouble() > 0.5;
                    var answerer = isOwnerAnswer
                        ? postOwner
                        : otherUsers[SeedDataConstants.Random.Next(otherUsers.Count)];

                    lastAnswerDate = lastAnswerDate.AddHours(SeedDataConstants.Random.Next(1, 48));

                    var answer = new CommunityPostQuestionSection
                    {
                        Uuid = Guid.NewGuid(),
                        ParentMessageUuid = questionUuid,
                        CommunityPostUuid = post.Uuid,
                        UserUuid = answerer.Uuid,
                        MessageText = SeedDataConstants.CommunityPostAnswers[
                            SeedDataConstants.Random.Next(SeedDataConstants.CommunityPostAnswers.Length)],
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
}