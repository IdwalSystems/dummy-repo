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
using MSNK.Models;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkPembekalController : Controller
    {
        public const string modul = "FL001";
        public const string namamodul = "Pembekal";

        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JNegeri, int, string> _jnegeriRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;

        public AkPembekalController(
            ApplicationDbContext context,
            IRepository<AkPembekal, int, string> AkPembekalRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JNegeri, int, string> JNegeriRepository,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog)
        {
            _context = context;
            _akpembekalRepo = AkPembekalRepository;
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
            var akpembekal = _akpembekalRepo.GetAll()
                .Result
                .Where(s => s.KodSykt.Contains(namasykt.Substring(0, 1)))
                .OrderByDescending(s => s.Id).FirstOrDefault();

            int intkodsykt = 0;
            if (akpembekal != null)
            {
                if (int.TryParse(akpembekal.KodSykt.Substring(1), out intkodsykt))
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

        // GET: AkPembekal
        public async Task<IActionResult> Index()
        {
            var akpembekal = await _akpembekalRepo.GetAll();
            return View(akpembekal);
        }

        // GET: AkPembekal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _akpembekalRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPembekal.JBankId);
            akPembekal.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPembekal.JNegeriId);
            akPembekal.JNegeri = negeri;

            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // GET: AkPembekal/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkPembekal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPembekal akPembekal, int jNegeriId, int jBankId)
        {
            AkPembekal akP = new();

            // check kalau ada no Akaun redundant
            var akPAkaunRedundant = _context.AkPembekal.Where(x => x.AkaunBank == akPembekal.AkaunBank).FirstOrDefault();

            if (akPAkaunRedundant != null)
            {
                TempData[SD.Error] = "No Akaun berikut telah didaftarkan. Sila cuba sekali lagi.";
                PopulateList();

                return View(akP);
            }
            // check end
            if (ModelState.IsValid)
            {
                if (akPembekal != null)
                {
                    akP.JBankId = jBankId;
                    akP.JNegeriId = jNegeriId;
                    akP.KodSykt = GetKodSykt(akPembekal.NamaSykt);
                    akP.NamaSykt = akPembekal.NamaSykt;
                    akP.NoPendaftaran = akPembekal.NoPendaftaran;
                    akP.Poskod = akPembekal.Poskod;
                    akP.Telefon1 = akPembekal.Telefon1;
                    akP.AkaunBank = akPembekal.AkaunBank;
                    akP.Alamat1 = akPembekal.Alamat1;
                    akP.Alamat2 = akPembekal.Alamat2;
                    akP.Alamat3 = akPembekal.Alamat3;
                    akP.Bandar = akPembekal.Bandar;
                    akP.Emel = akPembekal.Emel;
                    await _akpembekalRepo.Insert(akP);
                    //insert applog
                    await AddLogAsync("Tambah", akP.KodSykt + " - " + akP.NamaSykt, akP.KodSykt, 0, 0);
                    //insert applog end
                    await _akpembekalRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Syarikat adalah " + akP.KodSykt;

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();

            return View(akP);
        }

        // GET: AkPembekal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var akPembekal = await _context.AkPembekal.FindAsync(id);
            PopulateList();
            var akPembekal = await _akpembekalRepo.GetById((int)id);
            var bank = await _jbankRepo.GetById(akPembekal.JBankId);
            akPembekal.JBank = bank;
            var negeri = await _jnegeriRepo.GetById(akPembekal.JNegeriId);
            akPembekal.JNegeri = negeri;
            if (akPembekal == null)
            {
                return NotFound();
            }
            //ViewData["JBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.JBankId);
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // POST: AkPembekal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPembekal akPembekal, int jNegeriId, int jBankId)
        {
            if (id != akPembekal.Id)
            {
                return NotFound();
            }

            //AkPembekal akP = new();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    AkPembekal dataAsal = await _akpembekalRepo.GetById(id);
                    akPembekal.KodSykt = dataAsal.KodSykt;
                    var namaAsal = dataAsal.NamaSykt;
                    akPembekal.UserIdKemaskini = user.UserName;
                    akPembekal.TarKemaskini = DateTime.Now;
                    //_context.Update(akPembekal);
                    //await _context.SaveChangesAsync();

                    //akP.JBankId = jBankId;
                    //akP.JNegeriId = jNegeriId;
                    //akP.KodSykt = akPembekal.KodSykt;
                    //akP.NamaSykt = akPembekal.NamaSykt;
                    //akP.NoPendaftaran = akPembekal.NoPendaftaran;
                    //akP.Poskod = akPembekal.Poskod;
                    //akP.Telefon1 = akPembekal.Telefon1;
                    //akP.AkaunBank = akPembekal.AkaunBank;
                    //akP.Alamat1 = akPembekal.Alamat1;
                    //akP.Alamat2 = akPembekal.Alamat2;
                    //akP.Alamat3 = akPembekal.Alamat3;
                    //akP.Bandar = akPembekal.Bandar;
                    //akP.Emel = akPembekal.Emel;

                    await _akpembekalRepo.Update(akPembekal);
                    //insert applog
                    if (namaAsal != akPembekal.NamaSykt)
                    {
                        await AddLogAsync("Ubah", namaAsal + " -> " + akPembekal.NamaSykt, akPembekal.KodSykt, id, 0);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", akPembekal.KodSykt, id, 0);
                    }
                    //insert applog end
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya diubah..!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPembekalExists(akPembekal.Id))
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
            //ViewData["JBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPembekal.JBankId);
            //ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", akPembekal.JNegeriId);
            return View(akPembekal);
        }

        // GET: AkPembekal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPembekal = await _context.AkPembekal
                .Include(a => a.JBank)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPembekal == null)
            {
                return NotFound();
            }

            return View(akPembekal);
        }

        // POST: AkPembekal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPembekal = await _context.AkPembekal.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            akPembekal.UserIdKemaskini = user.UserName;
            akPembekal.TarKemaskini = DateTime.Now;

            _context.AkPembekal.Remove(akPembekal);
            await AddLogAsync("Hapus", akPembekal.KodSykt + " - " + akPembekal.NamaSykt, akPembekal.KodSykt, id, 0);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkPembekalExists(int id)
        {
            return _context.AkPembekal.Any(e => e.Id == id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akpembekalRepo.GetByIdIncludeDeletedItems(id);

            // Batal operation

            obj.FlHapus = 0;
            _context.AkPembekal.Update(obj);

            //await AddLogAsync("Rollback", obj.Kod + " - " + obj.Perihal, 0);
            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
    }
}
