using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class SuPekerjaController : Controller
    {
        //public const string modul = "JU001";
        //public const string namamodul = "Daftar Anggota";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<SuPekerja, int> _suPekerjaRepo;
        private readonly IRepository<JNegeri, int> _jNegeriRepo;
        private readonly IRepository<JAgama, int> _jAgamaRepo;
        private readonly IRepository<JBangsa, int> _jBangsaRepo;
        private readonly IRepository<JJawatanPekerja, int> _jJawatanPekerjaRepo;
        private readonly ListViewIRepository<SuTanggunganPekerja, int> _suTanggunganRepo;
        private readonly IRepository<JCaraBayar, int> _jCaraBayarRepo;
        private CartPekerja _cart;

        public SuPekerjaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<SuPekerja, int> suPekerjaRepo,
            IRepository<JNegeri, int> jNegeriRepo,
            IRepository<JAgama, int> jAgamaRepo,
            IRepository<JBangsa, int> jBangsaRepo,
            IRepository<JJawatanPekerja, int> jJawatanPekerjaRepo,
            ListViewIRepository<SuTanggunganPekerja, int> suTanggunganRepo,
            IRepository<JCaraBayar, int> jCaraBayarRepo,
            CartPekerja cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _suPekerjaRepo = suPekerjaRepo;
            _jNegeriRepo = jNegeriRepo;
            _jAgamaRepo = jAgamaRepo;
            _jBangsaRepo = jBangsaRepo;
            _jJawatanPekerjaRepo = jJawatanPekerjaRepo;
            _suTanggunganRepo = suTanggunganRepo;
            _jCaraBayarRepo = jCaraBayarRepo;
            _cart = cart;
        }

        private void PopulateList()
        {
            List<JNegeri> JNegeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = JNegeriList;

            List<JAgama> JAgamaList = _context.JAgama.OrderBy(b => b.Perihal).ToList();
            ViewBag.JAgama = JAgamaList;

            List<JBangsa> JBangsaList = _context.JBangsa.OrderBy(b => b.Perihal).ToList();
            ViewBag.JBangsa = JBangsaList;

            List<JCaraBayar> JCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = JCaraBayarList;

            List<JJawatanPekerja> JJawatanPekerjaList = _context.JJawatanPekerja.OrderBy(b => b.Kod).ToList();
            ViewBag.JJawatanPekerja = JJawatanPekerjaList;

        }

        private string GetNoGaji()
        {
            var suP = _suPekerjaRepo.GetAll()
                .Result
                .OrderByDescending(s => s.NoGaji).FirstOrDefault();
            int no = 0;
            if (suP != null)
            {
                if (int.TryParse(suP.NoGaji, out no))
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

        private void PopulateTable(int? id)
        {
            List<SuTanggunganPekerja> suTanggungan = _context.SuTanggunganPekerja.Where(b => b.SuPekerjaId == id).ToList();
            ViewBag.suTanggungan = suTanggungan;
        }

        private JsonResult CartEmpty()
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
        private void PopulateCart(SuPekerja suPekerja)
        {
            List<SuTanggunganPekerja> suTanggungan = _context.SuTanggunganPekerja
                .Where(b => b.SuPekerjaId == suPekerja.Id)
                .ToList();
            foreach (SuTanggunganPekerja suT in suTanggungan)
            {
                _cart.AddItem1(
                    suT.SuPekerjaId,
                    suT.Nama,
                    suT.Hubungan,
                    suT.NoKP
                    );
            }
        }

        // GET: SuPekerja
        public async Task<IActionResult> Index()
        {
            var suPekerja = await _suPekerjaRepo.GetAll();
            return View(suPekerja);
        }

        // GET: SuPekerja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _suPekerjaRepo.GetById((int)id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            return View(suPekerja);
        }

        // GET: SuPekerja/Create
        public IActionResult Create()
        {
            ViewBag.nogaji = GetNoGaji();
            PopulateList();
            CartEmpty();
            return View();
        }

        // POST: SuPekerja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuPekerja suPekerja)
        {
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);
            SuPekerja m = new SuPekerja();
            if (ModelState.IsValid)
            {
                //string noRujukan = GetKod(akJurnal.JKWId);
                if (suPekerja != null)
                {
                    m.NoGaji = GetNoGaji();
                    m.Nama = suPekerja.Nama;
                    //m.Alamat1 = suPekerja.Alamat1;
                    //m.Alamat2 = suPekerja.Alamat2;
                    //m.Alamat3 = suPekerja.Alamat3;
                    //m.Poskod = suPekerja.Poskod;
                    //m.Bandar = suPekerja.Bandar;
                    m.JNegeriId = suPekerja.JNegeriId;
                    //m.TelefonRumah = suPekerja.TelefonRumah;
                    //m.TelefonBimbit = suPekerja.TelefonBimbit;
                    //m.Emel = suPekerja.Emel;
                    //m.StatusKahwin = suPekerja.StatusKahwin;
                    //m.BilAnak = suPekerja.BilAnak;
                    //m.GajiPokok = suPekerja.GajiPokok;
                    //m.TarikhMasukKerja = suPekerja.TarikhMasukKerja;
                    //m.TarikhBerhentiKerja = suPekerja.TarikhBerhentiKerja;
                    //m.TarikhPencen = suPekerja.TarikhPencen;
                    //m.JAgamaId = suPekerja.JAgamaId;
                    //m.JBangsaId = suPekerja.JBangsaId;
                    //m.JJawatanPekerjaId = suPekerja.JJawatanPekerjaId;
                    //m.JCaraBayarId = suPekerja.JCaraBayarId;
                    m.NoAkaunBank = suPekerja.NoAkaunBank;
                    m.UserId = username;
                    m.TarMasuk = DateTime.Now;
                    m.SuTanggungan = _cart.Lines1.ToArray();

                    await _suPekerjaRepo.Insert(m);
                    //await AddLogAsync("Tambah", noRujukan, kredit);
                    await _context.SaveChangesAsync();

                    CartEmpty();
                    //TempData[SD.Success] = "Maklumat berjaya ditambah. No Gaji adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
                //_context.Add(suPekerja);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            PopulateList();
            return View(suPekerja);
        }

        // GET: SuPekerja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _suPekerjaRepo.GetById((int)id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(suPekerja);
            return View(suPekerja);
        }

        // POST: SuPekerja/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SuPekerja suPekerja)
        {
            if (id != suPekerja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suPekerja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuPekerjaExists(suPekerja.Id))
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
            PopulateList();
            return View(suPekerja);
        }

        // GET: SuPekerja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suPekerja = await _context.SuPekerja
                .Include(s => s.JAgama)
                .Include(s => s.JBangsa)
                .Include(s => s.JCaraBayar)
                .Include(s => s.JJawatanPekerja)
                .Include(s => s.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
            PopulateTable(id);
            if (suPekerja == null)
            {
                return NotFound();
            }

            return View(suPekerja);
        }

        // POST: SuPekerja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suPekerja = await _context.SuPekerja.FindAsync(id);
            _context.SuPekerja.Remove(suPekerja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuPekerjaExists(int id)
        {
            return _context.SuPekerja.Any(e => e.Id == id);
        }

        public JsonResult SaveTanggungan(SuTanggunganPekerja tanggungan)
        {
            try
            {
                if (tanggungan != null)
                {
                    _cart.AddItem1(
                        tanggungan.SuPekerjaId,
                        tanggungan.Nama,
                        tanggungan.Hubungan,
                        tanggungan.NoKP
                        );
                }
                return Json(new { result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public JsonResult RemoveTanggungan(SuTanggunganPekerja tanggungan)
        {
            try
            {
                if (tanggungan != null)
                {
                    _cart.RemoveItem1(tanggungan.NoKP);
                }
                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }


    }
}
