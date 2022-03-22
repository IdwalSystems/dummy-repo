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
    public class LaporanBaucerController : Controller
    {
        public const string modul = "LPV001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkPV1, int> _akPV1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkPV2, int> _akPV2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;

        public LaporanBaucerController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPV, int, string> akPVRepository,
            ListViewIRepository<AkPV1, int> akPV1Repository,
            ListViewIRepository<AkPV2, int> akPV2Repository,
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
            _akPVRepo = akPVRepository;
            _akPV1Repo = akPV1Repository;
            _akPV2Repo = akPV2Repository;
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

            LPV001PrintModel reportModel = new LPV001PrintModel();

            if (kodLaporan == "LPV00101")
            {
                
                tajuk = "Laporan Daftar Bil Kump Wang :";

                IEnumerable<AkPV> akT = _context.AkPV
                    .Include(b => b.JKW)
                    .Include(b=> b.JCaraBayar)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.SuPekerja)
                    .Include(b => b.AkPV1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2).ThenInclude(b => b.AkBelian).ThenInclude(b=> b.AkPO)
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
                    akT = akT.OrderBy(x => x.NoPV).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoPV).ToList();
                }
                //susunan condition end

                decimal jumlahDebit = 0;

                reportModel.AkPV = akT;

                foreach (AkPV item in reportModel.AkPV)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlahDebit += 0;
                    }
                    else
                    {
                        jumlahDebit += item.Jumlah;
                    }

                }
                reportModel.JumlahDebit = jumlahDebit;
            }
            else if (kodLaporan == "LPV00102")
            {
                tajuk = "Laporan Daftar Bil Kump Wang :";

                IEnumerable<AkPV> akT = _context.AkPV
                    .Include(b => b.JKW)
                    .Include(b => b.JCaraBayar)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.SuPekerja)
                    .Include(b => b.AkPV1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2).ThenInclude(b => b.AkBelian).ThenInclude(b => b.AkPO).ThenInclude(b=> b.AkNotaMinta)
                    .Where(b=> b.denganTanggungan == true)
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
                    akT = akT.OrderBy(x => x.NoPV).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoPV).ToList();
                }
                //susunan condition end

                decimal jumlahDebit = 0;

                reportModel.AkPV = akT;

                foreach (AkPV item in reportModel.AkPV)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlahDebit += 0;
                    }
                    else
                    {
                        jumlahDebit += item.Jumlah;
                    }

                }
                reportModel.JumlahDebit = jumlahDebit;
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
        public ActionResult Header(LPR001PrintModel reportModel)
        {
            return View(reportModel);
        }
    }
}

