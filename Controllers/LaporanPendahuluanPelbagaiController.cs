using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.PrintModel.Reporting;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanPendahuluanPelbagaiController : Controller
    {
        public const string modul = "LPT003";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LaporanPendahuluanPelbagaiController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            PopulateList();
            return View();
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(string kodLaporan, ReportFormModel param)
        {
            LPT003PrintModel reportModel = await PrepareData(kodLaporan, param);

            string customSwitches = string.Format(" --header-html  \"{0}\" " +
                                   "--header-spacing \"-12\" " +
                                   "--header-font-size \"10\" " +
                                   "--footer-center \"[page]/[toPage]\" " +
                                   "--footer-font-size \"7\" --footer-spacing 1",
                                   Url.Action("Header", "LaporanPendahuluanPelbagai",
                                   new
                                   {
                                       KodLaporan = reportModel.KodLaporan,
                                       ParamKodKw = reportModel.JKW.Kod,
                                       ParamPerihalKw = reportModel.JKW.Perihal,
                                       ParamBulan = reportModel.ParamBulan,
                                       ParamTahun = reportModel.ParamTahun
                                   },
                                   "https"));
            return new ViewAsPdf(reportModel.KodLaporan, reportModel)
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
        public ActionResult Header(LPT003PrintModel reportModel)
        {
            return View(reportModel);
        }
        private async Task<LPT003PrintModel> PrepareData(
            string kodLaporan,
            ReportFormModel param)
        {
            var pdfName = param.kodLaporan;
            if (param.JKWId != null)
            {
                JKW kW = _context.JKW.Where(x => x.Id == param.JKWId).FirstOrDefault();
                param.JKW = kW;
            }

            LPT003PrintModel reportModel = new LPT003PrintModel();

            if (kodLaporan == "LPT00301")
            {
                reportModel.ParamTajuk = "Laporan Daftar Pendahuluan Pelbagai Kump Wang :";

                IEnumerable<SpPendahuluanPelbagai> spp = _context.SpPendahuluanPelbagai
                    .IgnoreQueryFilters()
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.JTahapAktiviti)
                    .Include(b => b.JSukan)
                    .Include(b => b.AkCarta)
                    .Include(b => b.JNegeri)
                    .Include(b => b.SuPekerja).ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.SpPendahuluanPelbagai1).ThenInclude(b => b.JJantina)
                    .Include(b => b.SpPendahuluanPelbagai2)
                    .Include(b => b.AkPV).ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.AkTerima).ThenInclude(b => b.AkTerima2).ThenInclude(b => b.JCaraBayar)
                    .ToList();

                // bulan & tahun condition
                spp = spp.Where(x => x.TarMasuk.Month == param.bulanTahun.Month
                    && x.TarMasuk.Year == param.bulanTahun.Year)
                    .ToList();
                //bulan & tahun condition end

                //status condition
                switch (param.status)
                {
                    // belum posting
                    case StatusData.BelumPosting:
                        spp = spp.Where(x => x.FlPosting == 0).ToList();
                        break;
                    // sudah posting
                    case StatusData.SudahPosting:
                        spp = spp.Where(x => x.FlPosting == 1).ToList();
                        break;
                    // semua
                    default:
                        break;
                }
                //status condition end

                //susunan condition
                if (param.susunan == 1)
                {
                    spp = spp.OrderBy(x => x.NoPermohonan).ToList();
                }
                else
                {
                    spp = spp.OrderBy(x => x.TarMasuk).ThenBy(x => x.NoPermohonan).ToList();
                }
                //susunan condition end

                decimal jumlah = 0;

                reportModel.SpPendahuluanPelbagai = spp;

                foreach (SpPendahuluanPelbagai item in reportModel.SpPendahuluanPelbagai)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlah += 0;
                    }
                    else
                    {
                        jumlah += item.JumLulus;
                    }

                }
                reportModel.FormJumlah = jumlah;
            }

            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser.Nama;

            reportModel.KodLaporan = param.kodLaporan;
            reportModel.ParamBulan = param.bulanTahun.ToString("MM");
            reportModel.ParamTahun = param.bulanTahun.ToString("yyyy");
            reportModel.JKW = param.JKW;
            CompanyDetails company = new CompanyDetails();
            reportModel.CompanyDetail = company;

            return reportModel;
        }
    }
}
