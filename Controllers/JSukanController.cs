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
    public class JSukanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JSukanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JSukan
        public async Task<IActionResult> Index()
        {
            return View(await _context.JSukan.ToListAsync());
        }

        // GET: JSukan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jSukan == null)
            {
                return NotFound();
            }

            return View(jSukan);
        }

        // GET: JSukan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JSukan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JSukan jSukan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jSukan);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(jSukan);
        }

        // GET: JSukan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan.FindAsync(id);
            if (jSukan == null)
            {
                return NotFound();
            }
            return View(jSukan);
        }

        // POST: JSukan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JSukan jSukan)
        {
            if (id != jSukan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jSukan);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JSukanExists(jSukan.Id))
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
            return View(jSukan);
        }

        // GET: JSukan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jSukan = await _context.JSukan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jSukan == null)
            {
                return NotFound();
            }

            return View(jSukan);
        }

        // POST: JSukan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jSukan = await _context.JSukan.FindAsync(id);
            _context.JSukan.Remove(jSukan);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JSukanExists(int id)
        {
            return _context.JSukan.Any(e => e.Id == id);
        }
    }
}
