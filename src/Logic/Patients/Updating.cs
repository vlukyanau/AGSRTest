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
                    this.Verify();

                    IResult result = await this.Process();

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.New(exception);
                }
            }
            #endregion

            #region Assistants
            private void Verify()
            {
                if (this.Id == null)
                    throw new ArgumentNullException(nameof(this.Id));

                if (string.IsNullOrWhiteSpace(this.Use) == true)
                    throw new ArgumentNullException(nameof(this.Use));

                if (string.IsNullOrWhiteSpace(this.Family) == true)
                    throw new ArgumentNullException($"{nameof(this.Family)} cannot be null or whitespace");

                if (this.Given != null && this.Given.Any(string.IsNullOrWhiteSpace) == true)
                    throw new ArgumentNullException($"{nameof(this.Given)} cannot be null or whitespace");

                if (this.Gender != null && Enum.IsDefined(typeof(Gender), this.Gender) == false)
                    throw new ArgumentNullException($"{nameof(this.Gender)} not valid");

                if (this.BirthDate == null)
                    throw new ArgumentNullException(nameof(this.Use));
            }

            private async Task<IResult> Process()
            {
                Patient patient = await this.worker.Patients.GetId((Guid)this.Id);
                if (patient == null)
                    return Result.NotFound;

                HumanName humanName = await this.worker.HumanNames.GetId(patient.HumanNameId);
                if (humanName == null)
                    return Result.NotFound;

                patient.Gender = this.Gender;
                patient.BirthDate = (DateTime)this.BirthDate;
                patient.Active = this.Active;

                humanName.Use = this.Use;
                humanName.Family = this.Family;
                humanName.Given = this.Given;

                this.worker.HumanNames.Update(humanName);
                this.worker.Patients.Update(patient);

                this.worker.Save();

                return Result.NoContent;
            }
            #endregion
        }
    }
}
