using System;
using System.Threading.Tasks;

using Core;
using Core.Entities;


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
                this.worker = Worker.New();
            }
            #endregion

            #region Fields
            private readonly IWorker worker;
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
                Patient patient = await this.worker.Patients.GetId(this.Id);
                if (patient == null)
                    return Result.NotFound;

                HumanName humanName = await this.worker.HumanNames.GetId(patient.HumanNameId);
                if (humanName == null)
                    return Result.NotFound;

                this.worker.HumanNames.Delete(humanName);
                this.worker.Patients.Delete(patient);

                this.worker.Save();

                return Result.NoContent;
            }
            #endregion
        }
    }
}
