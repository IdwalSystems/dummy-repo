using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "Admin , Supervisor")]
    public class AkAkaunController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkAkaun, int> _akAkaunRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkCarta, int> _akCarta1Repo;
        private readonly IRepository<AkCarta, int> _akCarta2Repo;

        public AkAkaunController(
            ApplicationDbContext context,
            IRepository<AkAkaun, int> akAkaunRepository,
            IRepository<JKW, int> kwRepository,
            IRepository<AkCarta, int> akCarta1Repository,
            IRepository<AkCarta, int> akCarta2Repository)
        {
            _context = context;
            _akAkaunRepo = akAkaunRepository;
            _kwRepo = kwRepository;
            _akCarta1Repo = akCarta1Repository;
            _akCarta2Repo = akCarta2Repository;
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
                    akAkBakiAwal.Add(new AkAkaun() {
                        Tarikh = date1,
                        NoRujukan = "Baki Awal",
                        Debit = bakiawalDebit,
                        Kredit = bakiawalKredit
                    });
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
            kwSelect.Add(new SelectListItem() { Text = "-- Pilih Kumpulan Wang --", Value = "" });
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

            List<AkCarta> Carta1List = _context.AkCarta.OrderBy(b => b.Kod).ToList();
            List<SelectListItem> carta1Select = new();
            carta1Select.Add(new SelectListItem() { Text = "-- Pilih Carta 1 --", Value = "" });
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
    }
}
