using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.PrintModel.Reporting;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize]
    public class LaporanNotaMintaController : Controller
    {
        public const string modul = "LPV001";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LaporanNotaMintaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
            )
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
        public async Task<IActionResult> Print( string kodLaporan, ReportParamViewModel param)
        {
            var pdfName = param.kodLaporan;
            var tajuk = "";
            if (param.JKWId != null)
            {
                JKW kW = _context.JKW.Where(x => x.Id == param.JKWId).FirstOrDefault();
                param.JKW = kW;
            }
            else
            {
                TempData[SD.Error] = "Sila pilih Kump. Wang.";
                PopulateList();
                return RedirectToAction(nameof(Index));

            }

            LPN001PrintModel reportModel = new LPN001PrintModel();

            if (kodLaporan == "LPN00101")
            {
                tajuk = "Laporan Daftar Bil / Nota Minta Kump Wang :";

                IEnumerable<AkNotaMinta> akT = _context.AkNotaMinta
                    .IgnoreQueryFilters()
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.AkNotaMinta1)
                    .Include(b => b.AkNotaMinta2)
                    .Include(b=> b.AkPO).ThenInclude(b=> b.AkBelian).ThenInclude(b=> b.AkPV2).ThenInclude(b=> b.AkPV).ThenInclude(b=> b.JCaraBayar)
                    .ToList();

                // bulan & tahun condition
                akT = akT.Where(x => x.Tarikh.Month == param.bulanTahun.Month
                    && x.Tarikh.Year == param.bulanTahun.Year)
                    .ToList();
                //bulan & tahun condition end

                //status condition
                switch (param.status)
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
                if (param.susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoSiri).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoSiri).ToList();
                }
                //susunan condition end

                decimal jumlah = 0;

                reportModel.AkNotaMinta = akT;

                foreach (AkNotaMinta item in reportModel.AkNotaMinta)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlah += 0;
                    }
                    else
                    {
                        jumlah += item.Jumlah;
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

            string customSwitches = string.Format(" --header-html  \"{0}\" " +
                                   "--header-spacing \"-12\" " +
                                   "--header-font-size \"10\" " +
                                   "--footer-center \"[page]/[toPage]\" " +
                                   "--footer-font-size \"7\" --footer-spacing 1",
                                   Url.Action("Header", "LaporanNotaMinta",
                                   new
                                   {
                                       KodLaporan = reportModel.KodLaporan,
                                       ParamKodKw = reportModel.JKW.Kod,
                                       ParamPerihalKw = reportModel.JKW.Perihal,
                                       ParamBulan = reportModel.ParamBulan,
                                       ParamTahun = reportModel.ParamTahun,
                                       ParamTajuk = tajuk
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
        public ActionResult Header(LPN001PrintModel reportModel)
        {
            return View(reportModel);
        }
    }
}
