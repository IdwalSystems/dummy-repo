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
    public class JCaraBayarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JCaraBayarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CaraBayar
        public async Task<IActionResult> Index()
        {
            return View(await _context.JCaraBayar.ToListAsync());
        }

        // GET: CaraBayar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caraBayar == null)
            {
                return NotFound();
            }

            return View(caraBayar);
        }

        // GET: CaraBayar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaraBayar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal")] JCaraBayar caraBayar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caraBayar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caraBayar);
        }

        // GET: CaraBayar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar.FindAsync(id);
            if (caraBayar == null)
            {
                return NotFound();
            }
            return View(caraBayar);
        }

        // POST: CaraBayar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal")] JCaraBayar caraBayar)
        {
            if (id != caraBayar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caraBayar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaraBayarExists(caraBayar.Id))
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
            return View(caraBayar);
        }

        // GET: CaraBayar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caraBayar == null)
            {
                return NotFound();
            }

            return View(caraBayar);
        }

        // POST: CaraBayar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caraBayar = await _context.JCaraBayar.FindAsync(id);
            _context.JCaraBayar.Remove(caraBayar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaraBayarExists(int id)
        {
            return _context.JCaraBayar.Any(e => e.Id == id);
        }
    }
}
