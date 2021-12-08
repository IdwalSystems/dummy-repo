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
    public class AkCartaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly IRepository<JKW, int> _kwRepo;

        public AkCartaController(
            ApplicationDbContext context,
            IRepository<JKW, int> kwRepository,
            IRepository<AkCarta, int> akCartaRepository)
        {
            _context = context;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
        }

        // GET: AkCarta
        public async Task<IActionResult> Index(string searchString, string searchColumn)
        {
            ViewData["CurrentFilter"] = searchString;
            List<SelectListItem> test = new List<SelectListItem>();
            test.Add(new SelectListItem() { Text = "KW", Value = "" });
            test.Add(new SelectListItem() { Text = "Kod", Value = "1" });
            test.Add(new SelectListItem() { Text = "Perihal", Value = "2" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.searchColumn = new SelectList(test, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.searchColumn = new SelectList(test, "Value", "Text", "");
            }

            var akCarta = await _akCartaRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchColumn == "1")
                {
                    akCarta = akCarta.Where(s => s.Kod.ToUpper().Contains(searchString.ToUpper()));
                    ViewBag.searchColumn = new SelectList(test, "Value", "Text", "1");
                }
                else if (searchColumn == "2")
                {
                    akCarta = akCarta.Where(s => s.Perihal.ToUpper().Contains(searchString.ToUpper()));
                    ViewBag.searchColumn = new SelectList(test, "Value", "Text", "2");
                }
                else
                {
                    akCarta = akCarta.Where(s => s.JKW.Kod.ToUpper().Contains(searchString.ToUpper()));
                    ViewBag.searchColumn = new SelectList(test, "Value", "Text", "");
                }
            }

            return View(akCarta);
        }

        // GET: AkCarta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCarta = await _akCartaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;

            if (akCarta == null)
            {
                return NotFound();
            }

            return View(akCarta);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.Kw = kwList;

            List<JJenis> jenisList = _context.JJenis.OrderBy(b => b.Kod).ToList();
            ViewBag.Jenis = jenisList;

            List<JParas> parasList = _context.JParas.OrderBy(b => b.Kod).ToList();
            ViewBag.Paras = parasList;
        }

        // GET: AkCarta/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkCarta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkCarta akCarta, int JKWId, int JJenisId, int JParasId)
        {
            AkCarta akC = new AkCarta();
            if (ModelState.IsValid)
            {
                if (akCarta != null && JKWId != 0)
                {
                    akC.JKWId = JKWId;
                    akC.Kod = akCarta.Kod;
                    akC.JJenisId = JJenisId;
                    akC.Perihal = akCarta.Perihal;
                    akC.JParasId = JParasId;
                    akC.DebitKredit = akCarta.DebitKredit;
                    akC.UmumDetail = akCarta.UmumDetail;
                    akC.Baki = akCarta.Baki;
                    akC.Catatan1 = akCarta.Catatan1;
                    akC.Catatan2 = akCarta.Catatan2;
                    await _akCartaRepo.Insert(akC);
                    await _akCartaRepo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();

            return View(akCarta);
        }

        // GET: AkCarta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PopulateList();
            var akCarta = await _akCartaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;
            if (akCarta == null)
            {
                return NotFound();
            }
            
            return View(akCarta);
        }

        // POST: AkCarta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkCarta akCarta, int JKWId, int JJenisId, int JParasId)
        {

            if (id != akCarta.Id)
            {
                return NotFound();
            }

            AkCarta akC = new AkCarta();

            if (ModelState.IsValid)
            {
                try
                {
                    akC.JKWId = JKWId;
                    akC.Kod = akCarta.Kod;
                    akC.JJenisId = JJenisId;
                    akC.Perihal = akCarta.Perihal;
                    akC.JParasId = JParasId;
                    akC.UmumDetail = akCarta.UmumDetail;
                    akC.DebitKredit = akCarta.DebitKredit;
                    akC.Baki = akCarta.Baki;
                    akC.Catatan1 = akCarta.Catatan1;
                    akC.Catatan2 = akCarta.Catatan2;
                    await _akCartaRepo.Update(akCarta);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkCartaExists(akCarta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PopulateList();
                return RedirectToAction(nameof(Index));
            }

            return View(akCarta);
        }

        // GET: AkCarta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCarta = await _akCartaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;

            if (akCarta == null)
            {
                return NotFound();
            }

            return View(akCarta);
        }

        // POST: AkCarta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akCarta = await _context.AkCarta
                .Include(q => q.AkAkaun1)
                .Include(q => q.AkAkaun2)
                .Include(q => q.AkBank)
                .Include(q => q.AkPO1)
                .Include(q => q.AkTerima1).FirstOrDefaultAsync(q => q.Id == id);
            _context.AkCarta.Remove(akCarta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkCartaExists(int id)
        {
            return _context.AkCarta.Any(e => e.Id == id);
        }
    }
}
