using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Report;
using JoyModels.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/reports/")]
[ApiController]
public class ReportController(IReportService service) : ControllerBase
{
    [Authorize(Policy = "VerifiedUsers")]
    [HttpPost("create")]
    public async Task<ActionResult<ReportResponse>> Create([FromBody] ReportCreateRequest request)
    {
        return await service.Create(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("get/{reportUuid:guid}")]
    public async Task<ActionResult<ReportResponse>> GetByUuid([FromRoute] Guid reportUuid)
    {
        return await service.GetByUuid(reportUuid);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<ReportResponse>>> Search(
        [FromQuery] ReportSearchRequest request)
    {
        return await service.Search(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpGet("my-reports")]
    public async Task<ActionResult<PaginationResponse<ReportResponse>>> MyReports(
        [FromQuery] ReportSearchRequest request)
    {
        return await service.MyReports(request);
    }

    [Authorize(Policy = "HeadStaff")]
    [HttpPatch("status")]
    public async Task<ActionResult<ReportResponse>> PatchStatus([FromBody] ReportPatchStatusRequest request)
    {
        return await service.PatchStatus(request);
    }

    [Authorize(Policy = "VerifiedUsers")]
    [HttpDelete("delete/{reportUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid reportUuid)
    {
        await service.Delete(reportUuid);
        return NoContent();
    }
}