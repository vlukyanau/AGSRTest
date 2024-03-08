namespace Logic.Patients
{
    public static partial class Patients
    {
        public sealed class Creation
        {
            #region Static
            public static Creation New()
            {
                return new Creation();
            }
            #endregion

            #region Methods
            public Task<Guid> Go()
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
