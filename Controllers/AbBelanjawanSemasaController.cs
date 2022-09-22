using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MSNK.Models.Modules.ViewModel;
using System.Collections.Generic;
using MSNK.Models.Modules;
using System.Linq;
using System.Dynamic;
using MSNK.Models.Modules.FormModel;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Infrastructure;

namespace MSNK.Controllers
{
    [Authorize(Policy = "BJ002")]
    public class AbBelanjawanSemasaController : Controller
    {
        public const string modul = "BJ002";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BelanjawanSemasaIRepository<string, int> _bsRepo;
        private readonly UserService _userService;

        public AbBelanjawanSemasaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            BelanjawanSemasaIRepository<string, int> bsRepo,
            UserService userService)
        {
            _context = context;
            _userManager = userManager;
            _bsRepo = bsRepo;
            _userService = userService;
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

            ViewData["Tahun"] = DateTime.Now.ToString("yyyy");
            ViewData["TarHingga"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.JKWId = 1;
            ViewBag.JBahagianId = 1;

        }

        [HttpPost]
        public async Task<IActionResult> Index(
            int JKWId,
            int JBahagianId,
            string tahun,
            DateTime tarHingga)
        {
            tarHingga = tarHingga.AddHours(23.99);

            List<AbBelanjawanSemasaViewModel> vm = new List<AbBelanjawanSemasaViewModel>();

            // Waran
            List<AbWaran> warans = await _bsRepo.GetAbWaranBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> waranList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var waran in warans)
            {
                foreach (var waran1 in waran.AbWaran1)
                {
                    waranList = _bsRepo.RunWaranObjekOperation(waran.FlJenisWaran,
                        waran1.TK,
                        waran1.Amaun,
                        waran1.AkCarta.Kod,
                        waran1.AkCarta.Perihal,
                        waran1.AkCarta.JParas.Kod);

                    vm.AddRange(waranList);
                }
            }
            // Waran End

            // PO
            List<AkPO> POs = await _bsRepo.GetAkPOBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> poList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var po in POs)
            {
                foreach (var po1 in po.AkPO1)
                {
                    poList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(po1.Amaun, po1.AkCarta.Kod, po1.AkCarta.Perihal, "4");

                    vm.AddRange(poList);
                }
            }
            // PO End

