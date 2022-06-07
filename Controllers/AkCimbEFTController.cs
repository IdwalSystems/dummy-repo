using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;

namespace MSNK.Controllers
{
    public class AkCimbEFTController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkCimbEFTController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkCimbEFT
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkCimbEFT.Include(a => a.AkBank).Include(a => a.SuPekerja);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkCimbEFT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT
                .Include(a => a.AkBank)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Create
        public IActionResult Create()
        {
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel");
            return View();
        }

        // POST: AkCimbEFT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoPBI,TarJana,TarBayar,Jumlah,NamaFail,BilPV,FlKategori,SuPekerjaId,AkBankId,FlStatus,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkCimbEFT akCimbEFT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akCimbEFT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT.FindAsync(id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // POST: AkCimbEFT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoPBI,TarJana,TarBayar,Jumlah,NamaFail,BilPV,FlKategori,SuPekerjaId,AkBankId,FlStatus,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkCimbEFT akCimbEFT)
        {
            if (id != akCimbEFT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akCimbEFT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkCimbEFTExists(akCimbEFT.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT
                .Include(a => a.AkBank)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            return View(akCimbEFT);
        }

        // POST: AkCimbEFT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akCimbEFT = await _context.AkCimbEFT.FindAsync(id);
            _context.AkCimbEFT.Remove(akCimbEFT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkCimbEFTExists(int id)
        {
            return _context.AkCimbEFT.Any(e => e.Id == id);
        }
    }
}
