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
        private readonly IRepository<KW, int> _kwRepo;
        private readonly IRepository<AkCarta, int> _akCarta1Repo;
        private readonly IRepository<AkCarta, int> _akCarta2Repo;

        public AkAkaunController(
            ApplicationDbContext context,
            IRepository<AkAkaun, int> akAkaunRepository,
            IRepository<KW, int> kwRepository,
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
        public async Task<IActionResult> Index()
        {
            var akAkaun = await _akAkaunRepo.GetAll();
            return View(akAkaun);
        }

        // GET: AkAkaun/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akAkaun = await _akAkaunRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akAkaun.KWId);
            akAkaun.KW = kw;
            var akCarta1 = await _akCarta1Repo.GetById(akAkaun.AkCartaId1);
            akAkaun.AkCarta1 = akCarta1;
            var akCarta2 = await _akCarta2Repo.GetById(akAkaun.AkCartaId2);
            akAkaun.AkCarta2 = akCarta2;

            if (akAkaun == null)
            {
                return NotFound();
            }

            return View(akAkaun);
        }

        private void PopulateList()
        {
            List<KW> kwList = _context.KW.OrderBy(b => b.Kod).ToList();
            ViewBag.Kw = kwList;

            List<AkCarta> akCarta1List = _context.AkCarta.OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta1 = akCarta1List;

            List<AkCarta> akCarta2List = _context.AkCarta.OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta2 = akCarta2List;
        }
        // GET: AkAkaun/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkAkaun/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkAkaun akAkaun,int KWId, int AkCartaId1, int AkCartaId2)
        {
            AkAkaun m = new AkAkaun();
            if (ModelState.IsValid)
            {
                if (akAkaun != null && KWId != 0 && AkCartaId1 != 0 && AkCartaId2 != 0)
                {
                    m.KWId = KWId;
                    m.AkCartaId1 = AkCartaId1;
                    m.AkCartaId2 = AkCartaId2;
                    m.Tarikh = akAkaun.Tarikh;
                    m.NoRujukan = akAkaun.NoRujukan;
                    m.Debit = akAkaun.Debit;
                    m.Kredit = akAkaun.Kredit;
                    await _akAkaunRepo.Insert(m);
                    await _akAkaunRepo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akAkaun);
        }

        // GET: AkAkaun/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PopulateList();
            var akAkaun = await _akAkaunRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akAkaun.KWId);
            akAkaun.KW = kw;
            var akCarta1 = await _akCarta1Repo.GetById(akAkaun.AkCartaId1);
            akAkaun.AkCarta1 = akCarta1;
            var akCarta2 = await _akCarta2Repo.GetById(akAkaun.AkCartaId2);
            akAkaun.AkCarta2 = akCarta2;

            if (akAkaun == null)
            {
                return NotFound();
            }
           
            return View(akAkaun);
        }

        // POST: AkAkaun/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkAkaun akAkaun, int KWId, int AkCartaId1, int AkCartaId2)
        {
            if (id != akAkaun.Id)
            {
                return NotFound();
            }

            AkAkaun m = new AkAkaun();

            if (ModelState.IsValid)
            {
                try
                {
                    m.KWId = KWId;
                    m.AkCartaId1 = AkCartaId1;
                    m.AkCartaId2 = AkCartaId2;
                    m.Tarikh = akAkaun.Tarikh;
                    m.NoRujukan = akAkaun.NoRujukan;
                    m.Debit = akAkaun.Debit;
                    m.Kredit = akAkaun.Kredit;
                    await _akAkaunRepo.Update(akAkaun);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkAkaunExists(akAkaun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            return View(akAkaun);
        }

        // GET: AkAkaun/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akAkaun = await _context.AkAkaun
                .Include(a => a.AkCarta1)
                .Include(a => a.AkCarta2)
                .Include(a => a.KW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akAkaun == null)
            {
                return NotFound();
            }

            return View(akAkaun);
        }

        // POST: AkAkaun/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akAkaun = await _context.AkAkaun.FindAsync(id);
            _context.AkAkaun.Remove(akAkaun);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkAkaunExists(int id)
        {
            return _context.AkAkaun.Any(e => e.Id == id);
        }
    }
}
