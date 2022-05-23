using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class SuProfilJurulatihController : Controller
    {
        public const string modul = "SU002";
        public const string namamodul = "Profil Jurulatih";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<SuProfil, int, string> _suProfilRepo;
        private CartAtlet _cart;

        public SuProfilJurulatihController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuProfilJurulatih
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuProfil.Include(s => s.AkCarta).Include(s => s.JBahagian).Include(s => s.JKW);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuProfilJurulatih/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil
                .Include(s => s.AkCarta)
                .Include(s => s.JBahagian)
                .Include(s => s.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suProfil == null)
            {
                return NotFound();
            }

            return View(suProfil);
        }

        // GET: SuProfilJurulatih/Create
        public IActionResult Create()
        {
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit");
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: SuProfilJurulatih/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoRujukan,Bulan,Tahun,Jumlah,FlKategori,AkCartaId,JKWId,JBahagianId,FlHapus,TarHapus,FlPosting,TarikhPosting,FlCetak,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuProfil suProfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suProfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", suProfil.AkCartaId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", suProfil.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", suProfil.JKWId);
            return View(suProfil);
        }

        // GET: SuProfilJurulatih/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil.FindAsync(id);
            if (suProfil == null)
            {
                return NotFound();
            }
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", suProfil.AkCartaId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", suProfil.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", suProfil.JKWId);
            return View(suProfil);
        }

        // POST: SuProfilJurulatih/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoRujukan,Bulan,Tahun,Jumlah,FlKategori,AkCartaId,JKWId,JBahagianId,FlHapus,TarHapus,FlPosting,TarikhPosting,FlCetak,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuProfil suProfil)
        {
            if (id != suProfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suProfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuProfilExists(suProfil.Id))
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
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", suProfil.AkCartaId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", suProfil.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", suProfil.JKWId);
            return View(suProfil);
        }

        // GET: SuProfilJurulatih/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil
                .Include(s => s.AkCarta)
                .Include(s => s.JBahagian)
                .Include(s => s.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suProfil == null)
            {
                return NotFound();
            }

            return View(suProfil);
        }

        // POST: SuProfilJurulatih/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suProfil = await _context.SuProfil.FindAsync(id);
            _context.SuProfil.Remove(suProfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuProfilExists(int id)
        {
            return _context.SuProfil.Any(e => e.Id == id);
        }
    }
}
