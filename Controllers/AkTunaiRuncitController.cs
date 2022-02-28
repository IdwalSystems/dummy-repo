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
using MSNK.Models.Modules.ViewModel;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkTunaiRuncitController : Controller
    {
        public const string modul = "TR001";
        public const string namamodul = "Pemegang Tunai Runcit";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private CartTunaiRuncit _cart;

        public AkTunaiRuncitController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            CartTunaiRuncit cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
            _cart = cart;
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

        // GET: AkTunaiRuncit
        [Authorize(Policy = "TR001")]
        public async Task<IActionResult> Index()
        {
            var akTunaiRuncit = await _akTunaiRuncitRepo.GetAll();

            List<AkTunaiRuncitViewModel> viewModel = new List<AkTunaiRuncitViewModel>();

            foreach (AkTunaiRuncit item in akTunaiRuncit)
            {
                AkTunaiLejar akTunaiLejar = _context.AkTunaiLejar
                    .Where(x => x.AkTunaiRuncitId == item.Id)
                    .OrderByDescending(x => x.NoRujukan)
                    .ThenByDescending(x=> x.Tarikh)
                    .ThenByDescending(x=> x.Id)
                    .FirstOrDefault();

                decimal baki = 0;

                if (akTunaiLejar != null)
                {
                    baki = akTunaiLejar.Baki;
                }

                viewModel.Add(new AkTunaiRuncitViewModel
                {
                    Id = item.Id,
                    KodKW = item.JKW.Kod,
                    KodRujukan = item.KaunterPanjar,
                    KodAkaun = item.AkCarta.Kod,
                    Perihal = item.AkCarta.Perihal,
                    BakiLejarPanjar = baki
                });
            }
            return View(viewModel);
        }

        // GET: AkTunaiRuncit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);

            if (akTunaiRuncit == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);

            return View(akTunaiRuncit);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;


        }

        private void PopulateTable(int? id)
        {
            List<AkTunaiPemegang> akTunaiPemegangTable = _context.AkTunaiPemegang
                .Include(b => b.SuPekerja)
                .Where(b => b.AkTunaiRuncitId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTunaiPemegang = akTunaiPemegangTable;

            List<AkTunaiLejar> akTunaiLejarTable = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b=> b.AkTunaiRuncit.Id == id)
                .OrderBy(b=> b.Tarikh)
                .ToList();

            ViewBag.akTunaiLejar = akTunaiLejarTable;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akTunaiPemegang = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // GET: AkTunaiRuncit/Create
        [Authorize(Policy = "PR001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Kod == "100");

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.AkTunaiRuncit
                        .Where(x => x.JKW.Kod == kw.Kod)
                        .Max(x => x.KaunterPanjar);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;
            PopulateList();
            CartEmpty();
            return View();
        }

        // function json get no rujukan (running number)
        [HttpPost]
        public JsonResult JsonGetKod(int data)
        {
            try
            {
                var result = "";
                if (data == 0)
                {
                    result = "";
                }
                else
                {
                    // get latest no rujukan running number  
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw.Kod;
                    string prefix = kumpulanWang;
                    int x = 1;
                    string noRujukan = prefix + "00";

                    var LatestNoRujukan = _context.AkTunaiRuncit
                                .Where(x => x.JKW.Kod == kw.Kod)
                                .Max(x => x.KaunterPanjar);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(3));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "00}", x);
                    }

                    result = noRujukan;

                    // get latest no rujukan running number end
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // function json get no rujukan (running number) end

        public JsonResult GetSuPekerja(SuPekerja suPekerja)
        {
            try
            {
                var result = _context.SuPekerja.Where(b => b.Id == suPekerja.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult SaveAkTunaiPemegang(AkTunaiPemegang akTunaiPemegang)
        {

            try
            {
                if (akTunaiPemegang != null)
                {
                    _cart.AddItem1(akTunaiPemegang.AkTunaiRuncitId,
                                   akTunaiPemegang.SuPekerjaId);
                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTunaiPemegang(AkTunaiPemegang akTunaiPemegang)
        {

            try
            {
                if (akTunaiPemegang != null)
                {

                    _cart.RemoveItem1(akTunaiPemegang.SuPekerjaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkTunaiRuncit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkTunaiRuncit akTunaiRuncit, int JKWId, int AkCartaId, decimal Baki, DateTime? TarikhBaki)
        {
            AkTunaiRuncit m = new AkTunaiRuncit();
            var user = await _userManager.GetUserAsync(User);

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == JKWId);

            var kumpulanWang = kw.Kod;
            string prefix = kumpulanWang;
            int x = 1;
            string noRujukan = prefix + "00";

            var LatestNoRujukan = _context.AkTunaiRuncit
                        .Where(x => x.JKW.Kod == kw.Kod)
                        .Max(x => x.KaunterPanjar);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(3));
                x++;
                noRujukan = string.Format("{0:" + prefix + "00}", x);
            }

            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akTunaiRuncit != null && JKWId != 0 && AkCartaId != 0)
                {
                    m.JKWId = JKWId;
                    m.AkCartaId = AkCartaId;
                    m.KaunterPanjar = noRujukan;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkTunaiPemegang = _cart.Lines1.ToArray();

                    await _akTunaiRuncitRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.KaunterPanjar, m.KaunterPanjar, 0,0);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan kaunter panjar adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            return View(akTunaiRuncit);
        }

        // GET: AkTunaiRuncit/Edit/5
        [Authorize(Policy = "TR001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);

            if (akTunaiRuncit == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTunaiRuncit);

            return View(akTunaiRuncit);
        }

        private void PopulateCartFromDb(AkTunaiRuncit akTunaiRuncit)
        {
            List<AkTunaiPemegang> akTunaiPemegangTable = _context.AkTunaiPemegang
                .Include(b => b.SuPekerja)
                .Where(b => b.AkTunaiRuncitId == akTunaiRuncit.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTunaiPemegang akTunaiPemegang in akTunaiPemegangTable)
            {
                _cart.AddItem1(akTunaiPemegang.AkTunaiRuncitId,
                               akTunaiPemegang.SuPekerjaId);
            }

            ViewBag.akTunaiPemegang = akTunaiPemegangTable;

        }

        // POST: AkTunaiRuncit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TR001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkTunaiRuncit akTunaiRuncit, int JKWId, int AkCartaId, decimal Baki, DateTime? TarikhBaki)
        {
            if (id != akTunaiRuncit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    AkTunaiRuncit akTunaiRuncitAsal = await _akTunaiRuncitRepo.GetById(id);

                    // list of input that cannot be change
                    akTunaiRuncit.JKWId = akTunaiRuncitAsal.JKWId;
                    akTunaiRuncit.KaunterPanjar = akTunaiRuncitAsal.KaunterPanjar;
                    akTunaiRuncit.AkCartaId = akTunaiRuncitAsal.AkCartaId;
                    akTunaiRuncit.TarMasuk = akTunaiRuncitAsal.TarMasuk;
                    akTunaiRuncit.UserId = akTunaiRuncitAsal.UserId;
                    // list of input that cannot be change end

                    foreach (AkTunaiPemegang item in akTunaiRuncitAsal.AkTunaiPemegang)
                    {
                        var model = _context.AkTunaiPemegang.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }
                    _context.Entry(akTunaiRuncitAsal).State = EntityState.Detached;

                    akTunaiRuncit.AkTunaiPemegang = _cart.Lines1.ToList();

                    akTunaiRuncit.UserIdKemaskini = user.UserName;
                    akTunaiRuncit.TarKemaskini = DateTime.Now;

                    _context.Update(akTunaiRuncit);

                    //insert applog
                    await AddLogAsync("Ubah", "Ubah Data", akTunaiRuncit.KaunterPanjar, id, 0);
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiRuncitExists(akTunaiRuncit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                CartEmpty();

                TempData[SD.Success] = "Data berjaya diubah..!";

                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi.";
            PopulateList();
            PopulateTable(id);
            return View(akTunaiRuncit);
        }

        // GET: AkTunaiRuncit/Delete/5
        [Authorize(Policy ="TR001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);

            if (akTunaiRuncit == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);

            return View(akTunaiRuncit);
        }

        // POST: AkTunaiRuncit/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TR001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiRuncit = await _context.AkTunaiRuncit.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            akTunaiRuncit.UserIdKemaskini = user.UserName;
            akTunaiRuncit.TarKemaskini = DateTime.Now;

            _context.AkTunaiRuncit.Remove(akTunaiRuncit);

            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", akTunaiRuncit.KaunterPanjar, id, 0);
            //insert applog end


            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiRuncitExists(int id)
        {
            return _context.AkTunaiRuncit.Any(e => e.Id == id);
        }
    }
}
