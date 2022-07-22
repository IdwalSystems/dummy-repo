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
    public class JAgamaController : Controller
    {
        public const string modul = "JD001";
        public const string namamodul = "Jadual Agama";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JAgamaController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog,int> appLog)
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
        // GET: JAgama
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JAgama.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JAgama.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: JAgama/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // GET: JAgama/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JAgama/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JAgama jAgama)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jAgama);
                await AddLogAsync("Tambah", jAgama.Perihal,"",0, 0);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
            }
            return View(jAgama);
        }

        // GET: JAgama/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama.FindAsync(id);
            if (jAgama == null)
            {
                return NotFound();
            }
            return View(jAgama);
        }

        // POST: JAgama/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] JAgama jAgama)
        {
            if (id != jAgama.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JAgama.FirstOrDefaultAsync(x=> x.Id == jAgama.Id);
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(jAgama);

                    if (perihalAsal != jAgama.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jAgama.Perihal,jAgama.Id.ToString(),id, 0);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JAgamaExists(jAgama.Id))
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
            return View(jAgama);
        }

        // GET: JAgama/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jAgama = await _context.JAgama
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jAgama == null)
            {
                return NotFound();
            }

            return View(jAgama);
        }

        // POST: JAgama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jAgama = await _context.JAgama.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            jAgama.UserIdKemaskini = user.UserName;
            jAgama.TarKemaskini = DateTime.Now;

            _context.JAgama.Remove(jAgama);
            await AddLogAsync("Hapus", jAgama.Perihal, jAgama.Id.ToString(),id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JAgama.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x=> x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JAgama.Update(obj);

            //await AddLogAsync("Rollback", obj.Perihal, 0);
            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JBahagianExists(int id)
        {
            return _context.JBahagian.Any(e => e.Id == id);
        }
        private bool JAgamaExists(int id)
        {
            return _context.JAgama.Any(e => e.Id == id);
        }
    }
}
