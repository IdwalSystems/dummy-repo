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
    public class AkTunaiPanjarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkTunaiPanjarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkTunaiPanjar
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkTunaiPanjar.Include(a => a.AkBank).Include(a => a.JKW);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkTunaiPanjar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiPanjar = await _context.AkTunaiPanjar
                .Include(a => a.AkBank)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiPanjar == null)
            {
                return NotFound();
            }

            return View(akTunaiPanjar);
        }

        // GET: AkTunaiPanjar/Create
        public IActionResult Create()
        {
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: AkTunaiPanjar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KaunterPanjar,Catatan,JKWId,AkBankId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiPanjar akTunaiPanjar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akTunaiPanjar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTunaiPanjar.AkBankId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akTunaiPanjar.JKWId);
            return View(akTunaiPanjar);
        }

        // GET: AkTunaiPanjar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiPanjar = await _context.AkTunaiPanjar.FindAsync(id);
            if (akTunaiPanjar == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTunaiPanjar.AkBankId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akTunaiPanjar.JKWId);
            return View(akTunaiPanjar);
        }

        // POST: AkTunaiPanjar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KaunterPanjar,Catatan,JKWId,AkBankId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiPanjar akTunaiPanjar)
        {
            if (id != akTunaiPanjar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akTunaiPanjar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiPanjarExists(akTunaiPanjar.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTunaiPanjar.AkBankId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akTunaiPanjar.JKWId);
            return View(akTunaiPanjar);
        }

        // GET: AkTunaiPanjar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiPanjar = await _context.AkTunaiPanjar
                .Include(a => a.AkBank)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiPanjar == null)
            {
                return NotFound();
            }

            return View(akTunaiPanjar);
        }

        // POST: AkTunaiPanjar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiPanjar = await _context.AkTunaiPanjar.FindAsync(id);
            _context.AkTunaiPanjar.Remove(akTunaiPanjar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiPanjarExists(int id)
        {
            return _context.AkTunaiPanjar.Any(e => e.Id == id);
        }
    }
}
