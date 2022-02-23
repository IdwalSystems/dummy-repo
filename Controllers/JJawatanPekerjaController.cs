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
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JJawatanPekerjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JJawatanPekerjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JJawatanPekerja
        public async Task<IActionResult> Index()
        {
            return View(await _context.JJawatanPekerja.ToListAsync());
        }

        // GET: JJawatanPekerja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJawatanPekerja = await _context.JJawatanPekerja
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jJawatanPekerja == null)
            {
                return NotFound();
            }

            return View(jJawatanPekerja);
        }

        // GET: JJawatanPekerja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JJawatanPekerja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JJawatanPekerja jJawatanPekerja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jJawatanPekerja);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));

            }
            return View(jJawatanPekerja);
        }

        // GET: JJawatanPekerja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJawatanPekerja = await _context.JJawatanPekerja.FindAsync(id);
            if (jJawatanPekerja == null)
            {
                return NotFound();
            }
            return View(jJawatanPekerja);
        }

        // POST: JJawatanPekerja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JJawatanPekerja jJawatanPekerja)
        {
            if (id != jJawatanPekerja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jJawatanPekerja);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JJawatanPekerjaExists(jJawatanPekerja.Id))
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
            return View(jJawatanPekerja);
        }

        // GET: JJawatanPekerja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jJawatanPekerja = await _context.JJawatanPekerja
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jJawatanPekerja == null)
            {
                return NotFound();
            }

            return View(jJawatanPekerja);
        }

        // POST: JJawatanPekerja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jJawatanPekerja = await _context.JJawatanPekerja.FindAsync(id);
            _context.JJawatanPekerja.Remove(jJawatanPekerja);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JJawatanPekerjaExists(int id)
        {
            return _context.JJawatanPekerja.Any(e => e.Id == id);
        }
    }
}
