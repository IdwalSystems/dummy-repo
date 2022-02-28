using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class JTahapAktivitiController : Controller
    {
        public const string modul = "JD009";
        public const string namamodul = "Jadual Tahap Aktiviti";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JTahapAktivitiController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
        }

        private async Task AddLogAsync(
            string operasi,
            string nota,
            string rujukan,
            int idRujukan,
            decimal jumlah)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;

            await _appLog.Insert(appLog, modul, operasi);
        }

        // GET: JTahapAktiviti
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JTahapAktiviti.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JTahapAktiviti.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JTahapAktiviti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JTahapAktiviti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JTahapAktiviti jTahapAktiviti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jTahapAktiviti);
                await AddLogAsync("Tambah", jTahapAktiviti.Perihal,jTahapAktiviti.Perihal,0, 0);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }
            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JTahapAktiviti jTahapAktiviti)
        {
            if (id != jTahapAktiviti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JTahapAktiviti.FirstOrDefaultAsync(x => x.Id == jTahapAktiviti.Id);
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(jTahapAktiviti);

                    if (perihalAsal != jTahapAktiviti.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jTahapAktiviti.Perihal,jTahapAktiviti.Perihal,id, 0);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JTahapAktivitiExists(jTahapAktiviti.Id))
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
            return View(jTahapAktiviti);
        }

        // GET: JTahapAktiviti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jTahapAktiviti = await _context.JTahapAktiviti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jTahapAktiviti == null)
            {
                return NotFound();
            }

            return View(jTahapAktiviti);
        }

        // POST: JTahapAktiviti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jTahapAktiviti = await _context.JTahapAktiviti.FindAsync(id);
            _context.JTahapAktiviti.Remove(jTahapAktiviti);
            await AddLogAsync("Hapus", jTahapAktiviti.Perihal,jTahapAktiviti.Perihal,id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JTahapAktiviti.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JTahapAktiviti.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool JTahapAktivitiExists(int id)
        {
            return _context.JTahapAktiviti.Any(e => e.Id == id);
        }
    }
}
