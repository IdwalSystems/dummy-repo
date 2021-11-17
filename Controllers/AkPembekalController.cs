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
    public class AkPembekalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkPembekalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkPembekal
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkPembekal.Include(a => a.AkBank).Include(a => a.JNegeri);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkPembekal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _context.AkPembekal
                .Include(a => a.AkBank)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // GET: AkPembekal/Create
        public IActionResult Create()
        {
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod");
            return View();
        }

        // POST: AkPembekal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KodSykt,NamaSykt,NoPendaftaran,Alamat1,Alamat2,Alamat3,Poskod,Bandar,JNegeriId,Telefon1,Emel,AkaunBank,AkBankId")] AkPembekal akPembekal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akPembekal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.AkBankId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // GET: AkPembekal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _context.AkPembekal.FindAsync(id);
            if (akPembekal == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.AkBankId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // POST: AkPembekal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KodSykt,NamaSykt,NoPendaftaran,Alamat1,Alamat2,Alamat3,Poskod,Bandar,JNegeriId,Telefon1,Emel,AkaunBank,AkBankId")] AkPembekal akPembekal)
        {
            if (id != akPembekal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akPembekal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPembekalExists(akPembekal.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.AkBankId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // GET: AkPembekal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _context.AkPembekal
                .Include(a => a.AkBank)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // POST: AkPembekal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPembekal = await _context.AkPembekal.FindAsync(id);
            _context.AkPembekal.Remove(akPembekal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPembekalExists(int id)
        {
            return _context.AkPembekal.Any(e => e.Id == id);
        }
    }
}
