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


        public async Task<AbAlirTunaiViewModel> GetCarryPreviousBalanceEachStartingMonth(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> bakiAwal = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();

            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            // Masuk
            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh >= company.TarMula && b.Tarikh.Year <= int.Parse(Tahun))
                .ToList();

            if (JKWId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JKWId == JKWId).ToList();
            }
                
            

            if (JBahagianId != 0)
            {
                akAkaun =akAkaun.Where(b => b.JBahagianId == JBahagianId).ToList();
            }
               
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

            foreach (var a in akAkaun)
            {
                amaunJum += a.Debit;

                DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);
                if (a.Tarikh < jan)
                {
                    amaunJan += a.Debit;
                }
                DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
                if (a.Tarikh < feb)
                {
                    amaunFeb += a.Debit;
                }
                DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
                if (a.Tarikh < mac)
                {
                    amaunMac += a.Debit;
                }
                DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
                if (a.Tarikh < apr)
                {
                    amaunApr += a.Debit;
                }
                DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
                if (a.Tarikh < mei)
                {
                    amaunMei += a.Debit;
                }
                DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
                if (a.Tarikh < jun)
                {
                    amaunJun += a.Debit;
                }
                DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
                if (a.Tarikh < jul)
                {
                    amaunJul += a.Debit;
                }
                DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
                if (a.Tarikh < ogo)
                {
                    amaunOgo += a.Debit;
                }
                DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
                if (a.Tarikh < sep)
                {
                    amaunSep += a.Debit;
                }
                DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
                if (a.Tarikh < okt)
                {
                    amaunOkt += a.Debit;
                }
                DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
                if (a.Tarikh < nov)
                {
                    amaunNov += a.Debit;
                }
                DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
                if (a.Tarikh < dis)
                {
                    amaunDis += a.Debit;
                }
                DateTime jan2 = new DateTime(int.Parse(Tahun) + 1, 1, 1, 0, 0, 0);
                if (a.Tarikh < jan2)
                {
                    amaunJan2 += a.Debit;
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
                    JumAkaun = amaunJum,
                    KeluarMasuk = 0
                });
            }
            // Masuk END

            // Keluar
            List<AkAkaun> akAkaunK = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId2 == akBank.AkCartaId
                && b.Tarikh >= company.TarMula && b.Tarikh.Year <= int.Parse(Tahun)
                && b.Kredit != 0).ToList();

            if (JKWId != 0)
            {
                akAkaunK =akAkaunK.Where(b => b.JKWId == JKWId).ToList();
            }



            if (JBahagianId != 0)
            {
                akAkaunK =akAkaunK.Where(b => b.JBahagianId == JBahagianId).ToList();
            }

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

            foreach (var a in akAkaunK)
            {
                amaunJum -= a.Kredit;

                DateTime jan = new DateTime(int.Parse(Tahun), 1, 1, 0, 0, 0);
                if (a.Tarikh < jan)
                {
                    amaunJan -= a.Kredit;
                }
                DateTime feb = new DateTime(int.Parse(Tahun), 2, 1, 0, 0, 0);
                if (a.Tarikh < feb)
                {
                    amaunFeb -= a.Kredit;
                }
                DateTime mac = new DateTime(int.Parse(Tahun), 3, 1, 0, 0, 0);
                if (a.Tarikh < mac)
                {
                    amaunMac -= a.Kredit;
                }
                DateTime apr = new DateTime(int.Parse(Tahun), 4, 1, 0, 0, 0);
                if (a.Tarikh < apr)
                {
                    amaunApr -= a.Kredit;
                }
                DateTime mei = new DateTime(int.Parse(Tahun), 5, 1, 0, 0, 0);
                if (a.Tarikh < mei)
                {
                    amaunMei -= a.Kredit;
                }
                DateTime jun = new DateTime(int.Parse(Tahun), 6, 1, 0, 0, 0);
                if (a.Tarikh < jun)
                {
                    amaunJun -= a.Kredit;
                }
                DateTime jul = new DateTime(int.Parse(Tahun), 7, 1, 0, 0, 0);
                if (a.Tarikh < jul)
                {
                    amaunJul -= a.Kredit;
                }
                DateTime ogo = new DateTime(int.Parse(Tahun), 8, 1, 0, 0, 0);
                if (a.Tarikh < ogo)
                {
                    amaunOgo -= a.Kredit;
                }
                DateTime sep = new DateTime(int.Parse(Tahun), 9, 1, 0, 0, 0);
                if (a.Tarikh < sep)
                {
                    amaunSep -= a.Kredit;
                }
                DateTime okt = new DateTime(int.Parse(Tahun), 10, 1, 0, 0, 0);
                if (a.Tarikh < okt)
                {
                    amaunOkt -= a.Kredit;
                }
                DateTime nov = new DateTime(int.Parse(Tahun), 11, 1, 0, 0, 0);
                if (a.Tarikh < nov)
                {
                    amaunNov -= a.Kredit;
                }
                DateTime dis = new DateTime(int.Parse(Tahun), 12, 1, 0, 0, 0);
                if (a.Tarikh < dis)
                {
                    amaunDis -= a.Kredit;
                }
                DateTime jan2 = new DateTime(int.Parse(Tahun) + 1, 12, 1, 0, 0, 0);
                if (a.Tarikh < jan2)
                {
                    amaunJan2 -= a.Kredit;
                }
                bakiAwal.Add(new AbAlirTunaiViewModel
                {
                    NoAkaun = a.AkCarta2.Kod,
                    NamaAkaun = a.AkCarta2.Perihal,
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
                    JumAkaun = amaunJum,
                    KeluarMasuk = 0
                });
            }
            // Keluar END

            return bakiAwal.GroupBy(b => new { b.NoAkaun })
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
                    JumAkaun = l.Sum(c => c.JumAkaun)
                }).OrderBy(b => b.NoAkaun).FirstOrDefault();
        }

        public async Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiMasukBasedOnYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> tunaiMasuk = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId1 == akBank.AkCartaId
                && b.Tarikh.Year >= int.Parse(Tahun) && b.Tarikh.Year <= int.Parse(Tahun)
                && b.Debit != 0).ToList();

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
                jum += a.Debit;

                switch (a.Tarikh.Month)
                {
                    case 1:
                        jan += a.Debit;
                        break;
                    case 2:
                        feb += a.Debit;
                        break;
                    case 3:
                        mac += a.Debit;
                        break;
                    case 4:
                        apr += a.Debit;
                        break;
                    case 5:
                        mei += a.Debit;
                        break;
                    case 6:
                        jun += a.Debit;
                        break;
                    case 7:
                        jul += a.Debit;
                        break;
                    case 8:
                        ogo += a.Debit;
                        break;
                    case 9:
                        sep += a.Debit;
                        break;
                    case 10:
                        okt += a.Debit;
                        break;
                    case 11:
                        nov += a.Debit;
                        break;
                    case 12:
                        dis += a.Debit;
                        break;
                }

                tunaiMasuk.Add(
                    new AbAlirTunaiViewModel
                    {
                        NoAkaun = a.AkCarta2.Kod,
                        NamaAkaun = a.AkCarta2.Perihal,
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
                        JumAkaun = jum
                    });

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
                    JumAkaun = l.Sum(c => c.JumAkaun)
                }).OrderBy(b => b.NoAkaun).ToList();

        }

        public async Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiKeluarBasedOnYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun)
        {
            List<AbAlirTunaiViewModel> tunaiKeluar = new List<AbAlirTunaiViewModel>();

            var company = await _userService.GetCompanyDetails();
            var akBank = await context.AkBank.Where(b => b.Id == akBankId).FirstOrDefaultAsync();

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.AkCartaId2 == akBank.AkCartaId
                && b.Tarikh.Year >= int.Parse(Tahun) && b.Tarikh.Year <= int.Parse(Tahun)
                && b.Kredit != 0).ToList();

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
                jum += a.Kredit;

                switch (a.Tarikh.Month)
                {
                    case 1:
                        jan += a.Kredit;
                        break;
                    case 2:
                        feb += a.Kredit;
                        break;
                    case 3:
                        mac += a.Kredit;
                        break;
                    case 4:
                        apr += a.Kredit;
                        break;
                    case 5:
                        mei += a.Kredit;
                        break;
                    case 6:
                        jun += a.Kredit;
                        break;
                    case 7:
                        jul += a.Kredit;
                        break;
                    case 8:
                        ogo += a.Kredit;
                        break;
                    case 9:
                        sep += a.Kredit;
                        break;
                    case 10:
                        okt += a.Kredit;
                        break;
                    case 11:
                        nov += a.Kredit;
                        break;
                    case 12:
                        dis += a.Kredit;
                        break;
                }

                tunaiKeluar.Add(
                    new AbAlirTunaiViewModel
                    {
                        NoAkaun = a.AkCarta1.Kod,
                        NamaAkaun = a.AkCarta1.Perihal,
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
                        JumAkaun = jum
                    });

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
                    JumAkaun = l.Sum(c => c.JumAkaun)
                }).OrderBy(b => b.NoAkaun).ToList();
        }

        public async Task<List<AbTimbangDugaViewModel>> GetListTimbangDugaBasedOnDate(int JBahagianId, int? JKWId, DateTime TarHingga)
        {
            List<AbTimbangDugaViewModel> timbangDuga = new List<AbTimbangDugaViewModel>();

            var company = await _userService.GetCompanyDetails();

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.Tarikh <= TarHingga).ToList();

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
                var carta = await context.AkCarta.Include(b => b.JJenis)
                    .FirstOrDefaultAsync(b => b.Id == a.AkCartaId1);

                if (carta != null)
                {
                    if (carta.DebitKredit == "D")
                    {
                        timbangDuga.Add(new AbTimbangDugaViewModel()
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            DebitKredit = "D - DEBIT",
                            Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                            Debit = a.Debit
                        });

                        timbangDuga.Add(new AbTimbangDugaViewModel()
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            DebitKredit = "D - DEBIT",
                            Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                            Debit = -a.Kredit
                        });
                    }
                    else
                    {
                        timbangDuga.Add(new AbTimbangDugaViewModel()
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            DebitKredit = "K - KREDIT",
                            Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                            Kredit = a.Kredit
                        });

                        timbangDuga.Add(new AbTimbangDugaViewModel()
                        {
                            NoAkaun = a.AkCarta1.Kod,
                            NamaAkaun = a.AkCarta1.Perihal,
                            DebitKredit = "K - KREDIT",
                            Jenis = carta.JJenis.Kod + " - " + carta.JJenis.Nama,
                            Kredit = -a.Debit
                        });
                    }
                }
            }
            return timbangDuga.GroupBy(b => new {b.DebitKredit, b.NoAkaun})
                .Select( l => new AbTimbangDugaViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    DebitKredit = l.First().DebitKredit,
                    Jenis = l.First().Jenis,
                    Debit = l.Sum(b => b.Debit),
                    Kredit = l.Sum(b => b.Kredit)
                }).OrderBy(b => b.NoAkaun).ToList();
        }

        public async Task<List<AbUntungRugiViewModel>> GetListUntungRugiBasedOnRangeDate(int JBahagianId, int? JKWId, DateTime TarDari , DateTime TarHingga)
        {
            List<AbUntungRugiViewModel> untungRugi = new List<AbUntungRugiViewModel>();

            var company = await _userService.GetCompanyDetails();

            List<AkAkaun> akAkaun = context.AkAkaun.Include(b => b.AkCarta1).Include(b => b.AkCarta2)
                .Where(b => b.Tarikh >= TarDari && b.Tarikh <= TarHingga).ToList();

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
                var carta = await context.AkCarta.Include(b => b.JJenis)
                    .FirstOrDefaultAsync(b => b.Id == a.AkCartaId1);

                // pendapatan
                if (carta.JJenis.Kod == "H")
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
                else if (carta.JJenis.Kod == "B")
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
    }
}

