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
                    await Task.CompletedTask;

                    return new OkResult();
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
                if (this.id == null)
                    return false;

                return true;
            }
            #endregion
        }
    }
}
