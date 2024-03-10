using Core.Entities;


namespace Core.Repository
{
    internal class PatientsRepository : Repository<Patient>, IPatientsRepository
    {
        public static PatientsRepository New(ApplicationContext context)
        {
            return new PatientsRepository(context);
        }

        private PatientsRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
