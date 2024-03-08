using System;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.AspNetCore.Mvc;


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
                this.id = string.Empty;
            }
            #endregion

            #region Fields
            private string id;
            #endregion

            #region Properties
            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
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
                if (string.IsNullOrWhiteSpace(this.id) == true)
                    return false;

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
