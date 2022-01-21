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
    [Authorize(Roles = "Admin , Supervisor")]
    public class JTahapAktivitiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JTahapAktivitiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JTahapAktiviti
        public async Task<IActionResult> Index()
        {
            return View(await _context.JTahapAktiviti.ToListAsync());
        }

        // GET: JTahapAktiviti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JTahapAktiviti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JTahapAktiviti jTahapAktiviti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jTahapAktiviti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }
            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JTahapAktiviti jTahapAktiviti)
        {
            if (id != jTahapAktiviti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jTahapAktiviti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JTahapAktivitiExists(jTahapAktiviti.Id))
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
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            _context.JTahapAktiviti.Remove(jTahapAktiviti);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JTahapAktivitiExists(int id)
        {
            return _context.JTahapAktiviti.Any(e => e.Id == id);
        }
    }
}
