using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers;

[Route("api/notifications/")]
[ApiController]
[Authorize(Policy = "VerifiedUsers")]
public class NotificationController(INotificationService service) : ControllerBase
{
    [HttpGet("get/{notificationUuid:guid}")]
    public async Task<ActionResult<NotificationResponse>> GetByUuid([FromRoute] Guid notificationUuid)
    {
        return await service.GetByUuid(notificationUuid);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<NotificationResponse>>> Search(
        [FromQuery] NotificationSearchRequest request)
    {
        return await service.Search(request);
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        return await service.GetUnreadCount();
    }

    [HttpPatch("mark-as-read/{notificationUuid:guid}")]
    public async Task<ActionResult> MarkAsRead([FromRoute] Guid notificationUuid)
    {
        await service.MarkAsRead(notificationUuid);
        return NoContent();
    }

    [HttpPatch("mark-all-as-read")]
    public async Task<ActionResult> MarkAllAsRead()
    {
        await service.MarkAllAsRead();
        return NoContent();
    }

    [HttpDelete("delete/{notificationUuid:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid notificationUuid)
    {
        await service.Delete(notificationUuid);
        return NoContent();
    }
}