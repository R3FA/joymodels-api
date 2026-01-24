using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Report;
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
        return mapper.Map<ReportResponse>(entity);
    }

    public async Task<PaginationResponse<ReportResponse>> Search(ReportSearchRequest request)
    {
        var entities = await ReportHelperMethods.SearchReportEntities(context, request);
        return mapper.Map<PaginationResponse<ReportResponse>>(entities);
    }

    public async Task<PaginationResponse<ReportResponse>> MyReports(ReportSearchRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var entities = await ReportHelperMethods.SearchUserReportEntities(context, userUuid, request);
        return mapper.Map<PaginationResponse<ReportResponse>>(entities);
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
}