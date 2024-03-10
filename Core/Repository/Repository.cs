using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext context;

        protected Repository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<T> GetId(int id)
        {
            return await this.context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            this.context.Set<T>().Update(entity);
        }
    }
}
