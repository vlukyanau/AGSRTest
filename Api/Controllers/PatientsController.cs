using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Logic.Entities;
using Logic.Patients;


namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        [HttpGet("api/accounts/")]
        public async Task<IActionResult> Get()
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            var result = await retrieving.Go();

            return new JsonResult(result);
        }

        [HttpGet("api/accounts/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            var result = await retrieving.Go(id);

            return new JsonResult(result);
        }

        [HttpPost("api/accounts/")]
        public async Task<IActionResult> Post()
        {
            Patients.Creation creation = Patients.Creation.New();

            var result = await creation.Go();

            return new JsonResult(result);
        }

        [HttpPut("api/accounts/")]
        public async Task<IActionResult> Put()
        {
            Patients.Updating updating = Patients.Updating.New();

            StatusCodeResult result = await updating.Go();

            return result;
        }

        [HttpDelete("api/accounts/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Patients.Deleting deleting = Patients.Deleting.New();
            deleting.Id = id;

            StatusCodeResult result = await deleting.Go();

            return result;
        }
    }
}
