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
    public class JAgamaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JAgamaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JAgama
        public async Task<IActionResult> Index()
        {
            return View(await _context.JAgama.ToListAsync());
        }

        // GET: JAgama/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // GET: JAgama/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JAgama/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JAgama jAgama)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jAgama);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jAgama);
        }

        // GET: JAgama/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama.FindAsync(id);
            if (jAgama == null)
            {
                return NotFound();
            }
            return View(jAgama);
        }

        // POST: JAgama/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JAgama jAgama)
        {
            if (id != jAgama.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jAgama);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JAgamaExists(jAgama.Id))
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
            return View(jAgama);
        }

        // GET: JAgama/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // POST: JAgama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jAgama = await _context.JAgama.FindAsync(id);
            _context.JAgama.Remove(jAgama);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JAgamaExists(int id)
        {
            return _context.JAgama.Any(e => e.Id == id);
        }
    }
}
