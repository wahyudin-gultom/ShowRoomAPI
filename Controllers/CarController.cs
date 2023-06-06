using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowRoomAPI.Models.Entitas;

namespace ShowRoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public Car Data { get; set; }

        public CarController()
        {
            Data = new Car()
            {
                SerialNo = "342348032744",
                Brand = "Toyota",
                ModelNo = "afjal",
                Price = 1000000000m,
                Year = 2023
            };
        }

        [HttpGet("")]
        public Task<Car> Index()
        {
            return Task.FromResult(Data);
        }

        [HttpGet("SerialNoWithoutParam")]
        public Task<string> GetSerialNoWithoutParam()
        {
            var res = Task.Run(() => GetSerial());

            return res;
        }

        [HttpGet("SerialNoWithParam")]
        public string GetSerialNoWithParam()
        {
            return GetSerial(Data.SerialNo);
        }

        private string GetSerial()
        {
            return Data.SerialNo;
        }

        private string GetSerial(string serialNo)
        {
            return serialNo;
        }
    }
}
