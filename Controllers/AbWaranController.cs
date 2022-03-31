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
using MSNK.Infrastructure;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.EFRepository;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;

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
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPPRepo;
        private readonly CustomIRepository<string, int> _customRepo;
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
            IRepository<AkPO, int, string> akPORepo,
            IRepository<AkPV, int, string> akPVRepo,
            IRepository<AkTerima, int, string> akTerimaRepo,
            IRepository<SpPendahuluanPelbagai, int, string> spPPRepo,
            CustomIRepository<string, int> customrepo,
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
            _akPORepo = akPORepo;
            _akPVRepo = akPVRepo;
            _akTerimaRepo = akTerimaRepo;
            _spPPRepo = spPPRepo;
            _customRepo = customrepo;
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
            columnList.Add(new SelectListItem() { Text = "No Rujukan", Value = "NoRujukan" });
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

            var abWaran = await _abWaranRepo.GetByIdIncludeDeletedItems((int)id);

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
            string prefix = "WR/" + year + "/";
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
                x = int.Parse(LatestNoRujukan.Substring(9));
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

        public async Task<JsonResult> SaveAbWaran1(
            AbWaran1 abWaran1, 
            string tahun, 
            int jKWId, 
            int jBahagianId)
        {

            try
            {
                if (abWaran1 != null)
                {
                    // check for baki peruntukan
                    if (abWaran1.TK == "-")
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == tahun && x.VotId == abWaran1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, abWaran1.AkCartaId, jKWId, jBahagianId);

                            if (sum < abWaran1.Amaun)
                            {
                                return Json(new { result = "ERROR" });
                            }
                        }
                        else
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    // check for baki peruntukan end

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
        public async Task<JsonResult> SaveCartAbWaran1(
            AbWaran1 abWaran1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {
            try
            {

                var abW1 = _cart.Lines1.Where(x => x.AkCartaId == abWaran1.AkCartaId).FirstOrDefault();

                if (abW1 != null)
                {
                    // check for baki peruntukan
                    if (abWaran1.TK == "-")
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == tahun && x.VotId == abWaran1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, abWaran1.AkCartaId, jKWId, jBahagianId);

                            if (sum < abWaran1.Amaun)
                            {
                                return Json(new { result = "ERROR" });
                            }
                        }
                        else
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    // check for baki peruntukan end

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

            // check if Tahun, FlJenisWaran ,JBahagianId, JKWId already exist or not 
            var w = await _context.AbWaran.Where(x => x.Tahun == abWaran.Tahun
                                                && x.FlJenisWaran == FlJenisWaran 
                                                && x.JKWId == JKWId 
                                                && x.JBahagianId == JBahagianId)
                    .FirstOrDefaultAsync();

            if (w != null)
            {
                TempData[SD.Error] = "Data bagi Tahun, Jenis Waran, Kump. Wang dan Bahagian telah wujud.";
                PopulateList();
                CartEmpty();

                return View(abWaran);
            }
            // check end

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

                    // check for baki peruntukan
                    foreach (AbWaran1 item in m.AbWaran1)
                    {
                        if (item.TK == "-")
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == m.Tahun && x.VotId == item.AkCartaId && x.JKWId == m.JKWId && x.JBahagianId == m.JBahagianId)
                                .AnyAsync();

                            var carta = _context.AkCarta.Find(item.AkCartaId);

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(m.Tahun, item.AkCartaId, m.JKWId, m.JBahagianId);

                                if (sum < item.Amaun)
                                {
                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta.Kod + " tidak mencukupi.";
                                    PopulateList();
                                    CartEmpty();

                                    return View(abWaran);
                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                                PopulateList();
                                CartEmpty();

                                return View(abWaran);
                            }

                        }
                    }
                    // check for baki peruntukan end

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

            var abWaran = await _abWaranRepo.GetById((int)id);

            if (abWaran == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(abWaran);
            return View(abWaran);
        }

        private void PopulateCartFromDb(AbWaran abWaran)
        {
            List<AbWaran1> table1 = _context.AbWaran1
                .Include(b => b.AkCarta)
                .Where(b => b.AbWaranId == abWaran.Id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AbWaran1 item in table1)
            {
                _cart.AddItem1(item.AbWaranId,
                               item.Amaun,
                               item.AkCartaId,
                               item.TK);
            }

        }

        // POST: AbWaran/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AbWaran abWaran, int JKWId, int JBahagianId)
        {
            if (id != abWaran.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    AbWaran dataAsal = await _abWaranRepo.GetById(id);

                    // list of input that cannot be change
                    abWaran.Tahun = dataAsal.Tahun;
                    abWaran.JKWId = dataAsal.JKWId;
                    abWaran.JBahagianId = dataAsal.JBahagianId;
                    abWaran.FlJenisWaran = dataAsal.FlJenisWaran;
                    abWaran.TarMasuk = dataAsal.TarMasuk;
                    abWaran.UserId = dataAsal.UserId;
                    abWaran.FlCetak = 0;
                    // list of input that cannot be change end

                    foreach (AbWaran1 item in dataAsal.AbWaran1)
                    {
                        var model = _context.AbWaran1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }
                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    abWaran.AbWaran1 = _cart.Lines1.ToList();

                    // check for baki peruntukan
                    foreach (AbWaran1 item in _cart.Lines1)
                    {

                        if (item.TK == "-")
                        {
                            bool IsExistAbBukuVot = await _context.AbBukuVot
                               .Where(x => x.Tahun == abWaran.Tahun && x.VotId == item.AkCartaId && x.JKWId == abWaran.JKWId && x.JBahagianId == abWaran.JBahagianId)
                               .AnyAsync();

                            var carta = _context.AkCarta.Find(item.AkCartaId);

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(abWaran.Tahun, item.AkCartaId, abWaran.JKWId, abWaran.JBahagianId);

                                if (sum < item.Amaun)
                                {
                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta.Kod + " tidak mencukupi.";
                                    PopulateList();
                                    PopulateTable(id);

                                    return View(abWaran);
                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                                PopulateList();
                                PopulateTable(id);

                                return View(abWaran);
                            }
                        }
                    }
                    // check for baki peruntukan end

                    abWaran.UserIdKemaskini = user.UserName;
                    abWaran.TarKemaskini = DateTime.Now;

                    _context.Update(abWaran);
                    // insert applog
                    if (jumlahAsal != abWaran.Jumlah)
                    {
                        await AddLogAsync("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(abWaran.Jumlah).ToString("#,##0.00"), abWaran.NoRujukan, id, abWaran.Jumlah);

                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", abWaran.NoRujukan, id, abWaran.Jumlah);
                    }
                    //insert applog end


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
                CartEmpty();
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            PopulateTable(id);
            return View(abWaran);
        }

        // GET: AbWaran/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [Authorize("BJ001D")]
        // POST: AbWaran/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abWaran = await _context.AbWaran.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);

            abWaran.UserIdKemaskini = user.UserName;
            abWaran.TarKemaskini = DateTime.Now;
            // check if already posting redirect back
            if (abWaran.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            abWaran.FlCetak = 0;
            _context.AbWaran.Update(abWaran);

            //insert applog
            await AddLogAsync("Hapus", abWaran.NoRujukan, abWaran.NoRujukan, id, abWaran.Jumlah);
            //insert applog end

            _context.AbWaran.Remove(abWaran);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";

            return RedirectToAction(nameof(Index));
        }

        private bool AbWaranExists(int id)
        {
            return _context.AbWaran.Any(e => e.Id == id);
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "BJ001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _abWaranRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            _context.AbWaran.Update(obj);

            // Batal operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoRujukan, (int)id, obj.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // posting function
        [Authorize(Policy = "BJ001T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AbWaran obj = await _abWaranRepo.GetById((int)id);

                var jenisWaran = "";

                switch (obj.FlJenisWaran)
                {
                    case 0:
                        jenisWaran = "PERUNTUKAN ASAL";
                        obj.Tarikh = Convert.ToDateTime("01/01/" + obj.Tahun); 
                        break;
                    case 1:
                        jenisWaran = "PERUNTUKAN TAMBAH/ TARIK BALIK";
                        break;
                    default:
                        jenisWaran = "PERUNTUKAN PINDAHAN";
                        break;
                }
                //check for print
                if (obj.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AbWaran1> abWaran1 = obj.AbWaran1.ToList();

                // check for baki peruntukan
                foreach (AbWaran1 item in abWaran1)
                {


                    if (item.TK == "-")
                    {
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                               .Where(x => x.Tahun == obj.Tahun && x.VotId == item.AkCartaId && x.JKWId == obj.JKWId && x.JBahagianId == obj.JBahagianId)
                               .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(obj.Tahun, item.AkCartaId, obj.JKWId, obj.JBahagianId);

                            if (sum < item.Amaun)
                            {
                                TempData[SD.Error] = "Bajet untuk kod akaun " + item.AkCarta.Kod + " tidak mencukupi.";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + item.AkCarta.Kod;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                // check for baki peruntukan end

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoRujukan)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here

                    foreach (AbWaran1 item in abWaran1)
                    {
                        if (item.TK == "+")
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotPosting = new AbBukuVot()
                            {
                                Tahun = obj.Tahun,
                                JKWId = obj.JKWId,
                                JBahagianId = obj.JBahagianId,
                                Tarikh = obj.Tarikh,
                                //Kod = "",
                                Penerima = jenisWaran,
                                VotId = item.AkCartaId,
                                Rujukan = obj.NoRujukan,
                                Kredit = item.Amaun
                            };
                            await _abBukuVotRepo.Insert(abBukuVotPosting);
                        }
                        else
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotPosting = new AbBukuVot()
                            {
                                Tahun = obj.Tahun,
                                JKWId = obj.JKWId,
                                JBahagianId = obj.JBahagianId,
                                Tarikh = obj.Tarikh,
                                //Kod = "",
                                Penerima = jenisWaran,
                                VotId = item.AkCartaId,
                                Rujukan = obj.NoRujukan,
                                Debit = item.Amaun
                            };
                            await _abBukuVotRepo.Insert(abBukuVotPosting);
                        }

                        // insert into AbBukuVot end

                    }

                    //update posting status in akPO
                    obj.FlPosting = 1;
                    obj.TarikhPosting = DateTime.Now;
                    await _abWaranRepo.Update(obj);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", obj.NoRujukan, (int)id, obj.Jumlah);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "BJ001UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AbWaran obj = await _abWaranRepo.GetById((int)id);

                //check
                // dah ada po atau tidak
                foreach (var waran in obj.AbWaran1)
                {
                    var akPO = await _akPORepo.GetAll();

                    foreach (var i in akPO)
                    {
                        var akPO1 = await _context.AkPO1
                            .Where(x => x.AkPOId == i.Id && x.AkCartaId == waran.AkCartaId)
                            .FirstOrDefaultAsync();

                        if (akPO1 != null)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No PO " + i.NoPO;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                //
                // dah ada baucer atau tidak
                foreach (var waran in obj.AbWaran1)
                {
                    var akPV = await _akPVRepo.GetAll();

                    foreach (var i in akPV)
                    {
                        var akPV1 = await _context.AkPV1
                            .Where(x => x.AkPVId == i.Id && x.AkCartaId == waran.AkCartaId)
                            .FirstOrDefaultAsync();

                        if (akPV1 != null)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No PV " + i.NoPV;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                //
                // dah ada resit atau tidak
                foreach (var waran in obj.AbWaran1)
                {
                    var akTerima = await _akTerimaRepo.GetAll();

                    foreach (var i in akTerima)
                    {
                        var akTerima1 = await _context.AkTerima1
                            .Where(x => x.AkTerimaId == i.Id && x.AkCartaId == waran.AkCartaId)
                            .FirstOrDefaultAsync();

                        if (akTerima1 != null)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No Resit " + i.NoRujukan;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                //
                // dah ada pendahuluan pelbagai atau tidak
                foreach (var waran in obj.AbWaran1)
                {
                    var sp = await _spPPRepo.GetAll();

                    foreach (var i in sp)
                    {
                        var sp1 = await _context.SpPendahuluanPelbagai
                            .Where(x => x.Id == i.Id && x.AkCartaId == waran.AkCartaId)
                            .FirstOrDefaultAsync();

                        if (sp1 != null)
                        {
                            //duplicate id error
                            TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No Permohonan " + i.NoPermohonan;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                //

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoRujukan)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {

                    //unposting operation start here
                    //delete data from abBukuVot
                    foreach (AbBukuVot item in abBukuVot)
                    {
                        await _abBukuVotRepo.Delete(item.Id);
                    }
                    //delete data from abBukuVot end

                    //update posting status in akPOLaras
                    obj.FlPosting = 0;
                    obj.TarikhPosting = null;
                    await _abWaranRepo.Update(obj);

                    //insert applog
                    await AddLogAsync("UnPosting", "UnPosting Data", obj.NoRujukan, (int)id, obj.Jumlah);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // printing Waran by akPO.Id
        [Authorize(Policy = "BJ001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AbWaran obj = await _abWaranRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (obj.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - obj.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(obj.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);

            WaranPrintModel data = new WaranPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.AbWaran = obj;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            obj.FlCetak = 1;
            await _abWaranRepo.Update(obj);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", obj.NoRujukan, id, obj.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("WaranPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing Waran end
    }
}
