using MSNK.Models.Modules.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.IRepository
{
    public interface CustomIRepository<T1, T2>
    {
        Task<decimal> GetBalanceFromAbBukuVot(T1 tahun, int? akCartaId, int jKW, int? jBahagian);
        Task<decimal> GetBalanceFromKaunterPanjar(T1 bakiAwal, T2 akTunaiRuncitId);
        // Penyata Buku Tunai
        Task<decimal> GetCarryPreviousBalanceBasedOnStartingDate(T2 akBankId, int? JKWId, int? JBahagianId, DateTime TarMula);
        Task<List<AbBukuTunaiViewModel>> GetListBukuTunaiBasedOnRangeDate(T2 akBankId, int? JKWId, int? JBahagianId, DateTime TarMula, DateTime TarHingga);
        // Penyata Buku Tunai END

        // Penyata Alir Tunai
        Task<AbAlirTunaiViewModel> GetCarryPreviousBalanceEachStartingMonth(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiMasukBasedOnYear(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiKeluarBasedOnYear(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        // Penyata Alir Tunai END
    }
}
