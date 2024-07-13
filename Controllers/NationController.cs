using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using football.Services;

namespace football.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NationController : ControllerBase
    {
        private readonly INation _service;
        public NationController(INation service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public ActionResult getPic([FromQuery] int nationId)
        {
            byte[] content = _service.getPic(nationId);
            if (content == null)
                return NotFound();
            return File(content, "application/octet-stream");
        }
    }
}
