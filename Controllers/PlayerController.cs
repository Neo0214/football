using football.Entities;
using football.Models;
using football.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayer _service;
        public PlayerController(IPlayer service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize]
        public ActionResult getAllTransfer()
        {
            return Ok(_service.getAllPlayerTransfer());
        }
        [HttpPost]
        [Authorize]
        public ActionResult getData()
        {
            return Ok(_service.getAllPlayerData());
        }
        [HttpGet]
        [Authorize]
        public ActionResult getPlayerPic([FromQuery] int playerId)
        {
            byte[] content = _service.getPlayerPic(playerId);
            if (content == null)
                return NotFound();
            return File(content, "application/octet-stream");
        }

        [HttpPost]
        [Authorize]
        public ActionResult addPlayer([FromBody] AddPlayerDTO playerDTO)
        {
            return Ok(_service.addPlayer(playerDTO));
        }

        [HttpPost]
        public ActionResult uploadPic([FromForm] uploadPlayerPicDTO model)
        {
            if (model.file == null || model.file.Length == 0)
                return BadRequest("No file uploaded.");
            return Ok(_service.uploadPlayerPic(model.file, model.playerId));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getDetail([FromQuery] int playerId)
        {
            return Ok(_service.getPlayerDetail(playerId));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getMyPlayer([FromQuery] int clubId)
        {
            return Ok(_service.getMyPlayer(clubId));
        }
    }
}
