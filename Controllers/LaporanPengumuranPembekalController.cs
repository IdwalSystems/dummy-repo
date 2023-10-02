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
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanPengumuranPembekalController : Controller
    {
        public const string modul = "LPD001";
        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekal;
        private readonly IRepository<AkPenghutang, int, string> _akPenghutang;

        public LaporanPengumuranPembekalController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int, string> akBelianRepo,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<AkPenghutang, int, string> akPenghutang)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelianRepo;
            _akPembekal = akPembekal;
            _akPenghutang = akPenghutang;
        }
        public IActionResult LPD001Index()
        {
            List<AkPembekal> pembekalList = _context.AkPembekal.OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = pembekalList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(
            ReportFormModel model)
        {
            PengumuranPrintModel reportModel = new PengumuranPrintModel();
            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser.Nama;

            if (model.AkPembekalId == null)
            {
                return View(model);
            }
            var pembekal = await _akPembekal.GetById((int)model.AkPembekalId);
            reportModel.Pengumuran = await PrepareData((int)model.AkPembekalId, Convert.ToDateTime(model.tarikhHingga), model.susunan);

            reportModel.KodLaporan = model.kodLaporan;

            reportModel.CompanyDetail = new CompanyDetails();

            reportModel.Tajuk1 = $"Laporan Pengumuran Bagi Pembekal {pembekal.KodSykt} - {pembekal.NamaSykt} ";
            reportModel.Tajuk2 = $"Pada Tarikh {Convert.ToDateTime(model.tarikhHingga):dd/MM/yyyy}";
            dynamic dyModel = new ExpandoObject();

            dyModel.reportModel = reportModel;

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

        public async Task<List<Pengumuran>> PrepareData(int AkPembekalId, DateTime tarikhHingga, int susunan) {
            // find all pembekal
            var pembekal = await _akPembekal.GetById(AkPembekalId);

            List<Pengumuran> pengumuranList = new List<Pengumuran>();
            if (pembekal != null) 
            {

                    // find all invois belian that is posted
                    var belianList = _context.AkBelian
                        .Include(b => b.AkPV2).ThenInclude(b => b.AkPV)
                        .Include(b => b.AkNotaDebitKreditBelian).Where(b => b.AkPembekalId == pembekal.Id && b.FlPosting == 1 && b.TarikhTerima <= tarikhHingga).ToList();
                    
                    if (belianList != null && belianList.Count() > 0)
                    {
                        foreach (var belian in belianList)
                        {
                            

                            decimal tunggakan = belian.Jumlah;
                            // check if there is debit / kredit note
                            if (belian.AkNotaDebitKreditBelian != null && belian.AkNotaDebitKreditBelian.Count() > 0)
                            {
                                foreach (var notaDebitKredit in belian.AkNotaDebitKreditBelian)
                                {
                                    // if debit +
                                    if (notaDebitKredit.FlJenis ==  0)
                                    {
                                        tunggakan += notaDebitKredit.Jumlah;
                                    } else
                                    // if kredit -
                                    {
                                        tunggakan -= notaDebitKredit.Jumlah;
                                    }
                                }
                            }

                            // check if already paid or half paid
                            List<AkPV> akPVList = new List<AkPV>();

                            if (belian.AkPV2 != null && belian.AkPV2.Count() > 0)
                            {
                                foreach (var pv2 in belian.AkPV2)
                                {
                                    akPVList.Add(pv2.AkPV);
                                    tunggakan -= pv2.Amaun;
                                }
                            }

                            // check if tunggakan has balance or not
                            if (tunggakan > 0)
                            {
                                // check datediff for the date
                                if (belian.TarikhTerima != null)
                                {
                                    double bakiHari = (tarikhHingga - belian.TarikhTerima).Value.Days;
                                    switch (bakiHari)
                                    {
                                        case double n when n <= 30:
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                Tunggak30 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            });
                                            break;
                                        case double n when (n > 30 && n <= 60):
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                Tunggak60 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            });
                                            break;
                                        case double n when (n > 60 && n <= 90):
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                Tunggak90 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            }); ;
                                            break;
                                        case double n when (n > 90 && n <= 180):
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                Tunggak180 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            });
                                            break;
                                        case double n when (n > 180 && n <= 365):
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                Tunggak365 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            });
                                            break;
                                        default:
                                            pengumuranList.Add(new Pengumuran
                                            {
                                                NoInvois = belian.NoRujukan,
                                                TarikhTerima = (DateTime)belian.TarikhTerima,
                                                AkPV = akPVList,
                                                TunggakLebih365 = tunggakan,
                                                JumlahTunggakan = tunggakan
                                            });
                                            break;

                                    }

                                    
                                }
                            }
                        }
                    }

                // group by kod
                pengumuranList = pengumuranList.GroupBy(b => b.NoInvois)
                    .Select(l => new Pengumuran { 
                        NoInvois = l.Key,
                        TarikhTerima = l.First().TarikhTerima,
                        AkPV = l.First().AkPV,
                        Tunggak30 = l.Sum(b => b.Tunggak30),
                        Tunggak60 = l.Sum(b => b.Tunggak60),
                        Tunggak90 = l.Sum(b => b.Tunggak90),
                        Tunggak180 = l.Sum(b => b.Tunggak180),
                        Tunggak365 = l.Sum(b => b.Tunggak365),
                        TunggakLebih365 = l.Sum(b => b.TunggakLebih365),
                        JumlahTunggakan = l.Sum(b => b.JumlahTunggakan)
                    }).ToList();
            }
            if (susunan == 1)
            {
                pengumuranList =  pengumuranList.OrderBy(b => b.TarikhTerima).ToList();
            }
            else
            {
                pengumuranList = pengumuranList.OrderBy(b => b.NoInvois).ToList();
            }
            return pengumuranList;
        }

        public IActionResult LPD002Index()
        {
            return View();
        }
    }
}
