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
    public class LaporanAkTerimaController : Controller
    {
        public const string modul = "LPR001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int> _akTerimaRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<JNegeri, int> _negeriRepo;
        private readonly ListViewIRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkAkaun, int> _akAkaunRepo;

        public LaporanAkTerimaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int> akTerimaRepository,
            ListViewIRepository<AkTerima1, int> akTerima1Repository,
            ListViewIRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<JKW, int> kwRepository,
            IRepository<JNegeri, int> negeriRepository,
            IRepository<AkCarta, int> akCartaRepository,
            IRepository<AkAkaun, int> akAkaunRepository
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
        public IActionResult Print(string kodLaporan, string tarikhDari, string tarikhHingga)
        {
            var pdfName = kodLaporan;
            JKW kW = _context.JKW.Where(x => x.Kod == "100").FirstOrDefault();

            LPR0012PrintModel reportModel = new LPR0012PrintModel();

            if (kodLaporan == "LPR0012")
            {

                IEnumerable<AkTerima> akT = _context.AkTerima
                .Include(b => b.JKW)
                .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                .Include(b => b.JNegeri)
                .Include(b => b.AkTerima1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkTerima2).ThenInclude(b => b.JCaraBayar)
                .ToList();

                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);
                akT = akT.Where(x => x.Tarikh >= date1
                    && x.Tarikh <= date2)
                    .OrderBy(x => x.Tarikh)
                    .ToList();

                decimal debit = 0;
                decimal kredit = 0;
                
                reportModel.AkTerima = akT;
                foreach (AkTerima item in reportModel.AkTerima)
                {
                    foreach (AkTerima1 item1 in item.AkTerima1)
                    {
                        debit += item1.Amaun;
                    }
                    foreach (AkTerima2 item2 in item.AkTerima2)
                    {
                        kredit += item2.Amaun;
                    }
                    reportModel.Kredit = kredit;
                    reportModel.Debit = debit;
                }
       
            } else
            {
            }

            reportModel.KodLaporan = kodLaporan;
            reportModel.TarikhDari = tarikhDari;
            reportModel.TarikhHingga = tarikhHingga;
            reportModel.JKW = kW;
            CompanyDetails company = new CompanyDetails();
            reportModel.CompanyDetail = company;

            return new ViewAsPdf(pdfName, reportModel)
            {
                PageMargins = { Left = 10, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
    }
}
