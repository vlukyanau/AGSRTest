using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> GetAll();
        Task<T> GetId(Guid id);

        Task Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
