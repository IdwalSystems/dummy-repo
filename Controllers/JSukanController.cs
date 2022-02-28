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
    public class JSukanController : Controller
    {
        public const string modul = "JD008";
        public const string namamodul = "Jadual Sukan";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JSukanController(ApplicationDbContext context,
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

        // GET: JSukan
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JSukan.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JSukan.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
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
                await AddLogAsync("Tambah", jSukan.Perihal, jSukan.Perihal, 0, 0); 
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
                    var objAsal = await _context.JSukan.FirstOrDefaultAsync(x => x.Id == jSukan.Id);
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(jSukan);

                    if (perihalAsal != jSukan.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jSukan.Perihal,jSukan.Perihal,id, 0);
                    }

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

            var user = await _userManager.GetUserAsync(User);
            jSukan.UserIdKemaskini = user.UserName;
            jSukan.TarKemaskini = DateTime.Now;

            _context.JSukan.Remove(jSukan);
            await AddLogAsync("Hapus", jSukan.Perihal,jSukan.Perihal,id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JSukan.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JSukan.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool JSukanExists(int id)
        {
            return _context.JSukan.Any(e => e.Id == id);
        }
    }
}
