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
    //[Authorize(Roles = "SuperAdmin,Supervisor")]
    [Authorize(Roles = "SuperAdmin")]
    public class JJantinaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JJantinaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JJantina
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JJantina.ToListAsync();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                obj = await _context.JJantina.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JJantina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJantina = await _context.JJantina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jJantina == null)
            {
                return NotFound();
            }

            return View(jJantina);
        }

        // GET: JJantina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JJantina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JJantina jJantina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jJantina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jJantina);
        }

        // GET: JJantina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJantina = await _context.JJantina.FindAsync(id);
            if (jJantina == null)
            {
                return NotFound();
            }
            return View(jJantina);
        }

        // POST: JJantina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JJantina jJantina)
        {
            if (id != jJantina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jJantina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JJantinaExists(jJantina.Id))
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
            return View(jJantina);
        }

        // GET: JJantina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJantina = await _context.JJantina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jJantina == null)
            {
                return NotFound();
            }

            return View(jJantina);
        }

        // POST: JJantina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jJantina = await _context.JJantina.FindAsync(id);
            _context.JJantina.Remove(jJantina);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JJantina.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JJantina.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool JJantinaExists(int id)
        {
            return _context.JJantina.Any(e => e.Id == id);
        }
    }
}
