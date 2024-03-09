using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            public async Task<IResult> Go()
            {
                try
                {
                    IResult result = await this.Process();

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.Fail;
                }
            }
            public async Task<IResult> Go(Guid id)
            {
                try
                {
                    IResult result = await this.Process(id);

                    return result;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Result.Fail;
                }
            }
            #endregion

            #region Assistants
            private async Task<IResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    IReadOnlyList<Patient> patients = await context.Patients.Include(patient => patient.Name).ToListAsync();

                    return Result.New(patients);
                }
            }
            private async Task<IResult> Process(Guid id)
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.Include(patient => patient.Name).SingleAsync(item => item.Id == id);
                    if (patient == null)
                        return Result.NotFound;

                    return Result.New(patient);
                }
            }
            #endregion
        }
    }
}
