using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetId(Guid id);
        DbSet<T> GetAll();

        Task Add(T entity);

        void Delete(T entity);
        void Update(T entity);
    }
}
