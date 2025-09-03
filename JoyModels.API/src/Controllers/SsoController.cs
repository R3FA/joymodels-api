using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Services.Services.Sso;
using Microsoft.AspNetCore.Http;
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

        public async Task<ActionResult<SsoGet>> GetByUuid(string uuid)
        {
            return await _service.GetByUuid(uuid);
        }
    }
}