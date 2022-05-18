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
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class SuProfilAtletController : Controller
    {
        public const string modul = "SU001";
        public const string namamodul = "Profil Atlet";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<SuProfil, int, string> _suProfilRepo;
        private CartAtlet _cart;
        public SuProfilAtletController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<SuProfil, int, string> suProfilRepository,
            CartAtlet cart)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
            _suProfilRepo = suProfilRepository;
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

        // GET: SuProfilAtlet
        public async Task<IActionResult> Index()
        {
            var obj = await _suProfilRepo.GetAll();

            if (User.IsInRole("SuperAdmin"))
            {
                obj = await _suProfilRepo.GetAllIncludeDeletedItems();
            }

            return View(obj);
        }

        // GET: SuProfilAtlet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil
                .Include(s => s.AkCarta)
                .Include(s => s.JBahagian)
                .Include(s => s.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suProfil == null)
            {
                return NotFound();
            }

            return View(suProfil);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.Profil1 = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        private void PopulateTable(int status=1)
        {
            List<SuAtlet> data = _context.SuAtlet
                .Include(x => x.JSukan)
                .Where(b => b.FlStatus == status)
                .OrderBy(x => x.JSukanId).ThenBy(x => x.Nama)
                .ToList();

            List<SuProfil1> suProfil1Table = new List<SuProfil1>();

            foreach (var item in data)
            {
                suProfil1Table.Add(
                    new SuProfil1
                    {
                        SuAtlet = item,
                        SuAtletId = item.Id,
                        JSukan = item.JSukan,
                        JSukanId = item.JSukanId,
                        Amaun = 0,
                        AmaunSebelum = 0,
                        Tunggakan = 0,
                        Jumlah = 0
                    });
            }

            ViewBag.suProfil1 = suProfil1Table;
        }


        private void PopulateTableFromCart()
        {
            List<SuProfil1> suProfil1Table = _cart.Lines1
                .ToList();

            foreach (SuProfil1 item in suProfil1Table)
            {
                var suAtlet = _context.SuAtlet.Find(item.SuAtletId);

                item.SuAtlet = suAtlet;

                var jSukan = _context.JSukan.Find(item.JSukanId);

                item.JSukan = jSukan;
            }

            suProfil1Table = suProfil1Table.OrderBy(x => x.JSukanId)
                .ThenBy(x => x.SuAtlet.Nama).ToList();

            ViewBag.suProfil1 = suProfil1Table;
        }

        private void PopulateCartFromSuAtlet()
        {
            List<SuAtlet> suAtlet = _context.SuAtlet
                .Include(x => x.JSukan)
                .Where(b => b.FlStatus == 1)
                .OrderBy(x => x.JSukanId)
                .ThenBy(x => x.Nama)
                .ToList();

            foreach (SuAtlet item in suAtlet)
            {
                _cart.AddItem1(0,
                               item.Id,
                               item.JSukanId,
                               0,
                               0,
                               0,
                               0);
            }


        }

        // on change no PO controller
        [HttpPost]
        public JsonResult JsonGetKod(string year, string month, int bahagianId)
        {
            try
            {
                // get latest no rujukan running number 

                var bahagian = _context.JBahagian.Where(x => x.Id == bahagianId).FirstOrDefault();

                var kw = bahagian.JKWId;

                var result = bahagian.Kod + "/" + year + "/" + month;

                var IsExistNoRujukan = _context.SuProfil.Where(x => x.NoRujukan == result).FirstOrDefault();

                if (IsExistNoRujukan == null)
                {
                    return Json(new { result = "OK", record = result, kw = kw });
                }
                else
                {
                    return Json(new { result = "error" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // GET: SuProfilAtlet/Create
        [Authorize(Policy = "SU001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.ToString("MM");
            var bahagian = _context.JBahagian.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.NoRujukan = bahagian.Kod + "/" + year + "/" + month;

            PopulateList();
            CartEmpty();
            PopulateTable();
            PopulateCartFromSuAtlet();
            return View();

        }

        // get an item from cart abWaran1
        public JsonResult GetAnItemCartSuProfil1(SuProfil1 suProfil1)
        {

            try
            {
                SuProfil1 data = _cart.Lines1.
                    Where(x => x.SuAtletId == suProfil1.SuAtletId).FirstOrDefault();

                var suAtlet = _context.SuAtlet.FirstOrDefault(x => x.Id == data.SuAtletId);

                return Json(new { result = "OK", record = data, suAtlet = suAtlet });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart AbWaran1 end

        //save cart AbWaran1
        public JsonResult SaveCartSuProfil1(
            SuProfil1 suProfil1)
        {
            try
            {

                var suP1 = _cart.Lines1.FirstOrDefault(x => x.SuAtletId == suProfil1.SuAtletId);

                var jSukanId = suP1.JSukanId;

                if (suP1 != null)
                {
                    
                    _cart.RemoveItem1(suP1.SuAtletId);

                    _cart.AddItem1(0,
                        suProfil1.SuAtletId,
                        jSukanId,
                        suProfil1.Amaun,
                        suProfil1.AmaunSebelum,
                        suProfil1.Tunggakan,
                        suProfil1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPOLaras1 end

        // get all item from cart akPOLaras1
        public JsonResult GetAllItemCartSuProfil1()
        {

            try
            {
                List<SuProfil1> data = _cart.Lines1.ToList();

                foreach (SuProfil1 item in data)
                {
                    var suAtlet = _context.SuAtlet.Find(item.SuAtletId);

                    item.SuAtlet = suAtlet;

                    var jSukan = _context.JSukan.Find(item.JSukanId);

                    item.JSukan = jSukan;
                }

                data = data.OrderBy(x => x.JSukanId)
                    .ThenBy(x => x.SuAtlet.Nama).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPOLaras1 end

        // POST: SuProfilAtlet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "SU001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SuProfil suProfil, int JKWId, int JBahagianId, int AkCartaId)
        {
            SuProfil m = new SuProfil();
            var IsExistNoRujukan = _context.SuProfil.Where(x => x.NoRujukan == suProfil.NoRujukan).FirstOrDefault();

            // check if Tahun, Bulan ,JBahagianId, JKWId already exist or not 
            if (IsExistNoRujukan != null)
            {
                TempData[SD.Error] = "Data bagi Kump. Wang dan Bahagian telah wujud bagi Tahun dan Bulan ini.";
                PopulateList();
                CartEmpty();

                // get latest no rujukan running number 
                var year = DateTime.Now.Year.ToString();
                var month = DateTime.Now.ToString("MM");
                var bahagian = _context.JBahagian.Where(x => x.Id == 1).FirstOrDefault();

                ViewBag.NoRujukan = bahagian.Kod + "/" + year + "/" + month;

                PopulateCartFromSuAtlet();
                PopulateTableFromCart();
                return View(suProfil);
            }
            // check end

            var user = await _userManager.GetUserAsync(User);


            if (ModelState.IsValid)
            {
                if (suProfil != null && JKWId != 0 && JBahagianId != 0 && AkCartaId != 0)
                {
                    m.FlKategori = 0;
                    m.Tahun = suProfil.Tahun;
                    m.Bulan = suProfil.Bulan;
                    m.NoRujukan = suProfil.NoRujukan;
                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.AkCartaId = AkCartaId;
                    m.Jumlah = suProfil.Jumlah;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.SuProfil1 = _cart.Lines1.ToArray();

                    await _suProfilRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, suProfil.Jumlah);
                    //insert applog end
                    await _suProfilRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No Rujukan adalah " + m.NoRujukan;

                    return RedirectToAction(nameof(Index));
                }
                
            }
            PopulateList();
            CartEmpty();
            PopulateCartFromSuAtlet();
            PopulateTableFromCart();
            return View(suProfil);
        }

        // GET: SuProfilAtlet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil.FindAsync(id);
            if (suProfil == null)
            {
                return NotFound();
            }
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", suProfil.AkCartaId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", suProfil.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", suProfil.JKWId);
            return View(suProfil);
        }

        // POST: SuProfilAtlet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoRujukan,Bulan,Tahun,FlKategori,AkCartaId,JKWId,JBahagianId,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SuProfil suProfil)
        {
            if (id != suProfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suProfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuProfilExists(suProfil.Id))
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
            ViewData["AkCartaId"] = new SelectList(_context.AkCarta, "Id", "DebitKredit", suProfil.AkCartaId);
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", suProfil.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", suProfil.JKWId);
            return View(suProfil);
        }

        // GET: SuProfilAtlet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _context.SuProfil
                .Include(s => s.AkCarta)
                .Include(s => s.JBahagian)
                .Include(s => s.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suProfil == null)
            {
                return NotFound();
            }

            return View(suProfil);
        }

        // POST: SuProfilAtlet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suProfil = await _context.SuProfil.FindAsync(id);
            _context.SuProfil.Remove(suProfil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuProfilExists(int id)
        {
            return _context.SuProfil.Any(e => e.Id == id);
        }
    }
}
