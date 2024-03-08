using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Logic.Entities;
using Microsoft.AspNetCore.Mvc;


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
                return true;
            }

            private async Task<StatusCodeResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = Patient.New("Test", DateTime.UtcNow);
                    patient.Name.Use = "official";
                    patient.Name.Given.Add("Иван");
                    patient.Name.Given.Add("Иванович");
                    patient.Gender = Gender.Male;
                    patient.Active = true;

                    await context.AddAsync(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
