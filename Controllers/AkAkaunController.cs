using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkAkaunController : Controller
    {
        public const string modul = "AK002";
        public const string namamodul = "Lejar Am";

        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCarta1Repo;
        private readonly IRepository<AkCarta, int, string> _akCarta2Repo;
        private readonly UserService _userService;

        public AkAkaunController(
            ApplicationDbContext context,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCarta1Repository,
            IRepository<AkCarta, int, string> akCarta2Repository,
            UserService userService)
        {
            _context = context;
            _akAkaunRepo = akAkaunRepository;
            _kwRepo = kwRepository;
            _akCarta1Repo = akCarta1Repository;
            _akCarta2Repo = akCarta2Repository;
            _userService = userService;
        }

        // GET: AkAkaun
        public async Task<IActionResult> Index(
            string searchKW,
            string searchCarta,
            string searchFrom,
            string searchUntil)
        {
            PopulateList(!String.IsNullOrEmpty(searchKW) ? searchKW : "", !String.IsNullOrEmpty(searchCarta) ? searchCarta : "");
            ViewData["searchFrom"] = searchFrom;
            ViewData["searchUntil"] = searchUntil;
            if (string.IsNullOrEmpty(searchKW) 
                && string.IsNullOrEmpty(searchCarta) 
                && string.IsNullOrEmpty(searchFrom) 
                && string.IsNullOrEmpty(searchUntil))
            {
                List<AkAkaun> aka = new();
                return View(aka);
            }
            var akAkaun = await _akAkaunRepo.GetAll();
            var Carta = await _context.AkCarta.FirstOrDefaultAsync(b => b.Kod == searchCarta);

            List<AkAkaun> akAkBakiAwal = new();
            decimal bakiawalDebit = 0;
            decimal bakiawalKredit = 0;

            if (!String.IsNullOrEmpty(searchKW))
            {
                akAkaun = akAkaun.Where(q => q.JKW.Kod == searchKW);
            }

            if (!String.IsNullOrEmpty(searchCarta))
            {
                akAkaun = akAkaun.Where(q => q.AkCarta1.Kod == searchCarta);
            }

            if (!String.IsNullOrEmpty(searchFrom) && !String.IsNullOrEmpty(searchUntil))
            {
                DateTime date1 = DateTime.Parse(searchFrom);
                DateTime date2 = DateTime.Parse(searchUntil).AddHours(23.99);
                foreach (var i in akAkaun.Where(q=>q.Tarikh<date1))
                {
                    bakiawalDebit += i.Debit;
                    bakiawalKredit += i.Kredit;
                };
                akAkaun = akAkaun.Where(x => x.Tarikh >= date1 && x.Tarikh <= date2);
                //akAkaun = akAkaun.OrderByDescending(c => c.Tarikh.Date).ThenBy(c => c.Tarikh.TimeOfDay);

                if (bakiawalDebit>0 || bakiawalKredit > 0)
                {
                    if (Carta.DebitKredit == "K")
                    {
                        akAkBakiAwal.Add(new AkAkaun()
                        {
                            Tarikh = date1,
                            NoRujukan = "Baki Awal",
                            Debit = bakiawalKredit,
                            Kredit = bakiawalDebit
                        });
                    }
                    else
                    {
                        akAkBakiAwal.Add(new AkAkaun()
                        {
                            Tarikh = date1,
                            NoRujukan = "Baki Awal",
                            Debit = bakiawalDebit,
                            Kredit = bakiawalKredit
                        });
                    }
                    foreach(var i in akAkaun)
                    {
                        akAkBakiAwal.Add(new AkAkaun() {
                            JKWId = i.JKWId,
                            AkCartaId1=i.AkCartaId1,
                            Tarikh = i.Tarikh,
                            AkCartaId2=i.AkCartaId2,
                            Id=i.Id,
                            NoRujukan=i.NoRujukan,
                            Debit=i.Debit,
                            Kredit=i.Kredit,
                            JKW=i.JKW,
                            AkCarta1 = i.AkCarta1,
                            AkCarta2 = i.AkCarta2
                        });
                    }
                };
            }
            if (bakiawalDebit > 0 || bakiawalKredit > 0)
            {
                return View(akAkBakiAwal.OrderBy(c => c.Tarikh));
            }
            else
            {
                return View(akAkaun.OrderBy(c => c.Tarikh));
            }
        }

        private void PopulateList(string searchedKw, string searchedCarta)
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            List<SelectListItem> kwSelect = new();
            foreach (var q in kwList)
            {
                kwSelect.Add(new SelectListItem() { Text = q.Kod + " - " + q.Perihal, Value = q.Kod });
            }
            if (!String.IsNullOrEmpty(searchedKw))
            {
                ViewBag.Kw = new SelectList(kwSelect, "Value", "Text", searchedKw);
            }
            else
            {
                ViewBag.Kw = new SelectList(kwSelect, "Value", "Text", "");
            }

            List<AkCarta> Carta1List = _context.AkCarta.Include(b => b.JParas).Where(b => b.JParas.Kod == "4").OrderBy(b => b.Kod).ToList();
            List<SelectListItem> carta1Select = new();
            carta1Select.Add(new SelectListItem() { Text = "-- Pilih Kod Akaun --", Value = "" });
            foreach (var q in Carta1List)
            {
                carta1Select.Add(new SelectListItem() { Text = q.Kod + " - " + q.Perihal, Value = q.Kod });
            }
            if (!String.IsNullOrEmpty(searchedCarta))
            {
                ViewBag.AkCarta1 = new SelectList(carta1Select, "Value", "Text", searchedCarta);
            }
            else
            {
                ViewBag.AkCarta1 = new SelectList(carta1Select, "Value", "Text", "");
            }
        }

        // printing List of Carta
        [AllowAnonymous]
        public async Task<IActionResult> PrintLejarAkaun(
            string searchKW,
            string searchCarta,
            string tarDari,
            string tarHingga)
        {
            //IEnumerable<AkAkaun> akAkaun = await _akAkaunRepo.GetAll();
            
            if (string.IsNullOrEmpty(searchKW))
            {
                TempData[SD.Error] = "Sila isi ruangan Kump. Wang";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(searchCarta))
            {
                TempData[SD.Error] = "Sila isi ruangan Kod Akaun";
                return RedirectToAction(nameof(Index));
            }

            var akAkaun = await _context.AkAkaun
                .Include(b => b.JKW)
                .Include(b => b.AkCarta1)
                .Include(b => b.AkCarta2)
                .ToListAsync();

            List<AkAkaun> akAkBakiAwal = new List<AkAkaun>();
            decimal bakiawalDebit = 0;
            decimal bakiawalKredit = 0;

            if (!String.IsNullOrEmpty(searchKW))
            {
                akAkaun = akAkaun.Where(q => q.JKW.Kod == searchKW).ToList();
            }

            if (!String.IsNullOrEmpty(searchCarta))
            {
                akAkaun = akAkaun.Where(q => q.AkCarta1.Kod == searchCarta).ToList();
            }

            if (!String.IsNullOrEmpty(tarDari) && !String.IsNullOrEmpty(tarHingga))
            {
                DateTime date1 = DateTime.Parse(tarDari);
                DateTime date2 = DateTime.Parse(tarHingga).AddHours(23.99);
                foreach (var i in akAkaun.Where(q => q.Tarikh<date1))
                {
                    bakiawalDebit += i.Debit;
                    bakiawalKredit += i.Kredit;
                };
                akAkaun = akAkaun.Where(x => x.Tarikh >= date1 && x.Tarikh <= date2).ToList();
                //akAkaun = akAkaun.OrderByDescending(c => c.Tarikh.Date).ThenBy(c => c.Tarikh.TimeOfDay);

                if (bakiawalDebit>0 || bakiawalKredit > 0)
                {
                    akAkBakiAwal.Add(new AkAkaun()
                    {
                        Tarikh = date1,
                        NoRujukan = "Baki Awal",
                        Debit = bakiawalDebit,
                        Kredit = bakiawalKredit
                    });
                    foreach (var i in akAkaun)
                    {
                        akAkBakiAwal.Add(new AkAkaun()
                        {
                            JKWId = i.JKWId,
                            AkCartaId1=i.AkCartaId1,
                            Tarikh = i.Tarikh,
                            AkCartaId2=i.AkCartaId2,
                            Id=i.Id,
                            NoRujukan=i.NoRujukan,
                            Debit=i.Debit,
                            Kredit=i.Kredit,
                            JKW=i.JKW,
                            AkCarta1 = i.AkCarta1,
                            AkCarta2 = i.AkCarta2
                        });
                    }
                };
            }
            if (bakiawalDebit > 0 || bakiawalKredit > 0)
            {
                akAkBakiAwal = akAkBakiAwal.OrderBy(c => c.Tarikh).ToList();

                akAkaun = akAkBakiAwal;
            }
            else
            {
                akAkaun = akAkaun.OrderBy(c => c.Tarikh).ToList();
            }

            var kw = await _context.JKW.FirstOrDefaultAsync(x => x.Kod == searchKW);
            var carta = await _context.AkCarta.FirstOrDefaultAsync(x => x.Kod == searchCarta);

            searchKW = kw.Kod + " - " + kw.Perihal;
            searchCarta = carta.Kod + " - " + carta.Perihal;
            //string customSwitches = "--page-offset 0 --footer-center [page] / [toPage] --footer-font-size 6";

            var company = await _userService.GetCompanyDetails();

            return new ViewAsPdf("LejarAkaunPrintPDF", akAkaun, 
                new ViewDataDictionary(ViewData) { {"searchKW", searchKW },
                {"searchCarta", searchCarta },
                {"tarDari", tarDari },
                {"tarHingga", tarHingga },
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
