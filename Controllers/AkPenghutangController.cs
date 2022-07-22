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
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkPenghutangController : Controller
    {
        public const string modul = "DF003";
        public const string namamodul = "Penghutang";

        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkPenghutang, int, string> _akpenghutangRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JNegeri, int, string> _jnegeriRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public AkPenghutangController(
            ApplicationDbContext context,
            IRepository<AkPenghutang, int, string> AkPenghutangRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JNegeri, int, string> JNegeriRepository,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog
            )
        {
            _context = context;
            _akpenghutangRepo = AkPenghutangRepository;
            _jbankRepo = JBankRepository;
            _jnegeriRepo = JNegeriRepository;
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

        private void PopulateList()
        {
            List<JBank> JBankList = _context.JBank.OrderBy(b => b.Kod).ToList();
            List<JNegeri> jnegeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();

            ViewBag.jbank = JBankList;
            ViewBag.jnegeri = jnegeriList;
        }

        private string GetKodSykt(string namasykt)
        {
            var akPengutang = _akpenghutangRepo.GetAll()
                .Result
                .Where(s => s.KodSykt.Contains(namasykt.Substring(0, 1)))
                .OrderByDescending(s => s.KodSykt).FirstOrDefault();

            int intkodsykt = 0;
            if (akPengutang != null)
            {
                if (int.TryParse(akPengutang.KodSykt.Substring(1), out intkodsykt))
                {
                    intkodsykt += 1;
                }
            }
            else
            {
                intkodsykt = 1;
            }

            string newkodsykt = namasykt.Substring(0, 1) + intkodsykt.ToString("D4");
            return newkodsykt.ToUpper();
        }

        [HttpPost]
        public JsonResult StrCalculate(string data)
        {
            try
            {
                var result = "";
                if (data == null || data == "")
                {
                    result = "";
                }
                else
                {
                    result = GetKodSykt(data.ToUpper());
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [Authorize(Policy = "DF003")]
        // GET: AkPenghutang
        public async Task<IActionResult> Index()
        {
            var akpenghutang = await _akpenghutangRepo.GetAllIncludeDeletedItems();
            return View(akpenghutang);
        }

        // GET: AkPenghutang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPenghutang = await _akpenghutangRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPenghutang.JBankId);
            akPenghutang.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPenghutang.JNegeriId);
            akPenghutang.JNegeri = negeri;

            if (akPenghutang == null)
            {
                return NotFound();
            }

            return View(akPenghutang);
        }

        [Authorize(Policy = "DF003C")]
        // GET: AkPenghutang/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        [Authorize(Policy = "DF003C")]
        // POST: AkPenghutang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPenghutang akPenghutang, int jNegeriId, int jBankId)
        {
            AkPenghutang akP = new();
            var user = await _userManager.GetUserAsync(User);

            // check kalau ada no Akaun redundant
            var akPAkaunRedundant = _context.AkPenghutang.Where(x => x.AkaunBank == akPenghutang.AkaunBank).FirstOrDefault();

            if (akPAkaunRedundant != null)
            {
                TempData[SD.Error] = "No Akaun berikut telah didaftarkan. Sila cuba sekali lagi.";
                PopulateList();

                return View(akP);
            }
            // check end
            if (ModelState.IsValid)
            {
                if (akPenghutang != null)
                {
                    akP.JBankId = jBankId;
                    akP.JNegeriId = jNegeriId;
                    akP.KodSykt = GetKodSykt(akPenghutang.NamaSykt);
                    akP.NamaSykt = akPenghutang.NamaSykt?.ToUpper()?? "";
                    akP.NoPendaftaran = akPenghutang.NoPendaftaran;
                    akP.Poskod = akPenghutang.Poskod;
                    akP.Telefon1 = akPenghutang.Telefon1;
                    akP.AkaunBank = akPenghutang.AkaunBank;
                    akP.Alamat1 = akPenghutang.Alamat1?.ToUpper()?? "";
                    akP.Alamat2 = akPenghutang.Alamat2?.ToUpper()?? "";
                    akP.Alamat3 = akPenghutang.Alamat3?.ToUpper()?? "";
                    akP.Bandar = akPenghutang.Bandar?.ToUpper()?? "";
                    akP.Emel = akPenghutang.Emel;
                    akP.UserId = user.UserName;
                    await _akpenghutangRepo.Insert(akP);
                    //insert applog
                    await AddLogAsync("Tambah", akP.KodSykt + " - " + akP.NamaSykt?.ToUpper()?? "", akP.KodSykt, 0, 0);
                    //insert applog end
                    await _akpenghutangRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Penghutang adalah " + akP.KodSykt;

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();

            return View(akP);
        }

        [Authorize(Policy = "DF003E")]
        // GET: AkPenghutang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var akPembekal = await _context.AkPembekal.FindAsync(id);
            PopulateList();
            var akPenghutang = await _akpenghutangRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPenghutang.JBankId);
            akPenghutang.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPenghutang.JNegeriId);
            akPenghutang.JNegeri = negeri;
            if (akPenghutang == null)
            {
                return NotFound();
            }
            return View(akPenghutang);
        }

        [Authorize(Policy = "DF003E")]
        // POST: AkPenghutang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPenghutang akPenghutang, int jNegeriId, int jBankId)
        {
            if (id != akPenghutang.Id)
            {
                return NotFound();
            }

            //AkPembekal akP = new();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    AkPenghutang dataAsal = await _akpenghutangRepo.GetById(id);
                    akPenghutang.KodSykt = dataAsal.KodSykt;
                    var namaAsal = dataAsal.NamaSykt;

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akPenghutang.NamaSykt = akPenghutang.NamaSykt?.ToUpper()?? "";
                    akPenghutang.Alamat1 = akPenghutang.Alamat1?.ToUpper()?? "";
                    akPenghutang.Alamat2 = akPenghutang.Alamat2?.ToUpper()?? "";
                    akPenghutang.Alamat3 = akPenghutang.Alamat3?.ToUpper()?? "";
                    akPenghutang.Bandar = akPenghutang.Bandar?.ToUpper()?? "";

                    akPenghutang.UserIdKemaskini = user.UserName;
                    akPenghutang.TarKemaskini = DateTime.Now;

                    await _akpenghutangRepo.Update(akPenghutang);
                    //insert applog
                    if (namaAsal != akPenghutang.NamaSykt)
                    {
                        await AddLogAsync("Ubah", namaAsal + " -> " + akPenghutang.NamaSykt?.ToUpper()?? "", akPenghutang.KodSykt, id, 0);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", akPenghutang.KodSykt, id, 0);
                    }
                    //insert applog end
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPenghutangExists(akPenghutang.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                PopulateList();
                return RedirectToAction(nameof(Index));
            }
            return View(akPenghutang);
        }

        [Authorize(Policy = "DF003D")]
        // GET: AkPenghutang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPenghutang = await _context.AkPenghutang
                .Include(a => a.JBank)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPenghutang == null)
            {
                return NotFound();
            }

            return View(akPenghutang);
        }

        [Authorize(Policy = "DF003D")]
        // POST: AkPenghutang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPenghutang = await _context.AkPenghutang.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            akPenghutang.UserIdKemaskini = user.UserName;
            akPenghutang.TarKemaskini = DateTime.Now;

            _context.AkPenghutang.Remove(akPenghutang);
            await AddLogAsync("Hapus", akPenghutang.KodSykt + " - " + akPenghutang.NamaSykt, akPenghutang.KodSykt, id, 0);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkPenghutangExists(int id)
        {
            return _context.AkPenghutang.Any(e => e.Id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Policy = "DF003R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akpenghutangRepo.GetByIdIncludeDeletedItems(id);

            // Batal operation

            obj.FlHapus = 0;
            _context.AkPenghutang.Update(obj);

            await AddLogAsync("Rollback", obj.KodSykt + " - " + obj.NamaSykt, obj.KodSykt, id, 0);
            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

    }
}
