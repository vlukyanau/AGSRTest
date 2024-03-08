using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Logic.Entities;


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

            #region Properties
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
                    Patient patient = Patient.New(this.Family, (DateTime)this.BirthDate);
                    patient.Name.Use = this.Use;
                    patient.Name.Given.AddRange(this.Given);
                    patient.Gender = this.Gender;
                    patient.Active = this.Active;

                    await context.AddAsync(patient);

                    await context.SaveChangesAsync();
                }

                return new OkResult();
            }
            #endregion
        }
    }
}
