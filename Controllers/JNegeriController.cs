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
    public class JNegeriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JNegeriController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Negeri
        public async Task<IActionResult> Index()
        {
            return View(await _context.JNegeri.ToListAsync());
        }

        // GET: Negeri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negeri == null)
            {
                return NotFound();
            }

            return View(negeri);
        }

        // GET: Negeri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Negeri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal")] JNegeri negeri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(negeri);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(negeri);
        }

        // GET: Negeri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri.FindAsync(id);
            if (negeri == null)
            {
                return NotFound();
            }
            return View(negeri);
        }

        // POST: Negeri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal")] JNegeri negeri)
        {
            if (id != negeri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(negeri);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NegeriExists(negeri.Id))
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
            return View(negeri);
        }

        // GET: Negeri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negeri == null)
            {
                return NotFound();
            }

            return View(negeri);
        }

        // POST: Negeri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var negeri = await _context.JNegeri.FindAsync(id);
            _context.JNegeri.Remove(negeri);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool NegeriExists(int id)
        {
            return _context.JNegeri.Any(e => e.Id == id);
        }
    }
}
