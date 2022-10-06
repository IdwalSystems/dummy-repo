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
    public class JBankController : Controller
    {
        public const string modul = "JD004";
        public const string namamodul = "Jadual Bank";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public JBankController(ApplicationDbContext context,
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

        // GET: Bank
        public async Task<IActionResult> Index()
        {
            var obj = await _context.JBank.ToListAsync();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _context.JBank.IgnoreQueryFilters().ToListAsync();
            }

            return View(obj);
        }

        // GET: Bank/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.JBank
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // GET: Bank/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bank/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Nama,KodEFT")] JBank bank)
        {
            if (KodBankExists(bank.Kod) == false)
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    bank.SuPekerjaMasukId = pekerjaId ?? 1;
                    bank.UserId = user.Id;
                    bank.TarMasuk = DateTime.Now;

                    _context.Add(bank);
                    await AddLogAsync("Tambah", bank.Kod + " - " + bank.Nama , bank.Kod, 0, 0, pekerjaId); 
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya ditambah..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData[SD.Error] = "Kod ini telah wujud..!";
            }
            
            return View(bank);
        }

        // GET: Bank/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.JBank.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }

        // POST: Bank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Nama,KodEFT")] JBank bank)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    var objAsal = await _context.JBank.FirstOrDefaultAsync(x => x.Id == bank.Id);
                    var kodAsal = objAsal.Kod;
                    var perihalAsal = objAsal.Nama;
                    var kodEFTAsal = objAsal.KodEFT;
                    bank.UserId = objAsal.UserId;
                    bank.TarMasuk = objAsal.TarMasuk;
                    bank.SuPekerjaMasukId = pekerjaId;

                    _context.Entry(objAsal).State = EntityState.Detached;

                    bank.UserIdKemaskini = user.UserName;
                    bank.TarKemaskini = DateTime.Now;
                    bank.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(bank);

                    await AddLogAsync("Ubah", kodAsal + " -> " + bank.Kod  + ", " 
                        + perihalAsal + " -> " + bank.Nama + ", "
                        + kodEFTAsal + " -> " + bank.KodEFT + ", ",bank.Kod,id, 0, pekerjaId);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankExists(bank.Id))
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
            return View(bank);
        }

        // GET: Bank/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.JBank
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bank == null)
            {
                return NotFound();
            }

            return View(bank);
        }

        // POST: Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bank = await _context.JBank.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            bank.UserIdKemaskini = user.UserName;
            bank.TarKemaskini = DateTime.Now;
            bank.SuPekerjaKemaskiniId = pekerjaId;

            _context.JBank.Remove(bank);
            await AddLogAsync("Hapus", bank.Kod + " - " + bank.Nama,bank.Kod, id, 0, pekerjaId);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _context.JBank.IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            // Batal operation

            obj.FlHapus = 0;
            _context.JBank.Update(obj);

            // Batal operation end
            await AddLogAsync("Rollback", obj.Kod + " - " + obj.Nama, obj.Kod, id, 0, pekerjaId);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool BankExists(int id)
        {
            return _context.JBank.Any(e => e.Id == id);
        }

        private bool KodBankExists(string kod)
        {
            return _context.JBank.Any(e => e.Kod == kod);
        }
    }
}
