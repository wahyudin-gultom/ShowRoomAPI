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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly JwtConfig _config;
        private readonly ICarRepository _repo;
        private readonly IJwtBearerManager _jwtManager;
        private readonly GooglePubSubService _googlePubSub;
        public CarController(ICarRepository repo, IJwtBearerManager jwtManager, GooglePubSubService googlePubSub, IOptions<JwtConfig> config)
        {
            _repo = repo;
            _jwtManager = jwtManager;
            _googlePubSub = googlePubSub;
            _config = config.Value;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("send")]
        public async Task<IActionResult> Send(string message)
        {
            var publishId = await _googlePubSub.Publish(message);
            
            //var subscribeMsg = await _googlePubSub.Receive();
            return Ok(publishId);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDetail(string msg)
        {
            var sb = new StringBuilder();
            
            var tokenheader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenheader)) return BadRequest("Token invalid");
            tokenheader = tokenheader.Substring("Bearer ".Length);

            var claims = _jwtManager.GetAuthTokenResult(tokenheader);
            if (claims == null) return BadRequest("Error");


            var list = await _repo.GetAllAsync();
            return Ok(list);
        }

        [HttpPost("")]
        public async Task<IActionResult> Save([FromBody] Car entity)
        {
            var iscansave = await _repo.IsCanSave(entity);
            if (iscansave) return Ok("sukses");

            return BadRequest("Gagal");
        }

    }
}
