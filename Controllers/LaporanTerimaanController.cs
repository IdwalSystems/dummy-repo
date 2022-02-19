using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel.Reporting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize]
    public class LaporanTerimaanController : Controller
    {
        public const string modul = "LPR001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;

        public LaporanTerimaanController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int, string> akTerimaRepository,
            ListViewIRepository<AkTerima1, int> akTerima1Repository,
            ListViewIRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(
            string kodLaporan,
            string tarikhDari,
            string tarikhHingga,
            int status,
            int susunan)
        {
            var pdfName = kodLaporan;
            var tajuk = "";
            JKW kW = _context.JKW.Where(x => x.Kod == "100").FirstOrDefault();

            LPR0012PrintModel reportModel = new LPR0012PrintModel();

            if (kodLaporan == "LPR0012" || kodLaporan == "LPR0013")
            {
                if(kodLaporan == "LPR0012")
                {
                    tajuk = "Laporan Daftar Resit Terperinci Mengikut Pecahan Cara Bayar Kump Wang :";
                }
                else
                {
                    tajuk = "Laporan Daftar Resit Terperinci Mengikut Pecahan Kod Akaun Kump Wang :";
                }
                IEnumerable<AkTerima> akT = _context.AkTerima
                    .Include(b => b.JKW)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.JNegeri)
                    .Include(b => b.AkTerima1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkTerima2).ThenInclude(b => b.JCaraBayar)
                    .ToList();

                // date condition
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);
                akT = akT.Where(x => x.Tarikh >= date1
                      && x.Tarikh <= date2)
                    .ToList();
                //date condition end

                //status condition
                switch (status)
                {
                    // belum posting
                    case 1:
                        akT = akT.Where(x => x.FlPosting == 0).ToList();
                        break;
                    // sudah posting
                    case 2:
                        akT = akT.Where(x => x.FlPosting == 1).ToList();
                        break;
                    // semua
                    default:
                        break;
                }
                //status condition end

                //susunan condition
                if (susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoRujukan).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoRujukan).ToList();
                }
                //susunan condition end

                decimal debit = 0;
                decimal kredit = 0;
                decimal amaunUrusniaga = 0;

                reportModel.AkTerima = akT;
                foreach (AkTerima item in reportModel.AkTerima)
                {
                    if (item.FlBatal == 1)
                    {
                        debit += 0;
                    }
                    else
                    {
                        debit += item.Jumlah;
                    }


                    foreach (AkTerima1 item1 in item.AkTerima1)
                    {
                        if (item.FlBatal == 1)
                        {
                            kredit += 0;
                        }
                        else
                        {
                            kredit += item1.Amaun;
                        }
                    }
                    foreach (AkTerima2 item2 in item.AkTerima2)
                    {
                        if (item.FlBatal == 1)
                        {
                            amaunUrusniaga += 0;
                        }
                        else
                        {
                            amaunUrusniaga += item2.Amaun;
                        }
                    }

                }
                reportModel.AmaunUrusniaga = amaunUrusniaga;
                reportModel.Kredit = kredit;
                reportModel.Debit = debit;

                //Ringkasan Cara bayar
                var RingkasanCaraBayar = (from tbl1 in _context.AkTerima2.Include(x => x.JCaraBayar).ToList()
                                                     join tbl in _context.AkTerima.ToList()
                                                     on tbl1.AkTerimaId equals tbl.Id into tbl1Tbl
                                                     from tbl1_tbl in tbl1Tbl.DefaultIfEmpty().Where(x => x.FlBatal != 1)
                                                     select new
                                                     {
                                                         CaraBayar = tbl1.JCaraBayar.Perihal
                                                     }).GroupBy(x => x.CaraBayar).Select(group => new
                                                     {
                                                         Metric = group.Key,
                                                         Count = group.Count()
                                                     }).OrderBy(x => x.Metric).ToList();

                IEnumerable<RingkasanPrintModel> ringkasanCaraBayarResult = RingkasanCaraBayar.Select(l => new RingkasanPrintModel
                {
                    Perihal = l.Metric,
                    Kuantiti = l.Count.ToString(),
                }).ToList();

                //Ringkasan Cara bayar end
                //Ringkasan Debit group by kod Bank AkTerima
                var DebitLines = (from tbl in _context.AkTerima.Include(x => x.AkBank).ThenInclude(x => x.AkCarta).Where(x => x.FlBatal != 1).ToList()
                                  select new
                                  {
                                      kodAkaun = tbl.AkBank.AkCarta.Kod,
                                      Perihal = tbl.AkBank.AkCarta.Perihal,
                                      Debit = tbl.Jumlah

                                  }).GroupBy(x => x.kodAkaun).ToList();

                IEnumerable<RingkasanPrintModel> debitResult = DebitLines.Select(l => new RingkasanPrintModel
                {
                    KodAkaun = l.First().kodAkaun,
                    Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                    Debit = l.Sum(c => c.Debit).ToString(),
                    Kredit = "0.00"
                }).ToList();

                //Ringkasan Debit group by kod Bank AkTerima end
                // Ringkasan kredit group by Kod Objek AkTerima1
                var kreditLines = (from tbl1 in _context.AkTerima1.Include(x => x.AkCarta).ToList()
                                   join tbl in _context.AkTerima.ToList()
                                   on tbl1.AkTerimaId equals tbl.Id into tbl1Tbl
                                   from tbl1_tbl in tbl1Tbl.DefaultIfEmpty().Where(x => x.FlBatal != 1)
                                   select new
                                   {
                                       kodAkaun = tbl1.AkCarta.Kod,
                                       Perihal = tbl1.AkCarta.Perihal,
                                       Kredit = tbl1.Amaun

                                   }).GroupBy(x => x.kodAkaun).ToList();

                IEnumerable<RingkasanPrintModel> kreditResult = kreditLines.Select(l => new RingkasanPrintModel
                {
                    KodAkaun = l.First().kodAkaun,
                    Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                    Debit = "0.00",
                    Kredit = l.Sum(c => c.Kredit).ToString()
                }).ToList();

                IEnumerable<RingkasanPrintModel> result = debitResult.Concat(kreditResult);

                reportModel.LPR0012_1 = ringkasanCaraBayarResult;
                reportModel.LPR0013_1 = result;

                // ringkasan kredit group by Kod Objek AkTerima1 end
            }
            else
            {

            }
                    

            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser.Nama;

            reportModel.KodLaporan = kodLaporan;
            reportModel.TarikhDari = tarikhDari;
            reportModel.TarikhHingga = tarikhHingga;
            reportModel.JKW = kW;
            CompanyDetails company = new CompanyDetails();
            reportModel.CompanyDetail = company;

            string customSwitches = string.Format(" --header-html  \"{0}\" " +
                                   "--header-spacing \"-12\" " +
                                   "--header-font-size \"10\" " +
                                   "--footer-center \"[page]/[toPage]\" " +
                                   "--footer-font-size \"7\" --footer-spacing 1",
                                   Url.Action("Header", "LaporanTerimaan",
                                   new
                                   {
                                       KodLaporan = kodLaporan,
                                       KodKw = kW.Kod,
                                       PerihalKw = kW.Perihal,
                                       TarikhDari = tarikhDari,
                                       TarikhHingga = tarikhHingga,
                                       Tajuk = tajuk
                                   },
                                   "https"));
            return new ViewAsPdf(pdfName, reportModel)
            {
                PageMargins = { Left = 10, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = customSwitches,
                //CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                //        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        [AllowAnonymous]
        public ActionResult Header(LPR0012PrintModel reportModel)
        {
            return View(reportModel);
        }
    }
}
