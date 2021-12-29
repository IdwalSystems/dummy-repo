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
    public class SuPekerjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuPekerjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuPekerja
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuPekerja.Include(s => s.JAgama).Include(s => s.JBangsa).Include(s => s.JCaraBayar).Include(s => s.JJawatanPekerja).Include(s => s.JNegeri);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuPekerja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _context.SuPekerja
                .Include(s => s.JAgama)
                .Include(s => s.JBangsa)
                .Include(s => s.JCaraBayar)
                .Include(s => s.JJawatanPekerja)
                .Include(s => s.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            return View(suPekerja);
        }

        // GET: SuPekerja/Create
        public IActionResult Create()
        {
            ViewData["JAgamaId"] = new SelectList(_context.JAgama, "Id", "Id");
            ViewData["JBangsaId"] = new SelectList(_context.JBangsa, "Id", "Id");
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod");
            ViewData["JJawatanPekerjaId"] = new SelectList(_context.JJawatanPekerja, "Id", "Id");
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod");
            return View();
        }

        // POST: SuPekerja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoGaji,Nama,Alamat1,Alamat2,Alamat3,Poskod,Bandar,JNegeriId,TelefonRumah,TelefonBimbit,Emel,StatusKahwin,BilAnak,GajiPokok,TarikhMasukKerja,TarikhBerhentiKerja,TarikhPencen,JAgamaId,JBangsaId,JJawatanPekerjaId,JCaraBayarId,NoAkaunBank,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuPekerja suPekerja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suPekerja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JAgamaId"] = new SelectList(_context.JAgama, "Id", "Id", suPekerja.JAgamaId);
            ViewData["JBangsaId"] = new SelectList(_context.JBangsa, "Id", "Id", suPekerja.JBangsaId);
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod", suPekerja.JCaraBayarId);
            ViewData["JJawatanPekerjaId"] = new SelectList(_context.JJawatanPekerja, "Id", "Id", suPekerja.JJawatanPekerjaId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", suPekerja.JNegeriId);
            return View(suPekerja);
        }

        // GET: SuPekerja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _context.SuPekerja.FindAsync(id);
            if (suPekerja == null)
            {
                return NotFound();
            }
            ViewData["JAgamaId"] = new SelectList(_context.JAgama, "Id", "Id", suPekerja.JAgamaId);
            ViewData["JBangsaId"] = new SelectList(_context.JBangsa, "Id", "Id", suPekerja.JBangsaId);
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod", suPekerja.JCaraBayarId);
            ViewData["JJawatanPekerjaId"] = new SelectList(_context.JJawatanPekerja, "Id", "Id", suPekerja.JJawatanPekerjaId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", suPekerja.JNegeriId);
            return View(suPekerja);
        }

        // POST: SuPekerja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoGaji,Nama,Alamat1,Alamat2,Alamat3,Poskod,Bandar,JNegeriId,TelefonRumah,TelefonBimbit,Emel,StatusKahwin,BilAnak,GajiPokok,TarikhMasukKerja,TarikhBerhentiKerja,TarikhPencen,JAgamaId,JBangsaId,JJawatanPekerjaId,JCaraBayarId,NoAkaunBank,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuPekerja suPekerja)
        {
            if (id != suPekerja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suPekerja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuPekerjaExists(suPekerja.Id))
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
            ViewData["JAgamaId"] = new SelectList(_context.JAgama, "Id", "Id", suPekerja.JAgamaId);
            ViewData["JBangsaId"] = new SelectList(_context.JBangsa, "Id", "Id", suPekerja.JBangsaId);
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod", suPekerja.JCaraBayarId);
            ViewData["JJawatanPekerjaId"] = new SelectList(_context.JJawatanPekerja, "Id", "Id", suPekerja.JJawatanPekerjaId);
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", suPekerja.JNegeriId);
            return View(suPekerja);
        }

        // GET: SuPekerja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _context.SuPekerja
                .Include(s => s.JAgama)
                .Include(s => s.JBangsa)
                .Include(s => s.JCaraBayar)
                .Include(s => s.JJawatanPekerja)
                .Include(s => s.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            return View(suPekerja);
        }

        // POST: SuPekerja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suPekerja = await _context.SuPekerja.FindAsync(id);
            _context.SuPekerja.Remove(suPekerja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuPekerjaExists(int id)
        {
            return _context.SuPekerja.Any(e => e.Id == id);
        }
    }
}
