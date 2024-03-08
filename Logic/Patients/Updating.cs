using System;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.AspNetCore.Mvc;

namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Updating
        {
            #region Static
            public static Updating New()
            {
                return new Updating();
            }
            #endregion

            #region Methods
            public async Task<StatusCodeResult> Go()
            {
                try
                {
                    if (this.Verify() == false)
                        return new BadRequestResult();

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        StatusCodeResult result = await this.Process();

                        transaction.Complete();

                        return result;
                    }
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
                await Task.CompletedTask;

                return new OkResult();
            }
            #endregion
        }
    }
}
