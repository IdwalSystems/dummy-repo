
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class AbAlirTunaiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserService _userService;
        private readonly CustomIRepository<string, int> _custom;
        public AbAlirTunaiController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            UserService userService,
            CustomIRepository<string, int> custom)
        {
            _context=context;
            _userManager=userManager;
            _userService=userService;
            _custom=custom;
        }

        public async Task<IActionResult> Index(PenyataFormModel form)
        {
            var alirTunai = new List<AbAlirTunaiViewModel>();

            PopulateSelectList( form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1, null, null );

            var date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
            var date2 = DateTime.Now.Year.ToString() + "-12-31T23:59:59";
            //ViewData["Tahun"] = DateTime.Now.Year.ToString();

            if (form.AkBankId != 0)
            {
                // find previous balance
                
                AbAlirTunaiViewModel bakiAwal = await _custom.GetCarryPreviousBalanceEachStartingMonth(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.Add(bakiAwal);

                List<AbAlirTunaiViewModel> tunaiMasuk = await _custom.GetListAlirTunaiMasukBasedOnYear(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.AddRange(tunaiMasuk);

                List<AbAlirTunaiViewModel> tunaiKeluar = await _custom.GetListAlirTunaiKeluarBasedOnYear(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.AddRange(tunaiKeluar);

                AbAlirTunaiViewModel bakiAkhir = new AbAlirTunaiViewModel();

                if (bakiAwal != null)
                {
                    bakiAkhir.NoAkaun = bakiAwal.NoAkaun;
                    bakiAkhir.NamaAkaun = bakiAwal.NamaAkaun;
                    bakiAkhir.KeluarMasuk = 3;
                    bakiAkhir.Jan = bakiAwal.Feb;
                    bakiAkhir.Feb = bakiAwal.Mac;
                    bakiAkhir.Mac = bakiAwal.Apr;
                    bakiAkhir.Apr = bakiAwal.Mei;
                    bakiAkhir.Mei = bakiAwal.Jun;
                    bakiAkhir.Jun = bakiAwal.Jul;
                    bakiAkhir.Jul = bakiAwal.Ogo;
                    bakiAkhir.Ogo = bakiAwal.Sep;
                    bakiAkhir.Sep = bakiAwal.Okt;
                    bakiAkhir.Okt = bakiAwal.Nov;
                    bakiAkhir.Nov = bakiAwal.Dis;
                    bakiAkhir.Dis = bakiAwal.JumAkaun1;
                    bakiAkhir.JumAkaun1 = bakiAwal.JumAkaun1;

                    alirTunai.Add(bakiAkhir);
                }
            }

            return View(alirTunai);
        }

        public async Task<IActionResult> IndexTahun(PenyataFormModel form)
        {
            // dummy
            //form.AkBankId = 2;
            //form.JKWId = 1;
            //form.JBahagianId = 0;
            //form.Tahun1 = "2021";
            //form.Tahun2 = "2022";
            //form.Tahun3 = "2023";

            var bakiAkhir = new List<AbAlirTunaiViewModel>();

            PopulateSelectList(0, form.JKWId, form.JBahagianId, form.Tahun1,form.Tahun2,form.Tahun3);

            var date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
            var date2 = DateTime.Now.Year.ToString() + "-12-31T23:59:59";
            //ViewData["Tahun"] = DateTime.Now.Year.ToString();

            if (form.Tahun1 != null)
            {
                bakiAkhir = await _custom.GetListAlirTunaiBasedOnComparedYear(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1, form.Tahun2, form.Tahun3);

            }
            bakiAkhir = bakiAkhir.GroupBy(b => new { b.NoAkaun })
                .Select(l => new AbAlirTunaiViewModel
                {
                    NoAkaun = l.First().NoAkaun,
                    NamaAkaun = l.First().NamaAkaun,
                    JumAkaun1 = l.Sum(j => j.JumAkaun1),
                    JumAkaun2 = l.Sum(j => j.JumAkaun2),
                    JumAkaun3 = l.Sum(j => j.JumAkaun3)

                }).OrderBy(b => b.NoAkaun).ToList();
            return View(bakiAkhir);
        }

        public void PopulateSelectList(int AkBankId, int JKWId, int JBahagianId, string Tahun1, string Tahun2, string Tahun3)
        {
            if (String.IsNullOrWhiteSpace(Tahun1))
                ViewData["Tahun1"] = DateTime.Now.Year.ToString();
            else 
                ViewData["Tahun1"] = Tahun1;

            if (String.IsNullOrWhiteSpace(Tahun2))
                ViewData["Tahun2"] = null;
            else
                ViewData["Tahun2"] = Tahun2;

            if (String.IsNullOrWhiteSpace(Tahun3))
                ViewData["Tahun3"] = null;
            else
                ViewData["Tahun3"] = Tahun3;

            // populate list bank 
            List<AkBank> akBankList = _context.AkBank.Include(b => b.AkCarta).ToList();

            var bankSelect = new List<SelectListItem>();

            if (akBankList != null)
            {
                foreach (var item in akBankList)
                {
                    bankSelect.Add(new SelectListItem()
                    {
                        Text = item.NoAkaun + " (" + item.AkCarta.Kod + " - " + item.AkCarta.Perihal + ")",
                        Value = item.Id.ToString()
                    });
                }
                ViewBag.bank = new SelectList(bankSelect, "Value", "Text", AkBankId);

            }
            else
            {
                bankSelect.Add(new SelectListItem()
                {
                    Text = "-- Tiada Bank Berdaftar --",
                    Value = ""
                });

                ViewBag.bank = new SelectList(bankSelect, "Value", "Text", 0);
            }
            // populate list bank end

            // populate list JKW
            List<JKW> jKWList = _context.JKW.ToList();

            var jKWSelect = new List<SelectListItem>();

            if (jKWList != null)
            {
                foreach (var item in jKWList)
                {
                    jKWSelect.Add(new SelectListItem()
                    {
                        Text = item.Kod + " - " + item.Perihal,
                        Value = item.Id.ToString()
                    });
                }
                ViewBag.jKW = new SelectList(jKWSelect, "Value", "Text", JKWId);
            }
            else
            {
                jKWSelect.Add(new SelectListItem()
                {
                    Text = "-- Tiada Kump. Wang Berdaftar --",
                    Value = ""
                });
                ViewBag.jKW = new SelectList(jKWSelect, "Value", "Text", 0);
            }

            // populate list bahagian 
            List<JBahagian> akBahagianList = _context.JBahagian.Include(b => b.JKW).ToList();

            var bahagianSelect = new List<SelectListItem>();

            if (akBahagianList != null)
            {
                bahagianSelect.Add(new SelectListItem()
                {
                    Text = "SEMUA",
                    Value = "0"
                });
                //foreach (var item in akBahagianList)
                //{
                //    bahagianSelect.Add(new SelectListItem()
                //    {
                //        Text = item.Kod + " - " + item.Perihal ,
                //        Value = item.Id.ToString()
                //    });
                //}
                ViewBag.bahagian = new SelectList(bahagianSelect, "Value", "Text", JBahagianId);

            }
            else
            {
                bahagianSelect.Add(new SelectListItem()
                {
                    Text = "-- Tiada Bahagian Berdaftar --",
                    Value = ""
                });

                ViewBag.bahagian = new SelectList(bahagianSelect, "Value", "Text", 0);
            }
            // populate list bahagian end

        }

        public async Task<IActionResult> PrintPDF(PenyataFormModel form)
        {
            var alirTunai = new List<AbAlirTunaiViewModel>();

            PopulateSelectList(form.JKWId, form.AkBankId, form.JBahagianId, form.Tahun1, null, null);

            var date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
            var date2 = DateTime.Now.Year.ToString() + "-12-31T23:59:59";
            ViewData["Tahun"] = DateTime.Now.Year.ToString();

            if (form.AkBankId != 0)
            {

                AbAlirTunaiViewModel bakiAwal = await _custom.GetCarryPreviousBalanceEachStartingMonth(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.Add(bakiAwal);

                List<AbAlirTunaiViewModel> tunaiMasuk = await _custom.GetListAlirTunaiMasukBasedOnYear(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.AddRange(tunaiMasuk);

                List<AbAlirTunaiViewModel> tunaiKeluar = await _custom.GetListAlirTunaiKeluarBasedOnYear(form.AkBankId, form.JKWId, form.JBahagianId, form.Tahun1);

                alirTunai.AddRange(tunaiKeluar);

                AbAlirTunaiViewModel bakiAkhir = new AbAlirTunaiViewModel();

                bakiAkhir.NoAkaun = bakiAwal.NoAkaun;
                bakiAkhir.NamaAkaun = bakiAwal.NoAkaun;
                bakiAkhir.KeluarMasuk = 3;
                bakiAkhir.Jan = bakiAwal.Feb;
                bakiAkhir.Feb = bakiAwal.Mac;
                bakiAkhir.Mac = bakiAwal.Apr;
                bakiAkhir.Apr = bakiAwal.Mei;
                bakiAkhir.Mei = bakiAwal.Jun;
                bakiAkhir.Jun = bakiAwal.Jul;
                bakiAkhir.Jul = bakiAwal.Ogo;
                bakiAkhir.Ogo = bakiAwal.Sep;
                bakiAkhir.Sep = bakiAwal.Okt;
                bakiAkhir.Okt = bakiAwal.Nov;
                bakiAkhir.Nov = bakiAwal.Dis;
                bakiAkhir.Dis = bakiAwal.Jan2;
                bakiAkhir.JumAkaun1 = bakiAwal.Jan2;

                alirTunai.Add(bakiAkhir);

                var bank = await _context.AkBank
                    .Include(b => b.AkCarta)
                    .FirstOrDefaultAsync(b => b.Id == form.AkBankId);

                var jkw = await _context.JKW.FirstOrDefaultAsync(b => b.Id == form.JKWId);

                var company = await _userService.GetCompanyDetails();

                ViewData["Tahun"] = form.Tahun1;

                return new ViewAsPdf("AlirTunaiPrintPDF", alirTunai,
                    new ViewDataDictionary(ViewData)
                    {
                        { "NamaKW", jkw.Kod + " - " + jkw.Perihal },
                        { "NamaBank", bank.NoAkaun + " (" + bank.AkCarta.Kod + " - " + bank.AkCarta.Perihal +") "},
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
            else
            {

                date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
                date2 = DateTime.Now.Year.ToString() + "-12-31T23:59:59";
                ViewData["Tahun"] = DateTime.Now.Year.ToString();

                PopulateSelectList(form.JKWId, form.AkBankId, form.JBahagianId, form.Tahun1, null, null);

                TempData[SD.Error] = "Akaun Bank Tidak Wujud.";

                return View(alirTunai.OrderBy(b => b.KeluarMasuk).ToList());
            }

        }
    }
}
