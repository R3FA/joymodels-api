using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using ReportEntity = JoyModels.Models.Database.Entities.Report;

namespace JoyModels.Services.Services.Report.HelperMethods;

public static class ReportHelperMethods
{
    public static void ValidateCreateRequest(ReportCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ReportedEntityType))
            throw new ArgumentException("ReportedEntityType is required.");

        if (!Enum.TryParse<ReportedEntityType>(request.ReportedEntityType, out _))
            throw new ArgumentException($"Invalid ReportedEntityType: {request.ReportedEntityType}");

        if (request.ReportedEntityUuid == Guid.Empty)
            throw new ArgumentException("ReportedEntityUuid is required.");

        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new ArgumentException("Reason is required.");

        if (!Enum.TryParse<ReportReason>(request.Reason, out _))
            throw new ArgumentException($"Invalid Reason: {request.Reason}");
    }

    public static void ValidatePatchStatusRequest(ReportPatchStatusRequest request)
    {
        if (request.ReportUuid == Guid.Empty)
            throw new ArgumentException("ReportUuid is required.");

        if (string.IsNullOrWhiteSpace(request.Status))
            throw new ArgumentException("Status is required.");

        if (!Enum.TryParse<ReportStatus>(request.Status, out _))
            throw new ArgumentException($"Invalid Status: {request.Status}");
    }

    public static async Task ValidateReportedEntityExists(JoyModelsDbContext context, ReportCreateRequest request)
    {
        var entityType = Enum.Parse<ReportedEntityType>(request.ReportedEntityType);

        var exists = entityType switch
        {
            ReportedEntityType.User => await context.Users
                .AnyAsync(x => x.Uuid == request.ReportedEntityUuid),
            ReportedEntityType.CommunityPost => await context.CommunityPosts
                .AnyAsync(x => x.Uuid == request.ReportedEntityUuid),
            ReportedEntityType.CommunityPostComment => await context.CommunityPostQuestionSections
                .AnyAsync(x => x.Uuid == request.ReportedEntityUuid),
            ReportedEntityType.ModelReview => await context.ModelReviews
                .AnyAsync(x => x.Uuid == request.ReportedEntityUuid),
            ReportedEntityType.ModelFaqQuestion => await context.ModelFaqSections
                .AnyAsync(x => x.Uuid == request.ReportedEntityUuid),
            _ => throw new ArgumentException($"Unknown entity type: {request.ReportedEntityType}")
        };

        if (!exists)
            throw new KeyNotFoundException($"Entity with UUID `{request.ReportedEntityUuid}` does not exist.");
    }

    public static async Task<ReportEntity> GetReportEntity(JoyModelsDbContext context, Guid reportUuid)
    {
        var entity = await context.Reports
            .AsNoTracking()
            .Include(x => x.Reporter)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.ReviewedBy)
            .ThenInclude(x => x!.UserRoleUu)
            .FirstOrDefaultAsync(x => x.Uuid == reportUuid);

        return entity ??
               throw new KeyNotFoundException($"Report with UUID `{reportUuid}` does not exist.");
    }

    public static async Task<PaginationBase<ReportEntity>> SearchReportEntities(
        JoyModelsDbContext context,
        ReportSearchRequest request)
    {
        var baseQuery = context.Reports
            .AsNoTracking()
            .Include(x => x.Reporter)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.ReviewedBy)
            .ThenInclude(x => x!.UserRoleUu)
            .AsQueryable();

        baseQuery = ApplyFilters(baseQuery, request);

        var orderedQuery = baseQuery.OrderByDescending(x => x.CreatedAt);

        return await PaginationBase<ReportEntity>.CreateAsync(
            orderedQuery,
            request.Page,
            request.PageSize,
            "CreatedAt:desc");
    }

    public static async Task<PaginationBase<ReportEntity>> SearchUserReportEntities(
        JoyModelsDbContext context,
        Guid userUuid,
        ReportSearchRequest request)
    {
        var baseQuery = context.Reports
            .AsNoTracking()
            .Include(x => x.Reporter)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.ReviewedBy)
            .ThenInclude(x => x!.UserRoleUu)
            .Where(x => x.ReporterUuid == userUuid);

        baseQuery = ApplyFilters(baseQuery, request);

        var orderedQuery = baseQuery.OrderByDescending(x => x.CreatedAt);

        return await PaginationBase<ReportEntity>.CreateAsync(
            orderedQuery,
            request.Page,
            request.PageSize,
            "CreatedAt:desc");
    }

    private static IQueryable<ReportEntity> ApplyFilters(
        IQueryable<ReportEntity> query,
        ReportSearchRequest request)
    {
        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(x => x.Status == request.Status);

        if (!string.IsNullOrEmpty(request.ReportedEntityType))
            query = query.Where(x => x.ReportedEntityType == request.ReportedEntityType);

        if (!string.IsNullOrEmpty(request.Reason))
            query = query.Where(x => x.Reason == request.Reason);

        return query;
    }
}