            // Pendahuluan Pelbagai
            List<SpPendahuluanPelbagai> Sps = await _bsRepo.GetSpPendahuluanPelbagaiBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> spList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var sp in Sps)
            {
                spList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(sp.JumLulus, sp.AkCarta.Kod, sp.AkCarta.Perihal, "4");

                vm.AddRange(spList);

            }
            // Pendahuluan Pelbagai End

            // POLaras
            List<AkPOLaras> POLarass = await _bsRepo.GetAkPOLarasBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> poLarasList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var poLaras in POLarass)
            {
                foreach (var poLaras1 in poLaras.AkPOLaras1)
                {
                    poLarasList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(poLaras1.Amaun, poLaras1.AkCarta.Kod, poLaras1.AkCarta.Perihal, "4");

                    vm.AddRange(poLarasList);
                }
            }
            // POLaras End

            // Inden
            List<AkInden> Indens = await _bsRepo.GetAkIndenBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> indenList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var inden in Indens)
            {
                foreach (var inden1 in inden.AkInden1)
                {
                    indenList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(inden1.Amaun, inden1.AkCarta.Kod, inden1.AkCarta.Perihal, "4");

                    vm.AddRange(indenList);
                }
            }
            // Inden End

            // PV
            List<AkPV> PVs = await _bsRepo.GetAkPVBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> pvList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var pv in PVs)
            {
                foreach (var pv1 in pv.AkPV1)
                {
                    pvList = _bsRepo.RunBaucerObjekOperation(pv.denganTanggungan, pv1.Amaun, pv1.AkCarta.Kod, pv1.AkCarta.Perihal, "4");

                    vm.AddRange(pvList);
                }
            }
            // Pv End

            // Tunai CV
            List<AkTunaiCV> CVs = await _bsRepo.GetAkTunaiCVBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> cvList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var cv in CVs)
            {
                foreach (var cv1 in cv.AkTunaiCV1)
                {
                    cvList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(cv1.Amaun, cv1.AkCarta.Kod, cv1.AkCarta.Perihal, "4");

                    vm.AddRange(cvList);
                }
            }
            // TunaiCV End

            // Terima
            List<AkTerima> Terimas = await _bsRepo.GetAkTerimaBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> terimaList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var terima in Terimas)
            {
                foreach (var terima1 in terima.AkTerima1)
                {
                    if (terima1.AkCarta.JJenis.Kod == "B")
                    {
                        terimaList = _bsRepo.RunResitObjekOperation(terima1.Amaun, terima1.AkCarta.Kod, terima1.AkCarta.Perihal, "4");

                        vm.AddRange(terimaList);
                    }

                }
            }
            // Terima End

            // Jurnal
            List<AkJurnal> Jurnals = await _bsRepo.GetAkJurnalBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> jurnalList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var jurnal in Jurnals)
            {
                foreach (var jurnal1 in jurnal.AkJurnal1)
                {
                    if (jurnal1.AkCarta.JJenis.Kod == "B" || jurnal1.AkCarta.JJenis.Kod == "A")
                    {
                        jurnalList = _bsRepo.RunJurnalObjekOperation(jurnal1.Debit, jurnal1.Kredit, jurnal1.AkCarta.Kod, jurnal1.AkCarta.Perihal, "4");

                        vm.AddRange(jurnalList);
                    }

                }
            }
            // Jurnal End

            //
            vm = vm.GroupBy(b => b.Objek)
                .Select(l => new AbBelanjawanSemasaViewModel
                {
                    Objek = l.First().Objek,
                    Perihalan = l.First().Perihalan,
                    Paras = l.First().Paras,
                    Asal = l.Sum(c => c.Asal),
                    Tambah = l.Sum(c => c.Tambah),
                    Pindah = l.Sum(c => c.Pindah),
                    Jumlah = l.Sum(c => c.Asal + c.Tambah - c.Pindah),
                    Belanja = l.Sum(c => c.Belanja),
                    TBS = l.Sum(c => c.TBS),
                    TelahGuna = l.Sum(c => c.TBS + c.Belanja),
                    Baki = l.Sum(c => c.Asal + c.Tambah - c.Pindah - c.TBS - c.Belanja),
                }).OrderBy(b => b.Objek).ToList();

            PopulateList();
            return View(vm);
        }
        // printing List of Carta
        [AllowAnonymous]
        public async Task<IActionResult> PrintPDF(int JKWId,
            int JBahagianId,
            string tahun,
            DateTime tarHingga)
        {
            List<AbBelanjawanSemasaViewModel> vm = new List<AbBelanjawanSemasaViewModel>();

            // Waran
            List<AbWaran> warans = await _bsRepo.GetAbWaranBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> waranList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var waran in warans)
            {
                foreach (var waran1 in waran.AbWaran1)
                {
                    waranList = _bsRepo.RunWaranObjekOperation(waran.FlJenisWaran,
                        waran1.TK,
                        waran1.Amaun,
                        waran1.AkCarta.Kod,
                        waran1.AkCarta.Perihal,
                        waran1.AkCarta.JParas.Kod);

                    vm.AddRange(waranList);
                }
            }
            // Waran End

            // PO
            List<AkPO> POs = await _bsRepo.GetAkPOBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> poList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var po in POs)
            {
                foreach (var po1 in po.AkPO1)
                {
                    poList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(po1.Amaun, po1.AkCarta.Kod, po1.AkCarta.Perihal, "4");

                    vm.AddRange(poList);
                }
            }
            // PO End

            // Pendahuluan Pelbagai
            List<SpPendahuluanPelbagai> Sps = await _bsRepo.GetSpPendahuluanPelbagaiBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> spList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var sp in Sps)
            {
                spList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(sp.JumLulus, sp.AkCarta.Kod, sp.AkCarta.Perihal, "4");

                vm.AddRange(spList);

            }
            // Pendahuluan Pelbagai End

            // POLaras
            List<AkPOLaras> POLarass = await _bsRepo.GetAkPOLarasBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> poLarasList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var poLaras in POLarass)
            {
                foreach (var poLaras1 in poLaras.AkPOLaras1)
                {
                    poLarasList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(poLaras1.Amaun, poLaras1.AkCarta.Kod, poLaras1.AkCarta.Perihal, "4");

                    vm.AddRange(poLarasList);
                }
            }
            // POLaras End

            // Inden
            List<AkInden> Indens = await _bsRepo.GetAkIndenBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> indenList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var inden in Indens)
            {
                foreach (var inden1 in inden.AkInden1)
                {
                    indenList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(inden1.Amaun, inden1.AkCarta.Kod, inden1.AkCarta.Perihal, "4");

                    vm.AddRange(indenList);
                }
            }
            // Inden End

            // PV
            List<AkPV> PVs = await _bsRepo.GetAkPVBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> pvList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var pv in PVs)
            {
                foreach (var pv1 in pv.AkPV1)
                {
                    pvList = _bsRepo.RunBaucerObjekOperation(pv.denganTanggungan, pv1.Amaun, pv1.AkCarta.Kod, pv1.AkCarta.Perihal, "4");

                    vm.AddRange(pvList);
                }
            }
            // Pv End

            // Tunai CV
            List<AkTunaiCV> CVs = await _bsRepo.GetAkTunaiCVBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> cvList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var cv in CVs)
            {
                foreach (var cv1 in cv.AkTunaiCV1)
                {
                    cvList = _bsRepo.RunSpPOPOLarasIndenCVObjekOperation(cv1.Amaun, cv1.AkCarta.Kod, cv1.AkCarta.Perihal, "4");

                    vm.AddRange(cvList);
                }
            }
            // TunaiCV End

            // Terima
            List<AkTerima> Terimas = await _bsRepo.GetAkTerimaBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> terimaList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var terima in Terimas)
            {
                foreach (var terima1 in terima.AkTerima1)
                {
                    if (terima1.AkCarta.JJenis.Kod == "B")
                    {
                        terimaList = _bsRepo.RunResitObjekOperation(terima1.Amaun, terima1.AkCarta.Kod, terima1.AkCarta.Perihal, "4");

                        vm.AddRange(terimaList);
                    }

                }
            }
            // Terina End

            // Jurnal
            List<AkJurnal> Jurnals = await _bsRepo.GetAkJurnalBasedOnYear(tahun, JKWId, JBahagianId, tarHingga);

            List<AbBelanjawanSemasaViewModel> jurnalList = new List<AbBelanjawanSemasaViewModel>();

            foreach (var jurnal in Jurnals)
            {
                foreach (var jurnal1 in jurnal.AkJurnal1)
                {
                    if (jurnal1.AkCarta.JJenis.Kod == "B" || jurnal1.AkCarta.JJenis.Kod == "A")
                    {
                        jurnalList = _bsRepo.RunJurnalObjekOperation(jurnal1.Debit, jurnal1.Kredit, jurnal1.AkCarta.Kod, jurnal1.AkCarta.Perihal, "4");

                        vm.AddRange(jurnalList);
                    }

                }
            }
            // Jurnal End

            //
            vm = vm.GroupBy(b => b.Objek)
                .Select(l => new AbBelanjawanSemasaViewModel
                {
                    Objek = l.First().Objek,
                    Perihalan = l.First().Perihalan,
                    Paras = l.First().Paras,
                    Asal = l.Sum(c => c.Asal),
                    Tambah = l.Sum(c => c.Tambah),
                    Pindah = l.Sum(c => c.Pindah),
                    Jumlah = l.Sum(c => c.Asal + c.Tambah - c.Pindah),
                    Belanja = l.Sum(c => c.Belanja),
                    TBS = l.Sum(c => c.TBS),
                    TelahGuna = l.Sum(c => c.TBS + c.Belanja),
                    Baki = l.Sum(c => c.Asal + c.Tambah - c.Pindah - c.TBS - c.Belanja),
                }).ToList();

            //string customSwitches = "--page-offset 0 --footer-center [page] / [toPage] --footer-font-size 6";

            vm = vm.OrderBy(b => b.Objek).ToList();

            var kw = await _context.JKW.FirstOrDefaultAsync(x => x.Id == JKWId);
            var bahagian = await _context.JBahagian.FirstOrDefaultAsync(x => x.Id == JBahagianId);

            var KW = kw.Kod + " - " + kw.Perihal;
            var Bahagian = bahagian.Kod + " - " + bahagian.Perihal;
            var lastDate = tarHingga.ToString("dd/MM/yyyy"); 

            var company = await _userService.GetCompanyDetails();

            return new ViewAsPdf("BelanjawanSemasaPrintPDF", vm,
                new ViewDataDictionary(ViewData) {
                    { "KW", KW },
                    { "Bahagian", Bahagian },
                    { "TarHingga", lastDate },
                    { "NamaSyarikat", company.NamaSyarikat },
                    { "AlamatSyarikat1", company.AlamatSyarikat1 },
                    { "AlamatSyarikat2", company.AlamatSyarikat2 },
                    { "AlamatSyarikat3", company.AlamatSyarikat3 }
                })
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing List of Carta end
    }
}
