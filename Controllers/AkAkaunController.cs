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
    [Authorize]
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
            ViewData["searchFrom"] = searchFrom;
            ViewData["searchUntil"] = searchUntil;

            PopulateList(!String.IsNullOrEmpty(searchKW) ? searchKW : "", !String.IsNullOrEmpty(searchCarta) ? searchCarta : "");

            var akAkaun = await _akAkaunRepo.GetAll();

            if (!String.IsNullOrEmpty(searchKW))
            {
                akAkaun = akAkaun.Where(q => q.JKW.Kod == searchKW);
            }

            if (!String.IsNullOrEmpty(searchCarta))
            {
                akAkaun = akAkaun.Where(q => q.AkCarta1.Kod == searchCarta);
            }

            if (!String.IsNullOrEmpty(searchFrom))
            {
                akAkaun = akAkaun.Where(q => q.Tarikh > Convert.ToDateTime(searchFrom));
            }

            if (!String.IsNullOrEmpty(searchUntil))
            {
                akAkaun = akAkaun.Where(q => q.Tarikh < Convert.ToDateTime(searchUntil));
            }

            akAkaun = akAkaun.OrderBy(q => q.NoRujukan);

            return View(akAkaun);
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
            carta1Select.Add(new SelectListItem() { Text = "-- Pilih Carta --", Value = "" });
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
