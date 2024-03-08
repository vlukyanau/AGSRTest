using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;

            return this.Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            await Task.CompletedTask;

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await Task.CompletedTask;

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            await Task.CompletedTask;

            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await Task.CompletedTask;

            return this.Ok();
        }
    }
}
