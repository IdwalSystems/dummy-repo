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
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkTunaiCVController : Controller
    {

        public const string modul = "TR001";
        public const string namamodul = "Tunai Keluar";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiCV, int, string> _akTunaiCVRepo;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<AkTunaiLejar, int, string> _akTunaiLejarRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly UserService _userService;
        private CartTunaiCV _cart;

        public AkTunaiCVController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiCV, int, string> akTunaiCVRepository,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository,
            IRepository<AkTunaiLejar, int, string> akTunaiLejarRepository,
            IRepository<SuPekerja, int, string> suPekerjaRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            CustomIRepository<string, int> customRepo,
            UserService userService,
             CartTunaiCV cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akTunaiCVRepo = akTunaiCVRepository;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
            _akTunaiLejarRepo = akTunaiLejarRepository;
            _suPekerjaRepo = suPekerjaRepository;
            _kwRepo = kwRepository;
            _akPembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _customRepo = customRepo;
            _userService = userService;
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

        // GET: AkTunaiCV
        [Authorize(Policy = "TR001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No CV", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Penerima", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akTunaiCV = await _akTunaiCVRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akTunaiCV = await _akTunaiCVRepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akTunaiCV = akTunaiCV.Where(s => s.NoCV.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akTunaiCV = akTunaiCV.Where(s => s.Penerima.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akTunaiCV = akTunaiCV.Where(x => x.Tarikh >= date1
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

            List<AkTunaiCVViewModel> viewModel = new List<AkTunaiCVViewModel>();

            foreach (AkTunaiCV item in akTunaiCV)
            {
                viewModel.Add(new AkTunaiCVViewModel
                {
                    Id = item.Id,
                    KW = item.AkTunaiRuncit.JKW.Kod,
                    AkTunaiRuncit = item.AkTunaiRuncit,
                    NoCV = item.NoCV,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Penerima = item.Penerima,
                    Catatan = item.Catatan,
                    FlPosting = item.FlPosting,
                    FlCetak = item.FlCetak,
                    FlHapus = item.FlHapus
                });
            }
            var lastItem = akTunaiCV.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem != null)
            {
                ViewData["lastItem"] = lastItem.NoCV;
            }
            else
            {
                ViewData["lastItem"] = "NaN";
            }
            return View(viewModel);
        }

        // GET: AkTunaiCV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _akTunaiCVRepo.GetByIdIncludeDeletedItems((int)id);

            if (akTunaiCV == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            return View(akTunaiCV);
        }

        private void PopulateTable(int? id)
        {
            List<AkTunaiCV1> akTunaiCV1Table = _context.AkTunaiCV1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTunaiCVId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTunaiCV1 = akTunaiCV1Table;
        }

        // GET: AkTunaiCV/Create
        [Authorize(Policy ="TR001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kodKaunter = _context.AkTunaiRuncit.FirstOrDefault(x => x.KaunterPanjar == "10001");

            if (kodKaunter == null)
            {
                TempData[SD.Error] = "Tiada kaunter panjar yang berdaftar lagi. Sila berbuat demikian pada modul Pemegang Tunai Runcit";
                return RedirectToAction(nameof(Index));
            }
            var kaunter = kodKaunter.KaunterPanjar;
            var year = DateTime.Now.Year.ToString();
            string prefix = "CV/" + year + kaunter;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTunaiCV
                        .Where(x => x.Tahun == year && x.AkTunaiRuncit.KaunterPanjar == kodKaunter.KaunterPanjar)
                        .Max(x => x.NoCV);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(14));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;
            PopulateList();
            CartEmpty();
            return View();
        }

        private void PopulateList()
        {
            List<AkTunaiRuncit> akTunaiRuncitList = _context.AkTunaiRuncit.OrderBy(b => b.KaunterPanjar).ToList();
            ViewBag.akTunaiRuncit = akTunaiRuncitList;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
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
                ViewBag.akTunaiCV1 = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // function json get no rujukan (running number)
        [HttpPost]
        public JsonResult JsonGetKod(int data, string year)
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
                    var kodKaunter = _context.AkTunaiRuncit.FirstOrDefault(x => x.Id == data);

                    var kaunter = kodKaunter.KaunterPanjar;
                    string prefix = "CV/" + year + kaunter;
                    int x = 1;
                    string noRujukan = prefix + "000000";

                    var LatestNoRujukan = _context.AkTunaiCV
                                .Where(x => x.Tahun == year && x.AkTunaiRuncit.KaunterPanjar == kodKaunter.KaunterPanjar)
                                .Max(x => x.NoCV);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "000000}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(14));
                        x++;
                        noRujukan = string.Format("{0:" + prefix + "000000}", x);
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

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetPembekal(int data)
        {
            try
            {
                var result = await _akPembekalRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // on change kod Pekerja controller
        [HttpPost]
        public async Task<JsonResult> JsonGetPekerja(int data)
        {
            try
            {
                var result = await _suPekerjaRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod Pekerja controller end

        // function  json Create akPV1
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

        public async Task<JsonResult> SaveAkTunaiCV1(AkTunaiCV1 akTunaiCV1, int akTunaiRuncitId)
        {

            try
            {
                if (akTunaiCV1 != null)
                {
                    // check baki
                    decimal baki = await _customRepo.GetBalanceFromKaunterPanjar("BAKI AWAL", akTunaiRuncitId);
                    if(baki < akTunaiCV1.Amaun)
                    {
                        return Json(new { result = "ERROR" });
                    }
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akTunaiCV1.AkTunaiCVId,
                                   akTunaiCV1.Amaun,
                                   akTunaiCV1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTunaiCV1(AkTunaiCV1 akTunaiCV1)
        {

            try
            {
                if (akTunaiCV1 != null)
                {

                    _cart.RemoveItem1(akTunaiCV1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akTunaiCV1
        public JsonResult GetAnItemCartAkTunaiCV1(AkTunaiCV1 akTunaiCV1)
        {

            try
            {
                AkTunaiCV1 data = _cart.Lines1.Where(x => x.AkCartaId == akTunaiCV1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTunaiCV1 end

        //save cart akTunaiCV1
        public JsonResult SaveCartAkTunaiCV1(AkTunaiCV1 akTunaiCV1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akTunaiCV1.AkCartaId).FirstOrDefault();

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akTunaiCV1.AkCartaId);

                    _cart.AddItem1(akTunaiCV1.AkTunaiCVId,
                                   akTunaiCV1.Amaun,
                                   akTunaiCV1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPV1 end

        // get all item from cart akTunaiCV1
        public JsonResult GetAllItemCartAkTunaiCV1()
        {

            try
            {
                List<AkTunaiCV1> data = _cart.Lines1.ToList();

                foreach (AkTunaiCV1 item in data)
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
        // get all item from cart akPV1 end

        // on change NoKP controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoKP(string data)
        {
            try
            {
                var result = await _context.AkTunaiCV
                    .Where(x => x.NoKP == data)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    result = new AkTunaiCV
                    {
                        Penerima = "",
                        Alamat1 = "",
                        Alamat2 = "",
                        Alamat3 = "",
                    };
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change NoKP controller end

        // POST: AkTunaiCV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TR001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkTunaiCV akTunaiCV, int AkTunaiRuncitId, int? AkPembekalId, int? SuPekerjaId)
        {
            AkTunaiCV m = new AkTunaiCV();
            var pembekal = _context.AkPembekal.Find(AkPembekalId);
            var pekerja = _context.SuPekerja.Find(SuPekerjaId);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            //check if user fil in both pekerja and pembekal
            if (pembekal != null && pekerja != null)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Sila isi salah satu kod pekerja atau kod pembekal";
                //PopulateCart();
                CartEmpty();
                PopulateList();
                return View(akTunaiCV);
            }

            if(pembekal != null)
            {
                akTunaiCV.Penerima = pembekal.NamaSykt;
                akTunaiCV.Alamat1 = pembekal.Alamat1;
                akTunaiCV.Alamat2 = pembekal.Alamat2;
                akTunaiCV.Alamat3 = pembekal.Alamat3;
                akTunaiCV.KategoriPenerima = 1;
            }

            if (pekerja != null)
            {
                akTunaiCV.NoKP = pekerja.NoKp;
                akTunaiCV.Penerima = pekerja.Nama;
                akTunaiCV.Alamat1 = pekerja.Alamat1;
                akTunaiCV.Alamat2 = pekerja.Alamat2;
                akTunaiCV.Alamat3 = pekerja.Alamat3;
                akTunaiCV.KategoriPenerima = 2;
            }

            // get latest no rujukan running number  
            var kodKaunter = _context.AkTunaiRuncit.FirstOrDefault(x => x.Id == AkTunaiRuncitId);

            if (kodKaunter == null)
            {
                TempData[SD.Error] = "Tiada kaunter panjar yang berdaftar lagi. Sila berbuat demikian pada modul Pemegang Tunai Runcit";
                return RedirectToAction(nameof(Index));
            }
            var kaunter = kodKaunter.KaunterPanjar;
            var year = DateTime.Now.Year.ToString();
            string prefix = "CV/" + year + kaunter;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTunaiCV
                        .Where(x => x.Tahun == year && x.AkTunaiRuncit.KaunterPanjar == kodKaunter.KaunterPanjar)
                        .Max(x => x.NoCV);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(14));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }


            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akTunaiCV != null && AkTunaiRuncitId != 0 && akTunaiCV.Penerima != null)
                {
                    m.AkTunaiRuncitId = AkTunaiRuncitId;
                    
                    if (AkPembekalId != 0)
                    {
                        m.AkPembekalId = AkPembekalId;
                    }

                    if(SuPekerjaId != 0)
                    {
                        m.SuPekerjaId = SuPekerjaId;
                    }

                    m.Tahun = akTunaiCV.Tahun;
                    m.Tarikh = akTunaiCV.Tarikh;
                    m.NoCV = noRujukan;
                    m.NoKP = akTunaiCV.NoKP;
                    m.Penerima = akTunaiCV.Penerima;
                    m.Alamat1 = akTunaiCV.Alamat1;
                    m.Alamat2 = akTunaiCV.Alamat2;
                    m.Alamat3 = akTunaiCV.Alamat3;
                    if (akTunaiCV.Catatan == null)
                    {
                        m.Catatan = "";
                    }
                    else
                    {
                        m.Catatan = akTunaiCV.Catatan;
                    }

                    m.Jumlah = akTunaiCV.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.KategoriPenerima = akTunaiCV.KategoriPenerima;

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AkTunaiCV1 = _cart.Lines1.ToArray();

                    await _akTunaiCVRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoCV + " - " + m.Penerima, m.NoCV, 0, m.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();
                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
                
            }

            PopulateList();
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Edit/5
        [Authorize(Policy = "TR001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

            if (akTunaiCV == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTunaiCV);
            return View(akTunaiCV);
        }

        private void PopulateCartFromDb(AkTunaiCV akTunaiCV)
        {
            List<AkTunaiCV1> akTunaiCV1Table = _context.AkTunaiCV1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTunaiCVId == akTunaiCV.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTunaiCV1 akTunaiCV1 in akTunaiCV1Table)
            {
                _cart.AddItem1(akTunaiCV1.AkTunaiCVId,
                               akTunaiCV1.Amaun,
                               akTunaiCV1.AkCartaId);
            }

            ViewBag.akTunaiCV1 = akTunaiCV1Table;

        }

        // POST: AkTunaiCV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "TR001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AkTunaiCV akTunaiCV, int AkTunaiRuncitId, int? AkPembekalId, int? SuPekerjaId)
        {
            if (id != akTunaiCV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                    var dataAsal = await _akTunaiCVRepo.GetById(id);
                    var jumlah = dataAsal.Jumlah;

                    switch (akTunaiCV.KategoriPenerima)
                    {
                        case 1:
                            var pembekal = _context.AkPembekal.Find(akTunaiCV.AkPembekalId);
                            akTunaiCV.SuPekerjaId = null;
                            akTunaiCV.Penerima = pembekal.NamaSykt;
                            akTunaiCV.Alamat1 = pembekal.Alamat1;
                            akTunaiCV.Alamat2 = pembekal.Alamat2;
                            akTunaiCV.Alamat3 = pembekal.Alamat3;
                            break;
                        case 2:
                            var pekerja = _context.SuPekerja.Find(akTunaiCV.SuPekerjaId);
                            akTunaiCV.AkPembekalId = null;
                            akTunaiCV.NoKP = pekerja.NoKp;
                            akTunaiCV.Penerima = pekerja.Nama;
                            akTunaiCV.Alamat1 = pekerja.Alamat1;
                            akTunaiCV.Alamat2 = pekerja.Alamat2;
                            akTunaiCV.Alamat3 = pekerja.Alamat3;
                            break;
                        default:
                            akTunaiCV.AkPembekalId = null;
                            akTunaiCV.SuPekerjaId = null; 
                            break;
                    }

                    // list of input that cannot be change
                    akTunaiCV.Tahun = dataAsal.Tahun;
                    akTunaiCV.AkTunaiRuncitId = dataAsal.AkTunaiRuncitId;
                    akTunaiCV.NoCV = dataAsal.NoCV;
                    akTunaiCV.Tarikh = dataAsal.Tarikh;
                    akTunaiCV.TarMasuk = dataAsal.TarMasuk;
                    akTunaiCV.UserId = dataAsal.UserId;
                    akTunaiCV.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akTunaiCV.FlCetak = 0;
                    // list of input that cannot be change end

                    foreach (AkTunaiCV1 item in dataAsal.AkTunaiCV1)
                    {
                        var model = _context.AkTunaiCV1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }
                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akTunaiCV.AkTunaiCV1 = _cart.Lines1.ToList();

                    akTunaiCV.UserIdKemaskini = user.UserName;
                    akTunaiCV.TarKemaskini = DateTime.Now;
                    akTunaiCV.SuPekerjaKemaskiniId = pekerjaId;
                    if (akTunaiCV.Catatan == null)
                    {
                        akTunaiCV.Catatan = "";
                    }

                    _context.Update(akTunaiCV);

                    //insert applog
                    if (jumlahAsal != akTunaiCV.Jumlah)
                    {
                        await AddLogAsync("Ubah","RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" + 
                            Convert.ToDecimal(akTunaiCV.Jumlah).ToString("#,##0.00"), akTunaiCV.NoCV, id, akTunaiCV.Jumlah, pekerjaId);

                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", akTunaiCV.NoCV, id, akTunaiCV.Jumlah, pekerjaId);
                    }
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiCVExists(akTunaiCV.Id))
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
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Delete/5
        [Authorize(Policy = "TR001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

            if (akTunaiCV == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);
            return View(akTunaiCV);
        }

        // POST: AkTunaiCV/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "TR001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            akTunaiCV.UserIdKemaskini = user.UserName;
            akTunaiCV.TarKemaskini = DateTime.Now;
            akTunaiCV.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkTunaiCV.Remove(akTunaiCV);

            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", akTunaiCV.NoCV, id, akTunaiCV.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiCVExists(int id)
        {
            return _context.AkTunaiCV.Any(e => e.Id == id);
        }

        // posting function
        [Authorize(Policy = "TR001T")]
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

                AkTunaiCV akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

                //check for print
                if (akTunaiCV.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AkTunaiCV1> akTunaiCV1 = akTunaiCV.AkTunaiCV1.ToList();

                var akTunaiLejarDuplicate = await _context.AkTunaiLejar.Where(x => x.NoRujukan == akTunaiCV.NoCV).FirstOrDefaultAsync();
                if (akTunaiLejarDuplicate != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //find latest baki
                    AkTunaiLejar akT = _context.AkTunaiLejar
                    .Where(x => x.AkTunaiRuncitId == akTunaiCV.AkTunaiRuncitId)
                    .OrderByDescending(x => x.NoRujukan)
                    .ThenByDescending(x => x.Tarikh)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault();

                    decimal bakiAkhir = 0;

                    if (akT != null)
                    {
                        bakiAkhir = akT.Baki;

                        if (bakiAkhir < akTunaiCV.Jumlah)
                        {
                            TempData[SD.Warning] = "Baki akhir lejar tunai bagi kod kaunter panjar " + akTunaiCV.AkTunaiRuncit.KaunterPanjar + " tidak mencukupi.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        TempData[SD.Error] = "Baki awal belum dimasukkan ke dalam lejar tunai bagi kod kaunter panjar " + akTunaiCV.AkTunaiRuncit.KaunterPanjar+ ". Anda perlu membuat baucer pembayaran terlebih dahulu.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    //posting operation start here
                    foreach (AkTunaiCV1 item in akTunaiCV1)
                    {

                        //insert into AkTunaiLejar
                        AkTunaiLejar akTunaiLejar = new AkTunaiLejar()
                        {
                            JKWId = akTunaiCV.AkTunaiRuncit.JKWId,

                            AkTunaiRuncitId = akTunaiCV.AkTunaiRuncitId,
                            Tarikh = akTunaiCV.Tarikh,
                            AkCartaId = item.AkCartaId,
                            NoRujukan = akTunaiCV.NoCV,
                            Debit = 0,
                            Kredit = item.Amaun,
                            Baki = bakiAkhir - item.Amaun
                        }; 
                        // insert into AkTunaiLejar end

                        await _akTunaiLejarRepo.Insert(akTunaiLejar);
                    }

                    //update posting status in akTerima
                    akTunaiCV.FlPosting = 1;
                    akTunaiCV.TarikhPosting = DateTime.Now;
                    await _akTunaiCVRepo.Update(akTunaiCV);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", akTunaiCV.NoCV,(int) id, akTunaiCV.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TR001UT")]
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

                AkTunaiCV akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

                List<AkTunaiLejar> akTunaiLejar = _context.AkTunaiLejar.Where(x => x.NoRujukan == akTunaiCV.NoCV).ToList();

                if (akTunaiLejar == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    //unposting operation start here
                    //delete data from akTunaiLejar
                    foreach (AkTunaiLejar item in akTunaiLejar)
                    {
                        await _akTunaiLejarRepo.Delete(item.Id);
                    }

                    //update posting status in akTunaiCV
                    akTunaiCV.FlPosting = 0;
                    akTunaiCV.TarikhPosting = null;
                    await _akTunaiCVRepo.Update(akTunaiCV);

                    //insert applog
                    await AddLogAsync("UnPosting", "UnPosting Data", akTunaiCV.NoCV,(int) id, akTunaiCV.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // printing pelarasan PO 
        [Authorize(Policy = "TR001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkTunaiCV obj = await _akTunaiCVRepo.GetByIdIncludeDeletedItems(id);

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
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            TunaiCVPrintModel data = new TunaiCVPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkTunaiCV = obj;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            obj.FlCetak = 1;
            await _akTunaiCVRepo.Update(obj);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", obj.NoCV, id, obj.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("TunaiCVPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "TR001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var obj = await _akTunaiCVRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkTunaiCV.Update(obj);

            // rollback operation end

            //insert applog
            await AddLogAsync("Posting", "Posting Data", obj.NoCV, id, obj.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }
        // printing pelarasan PO end

        //// POST: AkTunaiCV/Cancel/5
        //[Authorize(Policy = "TR001B")]
        //public async Task<IActionResult> Cancel(int id)
        //{
        //    var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
        //    // check if already posting redirect back
        //    if (akTunaiCV.FlPosting == 1)
        //    {
        //        TempData[SD.Error] = "Akses tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    // check if this data is the last one (for preventing batal purpose)
        //    var lastItem = _context.AkTunaiCV.OrderByDescending(x => x.Id).FirstOrDefault();

        //    if (lastItem.Id == akTunaiCV.Id)
        //    {
        //        TempData[SD.Warning] = "Anda disarankan untuk hapus data ini. Operasi batal tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    // check end
        //    akTunaiCV.FlHapus = 1;

        //    _context.AkTunaiCV.Update(akTunaiCV);

        //    //insert applog
        //    var user = await _userManager.GetUserAsync(User);

        //    AppLog appLog = new AppLog();

        //    appLog.UserId = user.UserName;
        //    appLog.LgModule = modul + "B";
        //    appLog.LgOperation = "Batal";
        //    appLog.LgNote = modul + " Penerimaan - Batal";
        //    appLog.NoRujukan = akTunaiCV.NoCV;
        //    appLog.Jumlah = akTunaiCV.Jumlah;

        //    await _appLog.Insert(appLog);
        //    //insert applog end

        //    await _context.SaveChangesAsync();
        //    TempData[SD.Success] = "Data berjaya dibatalkan..!";
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
