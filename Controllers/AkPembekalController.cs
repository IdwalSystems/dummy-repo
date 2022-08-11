using System;
using System.Collections.Generic;
using System.Dynamic;
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
using MSNK.Models.Modules.ViewModel;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkPembekalController : Controller
    {
        public const string modul = "DF002";
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
                .OrderByDescending(s => s.KodSykt).FirstOrDefault();

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
        [Authorize(Policy = "DF002")]
        public async Task<IActionResult> Index()
        {
            var akpembekal = await _akpembekalRepo.GetAllIncludeDeletedItems();
            return View(akpembekal);
        }

        // GET: AkPembekal/Details/5
        public async Task<IActionResult> Details(int? id, DateTime? lastDate)
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
            // create table sublejar pembekal
            List<SublejarPembekalViewModel> vmodel = new List<SublejarPembekalViewModel>();

            // get baki awal
            var tahun = DateTime.Now.Year;
            var firstDate = Convert.ToDateTime("01/01/" + tahun);
            if (lastDate == null)
            {
                lastDate = DateTime.Now;
            }
            // get all akBelian and AkPV order by tarikh where posting = 1 where tarikh less than 01/01/[this-year]
            List<AkBelian> bakiAwalBelianList = _context.AkBelian.Where(b => b.AkPembekalId == akPembekal.Id
                && b.FlPosting == 1
                && b.TarikhKewanganTerima < firstDate.AddHours(23.99)).ToList();

            decimal bakiAwal = 0;
            decimal bayaranAwal = 0;
            decimal hutangAwal = 0;

            bakiAwal = bakiAwal + akPembekal.BakiAwal;
            if (akPembekal.BakiAwal < 0)
            {
                hutangAwal = hutangAwal - akPembekal.BakiAwal;
            }
            else
            {
                bayaranAwal = bayaranAwal + akPembekal.BakiAwal;
            }

            foreach (var item in bakiAwalBelianList)
            {
                hutangAwal = hutangAwal + item.Jumlah;
                bakiAwal = bakiAwal - item.Jumlah;
            }

            List<AkPV> bakiAwalPVList = _context.AkPV.Where(b => b.AkPembekalId == (int)id
                && b.FlPosting == 1
                && b.Tarikh < firstDate.AddHours(23.99)).ToList();

            foreach(var item in bakiAwalPVList)
            {
                bayaranAwal = bayaranAwal + item.Jumlah;
                bakiAwal = bakiAwal + item.Jumlah;
            }

            // insert into viewmodel first row
            vmodel.Add(new SublejarPembekalViewModel
            {
                Tarikh = firstDate,
                Rujukan = "BAKI AWAL",
                Bayaran = bayaranAwal,
                Hutang = hutangAwal,
                Baki = bakiAwal
            });

            // get all akBelian and AkPV order by tarikh where posting = 1
            List<AkBelian> BelianList = _context.AkBelian.Where(b => b.AkPembekalId == akPembekal.Id
                && b.FlPosting == 1
                && b.TarikhKewanganTerima >= firstDate
                && b.TarikhKewanganTerima <= lastDate).ToList();

            foreach (var item in BelianList)
            {
                vmodel.Add(new SublejarPembekalViewModel
                {
                    Tarikh = (DateTime)item.TarikhKewanganTerima,
                    Rujukan = item.NoInbois,
                    Bayaran = 0,
                    Hutang = item.Jumlah
                });
            }

            // get all akBelian and AkPV order by tarikh where posting = 1
            List<AkPV> PVList = _context.AkPV.Where(b => b.AkPembekalId == akPembekal.Id
                && b.FlPosting == 1
                && b.Tarikh >= firstDate
                && b.Tarikh <= lastDate).ToList();

            foreach (var item in PVList)
            {
                vmodel.Add(new SublejarPembekalViewModel
                {
                    Tarikh = item.Tarikh,
                    Rujukan = item.NoPV,
                    Bayaran = item.Jumlah,
                    Hutang = 0
                });
            }

            vmodel = vmodel.OrderBy(b => b.Tarikh).ToList();

            // insert into viewmodel with balance for each transaction
            var bil = 0;

            foreach (var i in vmodel)
            {
                bil++;
                i.Id = bil;

                if (i.Rujukan == "BAKI AWAL")
                {
                    continue;
                }

                hutangAwal = hutangAwal + i.Hutang;
                bayaranAwal = bayaranAwal + i.Bayaran;
                bakiAwal = bakiAwal + i.Bayaran - i.Hutang;
                i.Baki = bakiAwal;
            }

            dynamic dyModel = new ExpandoObject();
            dyModel.AkPembekal = akPembekal;
            dyModel.Sublejar = vmodel;
            dyModel.JumBayaran = bayaranAwal;
            dyModel.JumHutang = hutangAwal;

            return View(dyModel);

        }

        [Authorize(Policy = "DF002C")]
        // GET: AkPembekal/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        [Authorize(Policy = "DF002C")]
        // POST: AkPembekal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPembekal akPembekal, int jNegeriId, int jBankId)
        {
            AkPembekal akP = new();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

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
                    akP.NamaSykt = akPembekal.NamaSykt?.ToUpper()?? "";
                    akP.NoPendaftaran = akPembekal.NoPendaftaran;
                    akP.Poskod = akPembekal.Poskod;
                    akP.Telefon1 = akPembekal.Telefon1;
                    akP.AkaunBank = akPembekal.AkaunBank;
                    akP.Alamat1 = akPembekal.Alamat1?.ToUpper()?? "";
                    akP.Alamat2 = akPembekal.Alamat2?.ToUpper()?? "";
                    akP.Alamat3 = akPembekal.Alamat3?.ToUpper()?? "";
                    akP.Bandar = akPembekal.Bandar?.ToUpper()?? "";
                    akP.Emel = akPembekal.Emel;
                    akP.BakiAwal = akPembekal.BakiAwal;
                    akP.UserId = user.UserName;
                    akP.TarMasuk = DateTime.Now;
                    akP.SuPekerjaMasukId = pekerjaId;

                    await _akpembekalRepo.Insert(akP);
                    //insert applog
                    await AddLogAsync("Tambah", akP.KodSykt + " - " + akP.NamaSykt?.ToUpper()?? "", akP.KodSykt, 0, 0, pekerjaId);
                    //insert applog end
                    await _akpembekalRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Pembekal adalah " + akP.KodSykt;

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();

            return View(akP);
        }

        [Authorize(Policy = "DF002E")]
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

        [Authorize(Policy = "DF002E")]
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
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    AkPembekal dataAsal = await _akpembekalRepo.GetById(id);
                    akPembekal.KodSykt = dataAsal.KodSykt;
                    var namaAsal = dataAsal.NamaSykt;
                    akPembekal.UserId = dataAsal.UserId;
                    akPembekal.TarMasuk = dataAsal.TarMasuk;
                    akPembekal.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akPembekal.NamaSykt = akPembekal.NamaSykt?.ToUpper()?? "";
                    akPembekal.Alamat1 = akPembekal.Alamat1?.ToUpper()?? "";
                    akPembekal.Alamat2 = akPembekal.Alamat2?.ToUpper()?? "";
                    akPembekal.Alamat3 = akPembekal.Alamat3?.ToUpper()?? "";
                    akPembekal.Bandar = akPembekal.Bandar?.ToUpper()?? "";
                    akPembekal.UserIdKemaskini = user.UserName;
                    akPembekal.TarKemaskini = DateTime.Now;
                    akPembekal.SuPekerjaKemaskiniId = pekerjaId;
                    
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
                        await AddLogAsync("Ubah", namaAsal + " -> " + akPembekal.NamaSykt, akPembekal.KodSykt, id, 0, pekerjaId);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", akPembekal.KodSykt, id, 0, pekerjaId);
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

        [Authorize(Policy = "DF002D")]
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

        [Authorize(Policy = "DF002D")]
        // POST: AkPembekal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPembekal = await _context.AkPembekal.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            akPembekal.UserIdKemaskini = user.UserName;
            akPembekal.TarKemaskini = DateTime.Now;
            akPembekal.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkPembekal.Remove(akPembekal);
            await AddLogAsync("Hapus", akPembekal.KodSykt + " - " + akPembekal.NamaSykt, akPembekal.KodSykt, id, 0, pekerjaId);

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

        [Authorize(Policy = "DF002R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akpembekalRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Batal operation

            obj.FlHapus = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkPembekal.Update(obj);

            await AddLogAsync("Rollback", obj.KodSykt + " - " + obj.NamaSykt, obj.KodSykt, id, 0, pekerjaId);
            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
    }
}
