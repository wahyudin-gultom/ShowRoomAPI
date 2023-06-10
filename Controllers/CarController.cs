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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly JwtConfig _config;
        private readonly ICarRepository _repo;
        private readonly GooglePubSubService _googlePubSub;
        public CarController(ICarRepository repo,GooglePubSubService googlePubSub, IOptions<JwtConfig> config)
        {
            _repo = repo;
            _googlePubSub = googlePubSub;
            _config = config.Value;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var tokenheader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenheader)) return BadRequest("Token invalid");
            tokenheader = tokenheader.Substring("Bearer ".Length);

            var claims = _config.GetAuthTokenResult(tokenheader);
            if (claims == null) return BadRequest("Error");

            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDetail(string serialNo)
        {
            var tokenheader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenheader)) return BadRequest("Token invalid");
            tokenheader = tokenheader.Substring("Bearer ".Length);

            var claims = _config.GetAuthTokenResult(tokenheader);
            if (claims == null) return BadRequest("Error");

            var detail = await _repo.GetById(serialNo);
            return Ok(detail);
        }

        [HttpPost("")]
        public async Task<IActionResult> Save([FromBody] Car entity)
        {
            var tokenheader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenheader)) return BadRequest("Token invalid");
            tokenheader = tokenheader.Substring("Bearer ".Length);

            var claims = _config.GetAuthTokenResult(tokenheader);
            if (claims == null) return BadRequest("Error");

            var iscansave = await _repo.IsCanSave(entity);
            if (iscansave) return Ok("sukses");

            return BadRequest("Gagal");
        }

        [HttpPatch("{serialNo}")]
        public async Task<IActionResult> Update([FromRoute] string serialNo, [FromBody] VMCar entity)
        {
            var tokenheader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenheader)) return BadRequest("Token invalid");
            tokenheader = tokenheader.Substring("Bearer ".Length);

            var claims = _config.GetAuthTokenResult(tokenheader);
            if (claims == null) return BadRequest("Error");

            var detail = await _repo.GetById(serialNo);
            if (detail == null) return BadRequest("Data tidak tersedia");
            
            detail.Brand = entity.Brand;
            detail.ModelNo = entity.ModelNo;
            detail.Year = entity.Year;
            detail.Price = entity.Price;
            detail.Type = entity.Type;

            var iscansave = await _repo.IsCanUpdate(detail);
            if (iscansave) return Ok("sukses");

            return BadRequest("Gagal update");
        }

        [HttpDelete("{serialNo}")]
        public async Task<IActionResult> Delete([FromRoute] string serialNo)
        {
            var detail = await _repo.GetById(serialNo);
            if (detail == null) return BadRequest("Data tidak tersedia");

            var iscansave = await _repo.IsCanDelete(detail);
            if (iscansave) return Ok("sukses");

            return BadRequest("Gagal update");
        }
    }
}
