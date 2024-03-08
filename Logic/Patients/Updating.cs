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
            public Task<StatusCodeResult> Go()
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
