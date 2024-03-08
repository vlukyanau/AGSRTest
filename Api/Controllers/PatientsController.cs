using System.Collections.Generic;
using System.Threading.Tasks;

using Api.Results;

using Microsoft.AspNetCore.Mvc;

using Logic.Entities;
using Logic.Patients;
using System.Net;
using System;


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

            IReadOnlyList<IPatient> result = await retrieving.Go();

            return new OutputResult(result);
        }

        [HttpGet("api/accounts/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            IPatient result = await retrieving.Go(id);

            return new OutputResult(result);
        }

        [HttpGet("api/accounts/{search}")]
        public async Task<IActionResult> Search(DateTime date)
        {
            await Task.CompletedTask;

            return new OkResult();
        }

        [HttpPost("api/accounts/")]
        public async Task<IActionResult> Post()
        {
            Patients.Creation creation = Patients.Creation.New();

            StatusCodeResult result = await creation.Go();

            return result;
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
