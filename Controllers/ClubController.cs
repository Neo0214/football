using football.Models;
using football.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace football.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClub _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubController(IClub service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [Authorize]
        public IActionResult getClubId()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = header.ToString().Split(" ")[1];
            var tokenHandle = new JwtSecurityTokenHandler();
            var tokenS = tokenHandle.ReadJwtToken(token);
            var userId = int.Parse(tokenS.Claims.ToArray()[0].Value);
            var clubId = _service.getClubIdByUserId(userId);
            return Ok(clubId);

        }
        [HttpGet]
        public IActionResult getClubLogo([FromQuery] int clubId)
        {
            string imagePath = _service.getClubLogo(clubId);
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
        [HttpPost]
        [Authorize]
        public IActionResult getClubName([FromBody] ClubNameReq clubNameReq)
        {
            return Ok(_service.getClubNameById(clubNameReq.clubId));
        }
    }
}
