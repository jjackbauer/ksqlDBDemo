using ksqlDBDemo.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ksqlDBDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IKafkaService _message;
        private readonly Random _random;

        public DemoController(IKafkaService message, Random random)
        {
            _message = message;
            _random = random;
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] DemoDTO input)
        {
            var num = _random.Next()%1000;

            try
            {
                await _message.Publish(input.name,num);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created("SendMessage",new { key = input.name , message = num});
        }
    }

    public class DemoDTO
    {
        public string name { get; set; }
    }
}
