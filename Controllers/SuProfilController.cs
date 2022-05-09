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
    public class SuProfilController : Controller
    {
        public const string modul = "FL007";
        public const string namamodul = "Profil Atlet";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public SuProfilController(ApplicationDbContext context,
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

        // GET: SuProfil
        public async Task<IActionResult> Index()
        {
            var obj = await _context.SuProfil.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.SuProfil.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: SuProfil/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SuProfil = await _context.SuProfil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SuProfil == null)
            {
                return NotFound();
            }

            return View(SuProfil);
        }

        // GET: SuProfil/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuProfil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuProfil SuProfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(SuProfil);
                await AddLogAsync("Tambah", SuProfil.NoRujukan, SuProfil.NoRujukan, 0, 0); 
                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya ditambah..!";
                return RedirectToAction(nameof(Index));
                
            }
            return View(SuProfil);
        }

        // GET: SuProfil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SuProfil = await _context.SuProfil.FindAsync(id);
            if (SuProfil == null)
            {
                return NotFound();
            }
            return View(SuProfil);
        }

        // POST: SuProfil/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Perihal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuProfil SuProfil)
        {
            if (id != SuProfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.SuProfil.FirstOrDefaultAsync(x => x.Id == SuProfil.Id);
                    //var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(SuProfil);

                    //if (perihalAsal != SuProfil.Perihal)
                    //{
                    //    await AddLogAsync("Ubah", perihalAsal + " -> " + SuProfil.Perihal,SuProfil.Perihal,id, 0);
                    //}

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuProfilExists(SuProfil.Id))
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
            return View(SuProfil);
        }

        // GET: SuProfil/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SuProfil = await _context.SuProfil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SuProfil == null)
            {
                return NotFound();
            }

            return View(SuProfil);
        }

        // POST: SuProfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var SuProfil = await _context.SuProfil.FindAsync(id);

        //    var user = await _userManager.GetUserAsync(User);
        //    SuProfil.UserIdKemaskini = user.UserName;
        //    SuProfil.TarKemaskini = DateTime.Now;

        //    _context.SuProfil.Remove(SuProfil);
        //    await AddLogAsync("Hapus", SuProfil.Perihal,SuProfil.Perihal,id, 0);
        //    await _context.SaveChangesAsync();
        //    TempData[SD.Success] = "Data berjaya dihapuskan..!";
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.SuProfil.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.SuProfil.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool SuProfilExists(int id)
        {
            return _context.SuProfil.Any(e => e.Id == id);
        }
    }
}
