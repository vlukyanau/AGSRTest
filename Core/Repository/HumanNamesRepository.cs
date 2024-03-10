using Core.Entities;


namespace Core.Repository
{
    internal class HumanNamesRepository : Repository<HumanName>, IHumanNamesRepository
    {
        public static IHumanNamesRepository New(ApplicationContext context)
        {
            return new HumanNamesRepository(context);
        }

        private HumanNamesRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
