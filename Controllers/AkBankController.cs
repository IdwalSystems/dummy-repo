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
using MSNK.Models.Modules.ViewModel;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkBankController : Controller
    {
        public const string modul = "JD009";
        public const string namamodul = "Akaun Bank";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JBank, int, string> _bankRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;

        public AkBankController(ApplicationDbContext context,
                                AppLogIRepository<AppLog, int> appLog,
                                UserManager<IdentityUser> userManager,
                                IRepository<JKW, int, string> kwRepository,
                                IRepository<JBank, int, string> bankRepository,
                                IRepository<AkBank, int, string> akBankRepository)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _bankRepo = bankRepository;
            _akBankRepo = akBankRepository; 
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

            await _appLog.Insert(appLog,modul,operasi);
        }

        // GET: AkBank
        public async Task<IActionResult> Index()
        {
            var akBank = await _akBankRepo.GetAll();

           return View(akBank);
            
        }

        // GET: AkBank/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }

            return View(akBank);
        }
        
        private void PopulateList()
        {
            List<JBank> bankList = _context.JBank.ToList();
            bankList.Insert(0, new JBank { Id = 0, Nama = "-- Pilih Bank --" });
            ViewBag.JBank = bankList;
            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            bahagianList.Insert(0, new JBahagian { Id = 0, Perihal = "-- Pilih Bahagian --" });
            ViewBag.JBahagian = bahagianList;
            List<JKW> kwList = _context.JKW.ToList();
            kwList.Insert(0, new JKW { Id = 0, Perihal = "-- Pilih Kumpulan Wang --" });
            ViewBag.JKW = kwList;
            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b=> b.JParas)
                .Where(b => b.JParas.Kod == "4" && b.Kod.Substring(0, 2) == "A1")
                .OrderBy(b => b.Kod)
                .ToList();
            akCartaList.Insert(0, new AkCarta { Id = 0, Perihal = "-- Pilih Kod Akaun --" });
            ViewBag.AkCarta = akCartaList;
        }

        // GET: AkBank/Create
        public IActionResult Create()
        {
            PopulateList();

            return View();
        }

        // POST: AkBank/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBank akBank, int JKWId, int JBankId, int AkCartaId, int JBahagianId)
        {
            AkBank akB = new AkBank();
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (akBank != null && JKWId != 0 && JBankId != 0 && AkCartaId != 0 && JBahagianId != 0)
                {
                    akB.JBankId = JBankId;
                    akB.JKWId = JKWId;
                    akB.AkCartaId = AkCartaId;
                    akB.JBahagianId = JBahagianId;
                    akB.Kod = akBank.Kod;
                    akB.NoAkaun = akBank.NoAkaun;
                    akB.UserId = user.UserName;
                    akB.TarMasuk = DateTime.Now;

                    await _akBankRepo.Insert(akB);

                    //insert applog
                    await AddLogAsync("Tambah",akB.Kod + " - " + akB.NoAkaun, akB.Kod, 0, 0);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akBank.Kod;
                    return RedirectToAction(nameof(Index));
                }
                
            }

            PopulateList();
            return View(akBank);
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: AkBank/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }

            PopulateList();

            return View(akBank);
        }

        // POST: AkBank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBank akBank, int JKWId, int JBankId, int AkCartaId, int JBahagianId)
        {
            if (id != akBank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    var objAsal = await _context.AkBank.FirstOrDefaultAsync(x => x.Id == akBank.Id);

                    var kodAsal = objAsal.Kod;
                    var noAkaunAsal = objAsal.NoAkaun;

                    akBank.JKWId = objAsal.JKWId;
                    akBank.JBahagianId = objAsal.JBahagianId;
                    akBank.AkCartaId = objAsal.AkCartaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    akBank.UserIdKemaskini = user.UserName;
                    akBank.TarKemaskini = DateTime.Now;

                    _context.Update(akBank);
                    //insert applog
                    await AddLogAsync("Ubah",kodAsal + " -> " + akBank.Kod + ", "
                        + noAkaunAsal + " -> " + akBank.NoAkaun + ", ", akBank.Kod, id, 0);

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBankExists(akBank.Id))
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

            return View(akBank);
        }

        // GET: AkBank/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBank = await _akBankRepo.GetById((int)id);
            if (akBank == null)
            {
                return NotFound();
            }

            return View(akBank);
        }

        // POST: AkBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var akBank = await _context.AkBank.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            akBank.UserIdKemaskini = user.UserName;
            akBank.TarKemaskini = DateTime.Now;

            await _akBankRepo.Delete(id);
            //insert applog
            await AddLogAsync("Hapus",akBank.Kod + " - " + akBank.NoAkaun, akBank.Kod, id, 0);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkBankExists(int id)
        {
            return _context.AkBank.Any(e => e.Id == id);
        }
    }
}
