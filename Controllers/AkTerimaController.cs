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
    public class AkTerimaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AkTerimaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AkTerima
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AkTerima.Include(a => a.AkBank).Include(a => a.KW).Include(a => a.Negeri);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AkTerima/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _context.AkTerima
                .Include(a => a.AkBank)
                .Include(a => a.KW)
                .Include(a => a.Negeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTerima == null)
            {
                return NotFound();
            }

            return View(akTerima);
        }

        // GET: AkTerima/Create
        public IActionResult Create()
        {
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id");
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod");
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod");
            return View();
        }

        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tahun,KWId,NoRujukan,Tarikh,Jumlah,AkBankId,FlCetak,FlPosting,FlBatal,KodPembayar,NoKp,Nama,Alamat1,Alamat2,Alamat3,Poskod,Bandar,NegeriId,Tel,Emel,Sebab,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTerima akTerima)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akTerima);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTerima.AkBankId);
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod", akTerima.KWId);
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod", akTerima.NegeriId);
            return View(akTerima);
        }

        // GET: AkTerima/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _context.AkTerima.FindAsync(id);
            if (akTerima == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTerima.AkBankId);
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod", akTerima.KWId);
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod", akTerima.NegeriId);
            return View(akTerima);
        }

        // POST: AkTerima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,KWId,NoRujukan,Tarikh,Jumlah,AkBankId,FlCetak,FlPosting,FlBatal,KodPembayar,NoKp,Nama,Alamat1,Alamat2,Alamat3,Poskod,Bandar,NegeriId,Tel,Emel,Sebab,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTerima akTerima)
        {
            if (id != akTerima.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akTerima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTerimaExists(akTerima.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akTerima.AkBankId);
            ViewData["KWId"] = new SelectList(_context.KW, "Id", "Kod", akTerima.KWId);
            ViewData["NegeriId"] = new SelectList(_context.Negeri, "Id", "Kod", akTerima.NegeriId);
            return View(akTerima);
        }

        // GET: AkTerima/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _context.AkTerima
                .Include(a => a.AkBank)
                .Include(a => a.KW)
                .Include(a => a.Negeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTerima == null)
            {
                return NotFound();
            }

            return View(akTerima);
        }

        // POST: AkTerima/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTerima = await _context.AkTerima.FindAsync(id);
            _context.AkTerima.Remove(akTerima);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTerimaExists(int id)
        {
            return _context.AkTerima.Any(e => e.Id == id);
        }
    }
}
