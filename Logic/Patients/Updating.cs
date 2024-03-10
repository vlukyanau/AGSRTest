using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Core;
using Core.Entities;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Updating
        {
            #region Static
            public static Updating New()
            {
                return new Updating();
            }
            #endregion

            #region Constructors
            private Updating()
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
                if (this.Id == null)
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
                Patient patient = await this.work.Patients.GetId((Guid)this.Id);
                if (patient == null)
                    return Result.BadRequest;

                HumanName name = await this.work.HumanNames.GetId(patient.HumanNameId);
                if (name == null)
                    return Result.BadRequest;

                patient.Gender = this.Gender;
                patient.BirthDate = (DateTime)this.BirthDate;
                patient.Active = this.Active;

                name.Use = this.Use;
                name.Family = this.Family;
                name.Given = this.Given;

                this.work.HumanNames.Update(name);
                this.work.Patients.Update(patient);

                this.work.Save();

                return Result.NoContent;
            }
            #endregion
        }
    }
}
