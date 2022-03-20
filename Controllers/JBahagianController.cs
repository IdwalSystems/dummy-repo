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
    public class JBahagianController : Controller
    {
        public const string modul = "JD002";
        public const string namamodul = "Bahagian";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<JBahagian, int, string> _jBahagianRepo;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBahagianController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<JBahagian, int, string> jBahagianRepo,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _userManager = userManager;
            _jBahagianRepo = jBahagianRepo;
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
        // GET: JBahagian
        public async Task<IActionResult> Index()
        {
            var obj = await _jBahagianRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _jBahagianRepo.GetAllIncludeDeletedItems();
            }
            return View(obj);
        }

        // GET: JBahagian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _jBahagianRepo.GetById((int)id);

            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // GET: JBahagian/Create
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Kod == "100");

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x=> x.JKW.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list;
            return View();
        }

        [HttpPost]
        public JsonResult JsonGetKod(int data)
        {
            try
            {
                var result = "";
                if (data == 0)
                {
                    result = "";
                }
                else
                {
                    // get latest no rujukan running number  
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw.Kod;
                    string prefix = kumpulanWang;
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.JBahagian
                                .IgnoreQueryFilters()
                                .Where(x => x.JKW.Kod == kw.Kod)
                                .Max(x => x.Kod);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(3));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // POST: JBahagian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JBahagian jBahagian, int JKWId)
        {
            JBahagian m = new JBahagian();
            var user = await _userManager.GetUserAsync(User);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == JKWId);

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.JBahagian
                        .IgnoreQueryFilters()
                        .Where(x => x.JKW.Kod == kw.Kod)
                        .Max(x => x.Kod);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            if (jBahagian != null && JKWId != 0)
            {
                
                if (ModelState.IsValid)
                {
                    m.JKWId = JKWId;
                    m.Kod = noRujukan;
                    m.Perihal = jBahagian.Perihal;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    _context.Add(m);
                    await AddLogAsync("Tambah", m.Kod + " - " + m.Perihal,m.Kod,0, 0);
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }

            List<JKW> list = _context.JKW.ToList();
            ViewBag.NoRujukan = noRujukan;
            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // GET: JBahagian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _context.JBahagian.FindAsync(id);
            if (jBahagian == null)
            {
                return NotFound();
            }
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // POST: JBahagian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,JBahagian jBahagian, int JKWId)
        {
            if (id != jBahagian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    JBahagian jBahagianAsal = await _context.JBahagian.FindAsync(id);

                    // list of input that cannot be change
                    jBahagian.JKWId = jBahagianAsal.JKWId;
                    jBahagian.Kod = jBahagianAsal.Kod;
                    jBahagian.TarMasuk = jBahagianAsal.TarMasuk;
                    jBahagian.UserId = jBahagianAsal.UserId;
                    var perihalAsal = jBahagianAsal.Perihal;
                    // list of input that cannot be change end
                    _context.Entry(jBahagianAsal).State = EntityState.Detached;

                    jBahagian.UserIdKemaskini = user.UserName;
                    jBahagian.TarKemaskini = DateTime.Now;

                    _context.Update(jBahagian);

                    if (perihalAsal != jBahagian.Perihal)
                    {
                        await AddLogAsync("Ubah", perihalAsal + " -> " + jBahagian.Perihal, jBahagian.Kod,id, 0);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", jBahagian.Kod, id, 0);
                    }

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JBahagianExists(jBahagian.Id))
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
            List<JKW> list = _context.JKW.ToList();

            ViewBag.JKw = list; 
            return View(jBahagian);
        }

        // GET: JBahagian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jBahagian = await _jBahagianRepo.GetById((int)id);

            if (jBahagian == null)
            {
                return NotFound();
            }

            return View(jBahagian);
        }

        // POST: JBahagian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jBahagian = await _context.JBahagian.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            jBahagian.UserIdKemaskini = user.UserName;
            jBahagian.TarKemaskini = DateTime.Now;

            _context.JBahagian.Remove(jBahagian);
            await AddLogAsync("Hapus", jBahagian.Kod + " - " + jBahagian.Perihal, jBahagian.Kod,id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _jBahagianRepo.GetByIdIncludeDeletedItems(id);

            // Batal operation

            obj.FlHapus = 0;
            _context.JBahagian.Update(obj);

            //await AddLogAsync("Rollback", obj.Kod + " - " + obj.Perihal, 0);
            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JBahagianExists(int id)
        {
            return _context.JBahagian.Any(e => e.Id == id);
        }
    }
}
