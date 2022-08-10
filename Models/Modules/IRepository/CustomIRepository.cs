using MSNK.Models.Modules.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface CustomIRepository<T1, T2>
    {
        Task<List<AbWaran>> GetAbWaranBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);
        Task<decimal> GetBalanceFromAbBukuVot(T1 tahun, int? akCartaId, int jKW, int? jBahagian);
        Task<decimal> GetBalanceFromKaunterPanjar(T1 bakiAwal, T2 akTunaiRuncitId);
    }
}
