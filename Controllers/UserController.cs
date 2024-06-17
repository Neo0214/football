using football.Entities;
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
    public class UserController : ControllerBase
    {
        private readonly IUser _service;
        private readonly Token _token;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IUser service, Token token, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _token = token;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        public ActionResult login([FromBody] LoginDTO loginDTO)
        {
            User user = _service.login(loginDTO.username, loginDTO.password);
            if (user != null)
            {
                int id = user.UserId;
                var token = _token.GenerateToken(id);
                return Ok(new LoginResponseDTO("success", id, token));
            }
            return Ok(new LoginResponseDTO("failed", -1, ""));


        }
        [HttpPost]
        [Authorize]
        public ActionResult info()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = header.ToString().Split(" ")[1];
            var tokenHandle = new JwtSecurityTokenHandler();
            var tokenS = tokenHandle.ReadJwtToken(token);
            var userId = int.Parse(tokenS.Claims.ToArray()[0].Value);
            return Ok(_service.getUserInfo(userId));

        }
        [HttpGet]
        [Authorize]
        public ActionResult getUserAvatar([FromQuery] int userId)
        {
            string imagePath = _service.getUserAvatar(userId);
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
        public ActionResult getUserHistory()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = header.ToString().Split(" ")[1];
            var tokenHandle = new JwtSecurityTokenHandler();
            var tokenS = tokenHandle.ReadJwtToken(token);
            var userId = int.Parse(tokenS.Claims.ToArray()[0].Value);
            return Ok(_service.getUserHistory(userId));
        }
        [HttpPost]
        [Authorize]
        public ActionResult addHistory([FromBody] AddHistoryDTO addHistoryDTO)
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = header.ToString().Split(" ")[1];
            var tokenHandle = new JwtSecurityTokenHandler();
            var tokenS = tokenHandle.ReadJwtToken(token);
            var userId = int.Parse(tokenS.Claims.ToArray()[0].Value);
            return Ok(_service.addHistory(userId, addHistoryDTO));
        }
    }
}
