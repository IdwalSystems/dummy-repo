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
    [Authorize(Roles = "SuperAdmin,Supervisorm,User")]
    public class SuJurulatihController : Controller
    {
        public const string modul = "FL004";
        public const string namamodul = "Jurulatih";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<SuJurulatih, int, string> _suJurulatihRepo;
        private readonly IRepository<JNegeri, int, string> _jNegeriRepo;
        private readonly IRepository<JAgama, int, string> _jAgamaRepo;
        private readonly IRepository<JBangsa, int, string> _jBangsaRepo;
        private readonly IRepository<JSukan, int, string> _jSukanRepo;
        private readonly IRepository<JCaraBayar, int, string> _jCaraBayarRepo;

        public SuJurulatihController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<SuJurulatih, int, string> suJurulatihRepo,
            IRepository<JNegeri, int, string> jNegeriRepo,
            IRepository<JAgama, int, string> jAgamaRepo,
            IRepository<JBangsa, int, string> jBangsaRepo,
            IRepository<JSukan, int, string> jSukanRepo,
            IRepository<JCaraBayar, int, string> jCaraBayarRepo

            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _suJurulatihRepo = suJurulatihRepo;
            _jNegeriRepo = jNegeriRepo;
            _jAgamaRepo = jAgamaRepo;
            _jBangsaRepo = jBangsaRepo;
            _jSukanRepo = jSukanRepo;
            _jCaraBayarRepo = jCaraBayarRepo;

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

            List<JProfilKategori> JProfilKategoriList = _context.JProfilKategori.OrderBy(b => b.Kod).ToList();
            ViewBag.JProfilKategori = JProfilKategoriList;
        }

        private string GetKodJurulatih()
        {
            var suP = _suJurulatihRepo.GetAllIncludeDeletedItems()
                .Result
                .OrderByDescending(s => s.KodJurulatih).FirstOrDefault();
            int no = 0;
            if (suP != null)
            {
                if (int.TryParse(suP.KodJurulatih, out no))
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

        // GET: SuJurulatih
        public async Task<IActionResult> Index()
        {
            var suJurulatih = await _suJurulatihRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                suJurulatih = await _suJurulatihRepo.GetAllIncludeDeletedItems();
            }
            return View(suJurulatih);
        }

        // GET: SuJurulatih/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suJurulatih = await _suJurulatihRepo.GetById((int)id);
            if (suJurulatih == null)
            {
                return NotFound();
            }

            PopulateList();
            //PopulateTable(id);
            return View(suJurulatih);
        }

        // GET: SuJurulatih/Create
        public IActionResult Create()
        {
            ViewBag.KodJurulatih = GetKodJurulatih();
            PopulateList();
            //CartEmpty();
            return View();
        }

        // POST: SuJurulatih/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuJurulatih suJurulatih)
        {
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            var user = await _userManager.GetUserAsync(User);

            SuJurulatih m = new SuJurulatih();
            if (ICJurulatihExists(suJurulatih.NoKp) == false)
            {
                if (AkaunJurulatihExists(suJurulatih.NoAkaunBank) == false)
                {
                    if (ModelState.IsValid)
                    {
                        //string noRujukan = GetKod(akJurnal.JKWId);
                        if (suJurulatih != null)
                        {
                            m.KodJurulatih = GetKodJurulatih();
                            m.Nama = suJurulatih.Nama;
                            m.NoKp = suJurulatih.NoKp;
                            m.IsJSMBakat = suJurulatih.IsJSMBakat;
                            m.IsJSMPelapis = suJurulatih.IsJSMPelapis;
                            m.IsSukma = suJurulatih.IsSukma;
                            m.Alamat1 = suJurulatih.Alamat1;
                            m.Alamat2 = suJurulatih.Alamat2;
                            m.Alamat3 = suJurulatih.Alamat3;
                            m.Poskod = suJurulatih.Poskod;
                            m.Bandar = suJurulatih.Bandar;
                            m.JNegeriId = suJurulatih.JNegeriId;
                            m.JBankId = suJurulatih.JBankId;
                            m.Jawatan = suJurulatih.Jawatan;
                            m.JSukanId = suJurulatih.JSukanId;
                            m.Telefon = suJurulatih.Telefon;
                            m.Emel = suJurulatih.Emel;
                            m.TarikhAktif = suJurulatih.TarikhAktif;
                            m.TarikhBerhenti = suJurulatih.TarikhBerhenti;
                            m.FlStatus = 1;
                            //m.FlStatus = suJurulatih.FlStatus;
                            m.JAgamaId = suJurulatih.JAgamaId;
                            m.JBangsaId = suJurulatih.JBangsaId;
                            m.JCaraBayarId = suJurulatih.JCaraBayarId;
                            m.NoAkaunBank = suJurulatih.NoAkaunBank;
                            m.UserId = username;
                            m.TarMasuk = DateTime.Now;

                            //m.SuTanggungan = _cart.Lines1.ToArray();

                            await _suJurulatihRepo.Insert(m);

                            //insert applog
                            await AddLogAsync("Tambah", m.KodJurulatih + " - " + suJurulatih.NoKp, m.KodJurulatih, 0, 0);
                            //insert applog end

                            //await AddLogAsync("Tambah", noRujukan, kredit);
                            await _context.SaveChangesAsync();

                            //CartEmpty();
                            TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Jurulatih adalah " + m.KodJurulatih;
                            return RedirectToAction(nameof(Index));
                        }
                        //_context.Add(suJurulatih);
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

            ViewBag.KodJurulatih = GetKodJurulatih();
            PopulateList();
            return View(suJurulatih);
        }

        // GET: SuJurulatih/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suJurulatih = await _suJurulatihRepo.GetById((int)id);
            if (suJurulatih == null)
            {
                return NotFound();
            }

            //CartEmpty();
            PopulateList();
            //PopulateTable(id);
            //PopulateCart(suJurulatih);
            return View(suJurulatih);
        }

        // POST: SuJurulatih/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SuJurulatih suJurulatih)
        {
            if (id != suJurulatih.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    SuJurulatih dataAsal = await _suJurulatihRepo.GetById(id);

                    // list of input that cannot be change
                    //suJurulatih.Emel = dataAsal.Emel;
                    suJurulatih.TarMasuk = dataAsal.TarMasuk;
                    suJurulatih.UserId = dataAsal.UserId;
                    suJurulatih.NoKp = dataAsal.NoKp;
                    suJurulatih.KodJurulatih = dataAsal.KodJurulatih;
                    var noAkaunAsal = dataAsal.NoAkaunBank;
                    var namaAsal = dataAsal.Nama;
                    // list of input that cannot be change end

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    suJurulatih.UserIdKemaskini = user.UserName;
                    suJurulatih.TarKemaskini = DateTime.Now;

                    _context.Update(suJurulatih);

                    //insert applog
                    if (namaAsal != suJurulatih.Nama || noAkaunAsal != suJurulatih.NoAkaunBank)
                    {
                        await AddLogAsync("Ubah", namaAsal + " -> " + suJurulatih.Nama
                            + ", " + noAkaunAsal + " -> " + suJurulatih.NoAkaunBank, suJurulatih.KodJurulatih, id, 0);
                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", suJurulatih.KodJurulatih, id, 0);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuJurulatihExists(suJurulatih.Id))
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
            return View(suJurulatih);
        }

        // GET: SuJurulatih/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suJurulatih = await _suJurulatihRepo.GetById((int)id);
            //PopulateTable(id);
            if (suJurulatih == null)
            {
                return NotFound();
            }

            return View(suJurulatih);
        }

        // POST: SuJurulatih/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suJurulatih = await _context.SuJurulatih.FindAsync(id);
            _context.SuJurulatih.Remove(suJurulatih);
            await AddLogAsync("Hapus", suJurulatih.NoKp + " - " + suJurulatih.NoAkaunBank, suJurulatih.KodJurulatih, id, 0);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));

        }

        private bool SuJurulatihExists(int id)
        {
            return _context.SuJurulatih.Any(e => e.Id == id);
        }

        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _suJurulatihRepo.GetByIdIncludeDeletedItems(id);
            // Batal operation

            obj.FlHapus = 0;
            _context.SuJurulatih.Update(obj);

            // Batal operation end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool ICJurulatihExists(string kod)
        {
            return _context.SuJurulatih.Any(e => e.NoKp == kod);
        }

        private bool AkaunJurulatihExists(string kod)
        {
            return _context.SuJurulatih.Any(e => e.NoAkaunBank == kod);
        }

        private bool EmelJurulatihExists(string kod)
        {
            return _context.SuJurulatih.Any(e => e.Emel == kod);
        }

    }
}
