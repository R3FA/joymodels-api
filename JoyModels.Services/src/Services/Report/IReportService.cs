using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Report;

namespace JoyModels.Services.Services.Report;

public interface IReportService
{
    Task<ReportResponse> Create(ReportCreateRequest request);
    Task<ReportResponse> GetByUuid(Guid reportUuid);
    Task<PaginationResponse<ReportResponse>> Search(ReportSearchRequest request);
    Task<PaginationResponse<ReportResponse>> MyReports(ReportSearchRequest request);
    Task<ReportResponse> PatchStatus(ReportPatchStatusRequest request);
    Task Delete(Guid reportUuid);
}