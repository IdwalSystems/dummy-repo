using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using Rotativa.AspNetCore;
using MSNK.Infrastructure;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkPOController : Controller
    {
        public const string modul = "TG001";
        public const string namamodul = "Pesanan Tempatan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkNotaMinta, int, string> _akNotaMintaRepo;
        private readonly ListViewIRepository<AkPO1, int> _akPO1Repo;
        private readonly ListViewIRepository<AkPO2, int> _akPO2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartPO _cart;

        public AkPOController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int, string> akPORepository,
            IRepository<AkNotaMinta, int, string> akNotaMintaRepository,
            ListViewIRepository<AkPO1, int> akPO1Repository,
            ListViewIRepository<AkPO2, int> akPO2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CartPO cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPORepo = akPORepository;
            _akNotaMintaRepo = akNotaMintaRepository;
            _akPO1Repo = akPO1Repository;
            _akPO2Repo = akPO2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
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

        //Function Running Number
        private string RunningNumber(AkPO data)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data.JKWId);

            var kumpulanWang = kw.Kod;
            //var year = DateTime.Now.Year.ToString();
            var year = data.Tahun;
            string prefix = year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPO
                .IgnoreQueryFilters()
                .Where(x => x.NoPO.Substring(0, 9) == prefix)
                .Max(x => x.NoPO);
            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(12));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }
        [HttpPost]
        public JsonResult JsonGetKod(AkPO data)
        {
            try
            {
                var result = "";
                if (data == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(data);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // GET: AkPO
        [Authorize(Policy = "TG001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No PO", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Nama", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akPO = await _akPORepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akPO = await _akPORepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akPO = akPO.Where(s => s.NoPO.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Pembekal")
                    {
                        akPO = akPO.Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPO = akPO.Where(x => x.Tarikh >= date1
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

            var lastItem = akPO.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem != null)
            {
                ViewData["lastItem"] = lastItem.NoPO;
            }
            else
            {
                ViewData["lastItem"] = "NaN";
            }

            return View(akPO);
        }

        // GET: AkPO/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetByIdIncludeDeletedItems((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;

            // check if this data is the last one (for preventing batal purpose)
            var lastItem = _context.AkPO.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem.Id == akPO.Id)
            {
                ViewData["isLastItem"] = "Y";
            }
            else
            {
                ViewData["isLastItem"] = "N";
            }
            // check end

            if (akPO == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }
        // GET: AkPO/Delete/5
        [Authorize(Policy = "TG001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;
            if (akPO == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }

        // POST: AkPO/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TG001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPO = await _context.AkPO.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            akPO.UserIdKemaskini = user.UserName;
            akPO.TarKemaskini = DateTime.Now;
            // check if already posting redirect back
            if (akPO.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            akPO.FlCetak = 0;
            _context.AkPO.Update(akPO);

            _context.AkPO.Remove(akPO);

            //insert applog
            await AddLogAsync("Hapus", akPO.NoPO, akPO.NoPO, id, akPO.Jumlah);
            //insert applog end

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // on change no PO controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoNotaMinta(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkNotaMinta(id);
                var result = await _akNotaMintaRepo.GetById(id);

                List<AkNotaMinta1> akNotaMinta1Table = await _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkNotaMinta1 item in akNotaMinta1Table)
                {
                    result.AkNotaMinta1.Add(item);
                }

                List<AkNotaMinta2> akNotaMinta2Table = await _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkNotaMinta2 item in akNotaMinta2Table)
                {
                    result.AkNotaMinta2.Add(item);
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkNotaMinta(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkNotaMinta1 item in akNotaMinta1Table)
            {

                item.AkNotaMintaId = 0;

                _cart.AddItem1(item.AkNotaMintaId,
                                item.AkCartaId,
                               item.Amaun
                               );
            }

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .AsNoTracking()
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkNotaMinta2 item in akNotaMinta2Table)
            {
                item.AkNotaMintaId = 0;

                _cart.AddItem2(item.AkNotaMintaId,
                               item.Indek,
                               item.Baris,
                               item.Bil,
                               item.NoStok,
                               item.Perihal,
                               item.Kuantiti,
                               item.Unit,
                               item.Harga,
                               item.Amaun);
            }


        }
        //on change no PO controller end

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPembekal> PembekalList = _context.AkPembekal.OrderBy(b => b.Id).ToList();
            ViewBag.AkPembekal = PembekalList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkNotaMinta> akNotaMintaList = _context.AkNotaMinta.Where(x => x.FlPosting == 1).ToList();
            ViewBag.AkNotaMinta = akNotaMintaList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {

            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = akPO1Table;

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO2 = akPO2Table;
        }
        private void PopulateCart(AkPO akPO)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO1 akPO1 in akPO1Table)
            {
                _cart.AddItem1(akPO1.AkPOId,
                                akPO1.AkCartaId,
                                akPO1.Amaun);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.JPerihal)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO2 akPO2 in akPO2Table)
            {
                _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Baris,
                               akPO2.Bil,
                               akPO2.NoStok,
                               akPO2.Perihal,
                               akPO2.Kuantiti,
                               akPO2.Unit,
                               akPO2.Harga,
                               akPO2.Amaun);
            }
        }

        // GET: AkPO/Createt
        [Authorize(Policy = "TG001C")]
        public IActionResult Create()
        {
            CartEmpty();
            PopulateList();
            return View();
        }

        public JsonResult GetAnItemCartAkPO1(AkPO1 akPO1)
        {

            try
            {
                AkPO1 data = _cart.Lines1.Where(x => x.AkCartaId == akPO1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPO1 end

        //save cart akPO1
        public JsonResult SaveCartAkPO1(AkPO1 akPO1)
        {

            try
            {

                var akP1 = _cart.Lines1.Where(x => x.AkCartaId == akPO1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akP1 != null)
                {
                    _cart.RemoveItem1(akPO1.AkCartaId);

                    _cart.AddItem1(akPO1.AkPOId,
                                    akPO1.AkCartaId,
                                    akPO1.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPO1 end

        // get all item from cart akPO1
        public JsonResult GetAllItemCartAkPO1(AkPO1 akPO1)
        {

            try
            {
                List<AkPO1> data = _cart.Lines1.ToList();

                foreach (AkPO1 item in data)
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
        // get all item from cart akPO1 end

        // get an item from cart akPO2
        public JsonResult GetAnItemCartAkPO2(AkPO2 akPO2)
        {

            try
            {
                AkPO2 data = _cart.Lines2.Where(x => x.Indek == akPO2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPO2 end

        //save cart akPO2
        public JsonResult SaveCartAkPO2(AkPO2 akPO2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akPO2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akPO2.Indek);

                    _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Baris,
                               akPO2.Bil,
                               akPO2.NoStok,
                               akPO2.Perihal,
                               akPO2.Kuantiti,
                               akPO2.Unit,
                               akPO2.Harga,
                               akPO2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPO2 end

        // get all item from cart akPO2
        public JsonResult GetAllItemCartAkPO2()
        {

            try
            {
                List<AkPO2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPO2 end

        // POST: AkPO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPO akPO, int JKWId, int? AkNotaMintaId)
        {

            AkPO m = new AkPO();
            var user = await _userManager.GetUserAsync(User);
            var pembekal = _context.AkPembekal.FirstOrDefault(x => x.Id == akPO.AkPembekalId);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akPO);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akPO != null && JKWId != 0)
                {

                    m.JKWId = JKWId;
                    m.NoPO = noRujukan;
                    m.Tarikh = akPO.Tarikh;
                    m.AkNotaMintaId = AkNotaMintaId;
                    m.TarikhPosting = akPO.TarikhPosting;
                    m.AkPembekal = pembekal;
                    m.Jumlah = akPO.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.Tahun = akPO.Tahun;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkPO1 = _cart.Lines1.ToArray();
                    m.AkPO2 = _cart.Lines2.ToArray();

                    await _akPORepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoPO, m.NoPO, 0, m.Jumlah);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat Pesanan Tempatan berjaya ditambah. No Pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akPO);
        }

        // GET: AkPO/Edit/5
        [Authorize(Policy = "TG001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;

            // check if already posting redirect back
            if (akPO.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akPO);
            return View(akPO);
        }

        // get latest Index number in AkNotaMinta2
        public JsonResult GetLatestIndexNumberPerihal()
        {

            try
            {
                var data = _cart.Lines2.Max(x => x.Indek);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkPO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TG001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPO akPO, int JKWId, int JNegeriId, int AkBankId, decimal JumlahPerihal, int? AkNotaMintaId)
        {
            if (id != akPO.Id)
            {
                return NotFound();
            }

            if (akPO.Jumlah == JumlahPerihal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        AkPO dataAsal = await _akPORepo.GetById(id);

                        // list of input that cannot be change
                        akPO.Tahun = dataAsal.Tahun;
                        akPO.JKWId = dataAsal.JKWId;
                        akPO.NoPO = dataAsal.NoPO;
                        akPO.TarMasuk = dataAsal.TarMasuk;
                        akPO.UserId = dataAsal.UserId;
                        akPO.FlCetak = 0;
                        // list of input that cannot be change end

                        foreach (AkPO1 item in dataAsal.AkPO1)
                        {
                            var model = _context.AkPO1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkPO2 item in dataAsal.AkPO2)
                        {
                            var model = _context.AkPO2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        decimal jumlahAsal = dataAsal.Jumlah;
                        _context.Entry(dataAsal).State = EntityState.Detached;

                        akPO.AkPO1 = _cart.Lines1.ToList();
                        akPO.AkPO2 = _cart.Lines2.ToList();

                        akPO.UserIdKemaskini = user.UserName;
                        akPO.TarKemaskini = DateTime.Now;
                        akPO.FlCetak = 0;

                        _context.Update(akPO);

                        //insert applog
                        if (jumlahAsal != akPO.Jumlah)
                        {
                            await AddLogAsync("Ubah","RM" +  Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" + 
                                Convert.ToDecimal(akPO.Jumlah).ToString("#,##0.00"), akPO.NoPO, id, akPO.Jumlah);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", akPO.NoPO, id, akPO.Jumlah);
                        }
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkPOExists(akPO.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    CartEmpty();
                    // checking for jumlah objek & jumlah perihal
                    if (akPO.Jumlah != JumlahPerihal)
                    {
                        TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
                    }
                    else
                    {
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
            PopulateList();
            PopulateTable(id);
            //PopulateCart();
            return View(akPO);
        }

        private bool AkPOExists(int id)
        {
            return _context.AkPO.Any(e => e.Id == id);
        }

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

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akPO1 = new List<int>();
                ViewBag.akPO2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);


                    _cart.AddItem1(akPO1.AkPOId,
                                   akPO1.AkCartaId,
                                   akPO1.Amaun);

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akPO2.AkPOId,
                         akPO2.Indek,
                         akPO2.Baris,
                         akPO2.Bil,
                         akPO2.NoStok,
                         akPO2.Perihal,
                         akPO2.Kuantiti,
                         akPO2.Unit,
                         akPO2.Harga,
                         akPO2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {

                    _cart.RemoveItem1(akPO1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {

                    _cart.RemoveItem2(akPO2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {

                _cart.Clear1();

                AkPO1 akT1 = await _akPO1Repo.GetById(akPO1.Id);

                decimal originalAmount = akT1.Amaun;
                var user = await _userManager.GetUserAsync(User);

                akT1.Amaun = akPO1.Amaun;
                _context.AkPO1.Update(akT1);

                // update total akPO with date updated and userUpdated
                var akPO = await _akPORepo.GetById(akPO1.AkPOId);
                decimal total = 0;

                total = akPO.Jumlah - originalAmount + akT1.Amaun;
                akPO.Jumlah = total;
                akPO.UserIdKemaskini = user.UserName;
                akPO.TarKemaskini = DateTime.Now;
                await _akPORepo.Update(akPO);
                // update total akPO with date updated and userUpdated end

                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                _cart.Clear2();

                AkPO2 akT2 = await _akPO2Repo.GetById(akPO2.Id);
                var user = await _userManager.GetUserAsync(User);
                decimal originalAmount = akT2.Amaun;

                akT2.Amaun = akPO2.Amaun;
                akT2.Indek = akPO2.Indek;
                akT2.Bil = akPO2.Bil;
                akT2.Baris = akPO2.Baris;
                akT2.NoStok = akPO2.NoStok;
                akT2.Perihal = akPO2.Perihal;
                akT2.Kuantiti = akPO2.Kuantiti;
                akT2.Unit = akPO2.Unit;
                akT2.Harga = akPO2.Harga;
                akT2.Amaun = akPO2.Amaun;

                _context.AkPO2.Update(akT2);

                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart1(AkPO1 akPO1)
        {
            try
            {
                AkPO data = await _context.AkPO.Include(x => x.AkPO1).ThenInclude(x => x.AkCarta).FirstOrDefaultAsync(x => x.Id == akPO1.AkPOId);

                List<AkPO1> akT1 = data.AkPO1.ToList();

                foreach (AkPO1 item in akT1)
                {

                    _cart.AddItem1(item.AkPOId,
                                item.AkCartaId,
                                item.Amaun);

                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public async Task<JsonResult> GetCart2(AkPO2 akPO2)
        {
            try
            {
                AkPO data = await _context.AkPO.Include(x => x.AkPO2).ThenInclude(x => x.Perihal).FirstOrDefaultAsync(x => x.Id == akPO2.AkPOId);

                List<AkPO2> akT2 = data.AkPO2.ToList();

                foreach (AkPO2 item in akT2)
                {
                    _cart.AddItem2(akPO2.AkPOId,
                         akPO2.Indek,
                         akPO2.Baris,
                         akPO2.Bil,
                         akPO2.NoStok,
                         akPO2.Perihal,
                         akPO2.Kuantiti,
                         akPO2.Unit,
                         akPO2.Harga,
                         akPO2.Amaun);
                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> UpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                AkPO1 data = await _akPO1Repo.GetBy2Id(akPO1.AkPOId, akPO1.AkCartaId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> UpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                AkPO2 data = await _akPO2Repo.GetBy2Id(akPO2.AkPOId, akPO2.Indek);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null || akPO1.Amaun != 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akPO1.AkCartaId);
                    akPO1.AkCarta = akCarta;

                    await _akPO1Repo.Insert(akPO1);

                    decimal total = 0;

                    AkPO akPO = await _akPORepo.GetById(akPO1.AkPOId);

                    total = akPO.Jumlah + akPO1.Amaun;

                    akPO.Jumlah = total;
                    akPO.UserIdKemaskini = user.UserName;

                    await _akPORepo.Update(akPO);
                    await _context.SaveChangesAsync();

                    _cart.AddItem1(akPO1.AkPOId,
                                   akPO1.AkCartaId,
                                   akPO1.Amaun);


                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null || akPO2.Amaun != 0)
                {
                 
                    var user = await _userManager.GetUserAsync(User);

                    await _akPO2Repo.Insert(akPO2);

                    await _context.SaveChangesAsync();
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public async Task<JsonResult> RemoveUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akT1 = await _context.AkPO1.FirstOrDefaultAsync(x => x.AkCartaId == akPO1.AkCartaId && x.AkPOId == akPO1.AkPOId);
                    _context.AkPO1.Remove(akT1);

                    decimal total = 0;

                    AkPO akPO = await _akPORepo.GetById(akPO1.AkPOId);

                    total = akPO.Jumlah - akT1.Amaun;

                    akPO.Jumlah = total;
                    akPO.UserIdKemaskini = user.UserName;
                    akPO.TarKemaskini = DateTime.Now;
                    await _akPORepo.Update(akPO);

                    await _context.SaveChangesAsync();

                    _cart.RemoveItem1(akPO1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akP2 = await _context.AkPO2.FirstOrDefaultAsync(x => x.Indek == akPO2.Indek && x.AkPOId == akPO2.AkPOId);
                    _context.AkPO2.Remove(akP2);

                    await _context.SaveChangesAsync();

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetMaklumat(AkPembekal akPembekal)
        {
            try
            {
                var result = _context.AkPembekal.Where(b => b.Id == akPembekal.Id).Include(x => x.JBank).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        // posting function
        [Authorize(Policy = "TG001T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AkPO akPO = await _context.AkPO
                    .Include(x => x.AkPembekal)
                    .Include(x => x.AkPO1).ThenInclude(x => x.AkCarta)
                    .Include(x => x.AkPO2)
                    .FirstOrDefaultAsync(x => x.Id == id);

                //check for print
                if (akPO.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AkPO1> akPO1 = akPO.AkPO1.ToList();

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + akPO.NoPO)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here

                    foreach (AkPO1 item in akPO1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotPosting = new AbBukuVot()
                        {
                            Tahun = akPO.Tahun,
                            JKWId = akPO.JKWId,
                            Tarikh = akPO.Tarikh,
                            Kod = akPO.AkPembekal.KodSykt,
                            Penerima = akPO.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = "PO/"+akPO.NoPO,
                            Tanggungan = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotPosting);
                        // insert into AbBukuVot end
 
                    }

                    //update AkNotaMinta
                    if(akPO.AkNotaMintaId != null)
                    {
                        var noPO = "PO/" + akPO.NoPO;
                        var tarikhPO = DateTime.Now;

                        AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akPO.AkNotaMintaId);

                        akNM.NoCAS = noPO;
                        akNM.TarikhSeksyenKewangan = tarikhPO;

                        await _akNotaMintaRepo.Update(akNM);
                    }
                    
                    //update AkNotaMinta end

                    //update posting status in akPO
                    akPO.FlPosting = 1;
                    akPO.TarikhPosting = DateTime.Now;
                    await _akPORepo.Update(akPO);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", akPO.NoPO, (int)id, akPO.Jumlah);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TG001UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkPO akPO = await _context.AkPO
                    .Include(x => x.AkPembekal)
                    .Include(x => x.AkPO1).ThenInclude(x => x.AkCarta)
                    .Include(x => x.AkPO2)
                    .FirstOrDefaultAsync(x => x.Id == id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akPO.NoPO)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

                }
                else
                {
                    // check if already linked with AkBelian
                    //AkBelian akBelian = _context.AkBelian.Where(x => x.AkPOId == id).FirstOrDefault();

                    var akBelian = (from tblBelian in _context.AkBelian.ToList()
                                 join tblPO in _context.AkPO.ToList()
                                 on tblBelian.AkPOId equals tblPO.Id into tblBelianTblPO
                                 from tblBelian_tblPO in tblBelianTblPO.DefaultIfEmpty().Where(x => x.FlHapus != 1)
                                 select new
                                 {
                                     Id = tblBelian.Id,
                                     AkBelianId = tblBelian_tblPO.Id,
                                     AkPOId = tblBelian.AkPOId,
                                     NoInbois = tblBelian.NoInbois

                                 }).Where(x => x.AkBelianId == id).FirstOrDefault();

                    if (akBelian != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada No Inbois " + akBelian.NoInbois.ToUpper() + ". Batal posting tidak dibenarkan";
                    }
                    else
                    {
                        // check if already linked with AkPOLaras
                        AkPOLaras akPOLaras = _context.AkPOLaras.Where(x => x.AkPOId == id).FirstOrDefault();

                        if (akPOLaras != null)
                        {
                            //linkage id error
                            TempData[SD.Error] = "Data terkait pada No Pelarasan Tanggungan " + akPOLaras.NoRujukan.ToUpper() + ". Batal posting tidak dibenarkan";
                        } 
                        else
                        {
                            //unposting operation start here
                            //delete data from akAkaun
                            foreach (AbBukuVot item in abBukuVot)
                            {
                                await _abBukuVotRepo.Delete(item.Id);
                            }

                            //delete data from abBukuVot
                            //foreach (AbBukuVot item in abBukuVot)
                            //{
                            //    await _abBukuVotRepo.Delete(item.Id);
                            //}
                            //delete data from abBukuVot

                            //update posting status in akPO

                            //update AkNotaMinta

                            if (akPO.AkNotaMintaId != null)
                            {
                                AkNotaMinta akNM = await _akNotaMintaRepo.GetById((int)akPO.AkNotaMintaId);

                                akNM.NoCAS = "";
                                akNM.TarikhSeksyenKewangan = null;

                                await _akNotaMintaRepo.Update(akNM);
                            }

                            //update AkNotaMinta end

                            akPO.FlPosting = 0;
                            akPO.TarikhPosting = null;
                            await _akPORepo.Update(akPO);

                            //insert applog
                            await AddLogAsync("UnPosting", "UnPosting Data", akPO.NoPO, (int)id, akPO.Jumlah);

                            //insert applog end

                            await _context.SaveChangesAsync();

                            TempData[SD.Success] = "Data berjaya batal kemaskini dari lejar.";
                            //unposting operation end
                        }

                    }

                }

            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end
        //// POST: AkPO/Cancel/5
        //[Authorize(Policy = "TG001B")]
        //public async Task<IActionResult> Cancel(int id)
        //{
        //    var akPO = await _context.AkPO.FindAsync(id);
        //    // check if already posting redirect back
        //    if (akPO.FlPosting == 1)
        //    {
        //        TempData[SD.Error] = "Akses tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    // check if this data is the last one (for preventing batal purpose)
        //    var lastItem = _context.AkPO.OrderByDescending(x => x.Id).FirstOrDefault();

        //    if (lastItem.Id == akPO.Id)
        //    {
        //        TempData[SD.Warning] = "Anda disarankan untuk hapus data ini. Operasi batal tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    // check end
        //    akPO.FlHapus = 1;

        //    _context.AkPO.Update(akPO);

        //    //insert applog
        //    var user = await _userManager.GetUserAsync(User);

        //    AppLog appLog = new AppLog();

        //    appLog.UserId = user.UserName;
        //    appLog.LgModule = modul + "B";
        //    appLog.LgOperation = "Batal";
        //    appLog.LgNote = modul + " Pesanan Tempatan - Batal";
        //    appLog.NoRujukan = akPO.NoPO;
        //    appLog.Jumlah = akPO.Jumlah;

        //    await _appLog.Insert(appLog);
        //    //insert applog end

        //    await _context.SaveChangesAsync();
        //    TempData[SD.Success] = "Data berjaya dibatalkan..!";
        //    return RedirectToAction(nameof(Index));
        //}
        // POST: AkPV/Cancel/5
        [Authorize(Policy = "TG001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akPORepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            _context.AkPO.Update(obj);

            // Batal operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoPO, (int)id, obj.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // printing resit rasmi by akPO.Id
        [Authorize(Policy = "TG001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkPO akPO = await _akPORepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akPO.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - akPO.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akPO.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);

            POPrintModel data = new POPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.AkPO = akPO;
            //data.AkPO.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            akPO.FlCetak = 1;
            await _akPORepo.Update(akPO);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", akPO.NoPO, (int)id, akPO.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("POPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing resit rasmi end

    }
}
