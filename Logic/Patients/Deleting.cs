using System;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using Logic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Deleting
        {
            #region Static
            public static Deleting New()
            {
                return new Deleting();
            }
            #endregion

            #region Constructor
            private Deleting()
            {
            }
            #endregion

            #region Properties
            public Guid? Id { get; set; }
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

                return true;
            }

            private async Task<StatusCodeResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.SingleAsync(item => item.Id == this.Id);
                    if (patient == null)
                        return new NotFoundResult();

                    context.Patients.Remove(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
