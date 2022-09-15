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
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly AkPOLarasController _akPOLarasController;
        private readonly IRepository<AkPOLaras, int, string> _akPoLarasRepo;
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
            CustomIRepository<string, int> customRepo,
            AkPOLarasController akPOLarasController,
            IRepository<AkPOLaras, int, string> akPOLarasRepository,
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
            _customRepo = customRepo;
            _cart = cart;
            _akPOLarasController = akPOLarasController;
            _akPoLarasRepo = akPOLarasRepository;
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

            if (akPO == null)
            {
                return NotFound();
            }

            decimal jumlahPerihal = 0;
            foreach (var item in akPO.AkPO2)
            {
                jumlahPerihal = jumlahPerihal + item.Amaun;
            }

            ViewBag.JumlahPerihal = jumlahPerihal;

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
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            akPO.UserIdKemaskini = user.UserName;
            akPO.TarKemaskini = DateTime.Now;
            akPO.SuPekerjaKemaskiniId = pekerjaId;
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
            await AddLogAsync("Hapus", akPO.NoPO, akPO.NoPO, id, akPO.Jumlah, pekerjaId);
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
                .OrderBy(b => b.Bil)
                .ToListAsync();

                foreach (AkNotaMinta2 item in akNotaMinta2Table)
                {
                    result.AkNotaMinta2.Add(item);
                }

                result.AkNotaMinta2 = result.AkNotaMinta2.OrderBy(b => b.Bil).ToList();

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
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkNotaMinta2 item in akNotaMinta2Table)
            {
                item.AkNotaMintaId = 0;

                _cart.AddItem2(item.AkNotaMintaId,
                               item.Indek,
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

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkPembekal> PembekalList = _context.AkPembekal.OrderBy(b => b.Id).ToList();
            ViewBag.AkPembekal = PembekalList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkNotaMinta> akNotaMintaList = _context.AkNotaMinta.Include(b => b.AkPO)
                .Where(x => x.FlPosting == 1 &&
                x.FlJenis == 0)
                .ToList();

            List<AkNotaMinta> akNMUpdated = new List<AkNotaMinta>();

            foreach ( var item in akNotaMintaList)
            {
                decimal jumlahPO = 0;

                if (item.AkPO.Count() > 0)
                {
                    foreach ( var akPO in item.AkPO)
                    {
                        jumlahPO = jumlahPO + akPO.Jumlah;
                    }
                    if (jumlahPO == item.Jumlah)
                    {
                        continue;
                    }
                    else
                    {
                        akNMUpdated.Add(item);
                    }
                }
                else
                {
                    akNMUpdated.Add(item);
                }
            }

            ViewBag.AkNotaMinta = akNMUpdated;

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
                .OrderBy(b => b.Bil)
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
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkPO2 akPO2 in akPO2Table)
            {
                _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
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
            var kwId = 1;

            var kw = _context.JKW.FirstOrDefault(x => x.Id == kwId);

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            //var year = data.Tahun;
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

            ViewBag.NoRujukan = noRujukan;

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
        public async Task<JsonResult> SaveCartAkPO1(
            AkPO1 akPO1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {

            try
            {

                var akP1 = _cart.Lines1.Where(x => x.AkCartaId == akPO1.AkCartaId).FirstOrDefault();

                if (akP1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akP1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akP1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akPO1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

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
                List<AkPO2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

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
        public async Task<IActionResult> Create(AkPO akPO, int JKWId, int? AkNotaMintaId, int JBahagianId, bool IsInKewangan)
        {

            AkPO m = new AkPO();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            var pembekal = _context.AkPembekal.FirstOrDefault(x => x.Id == akPO.AkPembekalId);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akPO);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akPO != null && JKWId != 0 && JBahagianId != 0)
                {

                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.NoPO = noRujukan;
                    m.Tarikh = akPO.Tarikh;
                    m.AkNotaMintaId = AkNotaMintaId;
                    m.TarikhPosting = akPO.TarikhPosting;
                    m.AkPembekal = pembekal;
                    m.Jumlah = akPO.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.IsInKewangan = IsInKewangan;
                    m.TarikhBekalan = akPO.TarikhBekalan;
                    m.Tahun = akPO.Tahun;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AkPO1 = _cart.Lines1.ToArray();
                    m.AkPO2 = _cart.Lines2.ToArray();

                    // check for baki peruntukan
                    foreach (AkPO1 item in m.AkPO1)
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

                                return View(akPO);
                            }
                        }
                        else
                        {
                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                            PopulateList();
                            CartEmpty();

                            return View(akPO);
                        }
                    }
                    // check for baki peruntukan end

                    await _akPORepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoPO, m.NoPO, 0, m.Jumlah, pekerjaId);
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

        // get latest Index number in AkPO2
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
        public async Task<IActionResult> Edit(int id, AkPO akPO, int JKWId, int JNegeriId, int AkBankId, decimal JumlahPerihal, int? AkNotaMintaId, int JBahagianId)
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
                        int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                        AkPO dataAsal = await _akPORepo.GetById(id);

                        // list of input that cannot be change
                        akPO.Tahun = dataAsal.Tahun;
                        akPO.JKWId = dataAsal.JKWId;
                        akPO.JBahagianId = dataAsal.JBahagianId;
                        akPO.AkNotaMintaId = dataAsal.AkNotaMintaId;
                        akPO.NoPO = dataAsal.NoPO;
                        akPO.TarMasuk = dataAsal.TarMasuk;
                        akPO.UserId = dataAsal.UserId;
                        akPO.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
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

                        // check for baki peruntukan
                        foreach (AkPO1 item in _cart.Lines1)
                        {

                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == akPO.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPO.JKWId && x.JBahagianId == akPO.JBahagianId)
                                .AnyAsync();

                            var carta = _context.AkCarta.Find(item.AkCartaId);

                            if (IsExistAbBukuVot == true)
                            {
                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPO.Tahun, item.AkCartaId, akPO.JKWId, akPO.JBahagianId);

                                if (sum < item.Amaun)
                                {
                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta.Kod + " tidak mencukupi.";
                                    PopulateList();
                                    PopulateTable(id);

                                    return View(akPO);
                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                                PopulateList();
                                PopulateTable(id);

                                return View(akPO);
                            }
                        }

                        // check for baki peruntukan end
                        akPO.UserIdKemaskini = user.UserName;
                        akPO.TarKemaskini = DateTime.Now;
                        akPO.SuPekerjaKemaskiniId = pekerjaId;
                        akPO.FlCetak = 0;

                        _context.Update(akPO);

                        //insert applog
                        if (jumlahAsal != akPO.Jumlah)
                        {
                            await AddLogAsync("Ubah","RM" +  Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" + 
                                Convert.ToDecimal(akPO.Jumlah).ToString("#,##0.00"), akPO.NoPO, id, akPO.Jumlah, pekerjaId);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", akPO.NoPO, id, akPO.Jumlah, pekerjaId);
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

        public async Task<JsonResult> SaveAkPO1(
            AkPO1 akPO1,
            string tahun,
            int jKWId,
            int jBahagianId)
        {

            try
            {
                if (akPO1 != null)
                {
                    // check for baki peruntukan
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == tahun && x.VotId == akPO1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPO1.AkCartaId, jKWId, jBahagianId);

                        if (sum < akPO1.Amaun)
                        {
                            return Json(new { result = "ERROR" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "ERROR" });
                    }
                    // check for baki peruntukan end

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
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                AkPO akPO = await _akPORepo.GetById((int)id);

                //check for print
                if (akPO.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AkPO1> akPO1 = akPO.AkPO1.ToList();

                // check for baki peruntukan
                foreach (AkPO1 item in akPO1)
                {
                    bool IsExistAbBukuVot = await _context.AbBukuVot
                            .Where(x => x.Tahun == akPO.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPO.JKWId && x.JBahagianId == akPO.JBahagianId)
                            .AnyAsync();

                    if (IsExistAbBukuVot == true)
                    {
                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPO.Tahun, item.AkCartaId, akPO.JKWId, akPO.JBahagianId);

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
                // check for baki peruntukan end

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + akPO.NoPO)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

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
                            JBahagianId = akPO.JBahagianId,
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
                    await AddLogAsync("Posting", "Posting Data", akPO.NoPO, (int)id, akPO.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
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
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                AkPO akPO = await _context.AkPO
                    .Include(x => x.AkPembekal)
                    .Include(x => x.AkPO1).ThenInclude(x => x.AkCarta)
                    .Include(x => x.AkPO2)
                    .FirstOrDefaultAsync(x => x.Id == id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + akPO.NoPO)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    // check if already linked with AkBelian
                    AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == id).FirstOrDefault();

                    if (Belian != null)
                    {
                        //var akBelian = (from tblBelian in _context.AkBelian.ToList()
                        //                join tblPO in _context.AkPO.ToList()
                        //                on tblBelian.AkPOId equals tblPO.Id into tblBelianTblPO
                        //                from tblBelian_tblPO in tblBelianTblPO.DefaultIfEmpty().Where(x => x.FlHapus != 1)
                        //                select new
                        //                {
                        //                    Id = tblBelian.Id,
                        //                    AkBelianId = tblBelian_tblPO.Id,
                        //                    AkPOId = tblBelian.AkPOId,
                        //                    NoInbois = tblBelian.NoInbois

                        //                }).Where(x => x.AkBelianId == id).FirstOrDefault();

                        //if (akBelian != null)
                        //{
                            //linkage id error
                            TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal kelulusan tidak dibenarkan";
                        //}
                    }
                    else
                    {
                        // check if already linked with AkPOLaras
                        AkPOLaras akPOLaras = _context.AkPOLaras.Where(x => x.AkPOId == id).FirstOrDefault();

                        if (akPOLaras != null)
                        {
                            //linkage id error
                            TempData[SD.Error] = "Data terkait pada No Pelarasan Tanggungan " + akPOLaras.NoRujukan.ToUpper() + ". Batal kelulusan tidak dibenarkan";
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
                            await AddLogAsync("UnPosting", "UnPosting Data", akPO.NoPO, (int)id, akPO.Jumlah, pekerjaId);

                            //insert applog end

                            await _context.SaveChangesAsync();

                            TempData[SD.Success] = "Data berjaya batal kelulusan.";
                            //unposting operation end
                        }
                    }

                    

                }

            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end
        //// POST: AkPO/Cancel/5
        [Authorize(Policy = "TG001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var obj = await _akPORepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PO/" + obj.NoPO)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkPOId == id && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    //}
                }
                else
                {
                    // check if already linked with AkPOLaras
                    AkPOLaras akPOLaras = _context.AkPOLaras.Where(x => x.AkPOId == id && x.FlBatal == 0).FirstOrDefault();

                    if (akPOLaras != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada No Pelarasan Tanggungan " + akPOLaras.NoRujukan.ToUpper() + ". Batal tidak dibenarkan";
                    }
                    else
                    {
                        //unposting operation start here
                        //insert contra data into abBukuVot
                        foreach (AkPO1 item in obj.AkPO1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotCanceling = new AbBukuVot()
                            {
                                Tahun = obj.Tahun,
                                JKWId = obj.JKWId,
                                JBahagianId = obj.JBahagianId,
                                Tarikh = obj.Tarikh,
                                Kod = obj.AkPembekal.KodSykt,
                                Penerima = obj.AkPembekal.NamaSykt,
                                VotId = item.AkCartaId,
                                Rujukan = "PT/"+ _akPOLarasController.RunningNumber(DateTime.Now.ToString("yyyy")),
                                Tanggungan = 0 - item.Amaun
                            };

                            await _abBukuVotRepo.Insert(abBukuVotCanceling);
                            // insert into AbBukuVot end

                        }

                        //update AkPO

                        obj.FlBatal = 1;
                        obj.TarBatal = DateTime.Now;
                        await _akPORepo.Update(obj);

                        AkPOLaras l = new AkPOLaras();
                        l.JKWId = obj.JKWId;
                        l.JBahagianId = obj.JBahagianId;
                        l.NoRujukan = "PT/"+ _akPOLarasController.RunningNumber(DateTime.Now.ToString("yyyy"));
                        l.Tarikh = DateTime.Now;
                        l.AkPOId = obj.Id;
                        l.TarikhPosting = DateTime.Now;
                        l.Jumlah = 0 - obj.Jumlah;
                        l.FlPosting = 1;
                        l.FlHapus = 0;
                        l.FlCetak = 1;
                        l.Tahun = DateTime.Now.ToString("yyyy");
                        l.UserId = user.UserName;
                        l.TarMasuk = DateTime.Now;
                        l.SuPekerjaMasukId = pekerjaId;

                        List<AkPOLaras1> akPOLaras1 = new List<AkPOLaras1>();
                        foreach (AkPO1 item in obj.AkPO1)
                        {
                            akPOLaras1.Add(new AkPOLaras1
                            {
                                AkCartaId = item.AkCartaId,
                                Amaun = 0 - item.Amaun
                            });
                        }

                        l.AkPOLaras1 = akPOLaras1;

                        List<AkPOLaras2> akPOLaras2 = new List<AkPOLaras2>();

                        foreach (AkPO2 item in obj.AkPO2)
                        {
                            akPOLaras2.Add(new AkPOLaras2
                            {
                                Indek = item.Indek,
                                Bil = item.Bil,
                                NoStok = item.NoStok,
                                Perihal = "PELARASAN - " + item.Perihal,
                                Kuantiti = item.Kuantiti,
                                Unit = item.Unit,
                                Harga = 0 - item.Harga,
                                Amaun = 0 - item.Amaun

                            });
                        }

                        l.AkPOLaras2 = akPOLaras2;

                        await _akPoLarasRepo.Insert(l);

                        //insert applog
                        await AddLogAsync("Batal", "Batal Data", obj.NoPO, (int)id, obj.Jumlah, pekerjaId);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya dibatalkan.";
                        //unposting operation end
                    }
                }



            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "TG001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akPORepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            //check if Nota Minta exist && Posting == 1 or not
            if (obj.AkNotaMintaId != null)
            {
                var nm = await _context.AkNotaMinta.FirstOrDefaultAsync(x => x.Id == obj.AkNotaMintaId && x.FlPosting == 1);

                if (nm == null)
                {
                    TempData[SD.Error] = "Nota minta belum posting / tidak wujud..!";
                    return RedirectToAction(nameof(Index));
                }
            }
            
            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkPO.Update(obj);

            // Batal operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoPO, (int)id, obj.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // printing pesanan tempatan by akPO.Id
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
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            var pekerja = _context.SuPekerja.FirstOrDefault(x => x.Id == namaUser.SuPekerjaId);
            var jawatan = "Super Admin";
            if (pekerja != null)
            {
                jawatan = pekerja.Jawatan;
            }

            POPrintModel data = new POPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.AkPO = akPO;
            //data.AkPO.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = namaUser.Nama;
            data.Jawatan = jawatan;

            //update cetak -> 1
            akPO.FlCetak = 1;
            await _akPORepo.Update(akPO);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", akPO.NoPO, id, akPO.Jumlah, pekerjaId);

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
        // printing pesanan tempatan end

    }
}
