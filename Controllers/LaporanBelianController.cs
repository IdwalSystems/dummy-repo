using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel.Reporting;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanBelianController : Controller
    {
        public const string modul = "LPT002";
        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;

        public LaporanBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int, string> akBelianRepo)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelianRepo;
        }
        public IActionResult LPT002Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(ReportFormModel model)
        {
            LPT002PrintModel reportModel = new LPT002PrintModel();

            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser?.Nama;

            dynamic dyModel = new ExpandoObject();

            if (model.tarikhDari != null && model.tarikhHingga != null)
            {
                reportModel.AkBelian = await PrepareData(Convert.ToDateTime(model.tarikhDari), Convert.ToDateTime(model.tarikhHingga),model.status,model.susunan);

                reportModel.KodLaporan = model.kodLaporan;
                reportModel.CompanyDetail = new CompanyDetails();

                reportModel.Tajuk1 = "Laporan Daftar Belian Belum Bayar";
                reportModel.Tajuk2 = $"Tarikh : {Convert.ToDateTime(model.tarikhDari):dd/MM/yyyy} -> {Convert.ToDateTime(model.tarikhHingga):dd/MM/yyyy}";

                dyModel.reportModel = reportModel;

            }

            return new ViewAsPdf(model.kodLaporan, dyModel,
                new ViewDataDictionary(ViewData)
                {
                    { "NamaSyarikat", reportModel.CompanyDetail.NamaSyarikat },
                    { "AlamatSyarikat1", reportModel.CompanyDetail.AlamatSyarikat1 },
                    { "AlamatSyarikat2", reportModel.CompanyDetail.AlamatSyarikat2 },
                    { "AlamatSyarikat3", reportModel.CompanyDetail.AlamatSyarikat3 }
                })
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                            " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        public async Task<IEnumerable<AkBelian>> PrepareData(DateTime tarikhDari, DateTime tarikhHingga, StatusData status,int susunan)
        {
            var belianList = await _akBelianRepo.GetAllIncludeDeletedItems();

            belianList = belianList.Where(b => b.Tarikh >= tarikhDari && b.Tarikh <= tarikhHingga).ToList();

            switch (status)
            {
                case StatusData.BelumPosting:
                    belianList = belianList.Where(b => b.FlPosting == 0).ToList();
                    break;
                case StatusData.SudahPosting:
                    belianList = belianList.Where(b => b.FlPosting == 1).ToList();
                    break;
                case StatusData.Batal:
                    belianList = belianList.Where(b => b.FlBatal == 1).ToList();
                    break;
                default: break;
            }

            switch (susunan)
            {
                case 0:
                    belianList = belianList.OrderBy(b => b.NoRujukan).ToList(); break;
                case 1:
                    belianList = belianList.OrderBy(b => b.Tarikh).ToList(); break;
                case 2:
                    belianList = belianList.OrderBy(b => b.AkPembekal.KodSykt).ToList(); break;

            }

            return belianList;
        }
    }
}
