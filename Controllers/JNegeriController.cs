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
    //[Authorize(Roles = "SuperAdmin,Supervisor")]
    [Authorize(Roles = "SuperAdmin")]
    public class JNegeriController : Controller
    {
        public const string modul = "JD007";
        public const string namamodul = "Jadual Negeri";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JNegeriController(ApplicationDbContext context,
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
            decimal jumlah,
            int? pekerjaId)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = pekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        // GET: Negeri
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JNegeri.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JNegeri.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: Negeri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negeri == null)
            {
                return NotFound();
            }

            return View(negeri);
        }

        // GET: Negeri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Negeri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal")] JNegeri negeri)
        {
            if (KodNegeriExists(negeri.Kod) == false)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    negeri.UserId = user.UserName;
                    negeri.TarMasuk = DateTime.Now;
                    negeri.SuPekerjaMasukId = pekerjaId;

                    _context.Add(negeri);
                    await AddLogAsync("Tambah", negeri.Kod + " - " + negeri.Perihal,negeri.Kod, 0, 0, pekerjaId);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));

                }
            }
            else
            {
                TempData[SD.Error] = "Kod ini telah wujud..!";
            }
            
            return View(negeri);
        }

        // GET: Negeri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri.FindAsync(id);
            if (negeri == null)
            {
                return NotFound();
            }
            return View(negeri);
        }

        // POST: Negeri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal")] JNegeri negeri)
        {
            if (id != negeri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JNegeri.FirstOrDefaultAsync(x => x.Id == negeri.Id);
                    var kodAsal = objAsal.Kod;
                    var perihalAsal = objAsal.Perihal;
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    negeri.UserId = objAsal.UserId;
                    negeri.TarMasuk = objAsal.TarMasuk;
                    negeri.SuPekerjaMasukId = objAsal.SuPekerjaMasukId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    negeri.UserIdKemaskini = user.UserName;
                    negeri.TarKemaskini = DateTime.Now;
                    negeri.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(negeri);

                    await AddLogAsync("Ubah", kodAsal + " -> " + negeri.Kod + ", "
                        + perihalAsal + " -> " + negeri.Perihal + ", ", negeri.Kod, id, 0, pekerjaId);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NegeriExists(negeri.Id))
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
            return View(negeri);
        }

        // GET: Negeri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var negeri = await _context.JNegeri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (negeri == null)
            {
                return NotFound();
            }

            return View(negeri);
        }

        // POST: Negeri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var negeri = await _context.JNegeri.FindAsync(id);

            negeri.UserIdKemaskini = user.UserName;
            negeri.TarKemaskini = DateTime.Now;
            negeri.SuPekerjaKemaskiniId = pekerjaId;

            _context.JNegeri.Remove(negeri);
            await AddLogAsync("Hapus", negeri.Kod + " - " + negeri.Perihal, negeri.Kod, id, 0, pekerjaId);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JNegeri.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Batal operation
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            obj.FlHapus = 0;
            _context.JNegeri.Update(obj);

            // Batal operation end
            await AddLogAsync("Rollback", obj.Kod + " - " + obj.Perihal, obj.Kod, id, 0, pekerjaId);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool NegeriExists(int id)
        {
            return _context.JNegeri.Any(e => e.Id == id);
        }

        private bool KodNegeriExists(string kod)
        {
            return _context.JNegeri.Any(e => e.Kod == kod);
        }
    }
}
