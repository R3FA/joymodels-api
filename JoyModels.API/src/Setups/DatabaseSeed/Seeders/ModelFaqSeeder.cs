using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class ModelFaqSeeder
{
    public static async Task SeedModelFaqSections(JoyModelsDbContext context, ILogger logger)
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

            var questionCount = SeedDataConstants.Random.Next(5, 9);
            var selectedQuestions = SeedDataConstants.FaqQuestions.OrderBy(_ => SeedDataConstants.Random.Next())
                .Take(questionCount).ToList();

            foreach (var questionText in selectedQuestions)
            {
                var asker = otherUsers[SeedDataConstants.Random.Next(otherUsers.Count)];
                var questionUuid = Guid.NewGuid();
                var questionDate = model.CreatedAt.AddDays(SeedDataConstants.Random.Next(1, 30));

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

                var answerCount = SeedDataConstants.Random.Next(2, 4);
                var lastAnswerDate = questionDate;

                for (var i = 0; i < answerCount; i++)
                {
                    var isOwnerAnswer = i == 0 || SeedDataConstants.Random.NextDouble() > 0.5;
                    var answerer = isOwnerAnswer
                        ? modelOwner
                        : otherUsers[SeedDataConstants.Random.Next(otherUsers.Count)];

                    lastAnswerDate = lastAnswerDate.AddHours(SeedDataConstants.Random.Next(1, 48));

                    var answer = new ModelFaqSection
                    {
                        Uuid = Guid.NewGuid(),
                        ParentMessageUuid = questionUuid,
                        ModelUuid = model.Uuid,
                        UserUuid = answerer.Uuid,
                        MessageText =
                            SeedDataConstants.FaqAnswers[
                                SeedDataConstants.Random.Next(SeedDataConstants.FaqAnswers.Length)],
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
}