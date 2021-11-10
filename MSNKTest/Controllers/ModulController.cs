using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNKTest.Models;

namespace MSNKTest.Controllers
{
    public class ModulController : Controller
    {
        private readonly MSNKDBContext _context;

        public ModulController(MSNKDBContext context)
        {
            _context = context;
        }

        // GET: Modul
        public async Task<IActionResult> Index()
        {
            return View(await _context.Modul.ToListAsync());
        }

        // GET: Modul/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.Modul
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modul == null)
            {
                return NotFound();
            }

            return View(modul);
        }

        // GET: Modul/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modul/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FUNCID,FUNCNAME")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modul);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modul);
        }

        // GET: Modul/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.Modul.FindAsync(id);
            if (modul == null)
            {
                return NotFound();
            }
            return View(modul);
        }

        // POST: Modul/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FUNCID,FUNCNAME")] Modul modul)
        {
            if (id != modul.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modul);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulExists(modul.Id))
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
            return View(modul);
        }

        // GET: Modul/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.Modul
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modul == null)
            {
                return NotFound();
            }

            return View(modul);
        }

        // POST: Modul/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modul = await _context.Modul.FindAsync(id);
            _context.Modul.Remove(modul);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulExists(int id)
        {
            return _context.Modul.Any(e => e.Id == id);
        }
    }
}
