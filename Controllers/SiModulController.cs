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
    public class SiModulController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SiModulController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SiModul
        public async Task<IActionResult> Index()
        {
            return View(await _context.SiModul.OrderBy(b => b.FuncId).ToListAsync());
        }

        // GET: SiModul/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siModul = await _context.SiModul
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siModul == null)
            {
                return NotFound();
            }

            return View(siModul);
        }

        // GET: SiModul/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SiModul/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FuncId,FuncName,Model,FuncIdOld")] SiModul siModul)
        {
            if (ModelState.IsValid)
            {
                _context.Add(siModul);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(siModul);
        }

        // GET: SiModul/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siModul = await _context.SiModul.FindAsync(id);
            if (siModul == null)
            {
                return NotFound();
            }
            return View(siModul);
        }

        // POST: SiModul/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FuncId,FuncName,Model,FuncIdOld")] SiModul siModul)
        {
            if (id != siModul.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(siModul);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiModulExists(siModul.Id))
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
            return View(siModul);
        }

        // GET: SiModul/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siModul = await _context.SiModul
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siModul == null)
            {
                return NotFound();
            }

            return View(siModul);
        }

        // POST: SiModul/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var siModul = await _context.SiModul.FindAsync(id);
            _context.SiModul.Remove(siModul);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiModulExists(int id)
        {
            return _context.SiModul.Any(e => e.Id == id);
        }
    }
}
