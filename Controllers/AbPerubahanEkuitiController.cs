using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.ViewModel;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;
using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize]
    public class AbPerubahanEkuitiController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserService _userService;
        private readonly CustomIRepository<string, int> _custom;

        public AbPerubahanEkuitiController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            UserService userService,
            CustomIRepository<string, int> custom)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
            _custom = custom;
        }
        public async Task<IActionResult> Index(PenyataFormModel form)
        {

            var perubahanEkuitiKW = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiRizab = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiAnakSyarikat = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiKepentinganBukanKawalan = new AbPerubahanEkuitiViewModel();

            if (form.Tahun1 == null)
            {
                form.Tahun1 = DateTime.Now.Year.ToString();
            }

            PopulateSelectList(form.Tahun1);

            if (form.Tahun1 != null)
            {

                perubahanEkuitiKW = await _custom.GetAbPerubahanEkuiti(EnJenisLajurJadualPerubahanEkuiti.KumpWang, 1, form.Tahun1 ?? DateTime.Now.Year.ToString());

                perubahanEkuitiRizab = await _custom.GetAbPerubahanEkuiti(EnJenisLajurJadualPerubahanEkuiti.Rizab, null, form.Tahun1 ?? DateTime.Now.Year.ToString());


            }

            dynamic dyModel = new ExpandoObject();
            dyModel.PerubahanEkuitiKW = perubahanEkuitiKW;
            dyModel.PerubahanEkuitiRizab = perubahanEkuitiRizab;
            return View(dyModel);
        }

        private void PopulateSelectList(string tahun1)
        {

            // populate tahun
            if (String.IsNullOrWhiteSpace(tahun1))
                ViewData["Tahun1"] = DateTime.Now.Year.ToString();
            else
                ViewData["Tahun1"] = tahun1;
        }

        public async Task<IActionResult> PrintPDF(PenyataFormModel form)
        {
            var perubahanEkuitiKW = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiRizab = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiAnakSyarikat = new AbPerubahanEkuitiViewModel();
            var perubahanEkuitiKepentinganBukanKawalan = new AbPerubahanEkuitiViewModel();
            dynamic dyModel = new ExpandoObject();
            if (form.Tahun1 == null)
            {
                form.Tahun1 = DateTime.Now.Year.ToString();
            }

            PopulateSelectList(form.Tahun1);

            if (form.Tahun1 != null)
            {

                perubahanEkuitiKW = await _custom.GetAbPerubahanEkuiti(EnJenisLajurJadualPerubahanEkuiti.KumpWang, 1, form.Tahun1 ?? DateTime.Now.Year.ToString());

                perubahanEkuitiRizab = await _custom.GetAbPerubahanEkuiti(EnJenisLajurJadualPerubahanEkuiti.Rizab, null, form.Tahun1 ?? DateTime.Now.Year.ToString());


                dyModel.PerubahanEkuitiKW = perubahanEkuitiKW;
                dyModel.PerubahanEkuitiRizab = perubahanEkuitiRizab;
                var jkw = await _context.JKW.FirstOrDefaultAsync(jkw => jkw.Id == 1);

                var company = await _userService.GetCompanyDetails();

                return new ViewAsPdf("PerubahanEkuitiPDF", dyModel,
                    new ViewDataDictionary(ViewData)
                    {
                        { "NamaKW", jkw?.Kod + " - " + jkw?.Perihal },
                        { "Tahun", form.Tahun1 },
                        { "NamaSyarikat", company.NamaSyarikat },
                        { "AlamatSyarikat1", company.AlamatSyarikat1 },
                        { "AlamatSyarikat2", company.AlamatSyarikat2 },
                        { "AlamatSyarikat3", company.AlamatSyarikat3 }
                    })
                {
                    PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
            }

            else
            {
                ViewData["Tahun"] = DateTime.Now.Year.ToString();

                PopulateSelectList(form.Tahun1 ?? DateTime.Now.Year.ToString());

                TempData[SD.Error] = "Kump. Wang bagi tahun tersebut tidak wujud.";

                return View(dyModel);
            }

        }
    }
}
