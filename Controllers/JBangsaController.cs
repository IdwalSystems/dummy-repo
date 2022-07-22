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
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class JBangsaController : Controller
    {
        public const string modul = "JD003";
        public const string namamodul = "Jadual Bangsa";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBangsaController(ApplicationDbContext context,
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
        // GET: JBangsa
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JBangsa.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JBangsa.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
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
                await AddLogAsync("Tambah", jBangsa.Perihal, jBangsa.Perihal, 0, 0);
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
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
                    var objAsal = await _context.JBangsa.FirstOrDefaultAsync(x => x.Id == jBangsa.Id);
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(jBangsa);

                    if (perihalAsal != jBangsa.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jBangsa.Perihal,jBangsa.Perihal,id, 0);
                    }
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
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
            var user = await _userManager.GetUserAsync(User);
            jBangsa.UserIdKemaskini = user.UserName;
            jBangsa.TarKemaskini = DateTime.Now;

            _context.JBangsa.Remove(jBangsa);
            await AddLogAsync("Hapus", jBangsa.Perihal,jBangsa.Perihal,id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JBangsa.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JBangsa.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool JBangsaExists(int id)
        {
            return _context.JBangsa.Any(e => e.Id == id);
        }
    }
}
