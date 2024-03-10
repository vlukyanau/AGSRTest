using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core;


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
                this.context = new ApplicationContext();
            }
            #endregion

            #region Fields
            private readonly ApplicationContext context;
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
            private async Task<IResult> Process()
            {
                Patient patient = await this.context.Patients.FirstOrDefaultAsync(item => item.Id == this.Id);
                if (patient == null)
                    return Result.NotFound;

                this.context.Patients.Remove(patient);

                return Result.NoContent;
            }
            #endregion
        }
    }
}
