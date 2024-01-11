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
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class AbKunciKiraKiraController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserService _userService;
        private readonly CustomIRepository<string, int> _custom;
        public AbKunciKiraKiraController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            UserService userService,
            CustomIRepository<string, int> custom)
        {
            _context=context;
            _userManager=userManager;
            _userService=userService;
            _custom=custom;
        }

        // note :
        // filter = [JBahagianId], [JKWId], [comp.TarMula], [form.TarHingga]
        // formula :
        // 1. total aset
        // - Sum(Debit - Kredit) carta1 Jenis "A" from akAkaun
        // 2. total liabiliti
        // - Sum(Kredit - Debit) carta1 Jenis "L" from akAkaun
        // 3. Aset bersih = aset - liabiliti
        // - [3] = [1] - [2]
        // 4. total ekuiti
        // - Sum(Kredit - Debit) carta1 Jenis "E" from akAkaun
        // 5. untung / rugi = total pendapatan (hasil)  - total belanja
        // 5.1. total pendapatan = Sum(Kredit - Debit) carta1 Jenis "H" from akAkaun
        // 5.2. total belanja = Sum(Debit - Kredit) carta1 Jenis "B" from akAkaun
        // - [5] = [5.1] - [5.2]
        // 6. total ekuiti + untung / rugi = aset bersih
        // [4] + [5] = [3]

        public async Task<IActionResult> Index(PenyataFormModel form)
        {
            var kunciKiraKira = new List<AbKunciKiraKiraViewModel>();

            PopulateSelectList(form.JKWId, form.JBahagianId, form.TarHingga1);

            kunciKiraKira = await _custom.GetListKunciKirakiraBasedOnLastDate(form.JBahagianId, form.JKWId, form.TarHingga1);

            dynamic dyModel = new ExpandoObject();
            dyModel.KunciKirakira = kunciKiraKira;
            dyModel.KunciKirakiraGrouped = kunciKiraKira.GroupBy(b => b.Order);
            return View(dyModel);
        }

        public void PopulateSelectList(int JKWId, int JBahagianId, DateTime TarHingga)
        {
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
                //        Text = item.Kod + " - " + item.Perihal,
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

            if (TarHingga.ToString("yyyy/MM/dd") != "0001/01/01")
            {
                ViewData["DateTo"] = TarHingga.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            else
            {
                ViewData["DateTo"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }

        public async Task<IActionResult> PrintPDF(PenyataFormModel form)
        {
            var kunciKiraKira = new List<AbKunciKiraKiraViewModel>();

            PopulateSelectList(form.JKWId, form.JBahagianId, form.TarHingga1);

            if (form.JKWId != 0)
            {
                kunciKiraKira = await _custom.GetListKunciKirakiraBasedOnLastDate(form.JBahagianId, form.JKWId, form.TarHingga1);

                dynamic dyModel = new ExpandoObject();
                dyModel.KunciKirakira = kunciKiraKira;
                dyModel.KunciKirakiraGrouped = kunciKiraKira.GroupBy(b => b.Order);

                var jkw = await _context.JKW.FirstOrDefaultAsync(b => b.Id == form.JKWId);

                var company = await _userService.GetCompanyDetails();

                return new ViewAsPdf("KunciKirakiraPrintPDF", dyModel,
                        new ViewDataDictionary(ViewData)
                        {
                        { "TarHingga", form.TarHingga1.ToString("dd/MM/yyyy hh:mm:ss tt") },
                        { "NamaKW", jkw.Kod + " - " + jkw.Perihal },
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
                var date2 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                ViewData["DateTo"] = date2;

                PopulateSelectList(form.JKWId, form.JBahagianId, form.TarHingga1);

                TempData[SD.Error] = "Kump. Wang Tidak Wujud.";

                return View(kunciKiraKira);
            }
        }
    }
}
