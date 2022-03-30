using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    public class JPenyemakController : Controller
    {
        public const string modul = "JD011";
        public const string namamodul = "Jadual Penyemak";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<JPenyemak, int, string> _penyemakRepo;

        public JPenyemakController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<JPenyemak, int, string> penyemakRepo)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
            _penyemakRepo = penyemakRepo;
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

        // GET: JPenyemak
        public async Task<IActionResult> Index()
        {
            var obj = await _penyemakRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _penyemakRepo.GetAllIncludeDeletedItems();
            }

            return View(obj);
        }

        // GET: JPenyemak/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);

            if (jPenyemak == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(jPenyemak);
        }

        private void PopulateList()
        {
            List<SuPekerja> pekerjaList = _context.SuPekerja.ToList();
            ViewBag.SuPekerja = pekerjaList;
        }

        // GET: JPenyemak/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: JPenyemak/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JPenyemak jPenyemak , int SuPekerjaId)
        {
            JPenyemak m = new JPenyemak();
            var user = await _userManager.GetUserAsync(User);
            var pekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == SuPekerjaId);

            if (IsSuPekerjaExists(SuPekerjaId) == true)
            {
                TempData[SD.Error] = "Penyemak ini telah wujud..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (jPenyemak.MinAmaun > jPenyemak.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (ModelState.IsValid)
            {
                if (jPenyemak != null && SuPekerjaId != 0 && pekerja != null)
                {
                    m.SuPekerjaId = SuPekerjaId;
                    m.MinAmaun = jPenyemak.MinAmaun;
                    m.MaksAmaun = jPenyemak.MaksAmaun;
                    m.IsBelian = jPenyemak.IsBelian;
                    m.IsNotaMinta = jPenyemak.IsNotaMinta;
                    m.IsPendahuluan = jPenyemak.IsPendahuluan;
                    m.IsPO = jPenyemak.IsPO;
                    m.IsPV = jPenyemak.IsPV;
                    m.TarMasuk = DateTime.Now;
                    m.UserId = user.UserName;

                    await _penyemakRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", pekerja.NoGaji + " - " + pekerja.NoKp, pekerja.NoGaji, 0, 0);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    //CartEmpty();
                    TempData[SD.Success] = "Maklumat Penyemak berjaya ditambah";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData[SD.Error] = "Sila isi ruangan yang bertanda (*)..!";
            PopulateList();
            return View(jPenyemak);
        }

        // GET: JPenyemak/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);
            if (jPenyemak == null)
            {
                return NotFound();
            }
            PopulateList();
            return View(jPenyemak);
        }

        // POST: JPenyemak/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JPenyemak jPenyemak, int SuPekerjaId)
        {
            if (id != jPenyemak.Id)
            {
                return NotFound();
            }

            if (jPenyemak.MinAmaun > jPenyemak.MaksAmaun)
            {
                TempData[SD.Error] = "Amaun Minimum lebih besar dari Amaun Maksimum..!";
                PopulateList();
                return View(jPenyemak);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var objAsal = await _context.JPenyemak.Include(x => x.SuPekerja).FirstOrDefaultAsync(x => x.Id == id);

                    jPenyemak.SuPekerjaId = objAsal.SuPekerjaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    var objPekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == SuPekerjaId);

                    jPenyemak.UserIdKemaskini = user.UserName;
                    jPenyemak.TarKemaskini = DateTime.Now;

                    _context.Update(jPenyemak);
                    //insert applog

                    await AddLogAsync("Ubah", objPekerja.NoGaji + " - " + objPekerja.NoKp, objPekerja.NoGaji, id, 0);

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JPenyemakExists(jPenyemak.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            return View(jPenyemak);
        }

        // GET: JPenyemak/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jPenyemak = await _penyemakRepo.GetById((int)id);
            if (jPenyemak == null)
            {
                return NotFound();
            }

            return View(jPenyemak);
        }

        // POST: JPenyemak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _penyemakRepo.GetById(id);

            var user = await _userManager.GetUserAsync(User);
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;

            await _penyemakRepo.Delete(id);
            //insert applog
            await AddLogAsync("Hapus", obj.SuPekerja.NoGaji + " - " + obj.SuPekerja.NoKp, obj.SuPekerja.NoGaji, id, 0);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool JPenyemakExists(int id)
        {
            return _context.JPenyemak.Any(e => e.Id == id);
        }

        private bool IsSuPekerjaExists(int id)
        {
            return _context.JPenyemak.Any(e => e.SuPekerjaId == id);
        }
    }
}
