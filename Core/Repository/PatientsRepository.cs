using Core.Entities;


namespace Core.Repository
{
    internal class PatientsRepository : Repository<Patient>, IPatientsRepository
    {
        #region Static
        public static PatientsRepository New(ApplicationContext context)
        {
            return new PatientsRepository(context);
        }
        #endregion

        #region Constructors
        private PatientsRepository(ApplicationContext context) : base(context)
        {
        }
        #endregion
    }
}
