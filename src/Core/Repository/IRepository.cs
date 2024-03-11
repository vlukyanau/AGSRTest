using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetId(Guid id);

        Task<IReadOnlyList<T>> Where(Expression<Func<T,bool>> predicate);

        Task Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
