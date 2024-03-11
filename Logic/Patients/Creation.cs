using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core;
using Core.Entities;

using Logic.Models;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Creation
        {
            #region Static
            public static Creation New()
            {
                return new Creation();
            }
            #endregion

            #region Constructors
            private Creation()
            {
                this.worker = Worker.New();
            }
            #endregion

            #region Fields
            private readonly IWorker worker;
            #endregion

            #region Properties
            public Guid? Id { get; set; }
            public string Use { get; set; }
            public string Family { get; set; }
            public List<string> Given { get; set; }
            public Gender? Gender { get; set; }
            public DateTime? BirthDate { get; set; }
            public bool? Active { get; set; }
            #endregion

            #region Methods
            public async Task<IResult> Go()
            {
                try
                {
                    if (this.Verify() == false)
                        return Result.BadRequest;

                    IResult result = await this.Process();

                    return result;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.BadRequest;
                }
            }
            #endregion

            #region Assistants
            private bool Verify()
            {
                if (this.Id != null)
                    return false;

                if (string.IsNullOrWhiteSpace(this.Use) == true)
                    return false;

                if (string.IsNullOrWhiteSpace(this.Family) == true)
                    return false;

                if (this.Given.Count == 0)
                    return false;

                if (this.Given.Any(string.IsNullOrWhiteSpace) == true)
                    return false;

                if (this.Gender != null && Enum.IsDefined(typeof(Gender), this.Gender) == false)
                    return false;

                if (this.BirthDate == null)
                    return false;

                if (this.BirthDate.Value.ToUniversalTime() > DateTime.UtcNow)
                    return false;

                return true;
            }

            private async Task<IResult> Process()
            {
                HumanName humanName = HumanName.New();
                humanName.Use = this.Use;
                humanName.Family = this.Family;
                humanName.Given.AddRange(this.Given);

                Patient patient = Patient.New(humanName.Id);
                patient.Gender = this.Gender;
                patient.BirthDate = this.BirthDate.Value.ToUniversalTime();
                patient.Active = this.Active;

                await this.worker.HumanNames.Add(humanName);
                await this.worker.Patients.Add(patient);

                this.worker.Save();

                PatientInfo info = PatientInfo.New(patient, humanName);

                return Result.New(info, Result.Created);
            }
            #endregion
        }
    }
}
