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
    public class JBangsaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JBangsaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JBangsa
        public async Task<IActionResult> Index()
        {
            return View(await _context.JBangsa.ToListAsync());
        }

        // GET: JBangsa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBangsa == null)
            {
                return NotFound();
            }

            return View(jBangsa);
        }

        // GET: JBangsa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JBangsa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JBangsa jBangsa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jBangsa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jBangsa);
        }

        // GET: JBangsa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa.FindAsync(id);
            if (jBangsa == null)
            {
                return NotFound();
            }
            return View(jBangsa);
        }

        // POST: JBangsa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JBangsa jBangsa)
        {
            if (id != jBangsa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jBangsa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBangsaExists(jBangsa.Id))
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
            return View(jBangsa);
        }

        // GET: JBangsa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBangsa = await _context.JBangsa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jBangsa == null)
            {
                return NotFound();
            }

            return View(jBangsa);
        }

        // POST: JBangsa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jBangsa = await _context.JBangsa.FindAsync(id);
            _context.JBangsa.Remove(jBangsa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JBangsaExists(int id)
        {
            return _context.JBangsa.Any(e => e.Id == id);
        }
    }
}
