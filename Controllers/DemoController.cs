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
        [HttpPost("Codigo")]
        public async Task<IActionResult> GerarNovoCodigo([FromBody] CodeDTO input)
        {
            var num = _random.Next()%1000;

            string message = Newtonsoft.Json.JsonConvert.SerializeObject(new 
            {
                id = input.UserId,
                code = num
            });

            try
            {
                await _message.Publish("demo-code", message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created("GerarNovoCodigo", new { key = input.UserId , message = num});
        }
        [HttpPost("Usuario")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UserDTO input)
        {
            var id = Guid.NewGuid();
            string message = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Name = input.UserName,
                Id = id
            });

            try
            {
                await _message.Publish("demo-user",message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Created("CadastrarUsuario", new { key = input.UserName, message});
        }
    }
}
    


public class CodeDTO
{
    public string UserId { get; set; }
}
public class UserDTO
{
    public string UserName { get; set; }
}

