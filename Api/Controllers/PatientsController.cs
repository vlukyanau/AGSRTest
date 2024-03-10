using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Results;

using Logic;
using Logic.Patients;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {
        /// <summary>
        /// Get all entities 'Patient'
        /// </summary>
        /// <response code="200"></response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            IResult result = await retrieving.Go();

            return new OutputResult(result);
        }

        /// <summary>
        /// Get 'Patient' by 'Id' parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Patient not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(Guid id)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            IResult result = await retrieving.Go(id);

            return new OutputResult(result);
        }

        /// <summary>
        /// Search 'Patient' by 'Date' parameter.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        [HttpGet("birthdate")]
        public async Task<IActionResult> Search([FromQuery] string[] dates)
        {
            Patients.Retrieving retrieving = Patients.Retrieving.New();

            IResult result = await retrieving.Go(dates);

            return new OutputResult(result);
        }

        /// <summary>
        /// Create 'Patient'.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "name": {
        ///          "use": "official",
        ///          "family": "Иванов",
        ///          "given": [
        ///            "Иван",
        ///            "Иванович"
        ///          ]
        ///        },
        ///        "gender": 1,
        ///        "birthDate": "2024-03-09T17:59:51",
        ///        "active": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">'Patient' has been created</response>
        /// <response code="400">Invalid parameters</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody] Patient patient)
        {
            Patients.Creation creation = Patients.Creation.New();
            creation.Id = patient.Id;
            creation.Use = patient.Name.Use;
            creation.Family = patient.Name.Family;
            creation.Given = patient.Name.Given;
            creation.Gender = patient.Gender;
            creation.BirthDate = patient.BirthDate;
            creation.Active = patient.Active;

            IResult result = await creation.Go();

            return new OutputResult(result);
        }

        /// <summary>
        /// Update 'Patient'.
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "name": {
        ///          "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///          "use": "official",
        ///          "family": "Иванов",
        ///          "given": [
        ///            "Иван",
        ///            "Иванович"
        ///          ]
        ///        },
        ///        "gender": 1,
        ///        "birthDate": "2024-03-09T17:59:51",
        ///        "active": true
        ///     }
        ///
        /// </remarks>
        /// <response code="204">'Patient' has been updated</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="404">'Patient' not found</response>
        [HttpPut]
        [ProducesResponseType(204)]
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

            IResult result = await updating.Go();

            return new OutputResult(result);
        }

        /// <summary>
        /// Delete 'Patient'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">'Patient' has been deleted</response>
        /// <response code="404">'Patient' not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Patients.Deleting deleting = Patients.Deleting.New();
            deleting.Id = id;

            IResult result = await deleting.Go();

            return new OutputResult(result);
        }
    }
}
