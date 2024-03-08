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
                this.id = string.Empty;
            }
            #endregion

            #region Fields
            private string id;
            #endregion

            #region Properties
            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
            }
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
                if (string.IsNullOrWhiteSpace(this.id) == true)
                    return false;

                if (Guid.TryParse(this.id, out _) == false)
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

                    context.Patients.Remove(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
