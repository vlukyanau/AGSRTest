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
            public Task<IReadOnlyList<IPatient>> Go()
            {
                throw new NotImplementedException();
            }
            public Task<IPatient> Go(string id)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
