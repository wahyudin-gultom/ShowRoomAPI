using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowRoomAPI.DataAccess.Interface;
using ShowRoomAPI.Models.Entitas;
using System.Text;

namespace ShowRoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly ICarRepository _repo;

        public CarController(ICarRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDetail()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list.FirstOrDefault());
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
