using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        #region Constructors
        protected Repository(ApplicationContext context)
        {
            this.context = context;
        }
        #endregion

        #region Fields
        protected readonly ApplicationContext context;
        #endregion

        #region Methods
        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await this.context.Set<T>().ToListAsync();
        }
        public async Task<T> GetId(Guid id)
        {
            return await this.context.Set<T>().FindAsync(id);
        }
        
        public async Task<IReadOnlyList<T>> Where(Expression<Func<T, bool>> predicate)
        {
            await Task.CompletedTask;

            return this.context.Set<T>().Where(predicate).ToList();
        }

        public async Task Add(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
        }


        public void Update(T entity)
        {
            this.context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }
        #endregion
    }
}
