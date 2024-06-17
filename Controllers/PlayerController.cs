using football.Entities;
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
            string imagePath = _service.getPlayerPic(playerId);
            var imageFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            var imageExtension = Path.GetExtension(imagePath).ToLowerInvariant();
            string mimeType = imageExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
            return File(imageFileStream, mimeType);
        }
    }
}
