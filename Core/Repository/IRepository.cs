using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetId(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity); 
        void Delete(T entity);
        void Update(T entity);
    }
}
