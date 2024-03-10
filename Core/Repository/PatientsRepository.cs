using Core.Entities;


namespace Core.Repository
{
    public class ProductRepository : Repository<Patient>, IPatientRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
