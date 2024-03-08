using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

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
                    if (this.Verify() == false)
                        return new List<IPatient>();
                    //return new BadRequestResult();

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        IReadOnlyList<IPatient> result = await this.Process();

                        transaction.Complete();

                        return result;
                    }
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
                    if (this.Verify() == false)
                        return Patient.New("Test", DateTime.Now);
                    //return new BadRequestResult();

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        IPatient result = await this.Process(id);

                        transaction.Complete();

                        return result;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return Patient.New("Test", DateTime.Now);
                    //return new BadRequestResult();
                }
            }
            #endregion

            #region Assistants
            private bool Verify()
            {
                return true;
            }

            private async Task<IReadOnlyList<IPatient>> Process()
            {
                await Task.CompletedTask;

                return new List<IPatient>();
            }
            private async Task<IPatient> Process(string id)
            {
                await Task.CompletedTask;

                return Patient.New("Test", DateTime.Now);
            }
            #endregion
        }
    }
}
