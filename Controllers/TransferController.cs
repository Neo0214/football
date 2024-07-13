using football.Models;
using football.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransfer _service;
        public TransferController(ITransfer service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult getPlayerByPosition([FromQuery] int clubId)
        {
            return Ok(_service.getPlayerByPosition(clubId));
        }

        [HttpPost]
        [Authorize]
        public ActionResult transferPlayer([FromBody] TransferPlayerDTO transferPlayerDTO)
        {
            return Ok(_service.transferPlayer(transferPlayerDTO));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getAll([FromQuery] int playerId)
        {
            return Ok(_service.getAll(playerId));
        }
    }
}
