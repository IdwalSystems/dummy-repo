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

namespace MSNK.Controllers
{
    [Authorize(Policy = "PB001")]
    public class AkBankReconController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkBankReconController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkBankRecon
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkBankRecon.Include(a => a.AkBank);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkBankRecon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _context.AkBankRecon
                .Include(a => a.AkBank)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // GET: AkBankRecon/Create
        public IActionResult Create()
        {
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            return View();
        }

        // POST: AkBankRecon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tahun,Bulan,BakiPenyata,AkBankId,FlMuatNaik,TarMuatNaik,IsKunci,TarKunci,FlHapus,TarHapus,SuPekerjaMasukId,UserId,TarMasuk,SuPekerjaKemaskiniId,UserIdKemaskini,TarKemaskini")] AkBankRecon akBankRecon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akBankRecon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akBankRecon.AkBankId);
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _context.AkBankRecon.FindAsync(id);
            if (akBankRecon == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akBankRecon.AkBankId);
            return View(akBankRecon);
        }

        // POST: AkBankRecon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,Bulan,BakiPenyata,AkBankId,FlMuatNaik,TarMuatNaik,IsKunci,TarKunci,FlHapus,TarHapus,SuPekerjaMasukId,UserId,TarMasuk,SuPekerjaKemaskiniId,UserIdKemaskini,TarKemaskini")] AkBankRecon akBankRecon)
        {
            if (id != akBankRecon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akBankRecon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBankReconExists(akBankRecon.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akBankRecon.AkBankId);
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _context.AkBankRecon
                .Include(a => a.AkBank)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // POST: AkBankRecon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBankRecon = await _context.AkBankRecon.FindAsync(id);
            _context.AkBankRecon.Remove(akBankRecon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBankReconExists(int id)
        {
            return _context.AkBankRecon.Any(e => e.Id == id);
        }
    }
}
