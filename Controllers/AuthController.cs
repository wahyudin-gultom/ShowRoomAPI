using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShowRoomAPI.Const;
using ShowRoomAPI.DataAccess.Interface;
using ShowRoomAPI.Models.Entitas;
using System.Text;

namespace ShowRoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtConfig _config;
        public AuthController(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        [HttpGet("")]
        public IActionResult login(string username, string pass)
        {
            if (username == UserData.UserName && pass == UserData.Password)
            {
                var generateToken = _config.GenerateToken(UserData.UserName);
                return Ok(generateToken);
                //_config.GetAuthTokenResult(generateToken);

                //return Ok(_jwtManager.GenerateToken());
            }
            return BadRequest("Failed!");
        }

    }
}
