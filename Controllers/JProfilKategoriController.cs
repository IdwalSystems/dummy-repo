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
    public class JProfilKategoriController : Controller
    {
        public const string modul = "JD012";
        public const string namamodul = "Jadual Profil Kategori";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JProfilKategoriController(ApplicationDbContext context,
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

        // GET: JProfilKategori
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JProfilKategori.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JProfilKategori.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JProfilKategori/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jProfilKategori = await _context.JProfilKategori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jProfilKategori == null)
            {
                return NotFound();
            }

            return View(jProfilKategori);
        }

        // GET: JProfilKategori/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JProfilKategori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal,KadarGeran,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JProfilKategori jProfilKategori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jProfilKategori);
                await AddLogAsync("Tambah", jProfilKategori.Perihal, jProfilKategori.Perihal, 0, 0);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
            }
            return View(jProfilKategori);
        }

        // GET: JProfilKategori/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jProfilKategori = await _context.JProfilKategori.FindAsync(id);
            if (jProfilKategori == null)
            {
                return NotFound();
            }
            return View(jProfilKategori);
        }

        // POST: JProfilKategori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal,KadarGeran,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JProfilKategori jProfilKategori)
        {
            if (id != jProfilKategori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JProfilKategori.FirstOrDefaultAsync(x => x.Id == jProfilKategori.Id);
                    jProfilKategori.Kod = objAsal.Kod;
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(jProfilKategori);

                    if (perihalAsal != jProfilKategori.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jProfilKategori.Perihal, jProfilKategori.Perihal, id, 0);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JProfilKategoriExists(jProfilKategori.Id))
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
            return View(jProfilKategori);
        }

        // GET: JProfilKategori/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jProfilKategori = await _context.JProfilKategori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jProfilKategori == null)
            {
                return NotFound();
            }

            return View(jProfilKategori);
        }

        // POST: JProfilKategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jProfilKategori = await _context.JProfilKategori.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            jProfilKategori.UserIdKemaskini = user.UserName;
            jProfilKategori.TarKemaskini = DateTime.Now;

            _context.JProfilKategori.Remove(jProfilKategori);
            await AddLogAsync("Hapus", jProfilKategori.Perihal, jProfilKategori.Perihal, id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JProfilKategori.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JProfilKategori.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JProfilKategoriExists(int id)
        {
            return _context.JProfilKategori.Any(e => e.Id == id);
        }
    }
}
