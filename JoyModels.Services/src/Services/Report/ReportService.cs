using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.Report.HelperMethods;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using ReportEntity = JoyModels.Models.Database.Entities.Report;

namespace JoyModels.Services.Services.Report;

public class ReportService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation)
    : IReportService
{
    public async Task<ReportResponse> Create(ReportCreateRequest request)
    {
        ReportHelperMethods.ValidateCreateRequest(request);

        var reporterUuid = userAuthValidation.GetUserClaimUuid();

        await ReportHelperMethods.ValidateReportedEntityExists(context, request);

        var existingReport = await context.Reports
            .FirstOrDefaultAsync(x => x.ReporterUuid == reporterUuid
                                      && x.ReportedEntityUuid == request.ReportedEntityUuid
                                      && x.Status == nameof(ReportStatus.Pending));

        if (existingReport != null)
            throw new ArgumentException("You have already reported this.");

        var reportEntity = mapper.Map<ReportEntity>(request);
        reportEntity.Uuid = Guid.NewGuid();
        reportEntity.ReporterUuid = reporterUuid;
        reportEntity.Status = nameof(ReportStatus.Pending);
        reportEntity.CreatedAt = DateTime.UtcNow;

        await context.Reports.AddAsync(reportEntity);
        await context.SaveChangesAsync();

        return await GetByUuid(reportEntity.Uuid);
    }

    public async Task<ReportResponse> GetByUuid(Guid reportUuid)
    {
        var entity = await ReportHelperMethods.GetReportEntity(context, reportUuid);
        var response = mapper.Map<ReportResponse>(entity);
        await PopulateReportedEntity(response);
        return response;
    }

    public async Task<PaginationResponse<ReportResponse>> Search(ReportSearchRequest request)
    {
        var entities = await ReportHelperMethods.SearchReportEntities(context, request);
        var response = mapper.Map<PaginationResponse<ReportResponse>>(entities);
        foreach (var item in response.Data)
            await PopulateReportedEntity(item);
        return response;
    }

    public async Task<PaginationResponse<ReportResponse>> MyReports(ReportSearchRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var entities = await ReportHelperMethods.SearchUserReportEntities(context, userUuid, request);
        var response = mapper.Map<PaginationResponse<ReportResponse>>(entities);
        foreach (var item in response.Data)
            await PopulateReportedEntity(item);
        return response;
    }

    public async Task<ReportResponse> PatchStatus(ReportPatchStatusRequest request)
    {
        ReportHelperMethods.ValidatePatchStatusRequest(request);

        var reviewerUuid = userAuthValidation.GetUserClaimUuid();

        var reportEntity = await context.Reports
            .FirstOrDefaultAsync(x => x.Uuid == request.ReportUuid);

        if (reportEntity == null)
            throw new KeyNotFoundException($"Report with UUID `{request.ReportUuid}` does not exist.");

        reportEntity.Status = request.Status;
        reportEntity.ReviewedByUuid = reviewerUuid;
        reportEntity.ReviewedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return await GetByUuid(reportEntity.Uuid);
    }

    public async Task Delete(Guid reportUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var deleted = await context.Reports
            .Where(x => x.Uuid == reportUuid && x.ReporterUuid == userUuid)
            .ExecuteDeleteAsync();

        if (deleted == 0)
            throw new KeyNotFoundException(
                $"Report with UUID `{reportUuid}` does not exist or you don't have permission to delete it.");
    }

    private async Task PopulateReportedEntity(ReportResponse response)
    {
        if (!Enum.TryParse<ReportedEntityType>(response.ReportedEntityType, out var entityType))
            return;

        switch (entityType)
        {
            case ReportedEntityType.User:
                var user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.UserRoleUu)
                    .FirstOrDefaultAsync(x => x.Uuid == response.ReportedEntityUuid);
                response.ReportedUser = mapper.Map<UsersResponse>(user);
                break;

            case ReportedEntityType.CommunityPost:
                var post = await context.CommunityPosts
                    .AsNoTracking()
                    .Include(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.PostTypeUu)
                    .Include(x => x.CommunityPostPictures)
                    .FirstOrDefaultAsync(x => x.Uuid == response.ReportedEntityUuid);
                response.ReportedCommunityPost = mapper.Map<CommunityPostResponse>(post);
                break;

            case ReportedEntityType.CommunityPostComment:
                var comment = await context.CommunityPostQuestionSections
                    .AsNoTracking()
                    .Include(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.CommunityPostUu).ThenInclude(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.CommunityPostUu).ThenInclude(x => x.PostTypeUu)
                    .Include(x => x.ParentMessage).ThenInclude(x => x!.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.Replies).ThenInclude(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .FirstOrDefaultAsync(x => x.Uuid == response.ReportedEntityUuid);
                response.ReportedCommunityPostComment = mapper.Map<CommunityPostQuestionSectionResponse>(comment);
                break;

            case ReportedEntityType.ModelReview:
                var review = await context.ModelReviews
                    .AsNoTracking()
                    .Include(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.ReviewTypeUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelAvailabilityUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelCategories).ThenInclude(x => x.CategoryUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelPictures)
                    .FirstOrDefaultAsync(x => x.Uuid == response.ReportedEntityUuid);
                response.ReportedModelReview = mapper.Map<ModelReviewResponse>(review);
                break;

            case ReportedEntityType.ModelFaqQuestion:
                var faq = await context.ModelFaqSections
                    .AsNoTracking()
                    .Include(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelAvailabilityUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelCategories).ThenInclude(x => x.CategoryUu)
                    .Include(x => x.ModelUu).ThenInclude(x => x.ModelPictures)
                    .Include(x => x.ParentMessage).ThenInclude(x => x!.UserUu).ThenInclude(x => x.UserRoleUu)
                    .Include(x => x.Replies).ThenInclude(x => x.UserUu).ThenInclude(x => x.UserRoleUu)
                    .FirstOrDefaultAsync(x => x.Uuid == response.ReportedEntityUuid);
                response.ReportedModelFaqQuestion = mapper.Map<ModelFaqSectionResponse>(faq);
                break;
        }
    }
}