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
    public class AkInvoisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkInvoisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkInvois
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkInvois.Include(a => a.AkPO).Include(a => a.AkPenghutang).Include(a => a.JBahagian).Include(a => a.JKW).Include(a => a.KodObjekAP);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkInvois/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akInvois = await _context.AkInvois
                .Include(a => a.AkPO)
                .Include(a => a.AkPenghutang)
                .Include(a => a.JBahagian)
                .Include(a => a.JKW)
                .Include(a => a.KodObjekAP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akInvois == null)
            {
                return NotFound();
            }

            return View(akInvois);
        }

        // GET: AkInvois/Create
        public IActionResult Create()
        {
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id");
            ViewData["AkPenghutangId"] = new SelectList(_context.AkPenghutang, "Id", "AkaunBank");
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            ViewData["KodObjekAPId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit");
            return View();
        }

        // POST: AkInvois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tahun,Tarikh,TarikhPosting,NoInbois,Jumlah,FlPosting,JKWId,JBahagianId,AkPOId,KodObjekAPId,AkPenghutangId,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkInvois akInvois)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akInvois);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akInvois.AkPOId);
            ViewData["AkPenghutangId"] = new SelectList(_context.AkPenghutang, "Id", "AkaunBank", akInvois.AkPenghutangId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", akInvois.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akInvois.JKWId);
            ViewData["KodObjekAPId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", akInvois.KodObjekAPId);
            return View(akInvois);
        }

        // GET: AkInvois/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akInvois = await _context.AkInvois.FindAsync(id);
            if (akInvois == null)
            {
                return NotFound();
            }
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akInvois.AkPOId);
            ViewData["AkPenghutangId"] = new SelectList(_context.AkPenghutang, "Id", "AkaunBank", akInvois.AkPenghutangId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", akInvois.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akInvois.JKWId);
            ViewData["KodObjekAPId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", akInvois.KodObjekAPId);
            return View(akInvois);
        }

        // POST: AkInvois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,Tarikh,TarikhPosting,NoInbois,Jumlah,FlPosting,JKWId,JBahagianId,AkPOId,KodObjekAPId,AkPenghutangId,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkInvois akInvois)
        {
            if (id != akInvois.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akInvois);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkInvoisExists(akInvois.Id))
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
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akInvois.AkPOId);
            ViewData["AkPenghutangId"] = new SelectList(_context.AkPenghutang, "Id", "AkaunBank", akInvois.AkPenghutangId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", akInvois.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akInvois.JKWId);
            ViewData["KodObjekAPId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", akInvois.KodObjekAPId);
            return View(akInvois);
        }

        // GET: AkInvois/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akInvois = await _context.AkInvois
                .Include(a => a.AkPO)
                .Include(a => a.AkPenghutang)
                .Include(a => a.JBahagian)
                .Include(a => a.JKW)
                .Include(a => a.KodObjekAP)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akInvois == null)
            {
                return NotFound();
            }

            return View(akInvois);
        }

        // POST: AkInvois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akInvois = await _context.AkInvois.FindAsync(id);
            _context.AkInvois.Remove(akInvois);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkInvoisExists(int id)
        {
            return _context.AkInvois.Any(e => e.Id == id);
        }
    }
}
