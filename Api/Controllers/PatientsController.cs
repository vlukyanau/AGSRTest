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
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            var result = await retrieving.Go();

            return new OutputResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            var result = await retrieving.Go(id);

            return new OutputResult(result);
        }

        [HttpGet("birthDate")]
        public async Task<IActionResult> Search([FromQuery]DateTime[] date)
        {
            await Task.CompletedTask;

            return new OkResult();
        }

        [HttpPost]
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

        [HttpPut]
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Patients.Deleting deleting = Patients.Deleting.New();
            deleting.Id = id;

            StatusCodeResult result = await deleting.Go();

            return result;
        }
    }
}
