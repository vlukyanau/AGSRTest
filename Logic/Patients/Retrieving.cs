using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.EntityFrameworkCore;

using Logic.Entities;


namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Retrieving
        {
            #region Static
            public static Retrieving New()
            {
                return new Retrieving();
            }
            #endregion

            #region Methods
            public async Task<IReadOnlyList<IPatient>> Go()
            {
                try
                {
                    IReadOnlyList<IPatient> result = await this.Process();

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return new List<IPatient>();
                    //return new BadRequestResult();
                }
            }
            public async Task<IPatient> Go(string id)
            {
                try
                {
                    IPatient result = await this.Process(id);

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Patient.New("Error", DateTime.Now);
                    //return new BadRequestResult();
                }
            }
            #endregion

            #region Assistants
            private async Task<IReadOnlyList<IPatient>> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    IReadOnlyList<Patient> patients = await context.Patients.Include(patient => patient.Name).ToListAsync();

                    return patients;
                }
            }
            private async Task<IPatient> Process(string id)
            {
                if (Guid.TryParse(id, out Guid guid) == false)
                    return null;

                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.Include(patient => patient.Name).SingleAsync(item => item.Id == Guid.Parse(id));

                    return patient;
                }
            }
            #endregion
        }
    }
}
