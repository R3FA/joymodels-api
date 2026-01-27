using JoyModels.Models.DataTransferObjects.RequestTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace JoyModels.API.Controllers;

[Route("api/library/")]
[ApiController]
[Authorize(Policy = "UserOnly")]
public class LibraryController(ILibraryService service) : ControllerBase
{
    [HttpGet("get/{libraryUuid:guid}")]
    public async Task<ActionResult<LibraryResponse>> GetByUuid([FromRoute] Guid libraryUuid)
    {
        return await service.GetByUuid(libraryUuid);
    }

    [HttpGet("get-by-model/{modelUuid:guid}")]
    public async Task<ActionResult<LibraryResponse>> GetByModelUuid([FromRoute] Guid modelUuid)
    {
        return await service.GetByModelUuid(modelUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<LibraryResponse>>> Search(
        [FromQuery] LibrarySearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpGet("has-purchased/{modelUuid:guid}")]
    public async Task<ActionResult<bool>> HasUserPurchasedModel([FromRoute] Guid modelUuid)
    {
        return await service.HasUserPurchasedModel(modelUuid);
    }

    [HttpGet("download/{modelUuid:guid}")]
    public async Task<ActionResult> DownloadModel([FromRoute] Guid modelUuid)
    {
        var filePath = await service.GetModelDownloadPath(modelUuid);

        if (!System.IO.File.Exists(filePath))
            throw new FileNotFoundException("Model file not found on server.");

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out var contentType))
            contentType = "application/octet-stream";

        var fileName = Path.GetFileName(filePath);
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return File(fileStream, contentType, fileName);
    }
}