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
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Policy = "PB001")]
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkBankReconController : Controller
    {
        public const string modul = "PB001";
        public const string namamodul = "Penyesuaian Bank";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBankRecon, int, string> _akReconRepo;
        private readonly ListViewIRepository<AkBankReconPenyataBank, int> _akReconPenyataRepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartBankRecon _cart;


        public AkBankReconController(
            ApplicationDbContext context, 
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBankRecon, int, string> akReconRepo,
            ListViewIRepository<AkBankReconPenyataBank, int> akReconPenyataRepo,
            IRepository<AkPV, int, string> akPVRepo,
            IRepository<AkTerima, int, string> akTerimaRepo,
            CartBankRecon cart)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akReconRepo = akReconRepo;
            _akReconPenyataRepo = akReconPenyataRepo;
            _cart = cart;
            _akTerimaRepo=akTerimaRepo;
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

        // GET: AkBankRecon
        [Authorize(Policy = "PB001")]
        public IActionResult Index()
        {
            try
            {
                PopulateList();
                return View();
            } catch
            {
                return BadRequest();
            }
        }

        private void PopulateList()
        {
            List<AkBank> bankList = _context.AkBank
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = bankList;

            ViewBag.AkBankId = 1;

            ViewData["Tahun"] = DateTime.Now.ToString("yyyy");

        }

        [HttpPost]
        public async Task<IActionResult> Index(
            int AkBankId,
            string Tahun,
            string Bulan)
        {
            List<AkBankRecon> akRecon = await _context.AkBankRecon
                .Where(b => b.AkBankId == AkBankId).ToListAsync();

            if (!string.IsNullOrEmpty(Tahun))
            {
                akRecon = akRecon.Where(b => b.Tahun == Tahun).ToList();
            }

            if (!string.IsNullOrEmpty(Bulan))
            {
                akRecon = akRecon.Where(b => b.Bulan == Bulan).ToList();
            }

            PopulateList();
            ViewData["Tahun"] = Tahun;
            ViewData["Bulan"] = Bulan;

            return View(akRecon.OrderBy(b => b.Tahun).ThenBy(b => b.Bulan).ThenBy(b => b.AkBank.NoAkaun).ToList());
        }
            // GET: AkBankRecon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);

            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // GET: AkBankRecon/Create
        [Authorize(Policy = "PB001C")]
        public IActionResult Create()
        {

            PopulateList();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkBankRecon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PB001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBankRecon akBankRecon)
        {
            AkBankRecon m = new AkBankRecon();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            bool IsExistAkRecon = _context.AkBankRecon
                .Any(b => b.Tahun == akBankRecon.Tahun &&
                b.Bulan == akBankRecon.Bulan &&
                b.AkBankId == akBankRecon.AkBankId);

            if (IsExistAkRecon == true)
            {
                TempData[SD.Error] = "Data bagi Tahun, Bulan bagi Akaun Bank ini telah wujud.";

                PopulateList();
                return View(akBankRecon);
            }
            if (ModelState.IsValid)
            {
                m.Tahun = akBankRecon.Tahun;
                m.Bulan = akBankRecon.Bulan;
                m.AkBankId = akBankRecon.AkBankId;
                m.BakiPenyata = akBankRecon.BakiPenyata;
                m.UserId = user.UserName;
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                await _akReconRepo.Insert(m);

                //insert applog
                await AddLogAsync("Tambah", m.Tahun + "/" + m.Bulan + "/" + m.AkBankId, m.Tahun + "/" + m.Bulan + "/" + m.AkBankId, 0, m.BakiPenyata, pekerjaId);
                //insert applog end
                await _akReconRepo.Save();
                TempData[SD.Success] = "Maklumat berjaya ditambah.";

                return RedirectToAction(nameof(Index));
            }

            PopulateList(); 
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Edit/5
        [Authorize(Policy = "PB001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(akBankRecon);
        }

        // POST: AkBankRecon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PB001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBankRecon akBankRecon)
        {
            if (id != akBankRecon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    AkBankRecon dataAsal = await _akReconRepo.GetByIdIncludeDeletedItems(id);

                    // list of input that cannot be change
                    //suJurulatih.Emel = dataAsal.Emel;
                    akBankRecon.TarMasuk = dataAsal.TarMasuk;
                    akBankRecon.UserId = dataAsal.UserId;
                    akBankRecon.Tahun = dataAsal.Tahun;
                    akBankRecon.Bulan = dataAsal.Bulan;
                    akBankRecon.AkBankId = dataAsal.AkBankId;
                    var bakiPenyata = dataAsal.BakiPenyata;
                    akBankRecon.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akBankRecon.UserIdKemaskini = user.UserName;
                    akBankRecon.TarKemaskini = DateTime.Now;
                    akBankRecon.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akBankRecon);

                    await AddLogAsync("Ubah", bakiPenyata + " -> " + akBankRecon.BakiPenyata
                            , akBankRecon.Tahun + "/" + akBankRecon.Bulan + "/" + akBankRecon.AkBankId
                            , id, akBankRecon.BakiPenyata, pekerjaId);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBankReconExists(akBankRecon.Id))
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
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Delete/5
        [Authorize(Policy = "PB001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);

            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // POST: AkBankRecon/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PB001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBankRecon = await _context.AkBankRecon.FindAsync(id);
            // check if already posting redirect back
            if (!string.IsNullOrEmpty(akBankRecon.TarKunci?.ToString("yyyy/MM/dd")))
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            akBankRecon.UserIdKemaskini = user.UserName;
            akBankRecon.TarKemaskini = DateTime.Now;
            akBankRecon.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkBankRecon.Remove(akBankRecon);
            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", akBankRecon.Tahun + "/" + akBankRecon.Bulan + "/" + akBankRecon.AkBankId, id, akBankRecon.BakiPenyata, pekerjaId);
            //insert applog end
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBankReconExists(int id)
        {
            return _context.AkBankRecon.Any(e => e.Id == id);
        }
    }
}
