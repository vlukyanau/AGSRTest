using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core;
using Core.Repository;


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
                this.work = Work.New();
            }
            #endregion

            #region Fields
            private readonly IWork work;
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
                Patient patient = await this.work.Patients.GetId(this.Id);
                if (patient == null)
                    return Result.NotFound;

                HumanName name = await this.work.HumanNames.GetId(patient.HumanNameId);
                if (name == null)
                    return Result.NotFound;

                this.work.HumanNames.Delete(name);
                this.work.Patients.Delete(patient);

                this.work.Save();

                return Result.NoContent;
            }
            #endregion
        }
    }
}
