using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface CustomIRepository<T1, T2>
    {
        Task<decimal> GetBalanceFromAbBukuVot(T1 tahun, T2 akCartaId);
    }
}
