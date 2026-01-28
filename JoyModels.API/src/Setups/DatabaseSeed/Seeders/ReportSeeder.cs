using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class ReportSeeder
{
    public static async Task SeedReports(JoyModelsDbContext context, ILogger logger)
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

        var userReportCount = SeedDataConstants.Random.Next(10, 16);
        for (var i = 0; i < userReportCount; i++)
        {
            var reportedUser = regularUsers[SeedDataConstants.Random.Next(regularUsers.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != reportedUser.Uuid).ToList();
            var reporter = availableReporters[SeedDataConstants.Random.Next(availableReporters.Count)];

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

        var postReportCount = SeedDataConstants.Random.Next(15, 26);
        for (var i = 0; i < postReportCount && i < communityPosts.Count; i++)
        {
            var post = communityPosts[SeedDataConstants.Random.Next(communityPosts.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != post.UserUuid).ToList();
            var reporter = availableReporters[SeedDataConstants.Random.Next(availableReporters.Count)];

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

        var commentReportCount = SeedDataConstants.Random.Next(10, 21);
        for (var i = 0; i < commentReportCount && i < communityPostComments.Count; i++)
        {
            var comment = communityPostComments[SeedDataConstants.Random.Next(communityPostComments.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != comment.UserUuid).ToList();
            var reporter = availableReporters[SeedDataConstants.Random.Next(availableReporters.Count)];

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

        var reviewReportCount = SeedDataConstants.Random.Next(10, 16);
        for (var i = 0; i < reviewReportCount && i < modelReviews.Count; i++)
        {
            var review = modelReviews[SeedDataConstants.Random.Next(modelReviews.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != review.UserUuid).ToList();
            var reporter = availableReporters[SeedDataConstants.Random.Next(availableReporters.Count)];

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

        var faqReportCount = SeedDataConstants.Random.Next(5, 11);
        for (var i = 0; i < faqReportCount && i < modelFaqQuestions.Count; i++)
        {
            var faq = modelFaqQuestions[SeedDataConstants.Random.Next(modelFaqQuestions.Count)];
            var availableReporters = regularUsers.Where(u => u.Uuid != faq.UserUuid).ToList();
            var reporter = availableReporters[SeedDataConstants.Random.Next(availableReporters.Count)];

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
        var isPending = SeedDataConstants.Random.NextDouble() < 0.6;
        var status = isPending
            ? nameof(ReportStatus.Pending)
            : statuses.Where(s => s != nameof(ReportStatus.Pending)).ToArray()[
                SeedDataConstants.Random.Next(statuses.Length - 1)];

        var hasDescription = SeedDataConstants.Random.NextDouble() < 0.4;

        var report = new Report
        {
            Uuid = Guid.NewGuid(),
            ReporterUuid = reporterUuid,
            ReportedEntityType = entityType,
            ReportedEntityUuid = entityUuid,
            Reason = reasons[SeedDataConstants.Random.Next(reasons.Length)],
            Description = hasDescription
                ? SeedDataConstants.ReportDescriptions[
                    SeedDataConstants.Random.Next(SeedDataConstants.ReportDescriptions.Length)]
                : null,
            Status = status,
            CreatedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 60))
        };

        if (!isPending && adminUsers.Count > 0)
        {
            report.ReviewedByUuid = adminUsers[SeedDataConstants.Random.Next(adminUsers.Count)].Uuid;
            report.ReviewedAt = report.CreatedAt.AddDays(SeedDataConstants.Random.Next(1, 5));
        }

        return report;
    }
}