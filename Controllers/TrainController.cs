using football.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using football.Models;
namespace football.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrain _service;
        public TrainController(ITrain service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public ActionResult makeTrain([FromBody] TrainDTO trainDTO)
        {
            return Ok(_service.makeTrain(trainDTO));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getTrain([FromQuery] int playerId)
        {
            return Ok(_service.getTrain(playerId));
        }

    }
}
