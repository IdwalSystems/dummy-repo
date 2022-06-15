using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        private bool SukanExists(string perihal)
        {
            return _context.JSukan.Any(e => e.Perihal == perihal);
        }

        // POST: JSukan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JSukan jSukan)
        {
            if (ModelState.IsValid)
            {
                var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

                var user = await _userManager.GetUserAsync(User);

                JSukan m = new JSukan();
                if (SukanExists(jSukan.Perihal) == false)
                {
                    if (ModelState.IsValid)
                    {
                        //string noRujukan = GetKod(akJurnal.JKWId);
                        if (jSukan != null)
                        {
                            m.Kod = jSukan.Kod;
                            m.Perihal = jSukan.Perihal?.ToUpper() ?? null;
                            m.IsElit = jSukan.IsElit;
                            m.IsPembangunan = jSukan.IsPembangunan;
                            m.UserId = username;
                            m.TarMasuk = DateTime.Now;

                            //m.SuTanggungan = _cart.Lines1.ToArray();

                            _context.Add(m);

                            //insert applog
                            await AddLogAsync("Tambah", m.Kod + " - " + m.Perihal, m.Perihal, 0, 0);
                            //insert applog end

                            //await AddLogAsync("Tambah", noRujukan, kredit);
                            await _context.SaveChangesAsync();

                            //CartEmpty();
                            TempData[SD.Success] = "Maklumat berjaya ditambah.";
                            return RedirectToAction(nameof(Index));
                        }
                        //_context.Add(suJurulatih);
                        //await _context.SaveChangesAsync();
                        //return RedirectToAction(nameof(Index));

                    }
                }
                else
                {
                    TempData[SD.Error] = "Sukan ini telah wujud..!";
                }

                return View(jSukan);

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
        public async Task<IActionResult> Edit(int id,JSukan jSukan)
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
