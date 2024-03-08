using System;
using System.Threading.Tasks;

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
            public string Id { get; set; }
            public Gender? Gender { get; set; }
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
                if (Guid.TryParse(this.Id, out _) == false)
                    return false;

                return true;
            }

            private async Task<StatusCodeResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.SingleAsync(item => item.Id == Guid.Parse(this.Id));
                    if (patient == null)
                        return new NotFoundResult();

                    if (this.Gender != null)
                        patient.Gender = this.Gender;

                    context.Update(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
