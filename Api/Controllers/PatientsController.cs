using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Api.Results;

using Logic.Patients;
using Api.Models;


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

            return new OutputResult(result);
        }

        [HttpGet("api/accounts/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            var result = await retrieving.Go(id);

            return new OutputResult(result);
        }

        [HttpGet("api/accounts/{search}")]
        public async Task<IActionResult> Search(DateTime date)
        {
            await Task.CompletedTask;

            return new OkResult();
        }

        [HttpPost("api/accounts/")]
        public async Task<IActionResult> Post([FromBody] Patient patient)
        {
            Patients.Creation creation = Patients.Creation.New();
            creation.Use = patient.Name.Use;
            creation.Family = patient.Name.Family;
            creation.Given = patient.Name.Given;
            creation.Gender = patient.Gender;
            creation.BirthDate = patient.BirthDate;
            creation.Active = patient.Active;

            StatusCodeResult result = await creation.Go();

            return result;
        }

        [HttpPut("api/accounts/")]
        public async Task<IActionResult> Put([FromBody] Patient patient)
        {
            Patients.Updating updating = Patients.Updating.New();
            updating.Id = patient.Id;
            updating.Use = patient.Name.Use;
            updating.Family = patient.Name.Family;
            updating.Given = patient.Name.Given;
            updating.Gender = patient.Gender;
            updating.BirthDate = patient.BirthDate;
            updating.Active = patient.Active;

            StatusCodeResult result = await updating.Go();

            return result;
        }

        [HttpDelete("api/accounts/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Patients.Deleting deleting = Patients.Deleting.New();
            deleting.Id = id;

            StatusCodeResult result = await deleting.Go();

            return result;
        }
    }
}
