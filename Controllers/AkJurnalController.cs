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
    public class AkJurnalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkJurnalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkJurnal
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkJurnal.Include(a => a.JKW);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkJurnal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _context.AkJurnal
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akJurnal == null)
            {
                return NotFound();
            }

            return View(akJurnal);
        }

        // GET: AkJurnal/Create
        public IActionResult Create()
        {
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: AkJurnal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JKWId,NoJurnal,Tarikh,JumDebit,JumKredit,Catatan1,Catatan2,Catatan3,Catatan4,Posting,Cetak,Batal,UserId,TarikhMasuk")] AkJurnal akJurnal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akJurnal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
            return View(akJurnal);
        }

        // GET: AkJurnal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _context.AkJurnal.FindAsync(id);
            if (akJurnal == null)
            {
                return NotFound();
            }
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
            return View(akJurnal);
        }

        // POST: AkJurnal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JKWId,NoJurnal,Tarikh,JumDebit,JumKredit,Catatan1,Catatan2,Catatan3,Catatan4,Posting,Cetak,Batal,UserId,TarikhMasuk")] AkJurnal akJurnal)
        {
            if (id != akJurnal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akJurnal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkJurnalExists(akJurnal.Id))
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
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
            return View(akJurnal);
        }

        // GET: AkJurnal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _context.AkJurnal
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akJurnal == null)
            {
                return NotFound();
            }

            return View(akJurnal);
        }

        // POST: AkJurnal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akJurnal = await _context.AkJurnal.FindAsync(id);
            _context.AkJurnal.Remove(akJurnal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkJurnalExists(int id)
        {
            return _context.AkJurnal.Any(e => e.Id == id);
        }
    }
}
