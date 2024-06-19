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
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class SuAtletController : Controller
    {
        public const string modul = "DF005";
        public const string namamodul = "Daftar Atlet";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<SuAtlet, int, string> _suAtletRepo;
        private readonly IRepository<JNegeri, int, string> _jNegeriRepo;
        private readonly IRepository<JAgama, int, string> _jAgamaRepo;
        private readonly IRepository<JBangsa, int, string> _jBangsaRepo;
        private readonly IRepository<JSukan, int, string> _jSukanRepo;
        private readonly IRepository<JCaraBayar, int, string> _jCaraBayarRepo;
        private CartAtlet _cart;

        public SuAtletController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<SuAtlet, int, string> suAtletRepo,
            IRepository<JNegeri, int, string> jNegeriRepo,
            IRepository<JAgama, int, string> jAgamaRepo,
            IRepository<JBangsa, int, string> jBangsaRepo,
            IRepository<JSukan, int, string> jSukanRepo,
            IRepository<JCaraBayar, int, string> jCaraBayarRepo,
            CartAtlet cart

            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _suAtletRepo = suAtletRepo;
            _jNegeriRepo = jNegeriRepo;
            _jAgamaRepo = jAgamaRepo;
            _jBangsaRepo = jBangsaRepo;
            _jSukanRepo = jSukanRepo;
            _jCaraBayarRepo = jCaraBayarRepo;
            _cart = cart;

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
            List<JNegeri> JNegeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = JNegeriList;

            List<JBank> JBankList = _context.JBank.OrderBy(b => b.Kod).ToList();
            ViewBag.JBank = JBankList;

            List<JAgama> JAgamaList = _context.JAgama.OrderBy(b => b.Perihal).ToList();
            ViewBag.JAgama = JAgamaList;

            List<JBangsa> JBangsaList = _context.JBangsa.OrderBy(b => b.Perihal).ToList();
            ViewBag.JBangsa = JBangsaList;

            List<JSukan> JSukanList = _context.JSukan.OrderBy(b => b.Kod).ToList();
            ViewBag.JSukan = JSukanList;

            List<JCaraBayar> JCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = JCaraBayarList;
        }

        private string GetKodAtlet()
        {
            var suP = _suAtletRepo.GetAllIncludeDeletedItems()
                .Result
                .OrderByDescending(s => s.KodAtlet).FirstOrDefault();
            int no = 0;
            if (suP != null)
            {
                if (int.TryParse(suP.KodAtlet, out no))
                {
                    no += 1;
                }
            }
            else
            {
                no = 1;
            }
            return no.ToString("D5");
        }

        //Function Cart Empty
        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.suAtlet1 = new List<int>();
                //ViewBag.spPendahuluanPelbagai2 = new List<int>();
                _cart.Clear1();
                //_cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Function Cart Empty end

        [Authorize(Policy = "DF005")]
        // GET: SuAtlet
        public async Task<IActionResult> Index()
        {
            var suAtlet = await _suAtletRepo.GetAll(null);

            if (User.IsInRole("SuperAdmin"))
            {
                suAtlet = await _suAtletRepo.GetAllIncludeDeletedItems();
            }
            return View(suAtlet);
        }

        // GET: SuAtlet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            PopulateList();
            //PopulateTable(id);
            return View(suAtlet);
        }

        [Authorize(Policy = "DF005C")]
        // GET: SuAtlet/Create
        public IActionResult Create()
        {
            ViewBag.KodAtlet = GetKodAtlet();
            PopulateList();
            //CartEmpty();
            return View();
        }

        [Authorize(Policy = "DF005C")]
        // POST: SuAtlet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuAtlet suAtlet)
        {
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            SuAtlet m = new SuAtlet();
            if (ICAtletExists(suAtlet.NoKp) == false)
            {
                if (AkaunAtletExists(suAtlet.NoAkaunBank) == false)
                {
                    if (ModelState.IsValid)
                    {
                        //string noRujukan = GetKod(akJurnal.JKWId);
                        if (suAtlet != null)
                        {
                            m.KodAtlet = GetKodAtlet();
                            m.Nama = suAtlet.Nama;
                            m.NoKp = suAtlet.NoKp;
                            m.Alamat1 = suAtlet.Alamat1;
                            m.Alamat2 = suAtlet.Alamat2;
                            m.Alamat3 = suAtlet.Alamat3;
                            m.Poskod = suAtlet.Poskod;
                            m.Bandar = suAtlet.Bandar;
                            m.JNegeriId = suAtlet.JNegeriId;
                            m.JBankId = suAtlet.JBankId;
                            m.Jawatan = suAtlet.Jawatan;
                            m.JSukanId = suAtlet.JSukanId;
                            m.Telefon = suAtlet.Telefon;
                            m.Emel = suAtlet.Emel;
                            m.FlStatus = 1;
                            m.TarikhAktif = suAtlet.TarikhAktif;
                            m.TarikhBerhenti = suAtlet.TarikhBerhenti;
                            //m.FlStatus = suAtlet.FlStatus;
                            m.JAgamaId = suAtlet.JAgamaId;
                            m.JBangsaId = suAtlet.JBangsaId;
                            m.JCaraBayarId = suAtlet.JCaraBayarId;
                            m.NoAkaunBank = suAtlet.NoAkaunBank;
                            m.UserId = username;
                            m.TarMasuk = DateTime.Now;
                            m.SuPekerjaMasukId = pekerjaId;

                            //m.SuTanggungan = _cart.Lines1.ToArray();

                            await _suAtletRepo.Insert(m);

                            //insert applog
                            await AddLogAsync("Tambah", m.KodAtlet + " - " + suAtlet.NoKp, m.KodAtlet, 0, 0, pekerjaId);
                            //insert applog end

                            //await AddLogAsync("Tambah", noRujukan, kredit);
                            await _context.SaveChangesAsync();

                            //CartEmpty();
                            TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Atlet adalah " + m.KodAtlet;
                            return RedirectToAction(nameof(Index));
                        }
                        //_context.Add(suAtlet);
                        //await _context.SaveChangesAsync();
                        //return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        TempData[SD.Error] = "Emel ini telah wujud..!";
                    }

                }
                else
                {
                    TempData[SD.Error] = "No Akaun ini telah wujud..!";
                }

            }
            else
            {
                TempData[SD.Error] = "No Kad Pengenalan ini telah wujud..!";
            }

            ViewBag.KodAtlet = GetKodAtlet();
            PopulateList();
            return View(suAtlet);
        }

        [Authorize(Policy = "DF005E")]
        // GET: SuAtlet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            //CartEmpty();
            PopulateList();
            //PopulateTable(id);
            //PopulateCart(suAtlet);
            return View(suAtlet);
        }

        [Authorize(Policy = "DF005E")]
        // POST: SuAtlet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SuAtlet suAtlet)
        {
            if (id != suAtlet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    SuAtlet dataAsal = await _suAtletRepo.GetById(id);

                    // list of input that cannot be change
                    //suAtlet.Emel = dataAsal.Emel
                    suAtlet.TarMasuk = dataAsal.TarMasuk;
                    suAtlet.UserId = dataAsal.UserId;
                    suAtlet.NoKp = dataAsal.NoKp;
                    suAtlet.KodAtlet = dataAsal.KodAtlet;
                    suAtlet.FlStatus = dataAsal.FlStatus;
                    var noAkaunAsal = dataAsal.NoAkaunBank;
                    var namaAsal = dataAsal.Nama;
                    suAtlet.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    suAtlet.UserIdKemaskini = user.UserName;
                    suAtlet.TarKemaskini = DateTime.Now;
                    suAtlet.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(suAtlet);

                    //insert applog
                    if (namaAsal != suAtlet.Nama || noAkaunAsal != suAtlet.NoAkaunBank)
                    {
                        await AddLogAsync("Ubah", namaAsal + " -> " + suAtlet.Nama
                            + ", " + noAkaunAsal + " -> " + suAtlet.NoAkaunBank, suAtlet.KodAtlet, id, 0, pekerjaId);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", suAtlet.KodAtlet, id, 0, pekerjaId);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuAtletExists(suAtlet.Id))
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
            return View(suAtlet);
        }

        [Authorize(Policy = "DF005D")]
        // GET: SuAtlet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suAtlet = await _suAtletRepo.GetById((int)id);
            //PopulateTable(id);
            if (suAtlet == null)
            {
                return NotFound();
            }

            return View(suAtlet);
        }

        [Authorize(Policy = "DF005D")]
        // POST: SuAtlet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var suAtlet = await _context.SuAtlet.FindAsync(id);
            suAtlet.UserIdKemaskini = user.UserName;
            suAtlet.TarKemaskini = DateTime.Now;
            suAtlet.SuPekerjaKemaskiniId = pekerjaId;

            _context.SuAtlet.Remove(suAtlet);
            await AddLogAsync("Hapus", suAtlet.NoKp + " - " + suAtlet.NoAkaunBank, suAtlet.KodAtlet, id, 0, pekerjaId);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));

        }

        private bool SuAtletExists(int id)
        {
            return _context.SuAtlet.Any(e => e.Id == id);
        }

        [Authorize(Policy = "DF005R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var obj = await _suAtletRepo.GetByIdIncludeDeletedItems(id);
            // Batal operation

            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            obj.FlHapus = 0;
            _context.SuAtlet.Update(obj);

            // Batal operation end

            await AddLogAsync("Hapus", obj.NoKp + " - " + obj.NoAkaunBank, obj.KodAtlet, id, 0, pekerjaId);

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool ICAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.NoKp == kod && e.FlHapus == 0);
        }

        private bool AkaunAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.NoAkaunBank == kod && e.FlHapus == 0);
        }

        private bool EmelAtletExists(string kod)
        {
            return _context.SuAtlet.Any(e => e.Emel == kod && e.FlHapus == 0);
        }

    }
}
