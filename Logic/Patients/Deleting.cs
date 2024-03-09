using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Logic.Entities;


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
            public Guid Id { get; set; }
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

                    return Result.BadRequest;
                }
            }
            #endregion

            #region Assistants
            private async Task<IResult> Process()
            {
                using (ApplicationContext context = new ApplicationContext())
                {
                    Patient patient = await context.Patients.FirstOrDefaultAsync(item => item.Id == this.Id);
                    if (patient == null)
                        return Result.NotFound;

                    context.Patients.Remove(patient);

                    await context.SaveChangesAsync();
                }

                return Result.NoContent;
            }
            #endregion
        }
    }
}
