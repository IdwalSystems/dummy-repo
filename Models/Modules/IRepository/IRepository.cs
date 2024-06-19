using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface IRepository<T1, T2, T3> where T1 :class
    {
        Task<IEnumerable<T1>> GetAll(string filter);
        Task<IEnumerable<T1>> GetAllIncludeDeletedItems();
        Task<T1> GetById(T2 id);
        Task<T1> GetByIdIncludeDeletedItems(T2 id);
        Task<T1> GetByString(T3 id);
        Task<T1> Insert(T1 entity);
        Task Delete(T2 id);
        Task Save();
        Task Update(T1 entity);
        string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList);
        string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList);
        JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti);
    }
}
