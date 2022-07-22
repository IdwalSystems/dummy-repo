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
    public class JCaraBayarController : Controller
    {
        public const string modul = "JD005";
        public const string namamodul = "Jadual Cara Bayar";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JCaraBayarController(ApplicationDbContext context,
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
        // GET: CaraBayar
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JCaraBayar.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JCaraBayar.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: CaraBayar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caraBayar == null)
            {
                return NotFound();
            }

            return View(caraBayar);
        }

        // GET: CaraBayar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaraBayar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Perihal")] JCaraBayar caraBayar)
        {
            if (KodCaraBayarExists(caraBayar.Kod) == false)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(caraBayar);
                    await AddLogAsync("Tambah", caraBayar.Kod + " - " + caraBayar.Perihal,caraBayar.Kod,0, 0);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData[SD.Error] = "Kod ini telah wujud..!";
            }
                
            return View(caraBayar);
        }

        // GET: CaraBayar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar.FindAsync(id);
            if (caraBayar == null)
            {
                return NotFound();
            }
            return View(caraBayar);
        }

        // POST: CaraBayar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Perihal")] JCaraBayar caraBayar)
        {
            if (id != caraBayar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objAsal = await _context.JCaraBayar.FirstOrDefaultAsync(x => x.Id == caraBayar.Id);
                    var kodAsal = objAsal.Kod;
                    var perihalAsal = objAsal.Perihal;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    _context.Update(caraBayar);

                    await AddLogAsync("Ubah", kodAsal + " -> " + caraBayar.Kod + ", "
                        + perihalAsal + " -> " + caraBayar.Perihal + ", ",caraBayar.Kod,id, 0);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaraBayarExists(caraBayar.Id))
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
            return View(caraBayar);
        }

        // GET: CaraBayar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caraBayar = await _context.JCaraBayar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caraBayar == null)
            {
                return NotFound();
            }

            return View(caraBayar);
        }

        // POST: CaraBayar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caraBayar = await _context.JCaraBayar.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            caraBayar.UserIdKemaskini = user.UserName;
            caraBayar.TarKemaskini = DateTime.Now;

            _context.JCaraBayar.Remove(caraBayar);
            await AddLogAsync("Hapus", caraBayar.Kod + " - " + caraBayar.Perihal,caraBayar.Kod,id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JCaraBayar.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JCaraBayar.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        private bool CaraBayarExists(int id)
        {
            return _context.JCaraBayar.Any(e => e.Id == id);
        }
        private bool KodCaraBayarExists(string kod)
        {
            return _context.JCaraBayar.Any(e => e.Kod == kod);
        }
    }
}
