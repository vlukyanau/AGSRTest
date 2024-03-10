using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core;


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
                this.context = new ApplicationContext();
            }
            #endregion

            #region Fields
            private readonly ApplicationContext context;
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

                    await this.context.SaveChangesAsync();
                    await this.context.DisposeAsync();

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
                Patient patient = await this.context.Patients.Include(item => item.Name).FirstOrDefaultAsync(item => item.Id == this.Id);
                if (patient == null)
                    return Result.BadRequest;

                patient.Name.Use = this.Use;
                patient.Name.Family = this.Family;
                patient.Name.Given = this.Given;
                patient.Gender = this.Gender;
                patient.BirthDate = (DateTime)this.BirthDate;
                patient.Active = this.Active;

                this.context.Update(patient);

                return Result.NoContent;
            }
            #endregion
        }
    }
}
