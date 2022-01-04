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
    public class JKWController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JKWController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KW
        public async Task<IActionResult> Index()
        {
            return View(await _context.JKW.ToListAsync());
        }

        // GET: KW/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kW == null)
            {
                return NotFound();
            }

            return View(kW);
        }

        // GET: KW/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KW/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal")] JKW kW)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kW);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kW);
        }

        // GET: KW/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW.FindAsync(id);
            if (kW == null)
            {
                return NotFound();
            }
            return View(kW);
        }

        // POST: KW/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal")] JKW kW)
        {
            if (id != kW.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kW);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KWExists(kW.Id))
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
            return View(kW);
        }

        // GET: KW/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kW = await _context.JKW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kW == null)
            {
                return NotFound();
            }

            return View(kW);
        }

        // POST: KW/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kW = await _context.JKW.FindAsync(id);
            _context.JKW.Remove(kW);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KWExists(int id)
        {
            return _context.JKW.Any(e => e.Id == id);
        }
    }
}
