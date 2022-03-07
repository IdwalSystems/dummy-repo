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
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AbWaranController : Controller
    {
        public const string modul = "BJ001";
        public const string namamodul = "Waran";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AbWaran, int, string> _abWaranRepo;
        private readonly ListViewIRepository<AbWaran1, int> _abWaran1Repo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JBahagian, int, string> _jBahagianRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartWaran _cart;

        public AbWaranController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AbWaran, int, string> abWaranRepo,
            ListViewIRepository<AbWaran1, int> abWaran1Repo,
            IRepository<JKW, int, string> jkwRepo,
            IRepository<JBahagian, int, string> jBahagianRepo,
            IRepository<AkCarta, int, string> akCartaRepo,
            IRepository<AbBukuVot, int, string> abBukuVotRepo,
            CartWaran cart)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _abWaranRepo = abWaranRepo;
            _abWaran1Repo = abWaran1Repo;
            _kwRepo = jkwRepo;
            _jBahagianRepo = jBahagianRepo;
            _akCartaRepo = akCartaRepo;
            _abBukuVotRepo = abBukuVotRepo;
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

        // GET: AbWaran
        [Authorize(Policy = "BJ001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No PV", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Tahun", Value = "Tahun" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var abWaran = await _abWaranRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                abWaran = await _abWaranRepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        abWaran = abWaran.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Tahun")
                    {
                        abWaran = abWaran.Where(s => s.Tahun.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }


                    ViewBag.SearchData1 = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2))
                {
                    if (searchColumn == "Tarikh")
                    {
                        DateTime date1 = DateTime.Parse(searchDate1);
                        DateTime date2 = DateTime.Parse(searchDate2).AddHours(23.99);
                        abWaran = abWaran.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }

            return View(abWaran);
        }

        // GET: AbWaran/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _abWaranRepo.GetById((int)id);

            if (abWaran == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(abWaran);
        }

        private void PopulateTable(int? id)
        {
            List<AbWaran1> table1 = _context.AbWaran1
                .Include(b => b.AkCarta)
                .Where(b => b.AbWaranId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.abWaran1 = table1;
        }

        // GET: AkPV/Create
        [Authorize(Policy = "BJ001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();

            ViewBag.NoRujukan = GetNoRujukan(year);
            // get latest no rujukan running number end

            PopulateList();
            CartEmpty();
            return View();
        }

        private string GetNoRujukan(string year)
        {
            string prefix = year + "/";
            int x = 1;
            string noRujukan = prefix + "0000";

            var LatestNoRujukan = _context.AbWaran
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year)
                       .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(6));
                x++;
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            return noRujukan;
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
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
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // on change no PO controller
        [HttpPost]
        public JsonResult JsonGetKod(string year)
        {
            try
            {
                var result = GetNoRujukan(year);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // function  json Create
        public JsonResult GetCarta(AkCarta akCarta)
        {
            try
            {
                var result = _context.AkCarta.Where(b => b.Id == akCarta.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public async Task<JsonResult> SaveAbWaran1(AbWaran1 abWaran1)
        {

            try
            {
                if (abWaran1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(abWaran1.AbWaranId,
                                    abWaran1.Amaun,
                                    abWaran1.AkCartaId,
                                    abWaran1.TK
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAbWaran1(AbWaran1 abWaran1)
        {

            try
            {
                if (abWaran1 != null)
                {

                    _cart.RemoveItem1(abWaran1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart abWaran1
        public JsonResult GetAnItemCartAbWaran1(AbWaran1 abWaran1)
        {

            try
            {
                AbWaran1 data = _cart.Lines1.Where(x => x.AkCartaId == abWaran1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart AbWaran1 end

        //save cart AbWaran1
        public JsonResult SaveCartAbWaran1(AbWaran1 abWaran1)
        {

            try
            {

                var abW1 = _cart.Lines1.Where(x => x.AkCartaId == abWaran1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (abW1 != null)
                {
                    _cart.RemoveItem1(abW1.AkCartaId);

                    _cart.AddItem1(abWaran1.AbWaranId,
                                    abWaran1.Amaun,
                                    abWaran1.AkCartaId,
                                    abWaran1.TK
                                    );
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
        public JsonResult GetAllItemCartAbWaran1()
        {

            try
            {
                List<AbWaran1> data = _cart.Lines1.ToList();

                foreach (AbWaran1 item in data)
                {
                    var akCarta = _context.AkCarta.Find(item.AkCartaId);

                    item.AkCarta = akCarta;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPOLaras1 end

        // POST: AbWaran/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "BJ001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbWaran abWaran, int JKWId, int JBahagianId, int FlJenisWaran)
        {
            AbWaran m = new AbWaran();
            abWaran.NoRujukan = GetNoRujukan(abWaran.Tahun);
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (abWaran != null && JKWId != 0 && JBahagianId != 0)
                {
                    m.FlJenisWaran = FlJenisWaran;
                    m.Tahun = abWaran.Tahun;
                    m.Tarikh = abWaran.Tarikh;
                    m.NoRujukan = GetNoRujukan(abWaran.Tahun);
                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.Jumlah = abWaran.Jumlah;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AbWaran1 = _cart.Lines1.ToArray();

                    await _abWaranRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, abWaran.Jumlah);
                    //insert applog end
                    await _abWaranRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No Pendaftaran adalah " + m.NoRujukan;

                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            CartEmpty();

            return View(abWaran);
        }

        // GET: AbWaran/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _context.AbWaran.FindAsync(id);
            if (abWaran == null)
            {
                return NotFound();
            }
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", abWaran.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", abWaran.JKWId);
            return View(abWaran);
        }

        // POST: AbWaran/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoRujukan,Tahun,Tarikh,TarikhPosting,Jumlah,FlJenisWaran,FlHapus,TarHapus,FlPosting,FlCetak,JKWId,JBahagianId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AbWaran abWaran)
        {
            if (id != abWaran.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abWaran);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbWaranExists(abWaran.Id))
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
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", abWaran.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", abWaran.JKWId);
            return View(abWaran);
        }

        // GET: AbWaran/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _context.AbWaran
                .Include(a => a.JBahagian)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abWaran == null)
            {
                return NotFound();
            }

            return View(abWaran);
        }

        // POST: AbWaran/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abWaran = await _context.AbWaran.FindAsync(id);
            _context.AbWaran.Remove(abWaran);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbWaranExists(int id)
        {
            return _context.AbWaran.Any(e => e.Id == id);
        }
    }
}
