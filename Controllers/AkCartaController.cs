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
        private readonly IRepository<KW, int> _kwRepo;

        public AkCartaController(ApplicationDbContext context, IRepository<KW, int> kwRepository, IRepository<AkCarta, int> akCartaRepository)
        {
            _context = context;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
        }

        // GET: AkCarta
        public async Task<IActionResult> Index()
        {
            var akCarta = await _akCartaRepo.GetAll();
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
            if (akCarta == null)
            {
                return NotFound();
            }

            return View(akCarta);
        }

        private void PopulateList()
        {
            List<KW> kwList = _context.KW.ToList();
            ViewBag.Kw = kwList;

            List<Jenis> jenisList = _context.Jenis.ToList();
            ViewBag.Jenis = jenisList;

            List<Paras> parasList = _context.Paras.ToList();
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
        public async Task<IActionResult> Create(AkCarta akCarta, int KWId, int JenisId, int ParasId)
        {
            AkCarta akC = new AkCarta();
            if (ModelState.IsValid)
            {
                if (akCarta != null && KWId != 0)
                {
                    akC.KWId = KWId;
                    akC.Kod = akCarta.Kod;
                    akC.JenisId = JenisId;
                    akC.Nama = akCarta.Nama;
                    akC.ParasId = ParasId;
                    akC.UmumDetail = akCarta.UmumDetail;
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

            var akCarta = await _akCartaRepo.GetById((int)id);
            if (akCarta == null)
            {
                return NotFound();
            }
            PopulateList();
            return View(akCarta);
        }

        // POST: AkCarta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkCarta akCarta, int KWId, int JenisId, int ParasId)
        {

            if (id != akCarta.id)
            {
                return NotFound();
            }

            AkCarta akC = new AkCarta();

            if (ModelState.IsValid)
            {
                try
                {
                    akC.KWId = KWId;
                    akC.Kod = akCarta.Kod;
                    akC.JenisId = JenisId;
                    akC.Nama = akCarta.Nama;
                    akC.ParasId = ParasId;
                    akC.UmumDetail = akCarta.UmumDetail;
                    akC.Catatan1 = akCarta.Catatan1;
                    akC.Catatan2 = akCarta.Catatan2;
                    await _akCartaRepo.Update(akCarta);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkCartaExists(akCarta.id))
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

            return View(akCarta);
        }

        // GET: AkCarta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCarta = await _context.AkCarta
                .Include(a => a.KW)
                .FirstOrDefaultAsync(m => m.id == id);
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
            var akCarta = await _context.AkCarta.FindAsync(id);
            _context.AkCarta.Remove(akCarta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkCartaExists(int id)
        {
            return _context.AkCarta.Any(e => e.id == id);
        }
    }
}
