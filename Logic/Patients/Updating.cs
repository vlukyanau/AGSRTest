using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Logic.Entities;


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
            public async Task<StatusCodeResult> Go()
            {
                try
                {
                    if (this.Verify() == false)
                        return new BadRequestResult();

                    StatusCodeResult result = await this.Process();

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return new BadRequestResult();
                }
            }
            #endregion

            #region Assistants
            private bool Verify()
            {
                if (this.Id == null)
                    return false;

                if (this.Family == null)
                    return false;

                if (this.BirthDate == null)
                    return false;

                return true;
            }

            private async Task<StatusCodeResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.Include(item => item.Name).SingleAsync(item => item.Id == this.Id);
                    if (patient == null)
                        return new NotFoundResult();

                    patient.Name.Use = this.Use;
                    patient.Name.Family = this.Family;
                    patient.Name.Given = this.Given;
                    patient.Gender = this.Gender;
                    patient.BirthDate = (DateTime)this.BirthDate;
                    patient.Active = this.Active;

                    context.Update(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
