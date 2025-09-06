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

        [HttpGet("{uuid}")]
        public async Task<ActionResult<SsoGet>> GetByUuid(string uuid)
        {
            return await _service.GetByUuid(uuid);
        }

        [HttpPost]
        public async Task<ActionResult<UserGet>> Create([FromBody] UserCreate user)
        {
            return await _service.Create(user);
        }
    }
}