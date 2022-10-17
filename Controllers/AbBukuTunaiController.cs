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
    public class AbBukuTunaiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserService _userService;
        private readonly CustomIRepository<string, int> _custom;

        public AbBukuTunaiController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            UserService userService,
            CustomIRepository<string,int> custom)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
            _custom = custom;
        }

        public async Task<IActionResult> Index(PenyataFormModel form)
        {
            
            var bukuTunai = new List<AbBukuTunaiViewModel>();

            var date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
            var date2 = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
            ViewData["DateFrom"] = date1;
            ViewData["DateTo"] = date2;

            PopulateSelectList(form.AkBankId, form.TarDari, form.TarHingga);

            if (form.AkBankId != 0)
            {

                // cari baki bawa ke hadapan
                decimal previousBalance = await _custom.GetCarryPreviousBalanceBasedOnStartingDate(form.AkBankId, form.JKWId, form.JBahagianId, form.TarDari);

                bukuTunai.Add(new AbBukuTunaiViewModel()
                {
                    TarMasuk = null,
                    NamaAkaunMasuk = "BAKI BAWA KE HADAPAN",
                    NoRujukanMasuk = "",
                    AmaunMasuk = previousBalance,
                    JumlahMasuk = 0,
                    TarKeluar = null,
                    NamaAkaunKeluar = "",
                    AmaunKeluar = 0,
                    NoRujukanKeluar = "",
                    JumlahKeluar = 0,
                    KeluarMasuk = 0
                });

                var bukuTunaiSemasa = await _custom.GetListBukuTunaiBasedOnRangeDate(form.AkBankId, form.JKWId, form.JBahagianId, form.TarDari, form.TarHingga);

                bukuTunai.AddRange(bukuTunaiSemasa);
            }
            return View(bukuTunai.OrderBy(b => b.KeluarMasuk).ThenBy(b => b.TarMasuk).ThenBy(b => b.TarKeluar).ToList());
        }

        public void PopulateSelectList(int AkBankId, DateTime TarDari, DateTime TarHingga)
        {
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

                if (TarDari.ToString("yyyy/MM/dd") != "0001/01/01")
                {
                    ViewData["DateFrom"] = TarDari.ToString("yyyy-MM-ddThh:mm:ss");
                    ViewData["DateTo"] = TarHingga.ToString("yyyy-MM-ddThh:mm:ss");
                }
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

        }

        public async Task<IActionResult> PrintPDF(PenyataFormModel form)
        {
            var bukuTunai = new List<AbBukuTunaiViewModel>();

            if (form.AkBankId != 0)
            {

                // cari baki bawa ke hadapan
                decimal previousBalance = await _custom.GetCarryPreviousBalanceBasedOnStartingDate(form.AkBankId, form.JKWId, form.JBahagianId, form.TarDari);

                bukuTunai.Add(new AbBukuTunaiViewModel()
                {
                    TarMasuk = null,
                    NamaAkaunMasuk = "BAKI BAWA KE HADAPAN",
                    NoRujukanMasuk = "",
                    AmaunMasuk = previousBalance,
                    JumlahMasuk = 0,
                    TarKeluar = null,
                    NamaAkaunKeluar = "",
                    AmaunKeluar = 0,
                    NoRujukanKeluar = "",
                    JumlahKeluar = 0,
                    KeluarMasuk = 0
                });

                var bukuTunaiSemasa = await _custom.GetListBukuTunaiBasedOnRangeDate(form.AkBankId, form.JKWId, form.JBahagianId, form.TarDari, form.TarHingga);

                bukuTunai.AddRange(bukuTunaiSemasa);

                var bank = await _context.AkBank
                    .Include(b => b.AkCarta)
                    .FirstOrDefaultAsync(b => b.Id == form.AkBankId);

                var company = await _userService.GetCompanyDetails();

                return new ViewAsPdf("BukuTunaiPrintPDF", bukuTunai,
                    new ViewDataDictionary(ViewData)
                    {
                        { "TarDari", form.TarDari.ToString("dd/MM/yyyy hh:mm:ss tt") },
                        { "TarHingga", form.TarHingga.ToString("dd/MM/yyyy hh:mm:ss tt") },
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

                var date1 = DateTime.Now.Year.ToString() + "-01-01T00:00:01";
                var date2 = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                ViewData["DateFrom"] = date1;
                ViewData["DateTo"] = date2;

                PopulateSelectList(form.AkBankId, form.TarDari, form.TarHingga);

                TempData[SD.Error] = "Akaun Bank Tidak Wujud.";

                return View(bukuTunai.OrderBy(b => b.KeluarMasuk).ThenBy(b => b.TarMasuk).ThenBy(b => b.TarKeluar).ToList());
            }

        }

    }
}
