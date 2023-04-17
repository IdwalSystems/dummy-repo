using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using MSNK.Models.Operations;
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

            return sql.Select(t => t.Baki + t.Kredit - t.Debit - t.Tanggungan).Sum();
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

            var startingYear = company.TarMula.Year;

            var akBank = await context.AkBank.Include(b => b.AkCarta).FirstOrDefaultAsync(b => b.Id == akBankId);
            decimal previousBalance = 0;

            if (akBank != null)
            {
                List<AkAkaun> akAkaun = await context.AkAkaun.Where(b => b.AkCartaId1 == akBank.AkCartaId && b.Tarikh < TarMula).ToListAsync();

                if (JKWId != 0)
                {
                    akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
                }

                if (JBahagianId != 0)
                {
                    akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
                }

                

                foreach (var item in akAkaun)
                {
                    previousBalance = previousBalance + item.Debit - item.Kredit;
                }
            }
            

            return previousBalance;
        }

        public async Task<List<AbBukuTunaiViewModel>> GetListBukuTunaiBasedOnRangeDate(int akBankId, int? JKWId, int? JBahagianId, DateTime TarMula, DateTime TarHingga)
        {
            var bukuTunai = new List<AbBukuTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();
            var startingYear = company.TarMula.Year;
            //var previousBalance = await GetCarryPreviousBalanceBasedOnStartingDate(akBankId, JKWId, JBahagianId, TarMula);

            // search CartaId from AkBankId
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            // PV
            List<AkAkaun> bukuTunaiPV = await context.AkAkaun
                .Include(b => b.AkCarta2)
                .Where(b => b.NoRujukan.Contains("PV")
                && b.Tarikh.Year >= startingYear
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
                && b.Tarikh.Year >= startingYear
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
                && b.Tarikh.Year >= startingYear
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

        public async Task<List<AbAlirTunaiViewModel>> GetCarryPreviousBalanceEachStartingMonthDebug(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> bakiAwal = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();

            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            DateTime untilDate = new DateTime(int.Parse(Tahun), 12, 31, 23, 59, 59);

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh > company.TarMula.AddYears(-1)
                && b.Tarikh < untilDate
                && b.Debit != 0).ToList();

            decimal amaunJan = 0;
            decimal amaunFeb = 0;
            decimal amaunMac = 0;
            decimal amaunApr = 0;
            decimal amaunMei = 0;
            decimal amaunJun = 0;
            decimal amaunJul = 0;
            decimal amaunOgo = 0;
            decimal amaunSep = 0;
            decimal amaunOkt = 0;
            decimal amaunNov = 0;
            decimal amaunDis = 0;
            decimal amaunJan2 = 0;
            decimal amaunJum = 0;

            // Masuk
            if (Tahun == company.TarMula.AddYears(-1).ToString("yyyy"))
            {
                akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh >= company.TarMula.AddYears(-1) && b.Tarikh < untilDate)
                .ToList();
            }


            if (JKWId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }



            if (JBahagianId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }


            foreach (var a in akAkaun)
            {
                //if (a.Tarikh.Year <= int.Parse(Tahun))
                //{
                if (a.AkCarta2 != null)
                {
                    if (a.AkCarta2.Kod == "B52201")
                    {

                    }
                }
                
                //}
                amaunJan = 0;
                amaunFeb = 0;
                amaunMac = 0;
                amaunApr = 0;
                amaunMei = 0;
                amaunJun = 0;
                amaunJul = 0;
                amaunOgo = 0;
                amaunSep = 0;
                amaunOkt = 0;
                amaunNov = 0;
                amaunDis = 0;
                amaunJan2 = 0;
                amaunJum = 0;

                DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);

                if (a.Tarikh.Year < int.Parse(Tahun))
                {
                    amaunJan = a.Debit;
                    amaunJum = a.Debit;

                    amaunFeb = a.Debit;
                    amaunMac = a.Debit;
                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
                if (a.Tarikh.Month < feb.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunFeb = a.Debit;
                    amaunJum = a.Debit;

                    amaunMac = a.Debit;
                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
                if (a.Tarikh.Month < mac.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMac = a.Debit;
                    amaunJum = a.Debit;

                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
                if (a.Tarikh.Month < apr.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunApr = a.Debit;
                    amaunJum = a.Debit;

                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
                if (a.Tarikh.Month < mei.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMei = a.Debit;
                    amaunJum = a.Debit;

                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
                if (a.Tarikh.Month < jun.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJun = a.Debit;
                    amaunJum = a.Debit;

                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
                if (a.Tarikh.Month < jul.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJul = a.Debit;
                    amaunJum = a.Debit;

                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
                if (a.Tarikh.Month < ogo.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOgo = a.Debit;
                    amaunJum = a.Debit;

                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
                if (a.Tarikh.Month < sep.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunSep = a.Debit;
                    amaunJum = a.Debit;

                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
                if (a.Tarikh.Month < okt.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOkt = a.Debit;
                    amaunJum = a.Debit;

                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                }
                DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
                if (a.Tarikh.Month < nov.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunNov = a.Debit;
                    amaunJum = a.Debit;

                    amaunDis = a.Debit;
                }
                DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
                if (a.Tarikh.Month < dis.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunDis = a.Debit;
                    amaunJum = a.Debit;
                }
                DateTime jan2 = new DateTime(int.Parse(Tahun) + 1, 1, 1, 0, 0, 0);
                if (a.Tarikh.Month < jan2.Month && a.Tarikh.Year == int.Parse(Tahun) + 1)
                {
                    amaunJan2 = a.Debit;
                }
                bakiAwal.Add(new AbAlirTunaiViewModel
                {
                    NoAkaun = a.AkCarta2?.Kod ?? "",
                    NamaAkaun = a.AkCarta2?.Perihal ?? "BAKI AWAL",
                    Jan = amaunJan,
                    Feb = amaunFeb,
                    Mac = amaunMac,
                    Apr = amaunApr,
                    Mei = amaunMei,
                    Jun = amaunJun,
                    Jul = amaunJul,
                    Ogo = amaunOgo,
                    Sep = amaunSep,
                    Okt = amaunOkt,
                    Nov = amaunNov,
                    Dis = amaunDis,
                    Jan2 = amaunJan2,
                    JumAkaun1 = amaunJum,
                    KeluarMasuk = 0
                });
            }
            // Masuk END

            // Keluar
            //List<AkAkaun> akAkaunK = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
            //    .Where(b => b.AkCartaId1 == akBank.AkCartaId
            //    && b.Tarikh > company.TarMula.AddYears(-1)
            //    && b.Tarikh < untilDate
            //    && b.Kredit != 0).ToList();

            //if (JKWId != 0)
            //{
            //    akAkaunK =akAkaunK.Where(b => b.JKWId == JKWId).ToList();
            //}



            //if (JBahagianId != 0)
            //{
            //    akAkaunK =akAkaunK.Where(b => b.JBahagianId == JBahagianId).ToList();
            //}

            //foreach (var a in akAkaunK)
            //{
            //    amaunJan = 0;
            //    amaunFeb = 0;
            //    amaunMac = 0;
            //    amaunApr = 0;
            //    amaunMei = 0;
            //    amaunJun = 0;
            //    amaunJul = 0;
            //    amaunOgo = 0;
            //    amaunSep = 0;
            //    amaunOkt = 0;
            //    amaunNov = 0;
            //    amaunDis = 0;
            //    amaunJan2 = 0;
            //    amaunJum = 0;

            //    DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);

            //    if (a.Tarikh.Year < int.Parse(Tahun))
            //    {
            //        amaunJan = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunFeb = -a.Kredit;
            //        amaunMac = -a.Kredit;
            //        amaunApr = -a.Kredit;
            //        amaunMei = -a.Kredit;
            //        amaunJun = -a.Kredit;
            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < feb.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunFeb = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunMac = -a.Kredit;
            //        amaunApr = -a.Kredit;
            //        amaunMei = -a.Kredit;
            //        amaunJun = -a.Kredit;
            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < mac.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunMac = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunApr = -a.Kredit;
            //        amaunMei = -a.Kredit;
            //        amaunJun = -a.Kredit;
            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < apr.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunApr = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunMei = -a.Kredit;
            //        amaunJun = -a.Kredit;
            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < mei.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunMei = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunJun = -a.Kredit;
            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < jun.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunJun = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunJul = -a.Kredit;
            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < jul.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunJul = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunOgo = -a.Kredit;
            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < ogo.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunOgo = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunSep = -a.Kredit;
            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < sep.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunSep = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunOkt = -a.Kredit;
            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < okt.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunOkt = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunNov = -a.Kredit;
            //        amaunDis = -a.Kredit;
            //    }
            //    DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
            //    if (a.Tarikh.Month < nov.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    {
            //        amaunNov = -a.Kredit;
            //        amaunJum = -a.Kredit;

            //        amaunDis = -a.Kredit;
            //    }
            //    //DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
            //    //if (a.Tarikh.Month < dis.Month && a.Tarikh.Year == int.Parse(Tahun))
            //    //{
            //    //    amaunDis = -a.Kredit;
            //    //    amaunJum = -a.Kredit;
            //    //}
            //    bakiAwal.Add(new AbAlirTunaiViewModel
            //    {
            //        NoAkaun = a.AkCarta1.Kod,
            //        NamaAkaun = a.AkCarta1.Perihal,
            //        Jan = amaunJan,
            //        Feb = amaunFeb,
            //        Mac = amaunMac,
            //        Apr = amaunApr,
            //        Mei = amaunMei,
            //        Jun = amaunJun,
            //        Jul = amaunJul,
            //        Ogo = amaunOgo,
            //        Sep = amaunSep,
            //        Okt = amaunOkt,
            //        Nov = amaunNov,
            //        Dis = amaunDis,
            //        Jan2 = amaunJan2,
            //        JumAkaun = amaunJum,
            //        KeluarMasuk = 0
            //    });
            //}
            // Keluar END

            //return bakiAwal;
            return bakiAwal.GroupBy(b => new { b.NoAkaun, b.NamaAkaun, b.KeluarMasuk })
                .Select(l => new AbAlirTunaiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    KeluarMasuk = l.First().KeluarMasuk,
                    Jan = l.Sum(c => c.Jan),
                    Feb = l.Sum(c => c.Feb),
                    Mac = l.Sum(c => c.Mac),
                    Apr = l.Sum(c => c.Apr),
                    Mei = l.Sum(c => c.Mei),
                    Jun = l.Sum(c => c.Jun),
                    Jul = l.Sum(c => c.Jul),
                    Ogo = l.Sum(c => c.Ogo),
                    Sep = l.Sum(c => c.Sep),
                    Okt = l.Sum(c => c.Okt),
                    Nov = l.Sum(c => c.Nov),
                    Dis = l.Sum(c => c.Dis),
                    Jan2 = l.Sum(c => c.Jan2),
                    JumAkaun1 = l.Sum(c => c.JumAkaun1)
                }).OrderBy(b => b.NoAkaun).ToList();
        } 

        public async Task<AbAlirTunaiViewModel> GetCarryPreviousBalanceEachStartingMonth(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> bakiAwal = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();

            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            DateTime untilDate = new DateTime(int.Parse(Tahun), 12, 31, 23, 59, 59);

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh > company.TarMula.AddYears(-1)
                && b.Tarikh < untilDate
                && b.Debit != 0).ToList();

            decimal amaunJan = 0;
            decimal amaunFeb = 0;
            decimal amaunMac = 0;
            decimal amaunApr = 0;
            decimal amaunMei = 0;
            decimal amaunJun = 0;
            decimal amaunJul = 0;
            decimal amaunOgo = 0;
            decimal amaunSep = 0;
            decimal amaunOkt = 0;
            decimal amaunNov = 0;
            decimal amaunDis = 0;
            decimal amaunJan2 = 0;
            decimal amaunJum = 0;

            // Masuk
            if (Tahun == company.TarMula.AddYears(-1).ToString("yyyy"))
            {
                akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh >= company.TarMula.AddYears(-1) && b.Tarikh <= untilDate)
                .ToList();
            }


            if (JKWId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }



            if (JBahagianId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }


            foreach (var a in akAkaun)
            {
                //if (a.Tarikh.Year <= int.Parse(Tahun))
                //{

                //}
                amaunJan = 0;
                amaunFeb = 0;
                amaunMac = 0;
                amaunApr = 0;
                amaunMei = 0;
                amaunJun = 0;
                amaunJul = 0;
                amaunOgo = 0;
                amaunSep = 0;
                amaunOkt = 0;
                amaunNov = 0;
                amaunDis = 0;
                amaunJan2 = 0;
                amaunJum = 0;

                DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);

                if (a.Tarikh.Year < int.Parse(Tahun))
                {
                    amaunJan = a.Debit;
                    amaunJum = a.Debit;

                    amaunFeb = a.Debit;
                    amaunMac = a.Debit;
                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
                if (a.Tarikh.Month < feb.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunFeb = a.Debit;
                    amaunJum = a.Debit;

                    amaunMac = a.Debit;
                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
                if (a.Tarikh.Month < mac.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMac = a.Debit;
                    amaunJum = a.Debit;

                    amaunApr = a.Debit;
                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
                if (a.Tarikh.Month < apr.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunApr = a.Debit;
                    amaunJum = a.Debit;

                    amaunMei = a.Debit;
                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
                if (a.Tarikh.Month < mei.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMei = a.Debit;
                    amaunJum = a.Debit;

                    amaunJun = a.Debit;
                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
                if (a.Tarikh.Month < jun.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJun = a.Debit;
                    amaunJum = a.Debit;

                    amaunJul = a.Debit;
                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
                if (a.Tarikh.Month < jul.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJul = a.Debit;
                    amaunJum = a.Debit;

                    amaunOgo = a.Debit;
                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
                if (a.Tarikh.Month < ogo.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOgo = a.Debit;
                    amaunJum = a.Debit;

                    amaunSep = a.Debit;
                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
                if (a.Tarikh.Month < sep.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunSep = a.Debit;
                    amaunJum = a.Debit;

                    amaunOkt = a.Debit;
                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
                if (a.Tarikh.Month < okt.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOkt = a.Debit;
                    amaunJum = a.Debit;

                    amaunNov = a.Debit;
                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
                if (a.Tarikh.Month < nov.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunNov = a.Debit;
                    amaunJum = a.Debit;

                    amaunDis = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
                if (a.Tarikh.Month < dis.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunDis = a.Debit;
                    amaunJum = a.Debit;
                    amaunJan2 = a.Debit;
                }
                DateTime jan2 = new DateTime(int.Parse(Tahun) + 1, 1, 1, 0, 0, 0);
                if (a.Tarikh.Year < int.Parse(Tahun) + 1)
                {
                    amaunJan2 = a.Debit;
                    amaunJum = a.Debit;
                }
                bakiAwal.Add(new AbAlirTunaiViewModel
                {
                    NoAkaun = a.AkCarta1.Kod,
                    NamaAkaun = a.AkCarta1.Perihal,
                    Jan = amaunJan,
                    Feb = amaunFeb,
                    Mac = amaunMac,
                    Apr = amaunApr,
                    Mei = amaunMei,
                    Jun = amaunJun,
                    Jul = amaunJul,
                    Ogo = amaunOgo,
                    Sep = amaunSep,
                    Okt = amaunOkt,
                    Nov = amaunNov,
                    Dis = amaunDis,
                    Jan2 = amaunJan2,
                    JumAkaun1 = amaunJum,
                    KeluarMasuk = 0
                });
            }
            // Masuk END

            // Keluar
            List<AkAkaun> akAkaunK = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh > company.TarMula.AddYears(-1)
                && b.Tarikh < untilDate
                && b.Kredit != 0).ToList();

            if (JKWId != 0)
            {
                akAkaunK =akAkaunK.Where(b => b.JKWId == JKWId).ToList();
            }



            if (JBahagianId != 0)
            {
                akAkaunK =akAkaunK.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            foreach (var a in akAkaunK)
            {
                amaunJan = 0;
                amaunFeb = 0;
                amaunMac = 0;
                amaunApr = 0;
                amaunMei = 0;
                amaunJun = 0;
                amaunJul = 0;
                amaunOgo = 0;
                amaunSep = 0;
                amaunOkt = 0;
                amaunNov = 0;
                amaunDis = 0;
                amaunJan2 = 0;
                amaunJum = 0;

                DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);

                if (a.Tarikh.Year < int.Parse(Tahun))
                {
                    amaunJan = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunFeb = -a.Kredit;
                    amaunMac = -a.Kredit;
                    amaunApr = -a.Kredit;
                    amaunMei = -a.Kredit;
                    amaunJun = -a.Kredit;
                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
                if (a.Tarikh.Month < feb.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunFeb = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunMac = -a.Kredit;
                    amaunApr = -a.Kredit;
                    amaunMei = -a.Kredit;
                    amaunJun = -a.Kredit;
                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
                if (a.Tarikh.Month < mac.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMac = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunApr = -a.Kredit;
                    amaunMei = -a.Kredit;
                    amaunJun = -a.Kredit;
                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
                if (a.Tarikh.Month < apr.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunApr = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunMei = -a.Kredit;
                    amaunJun = -a.Kredit;
                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
                if (a.Tarikh.Month < mei.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunMei = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunJun = -a.Kredit;
                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
                if (a.Tarikh.Month < jun.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJun = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunJul = -a.Kredit;
                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
                if (a.Tarikh.Month < jul.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunJul = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunOgo = -a.Kredit;
                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
                if (a.Tarikh.Month < ogo.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOgo = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunSep = -a.Kredit;
                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
                if (a.Tarikh.Month < sep.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunSep = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunOkt = -a.Kredit;
                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
                if (a.Tarikh.Month < okt.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunOkt = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunNov = -a.Kredit;
                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
                if (a.Tarikh.Month < nov.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunNov = -a.Kredit;
                    amaunJum = -a.Kredit;

                    amaunDis = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
                if (a.Tarikh.Month < dis.Month && a.Tarikh.Year == int.Parse(Tahun))
                {
                    amaunDis = -a.Kredit;
                    amaunJum = -a.Kredit;
                    amaunJan2 = -a.Kredit;
                }
                DateTime jan2 = new DateTime(int.Parse(Tahun) + 1, 1, 1, 0, 0, 0);
                if (a.Tarikh.Year < int.Parse(Tahun) + 1)
                {
                    amaunJan2 = -a.Kredit;
                    amaunJum = -a.Kredit;
                }
                bakiAwal.Add(new AbAlirTunaiViewModel
                {
                    NoAkaun = a.AkCarta1.Kod,
                    NamaAkaun = a.AkCarta1.Perihal,
                    Jan = amaunJan,
                    Feb = amaunFeb,
                    Mac = amaunMac,
                    Apr = amaunApr,
                    Mei = amaunMei,
                    Jun = amaunJun,
                    Jul = amaunJul,
                    Ogo = amaunOgo,
                    Sep = amaunSep,
                    Okt = amaunOkt,
                    Nov = amaunNov,
                    Dis = amaunDis,
                    Jan2 = amaunJan2,
                    JumAkaun1 = amaunJum,
                    KeluarMasuk = 0
                });
            }
            // Keluar END

            return bakiAwal.GroupBy(b => new { b.NoAkaun, b.NamaAkaun, b.KeluarMasuk })
                .Select(l => new AbAlirTunaiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    KeluarMasuk = l.First().KeluarMasuk,
                    Jan = l.Sum(c => c.Jan),
                    Feb = l.Sum(c => c.Feb),
                    Mac = l.Sum(c => c.Mac),
                    Apr = l.Sum(c => c.Apr),
                    Mei = l.Sum(c => c.Mei),
                    Jun = l.Sum(c => c.Jun),
                    Jul = l.Sum(c => c.Jul),
                    Ogo = l.Sum(c => c.Ogo),
                    Sep = l.Sum(c => c.Sep),
                    Okt = l.Sum(c => c.Okt),
                    Nov = l.Sum(c => c.Nov),
                    Dis = l.Sum(c => c.Dis),
                    Jan2 = l.Sum(c => c.Jan2),
                    JumAkaun1 = l.Sum(c => c.JumAkaun1)
                }).OrderBy(b => b.NoAkaun).FirstOrDefault();
        }

        
        public async Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiBasedOnComparedYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun1, string Tahun2, string Tahun3)
        {
            
            List<AbAlirTunaiViewModel> alirTunai = new List<AbAlirTunaiViewModel>();

            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            // find total debit - kredit from akAkaun from current Tahun1
            if (Tahun1 != null)
            {
                List<AkAkaun> akAkaun1 = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                    .Where(b => b.Tarikh.Year == int.Parse(Tahun1)).ToList();

                if (JKWId != 0)
                {
                    akAkaun1 = akAkaun1.Where(b => b.JKWId == JKWId).ToList();
                }

                if (JBahagianId != 0)
                {
                    akAkaun1 = akAkaun1.Where(b => b.JBahagianId == JBahagianId).ToList();
                }

                akAkaun1 = akAkaun1.GroupBy(a => new { a.JKWId, a.AkCartaId1 })
                    .Select(l => new AkAkaun
                    {
                        AkCartaId1 = l.First().AkCartaId1,
                        AkCarta1 = l.First().AkCarta1,
                        Debit = l.Sum(d => d.Debit),
                        Kredit = l.Sum(k => k.Kredit)

                    }).OrderBy(a => a.AkCartaId1).ToList();

                if (akAkaun1 != null && akAkaun1.Count > 0)
                {
                    foreach (var a in akAkaun1)
                    {
                        alirTunai.Add(new AbAlirTunaiViewModel
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            JumAkaun1 = a.Debit - a.Kredit
                        });
                    }
                }
            }
            // find total debit - kredit from akAkaun from current Tahun1 end

            // find total debit - kredit from akAkaun from current Tahun2
            if (Tahun2 != null)
            {
                List<AkAkaun> akAkaun2 = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                    .Where(b => b.Tarikh.Year == int.Parse(Tahun2)).ToList();

                if (JKWId != 0)
                {
                    akAkaun2 = akAkaun2.Where(b => b.JKWId == JKWId).ToList();
                }

                if (JBahagianId != 0)
                {
                    akAkaun2 = akAkaun2.Where(b => b.JBahagianId == JBahagianId).ToList();
                }

                akAkaun2 = akAkaun2.GroupBy(a => new { a.JKWId, a.AkCartaId1 })
                    .Select(l => new AkAkaun
                    {
                        AkCartaId1 = l.First().AkCartaId1,
                        AkCarta1 = l.First().AkCarta1,
                        Debit = l.Sum(d => d.Debit),
                        Kredit = l.Sum(k => k.Kredit)

                    }).OrderBy(a => a.AkCartaId1).ToList();

                if (akAkaun2 != null && akAkaun2.Count > 0)
                {
                    foreach (var a in akAkaun2)
                    {
                        alirTunai.Add(new AbAlirTunaiViewModel
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            JumAkaun2 = a.Debit - a.Kredit
                        });
                    }
                }
            }

            // find total debit - kredit from akAkaun from current Tahun2 end

            // find total debit - kredit from akAkaun from current Tahun3
            if (Tahun3 != null)
            {
                List<AkAkaun> akAkaun3 = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                    .Where(b => b.Tarikh.Year == int.Parse(Tahun3)).ToList();

                if (JKWId != 0)
                {
                    akAkaun3 = akAkaun3.Where(b => b.JKWId == JKWId).ToList();
                }

                if (JBahagianId != 0)
                {
                    akAkaun3 = akAkaun3.Where(b => b.JBahagianId == JBahagianId).ToList();
                }

                akAkaun3 = akAkaun3.GroupBy(a => new { a.JKWId, a.AkCartaId1 })
                    .Select(l => new AkAkaun
                    {
                        AkCartaId1 = l.First().AkCartaId1,
                        AkCarta1 = l.First().AkCarta1,
                        Debit = l.Sum(d => d.Debit),
                        Kredit = l.Sum(k => k.Kredit)

                    }).OrderBy(a => a.AkCartaId1).ToList();

                if (akAkaun3 != null && akAkaun3.Count > 0)
                {
                    foreach (var a in akAkaun3)
                    {
                        alirTunai.Add(new AbAlirTunaiViewModel
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            JumAkaun3 = a.Debit - a.Kredit
                        });
                    }
                }
            }
            // find total debit - kredit from akAkaun from current Tahun3 end

            // insert into main array
       
            return alirTunai;
        }
        public async Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiMasukBasedOnYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> tunaiMasuk = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();
            
            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh > company.TarMula.AddYears(-1)
                && b.Tarikh.Year == int.Parse(Tahun)
                && b.Debit != 0).ToList();
            //List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
            //    .Where(b => b.AkCartaId1 == akBank.AkCartaId
            //    && b.Tarikh > company.TarMula.AddYears(-1)
            //    && b.Tarikh.Year == int.Parse(Tahun)
            //    && b.Debit != 0).ToList();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            decimal jan = 0;
            decimal feb = 0;
            decimal mac = 0;
            decimal apr = 0;
            decimal mei = 0;
            decimal jun = 0;
            decimal jul = 0;
            decimal ogo = 0;
            decimal sep = 0;
            decimal okt = 0;
            decimal nov = 0;
            decimal dis = 0;
            decimal jum = 0;

            foreach (var a in akAkaun)
            {
                jan = 0;
                feb = 0;
                mac = 0;
                apr = 0;
                mei = 0;
                jun = 0;
                jul = 0;
                ogo = 0;
                sep = 0;
                okt = 0;
                nov = 0;
                dis = 0;
                jum = a.Debit;

                if (a.Tarikh.Year == int.Parse(Tahun))
                {
                    switch (a.Tarikh.Month)
                    {
                        case 1:
                            jan = a.Debit;
                            break;
                        case 2:
                            feb = a.Debit;
                            break;
                        case 3:
                            mac = a.Debit;
                            break;
                        case 4:
                            apr = a.Debit;
                            break;
                        case 5:
                            mei = a.Debit;
                            break;
                        case 6:
                            jun = a.Debit;
                            break;
                        case 7:
                            jul = a.Debit;
                            break;
                        case 8:
                            ogo = a.Debit;
                            break;
                        case 9:
                            sep = a.Debit;
                            break;
                        case 10:
                            okt = a.Debit;
                            break;
                        case 11:
                            nov = a.Debit;
                            break;
                        case 12:
                            dis = a.Debit;
                            break;
                    }

                    tunaiMasuk.Add(
                        new AbAlirTunaiViewModel
                        {
                            NoAkaun = a.AkCarta2?.Kod ?? "",
                            NamaAkaun = a.AkCarta2?.Perihal ?? "BAKI AWAL",
                            KeluarMasuk = 1,
                            Jan = jan,
                            Feb = feb,
                            Mac = mac,
                            Apr = apr,
                            Mei = mei,
                            Jun = jun,
                            Jul = jul,
                            Ogo = ogo,
                            Sep = sep,
                            Okt = okt,
                            Nov = nov,
                            Dis = dis,
                            JumAkaun1 = jum
                        });
                }
            }

            return tunaiMasuk.GroupBy(b => new { b.NoAkaun })
                .Select(l => new AbAlirTunaiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    KeluarMasuk = l.First().KeluarMasuk,
                    Jan = l.Sum(c => c.Jan),
                    Feb = l.Sum(c => c.Feb),
                    Mac = l.Sum(c => c.Mac),
                    Apr = l.Sum(c => c.Apr),
                    Mei = l.Sum(c => c.Mei),
                    Jun = l.Sum(c => c.Jun),
                    Jul = l.Sum(c => c.Jul),
                    Ogo = l.Sum(c => c.Ogo),
                    Sep = l.Sum(c => c.Sep),
                    Okt = l.Sum(c => c.Okt),
                    Nov = l.Sum(c => c.Nov),
                    Dis = l.Sum(c => c.Dis),
                    JumAkaun1 = l.Sum(c => c.JumAkaun1)
                }).OrderBy(b => b.NoAkaun).ToList();

        }

        public async Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiKeluarBasedOnYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> tunaiKeluar = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh.Year == int.Parse(Tahun)
                && b.Kredit != 0).ToList();

            //List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
            //    .Where(b => b.AkCartaId1 == akBank.AkCartaId
            //    && b.Tarikh > company.TarMula.AddYears(-1)
            //    && b.Tarikh.Year == int.Parse(Tahun)
            //    && b.Kredit != 0).ToList();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            decimal jan = 0;
            decimal feb = 0;
            decimal mac = 0;
            decimal apr = 0;
            decimal mei = 0;
            decimal jun = 0;
            decimal jul = 0;
            decimal ogo = 0;
            decimal sep = 0;
            decimal okt = 0;
            decimal nov = 0;
            decimal dis = 0;
            decimal jum = 0;

            foreach (var a in akAkaun)
            {
                jan = 0;
                feb = 0;
                mac = 0;
                apr = 0;
                mei = 0;
                jun = 0;
                jul = 0;
                ogo = 0;
                sep = 0;
                okt = 0;
                nov = 0;
                dis = 0;

                jum = a.Kredit;

                if (a.Tarikh.Year == int.Parse(Tahun))
                {
                    switch (a.Tarikh.Month)
                    {
                        case 1:
                            jan = a.Kredit;
                            break;
                        case 2:
                            feb = a.Kredit;
                            break;
                        case 3:
                            mac = a.Kredit;
                            break;
                        case 4:
                            apr = a.Kredit;
                            break;
                        case 5:
                            mei = a.Kredit;
                            break;
                        case 6:
                            jun = a.Kredit;
                            break;
                        case 7:
                            jul = a.Kredit;
                            break;
                        case 8:
                            ogo = a.Kredit;
                            break;
                        case 9:
                            sep = a.Kredit;
                            break;
                        case 10:
                            okt = a.Kredit;
                            break;
                        case 11:
                            nov = a.Kredit;
                            break;
                        case 12:
                            dis = a.Kredit;
                            break;
                    }

                    tunaiKeluar.Add(
                        new AbAlirTunaiViewModel
                        {
                            NoAkaun = a.AkCarta2.Kod,
                            NamaAkaun = a.AkCarta2.Perihal,
                            KeluarMasuk = 2,
                            Jan = jan,
                            Feb = feb,
                            Mac = mac,
                            Apr = apr,
                            Mei = mei,
                            Jun = jun,
                            Jul = jul,
                            Ogo = ogo,
                            Sep = sep,
                            Okt = okt,
                            Nov = nov,
                            Dis = dis,
                            JumAkaun1 = jum
                        });
                }
            }

            return tunaiKeluar.GroupBy(b => new { b.NoAkaun })
                .Select(l => new AbAlirTunaiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    KeluarMasuk = l.First().KeluarMasuk,
                    Jan = l.Sum(c => c.Jan),
                    Feb = l.Sum(c => c.Feb),
                    Mac = l.Sum(c => c.Mac),
                    Apr = l.Sum(c => c.Apr),
                    Mei = l.Sum(c => c.Mei),
                    Jun = l.Sum(c => c.Jun),
                    Jul = l.Sum(c => c.Jul),
                    Ogo = l.Sum(c => c.Ogo),
                    Sep = l.Sum(c => c.Sep),
                    Okt = l.Sum(c => c.Okt),
                    Nov = l.Sum(c => c.Nov),
                    Dis = l.Sum(c => c.Dis),
                    JumAkaun1 = l.Sum(c => c.JumAkaun1)
                }).OrderBy(b => b.NoAkaun).ToList();
        }

        public async Task<List<AbTimbangDugaViewModel>> GetListTimbangDugaBasedOnLastDate(int JBahagianId, int? JKWId, DateTime TarHingga)
        {
            List<AbTimbangDugaViewModel> timbangDuga = new List<AbTimbangDugaViewModel>();

            List<AkAkaun> akAkaun = await context.AkAkaun
                .Include(b => b.AkCarta1)
                    .ThenInclude(b => b.JJenis)
                .Include(b => b.AkCarta2)
                .Where(b => b.Tarikh <= TarHingga).ToListAsync();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            foreach (var a in akAkaun)
            {
                        //if (a.AkCarta1.Kod == "B27299")
                        //{
                        //    var test = a.AkCarta1.Kod;
                        //}
                        if (a.Debit != 0)
                        {
                            timbangDuga.Add(new AbTimbangDugaViewModel()
                            {
                                NoAkaun = a.AkCarta1.Kod,
                                NamaAkaun = a.AkCarta1.Perihal,
                                DebitKredit = "D - DEBIT",
                                Jenis = a.AkCarta1.JJenis.Kod + " - " + a.AkCarta1.JJenis.Nama,
                                Debit = a.Debit
                            });
                        }
                        //else
                        //{
                        //    timbangDuga.Add(new AbTimbangDugaViewModel()
                        //    {
                        //        NoAkaun = a.AkCarta1.Kod,
                        //        NamaAkaun = a.AkCarta1.Perihal,
                        //        DebitKredit = "D - DEBIT",
                        //        Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                        //        Debit = a.Kredit
                        //    });
                        //}
                        if (a.Kredit != 0)
                        {
                            timbangDuga.Add(new AbTimbangDugaViewModel()
                            {
                                NoAkaun = a.AkCarta1.Kod,
                                NamaAkaun = a.AkCarta1.Perihal,
                                DebitKredit = "K - KREDIT",
                                Jenis = a.AkCarta1.JJenis.Kod + " - " + a.AkCarta1.JJenis.Nama,
                                Kredit = a.Kredit
                            });
                        }
                        //else
                        //{
                        //    timbangDuga.Add(new AbTimbangDugaViewModel()
                        //    {
                        //        NoAkaun = a.AkCarta1.Kod,
                        //        NamaAkaun = a.AkCarta1.Perihal,
                        //        DebitKredit = "K - KREDIT",
                        //        Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                        //        Kredit = a.Debit
                        //    });
                        //}

            }
            return timbangDuga.GroupBy(b => new { b.NoAkaun, b.NamaAkaun })
                .Select(l => new AbTimbangDugaViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    DebitKredit = l.First().DebitKredit,
                    Jenis = l.First().Jenis,
                    Debit = l.Sum(b => b.Debit - b.Kredit),
                    Kredit = l.Sum(b => b.Kredit - b.Debit)
                }).OrderBy(b => b.NoAkaun).ToList();
        }

        public async Task<List<AbUntungRugiViewModel>> GetListUntungRugiBasedOnRangeDate(int JBahagianId, int? JKWId, DateTime TarDari , DateTime TarHingga)
        {
            List<AbUntungRugiViewModel> untungRugi = new List<AbUntungRugiViewModel>();

            List<AkAkaun> akAkaun = await context.AkAkaun
                .Include(b => b.AkCarta1)
                    .ThenInclude(b => b.JJenis)
                .Include(b => b.AkCarta2)
                .Where(b => b.Tarikh.Year >= TarDari.Year && b.Tarikh.Year <= TarHingga.Year).ToListAsync();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            foreach (var a in akAkaun)
            {
                // pendapatan
                if (a.AkCarta1.JJenis.Kod == "H")
                {
                    untungRugi.Add( new AbUntungRugiViewModel()
                    {
                        Jenis = "H",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Kredit - a.Debit,
                    } );

                }
                // belanja
                else if (a.AkCarta1.JJenis.Kod == "B")
                {
                    untungRugi.Add(new AbUntungRugiViewModel()
                    {
                        Jenis = "B",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Debit - a.Kredit,
                    });

                }
            }
            return untungRugi.GroupBy(b => new { b.Jenis, b.NoAkaun })
                .Select(l => new AbUntungRugiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    Jenis = l.First().Jenis,
                    Amaun = l.Sum(b => b.Amaun)
                }).OrderByDescending(b => b.Jenis).ThenBy(b => b.NoAkaun).ToList();
        }

        public async Task<List<AbKunciKiraKiraViewModel>> GetListKunciKirakiraBasedOnLastDate(int JBahagianId, int? JKWId, DateTime TarHingga)
        {
            List<AbKunciKiraKiraViewModel> kirakira = new List<AbKunciKiraKiraViewModel>();

            List<AkAkaun> akAkaun = await context.AkAkaun
                .Include(b => b.AkCarta1)
                    .ThenInclude(b => b.JJenis)
                .Include(b => b.AkCarta2)
                .Where(b => b.Tarikh.Year <= TarHingga.Year).ToListAsync();

            if (JKWId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }

            if (JBahagianId != 0)
            {
                akAkaun = akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

            foreach (var a in akAkaun)
            {
                if (a.AkCarta1.Kod.Contains("A1") || a.AkCarta1.Kod.Contains("A7"))
                {
                    kirakira.Add(new AbKunciKiraKiraViewModel()
                    {
                        Order = 1,
                        Jenis = "ASET SEMASA",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Debit - a.Kredit,
                    });
                }

                if (a.AkCarta1.Kod.Contains("A3") || a.AkCarta1.Kod.Contains("A4"))
                {
                    kirakira.Add(new AbKunciKiraKiraViewModel()
                    {
                        Order = 2,
                        Jenis = "ASET TETAP",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Debit - a.Kredit,
                    });
                }

                if (a.AkCarta1.Kod.Contains("L1"))
                {
                    kirakira.Add(new AbKunciKiraKiraViewModel()
                    {
                        Order = 3,
                        Jenis = "LIABILITI SEMASA",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Kredit - a.Debit,
                    });
                }

                if (a.AkCarta1.Kod.Contains("E") && !a.AkCarta1.Kod.Contains("LEBIHAN / KURANGAN SEMASA",StringComparison.OrdinalIgnoreCase))
                {
                    kirakira.Add(new AbKunciKiraKiraViewModel()
                    {
                        Order = 4,
                        Jenis = "EKUITI",
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
                        Amaun = a.Kredit - a.Debit,
                    });
                }

                if (a.AkCarta1.Kod.Contains("B") || a.AkCarta1.Kod.Contains("H"))
                {
                    kirakira.Add(new AbKunciKiraKiraViewModel()
                    {
                        Order = 4,
                        Jenis = "EKUITI",
                        NoAkaun = "E13201",
                        NamaAkaun = "LEBIHAN / KURANGAN SEMASA",
                        Amaun = a.Kredit - a.Debit,
                    });
                }
                //switch (a.AkCarta1.JJenis.Kod)
                //{
                //    // Aset
                //    case "A":
                //        kirakira.Add(new AbKunciKiraKiraViewModel()
                //        {
                //            Order = 1,
                //            Jenis = "A",
                //            NoAkaun = a.AkCarta1.Kod,
                //            NamaAkaun = a.AkCarta1.Perihal,
                //            Amaun = a.Debit - a.Kredit,
                //        });
                //        break;
                //    // Liabiliti
                //    case "L":
                //        kirakira.Add(new AbKunciKiraKiraViewModel()
                //        {
                //            Order = 2,
                //            Jenis = "L",
                //            NoAkaun = a.AkCarta1.Kod,
                //            NamaAkaun = a.AkCarta1.Perihal,
                //            Amaun = a.Kredit - a.Debit,
                //        });
                //        break;
                //    // Ekuiti
                //    case "E":
                //        kirakira.Add(new AbKunciKiraKiraViewModel()
                //        {
                //            Order = 4,
                //            Jenis = "E",
                //            NoAkaun = a.AkCarta1.Kod,
                //            NamaAkaun = a.AkCarta1.Perihal,
                //            Amaun = a.Kredit - a.Debit,
                //        });
                //        break;
                //    // Untung /Rugi
                //    // Pendapatan (Hasil)
                //    case "H":
                //        kirakira.Add(new AbKunciKiraKiraViewModel()
                //        {
                //            Order = 5,
                //            Jenis = "H",
                //            NoAkaun = a.AkCarta1.Kod,
                //            NamaAkaun = a.AkCarta1.Perihal,
                //            Amaun = a.Kredit - a.Debit,
                //        });
                //        break;
                //    // Belanja
                //    case "B":
                //        kirakira.Add(new AbKunciKiraKiraViewModel()
                //        {
                //            Order = 5,
                //            Jenis = "B",
                //            NoAkaun = a.AkCarta1.Kod,
                //            NamaAkaun = a.AkCarta1.Perihal,
                //            Amaun = a.Debit - a.Kredit,
                //        });
                //        break;
                //    // Untung / Rugi END
                //}
            }
            return kirakira.GroupBy(b => new { b.Jenis, b.NoAkaun })
                .Select(l => new AbKunciKiraKiraViewModel
                {
                    Order = l.First().Order,
                    Jenis = l.First().Jenis,
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    Amaun = l.Sum(b => b.Amaun)
                }).OrderBy(b => b.Order).ThenBy(b => b.NoAkaun).ToList();

        }


    }
}

