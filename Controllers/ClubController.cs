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
        public ActionResult getClubLogo([FromQuery] int clubId)
        {
            byte[] content = _service.getClubLogo(clubId);
            if (content == null)
                return NotFound();
            return File(content, "application/octet-stream");

        }
        [HttpPost]
        [Authorize]
        public IActionResult getClubName([FromBody] ClubNameReq clubNameReq)
        {
            return Ok(_service.getClubNameById(clubNameReq.clubId));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getAllClubs([FromQuery] int curClub)
        {
            return Ok(_service.getAllClubs(curClub));
        }

        [HttpGet]
        [Authorize]
        public ActionResult getAllClubsWithScore([FromQuery] int curClub)
        {
            return Ok(_service.getAllClubsWithScore(curClub));
        }

    }
}
