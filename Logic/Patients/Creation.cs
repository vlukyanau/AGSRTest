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
                this.work = Work.New();
            }
            #endregion

            #region Fields
            private readonly IWork work;
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

                return true;
            }

            private async Task<IResult> Process()
            {
                HumanName name = HumanName.New();
                name.Use = this.Use;
                name.Family = this.Family;
                name.Given.AddRange(this.Given);

                Patient patient = Patient.New(name.Id);
                patient.Gender = this.Gender;
                patient.BirthDate = ((DateTime)this.BirthDate).ToUniversalTime();
                patient.Active = this.Active;

                await this.work.HumanNames.Add(name);
                await this.work.Patients.Add(patient);

                this.work.Save();

                PatientInfo info = PatientInfo.New(patient, name);

                return Result.New(info, Result.Created);
            }
            #endregion
        }
    }
}
