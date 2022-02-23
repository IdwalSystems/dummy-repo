using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface IRepository<T1, T2, T3> where T1 :class
    {
        Task<IEnumerable<T1>> GetAll();
        Task<IEnumerable<T1>> GetAllIncludeDeletedItems();
        Task<T1> GetById(T2 id);
        Task<T1> GetByIdForDeletedItems(T2 id);
        Task<T1> GetByString(T3 id);
        Task<T1> Insert(T1 entity);
        Task Delete(T2 id);
        Task Save();
        Task Update(T1 entity);
        
    }
}
