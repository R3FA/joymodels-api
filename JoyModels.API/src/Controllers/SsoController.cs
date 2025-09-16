using JoyModels.Models.DataTransferObjects.CustomReturnTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Services.Services.Sso;
using Microsoft.AspNetCore.Mvc;

namespace JoyModels.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SsoController : ControllerBase
    {
        private readonly ISsoService _service;

        public SsoController(ISsoService service)
        {
            _service = service;
        }

        [HttpGet("GetByUuid")]
        public async Task<ActionResult<SsoReturn>> GetByUuid([FromQuery] SsoGet request)
        {
            return await _service.GetByUuid(request);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<PaginationResponse<SsoReturn>>> Search([FromQuery] SsoSearch request)
        {
            return await _service.Search(request);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserGet>> Create([FromBody] UserCreate user)
        {
            return await _service.Create(user);
        }

        [HttpPost("Verify")]
        public async Task<ActionResult<UserGet>> Verify([FromBody] SsoVerify request)
        {
            return await _service.Verify(request);
        }

        [HttpPost("ResendOtpCode")]
        public async Task<ActionResult<SuccessReturnDetails>> ResendOtpCode(
            [FromBody] SsoResendOtpCode request)
        {
            return await _service.ResendOtpCode(request);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<SuccessReturnDetails>> Delete([FromQuery] SsoDelete request)
        {
            return await _service.Delete(request);
        }
    }
}