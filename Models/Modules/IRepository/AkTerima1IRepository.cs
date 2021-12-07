using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface AkTerima1IRepository<T1, T2> where T1 : class
    {
        Task<IEnumerable<T1>> GetAll(T2 akTerimaId);
        Task<T1> GetById(T2 id);
        Task<T1> GetBy2Id(T2 akTerimaId, T2 akCartaId);
        Task<T1> Insert(T1 entity);
        Task Delete(T2 id);
        Task Save();
        Task Update(T1 entity);

    }
}
