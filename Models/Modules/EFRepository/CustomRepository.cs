using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class CustomRepository : CustomIRepository<string, int>
    {

        public readonly ApplicationDbContext context;
        public readonly UserService _userService;
        public CustomRepository(ApplicationDbContext context,
            UserService userService)
        {
            this.context = context;
            _userService = userService;
        }

        public async Task<decimal> GetBalanceFromAbBukuVot(string tahun, int? akCartaId, int jKWId, int? jBahagianId)
        {

            var sql = (from tbl in await context.AbBukuVot
                       .Include(x => x.Vot)
                       .Include(x => x.JKW)
                       .Include(x => x.JBahagian)
                       .Where(x => x.Tahun == tahun && x.VotId == akCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                       .ToListAsync()
                       select new
                       {
                           Id = tbl.VotId,
                           Tahun = tbl.Tahun,
                           KW = tbl.JKW.Kod,
                           Bahagian = tbl.JBahagian.Kod,
                           KodAkaun = tbl.Vot.Kod,
                           Perihal = tbl.Vot.Perihal,
                           Debit = tbl.Debit,
                           Kredit = tbl.Kredit,
                           Tanggungan = tbl.Tanggungan,
                           Liabiliti = tbl.Liabiliti,
                           Baki = tbl.Baki
                       }).GroupBy(x => new { x.Tahun, x.KodAkaun, x.KW, x.Bahagian }).FirstOrDefault();

            return sql.Select(t => t.Baki + t.Kredit - t.Debit - t.Tanggungan - t.Liabiliti).Sum();
        }

        public async Task<decimal> GetBalanceFromKaunterPanjar(string bakiAwal, int akTunaiRuncitId)
        {
            // baki awal
            List<AkTunaiLejar> tunaiLejar = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup == bakiAwal)
                .OrderBy(b => b.Tarikh)
                .ToListAsync();

            // rekupan
            List<AkTunaiLejar> tunaiLejarRekup = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup != bakiAwal && b.Rekup != null)
                .OrderBy(b => b.Rekup).ThenBy(b => b.Tarikh)
                .ToListAsync();

            tunaiLejar.AddRange(tunaiLejarRekup);
            // belum rekup
            List<AkTunaiLejar> tunaiLejarBelumRekup = await context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == akTunaiRuncitId && b.Rekup == null)
                .OrderBy(b => b.Tarikh)
                .ToListAsync();

            tunaiLejar.AddRange(tunaiLejarBelumRekup);

            decimal baki = 0;

            if (tunaiLejar != null)
            {
                foreach (var balance in tunaiLejar)
                {
                    baki = baki + balance.Debit - balance.Kredit;
                }

            }

            return baki;
        }

        public async Task<decimal> GetCarryPreviousBalanceBasedOnStartingDate(int akBankId, int? JKWId, int? JBahagianId, DateTime TarMula)
        {
            var company = await _userService.GetCompanyDetails();

            List<AkAkaun> akAkaun = await context.AkAkaun.Where(b => b.Tarikh > company.TarMula
                && b.Tarikh < TarMula).ToListAsync();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            decimal previousBalance = 0;

            foreach (var item in akAkaun)
            {
                previousBalance = previousBalance + item.Debit - item.Kredit;
            }

            return previousBalance;
        }

        public async Task<List<AbBukuTunaiViewModel>> GetListBukuTunaiBasedOnRangeDate(int akBankId, int? JKWId, int? JBahagianId, DateTime TarMula, DateTime TarHingga)
        {
            var bukuTunai = new List<AbBukuTunaiViewModel>();

            //var previousBalance = await GetCarryPreviousBalanceBasedOnStartingDate(akBankId, JKWId, JBahagianId, TarMula);

            // search CartaId from AkBankId
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            // PV
            List<AkAkaun> bukuTunaiPV = await context.AkAkaun
                .Include(b => b.AkCarta2)
                .Where(b => b.NoRujukan.Contains("PV")
                && b.Tarikh >= TarMula && b.Tarikh <= TarHingga
                && b.AkCartaId1 == akBank.AkCartaId
                && b.Kredit != 0).OrderBy(b => b.Tarikh).ToListAsync();
            
            if (JKWId != 0)
            {
                bukuTunaiPV = bukuTunaiPV.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                bukuTunaiPV = bukuTunaiPV.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            decimal jumlahKeluar = 0;
            foreach (var item in bukuTunaiPV)
            {
                jumlahKeluar += item.Kredit;

                bukuTunai.Add(new AbBukuTunaiViewModel()
                {
                    TarMasuk = null,
                    NamaAkaunMasuk = "",
                    NoRujukanMasuk = "",
                    AmaunMasuk = 0,
                    JumlahMasuk = 0,
                    TarKeluar = item.Tarikh,
                    NamaAkaunKeluar = item.AkCarta2.Perihal,
                    NoRujukanKeluar = item.NoRujukan,
                    AmaunKeluar = item.Kredit,
                    JumlahKeluar = jumlahKeluar,
                    KeluarMasuk = 1
                });
            }
            // PV end
            // Terima
            List<AkAkaun> bukuTunaiResit = await context.AkAkaun
                .Include(b => b.AkCarta2)
                .Where(b => b.NoRujukan.Contains("RR")
                && b.Tarikh >= TarMula && b.Tarikh <= TarHingga
                && b.AkCartaId1 == akBank.AkCartaId
                && b.Debit != 0).OrderBy(b => b.Tarikh).ToListAsync();

            if (JKWId != 0)
            {
                bukuTunaiResit = bukuTunaiResit.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                bukuTunaiResit = bukuTunaiResit.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            decimal jumlahMasuk = 0;
            foreach (var item in bukuTunaiResit)
            {
                jumlahMasuk += item.Debit;

                bukuTunai.Add(new AbBukuTunaiViewModel()
                {
                    TarMasuk = item.Tarikh,
                    NamaAkaunMasuk = item.AkCarta2.Perihal,
                    NoRujukanMasuk = item.NoRujukan,
                    AmaunMasuk = item.Debit,
                    JumlahMasuk = jumlahMasuk,
                    TarKeluar = null,
                    NamaAkaunKeluar = "",
                    NoRujukanKeluar = "",
                    JumlahKeluar = 0,
                    KeluarMasuk = 0
                });
            }
            // Terima end
            // Jurnal1
            // refer AkBank, if debit = masuk, if kredit = keluar
            List<AkAkaun> bukuTunaiJurnal = await context.AkAkaun
                .Include(b => b.AkCarta2)
                .Where(b => b.NoRujukan.Contains("JU")
                && b.Tarikh >= TarMula && b.Tarikh <= TarHingga
                && b.AkCartaId1 == akBank.AkCartaId).OrderBy(b => b.Tarikh).ToListAsync();

            if (JKWId != 0)
            {
                bukuTunaiJurnal = bukuTunaiJurnal.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                bukuTunaiJurnal = bukuTunaiJurnal.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            foreach (var item in bukuTunaiJurnal)
            {

                jumlahMasuk += item.Debit;
                jumlahKeluar += item.Kredit;
                if (item.Debit != 0)
                {
                    bukuTunai.Add(new AbBukuTunaiViewModel()
                    {
                        TarMasuk = item.Tarikh,
                        NamaAkaunMasuk = item.AkCarta2.Perihal,
                        NoRujukanMasuk = item.NoRujukan,
                        AmaunMasuk = item.Debit,
                        JumlahMasuk = jumlahMasuk,
                        TarKeluar = null,
                        NamaAkaunKeluar = "",
                        NoRujukanKeluar = "",
                        AmaunKeluar = 0,
                        JumlahKeluar = 0,
                        KeluarMasuk = 0
                    });
                }
                else
                {
                    bukuTunai.Add(new AbBukuTunaiViewModel()
                    {
                        TarMasuk = null,
                        NamaAkaunMasuk = "",
                        NoRujukanMasuk = "",
                        AmaunMasuk = item.Debit,
                        JumlahMasuk = jumlahMasuk,
                        TarKeluar = item.Tarikh,
                        NamaAkaunKeluar = item.AkCarta2.Perihal,
                        NoRujukanKeluar = item.NoRujukan,
                        AmaunKeluar = item.Kredit,
                        JumlahKeluar = jumlahKeluar,
                        KeluarMasuk = 1
                    });
                }
            }
            // search CartaId from AkBankId

            // jurnal1 end

            return bukuTunai.OrderBy(b => b.KeluarMasuk).ThenBy(b => b.TarMasuk).ThenBy(b => b.TarKeluar).ToList();
        }
    }
}
