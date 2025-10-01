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
    public class AkIndenLarasController : Controller
    {
        
        public const string modul = "PI001";
        public const string namamodul = "Pelarasan Inden Kerja";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkInden, int, string> _akIndenRepo;
        private readonly IRepository<AkIndenLaras, int, string> _akIndenLarasRepo;
        private readonly ListViewIRepository<AkIndenLaras1, int> _akIndenLaras1Repo;
        private readonly ListViewIRepository<AkIndenLaras2, int> _akIndenLaras2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly UserService _userService;
        private CartIndenLaras _cart;

        public AkIndenLarasController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkInden, int, string> akIndenRepository,
            IRepository<AkIndenLaras, int, string> akIndenLarasRepository,
            ListViewIRepository<AkIndenLaras1, int> akIndenLaras1Repository,
            ListViewIRepository<AkIndenLaras2, int> akIndenLaras2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            UserService userService,
            CartIndenLaras cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akIndenRepo = akIndenRepository;
            _akIndenLarasRepo = akIndenLarasRepository;
            _akIndenLaras1Repo = akIndenLaras1Repository;
            _akIndenLaras2Repo = akIndenLaras2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
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

        // GET: AkIndenLaras
        [Authorize(Policy = "PI001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tarikh", Value = "Tarikh" },
                new SelectListItem() { Text = "No Rujukan", Value = "NoRujukan" },
                new SelectListItem() { Text = "Nama", Value = "Nama" }
            };

            if (!string.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akIndenLaras = new List<AkIndenLaras>().AsEnumerable();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akIndenLaras = await _akIndenLarasRepo.GetAllIncludeDeletedItemsFiltered(searchString,searchDate1,searchDate2,searchColumn);
            }
            else
            {
                akIndenLaras = await _akIndenLarasRepo.GetAllFiltered(searchString, searchDate1,searchDate2,searchColumn);
            }

            if (!string.IsNullOrEmpty(searchString) || (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!string.IsNullOrEmpty(searchString))
                {

                    ViewBag.SearchData1 = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!string.IsNullOrEmpty(searchDate1) && !string.IsNullOrEmpty(searchDate2))
                {
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

            return View(akIndenLaras);
        }

        // GET: AkIndenLaras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akIndenLaras = await _akIndenLarasRepo.GetByIdIncludeDeletedItems((int)id);

            if (akIndenLaras == null)
            {
                return NotFound();
            }

            AkIndenLarasViewModel viewModel = new AkIndenLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkIndenId = akIndenLaras.AkIndenId;
            viewModel.AkInden = akIndenLaras.AkInden;
            viewModel.Id = akIndenLaras.Id;
            viewModel.Tahun = akIndenLaras.Tahun;
            viewModel.NoRujukan = akIndenLaras.NoRujukan;
            viewModel.Tarikh = akIndenLaras.Tarikh;
            viewModel.Tajuk = akIndenLaras.Tajuk;
            viewModel.JKW = akIndenLaras.JKW;
            viewModel.JKWId = akIndenLaras.JKWId;
            viewModel.JBahagian = akIndenLaras.JBahagian;
            viewModel.JBahagianId = akIndenLaras.JBahagianId;
            viewModel.Jumlah = akIndenLaras.Jumlah;
            viewModel.TarikhPosting = akIndenLaras.TarikhPosting;
            viewModel.FlPosting = akIndenLaras.FlPosting;
            viewModel.FlHapus = akIndenLaras.FlHapus;
            viewModel.FlCetak = akIndenLaras.FlCetak;

            foreach (AkIndenLaras2 item in akIndenLaras.AkIndenLaras2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkIndenLaras1 = akIndenLaras.AkIndenLaras1;
            viewModel.AkIndenLaras2 = akIndenLaras.AkIndenLaras2;

            PopulateTable(id);
            return View(viewModel);
        }

        private void PopulateTable(int? id)
        {
            List<AkIndenLaras1> akIndenLaras1Table = _context.AkIndenLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenLarasId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akIndenLaras1 = akIndenLaras1Table;

            List<AkIndenLaras2> akIndenLaras2Table = _context.AkIndenLaras2
                .Where(b => b.AkIndenLarasId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.akIndenLaras2 = akIndenLaras2Table;
        }

        // GET: AkIndenLaras/Create
        [Authorize(Policy = "PI001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            string prefix = year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkIndenLaras
                        .IgnoreQueryFilters()
                        .Where(x => x.Tahun == year )
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(7));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            CartEmpty();
            PopulateList();
            return View();
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkInden> akIndenList = _context.AkInden.Include(x=> x.AkPembekal).Where(x => x.FlPosting == 1).ToList();
            ViewBag.AkInden = akIndenList;

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
                ViewBag.akInden1 = new List<int>();
                ViewBag.akInden2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        //Function Running Number
        public string RunningNumber(string year)
        {

            string prefix = year ;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkIndenLaras
                        .IgnoreQueryFilters()
                        .Where(x => x.Tahun == year)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(7));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }

        [HttpPost]
        public JsonResult JsonGetKod(string year)
        {
            try
            {
                var result = "";
                if (year == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(year);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // on change no Inden controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoInden(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkInden(id);
                var result = await _akIndenRepo.GetById(id);

                List<AkInden1> akInden1Table = await _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkInden1 item in akInden1Table)
                {
                    result.AkInden1.Add(item);
                }

                List<AkInden2> akInden2Table = await _context.AkInden2
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                foreach (AkInden2 item in akInden2Table)
                {
                    result.AkInden2.Add(item);
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkInden(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkInden1> akInden1Table = _context.AkInden1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkInden1 item in akInden1Table)
            {

                item.AkIndenId = 0;

                _cart.AddItem1(item.AkIndenId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkInden2> akInden2Table = _context.AkInden2
                .AsNoTracking()
                .Where(b => b.AkIndenId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkInden2 item in akInden2Table)
            {
                item.AkIndenId = 0;


                item.Perihal = "PELARASAN -" + item.Perihal;

                _cart.AddItem2(item.AkIndenId,
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
        //on change no Inden controller end

        // IndenST: AkIndenLaras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PI001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkIndenLaras akIndenLaras, int JKWId, int AkIndenId, int JBahagianId)
        {
            AkIndenLaras m = new AkIndenLaras();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akIndenLaras.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akIndenLaras != null && JKWId != 0 && JBahagianId != 0)
                {

                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.NoRujukan = "PI/" + noRujukan;
                    m.Tarikh = akIndenLaras.Tarikh;
                    m.Tajuk = akIndenLaras.Tajuk;
                    m.AkIndenId = AkIndenId;
                    m.TarikhPosting = akIndenLaras.TarikhPosting;
                    m.Jumlah = akIndenLaras.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.Tahun = akIndenLaras.Tahun;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AkIndenLaras1 = _cart.Lines1.ToArray();
                    m.AkIndenLaras2 = _cart.Lines2.ToArray();

                    await _akIndenLarasRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat Pelarasan Tanggungan berjaya ditambah. No Pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            return View(akIndenLaras);
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

        public async Task<JsonResult> SaveAkIndenLaras1(AkIndenLaras1 akIndenLaras1)
        {

            try
            {
                if (akIndenLaras1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akIndenLaras1.AkIndenLarasId,
                                    akIndenLaras1.AkCartaId,
                                    akIndenLaras1.Amaun
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkIndenLaras1(AkIndenLaras1 akIndenLaras1)
        {

            try
            {
                if (akIndenLaras1 != null)
                {

                    _cart.RemoveItem1(akIndenLaras1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkIndenLaras2(AkIndenLaras2 akIndenLaras2)
        {

            try
            {
                if (akIndenLaras2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akIndenLaras2.AkIndenLarasId,
                                   akIndenLaras2.Indek,
                                   akIndenLaras2.Bil,
                                   akIndenLaras2.NoStok,
                                   akIndenLaras2.Perihal,
                                   akIndenLaras2.Kuantiti,
                                   akIndenLaras2.Unit,
                                   akIndenLaras2.Harga,
                                   akIndenLaras2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkIndenLaras2(AkIndenLaras2 akIndenLaras2)
        {

            try
            {
                if (akIndenLaras2 != null)
                {

                    _cart.RemoveItem2(akIndenLaras2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akIndenLaras1
        public JsonResult GetAnItemCartAkIndenLaras1(AkIndenLaras1 akIndenLaras1)
        {

            try
            {
                AkIndenLaras1 data = _cart.Lines1.Where(x => x.AkCartaId == akIndenLaras1.AkCartaId).FirstOrDefault();
                
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akIndenLaras1 end

        //save cart akIndenLaras1
        public JsonResult SaveCartAkIndenLaras1(AkIndenLaras1 akIndenLaras1)
        {

            try
            {

                var akIndenL1 = _cart.Lines1.Where(x => x.AkCartaId == akIndenLaras1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akIndenL1 != null)
                {
                    _cart.RemoveItem1(akIndenLaras1.AkCartaId);

                    _cart.AddItem1(akIndenLaras1.AkIndenLarasId,
                                    akIndenLaras1.AkCartaId,
                                    akIndenLaras1.Amaun
                                    );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akIndenLaras1 end

        // get all item from cart akIndenLaras1
        public JsonResult GetAllItemCartAkIndenLaras1()
        {

            try
            {
                List<AkIndenLaras1> data = _cart.Lines1.ToList();

                foreach (AkIndenLaras1 item in data)
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
        // get all item from cart akIndenLaras1 end

        // get an item from cart akIndenLaras2
        public JsonResult GetAnItemCartAkIndenLaras2(AkIndenLaras2 akIndenLaras2)
        {

            try
            {
                AkIndenLaras2 data = _cart.Lines2.Where(x => x.Indek == akIndenLaras2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akIndenLaras2 end

        //save cart akIndenLaras2
        public JsonResult SaveCartAkIndenLaras2(AkIndenLaras2 akIndenLaras2)
        {

            try
            {

                var akIndenL2 = _cart.Lines2.Where(x => x.Indek == akIndenLaras2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akIndenL2 != null)
                {
                    _cart.RemoveItem2(akIndenLaras2.Indek);

                    _cart.AddItem2(akIndenLaras2.AkIndenLarasId,
                                   akIndenLaras2.Indek,
                                   akIndenLaras2.Bil,
                                   akIndenLaras2.NoStok,
                                   akIndenLaras2.Perihal,
                                   akIndenLaras2.Kuantiti,
                                   akIndenLaras2.Unit,
                                   akIndenLaras2.Harga,
                                   akIndenLaras2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akIndenLaras2 end

        // get all item from cart akIndenLaras2
        public JsonResult GetAllItemCartAkIndenLaras2()
        {

            try
            {
                List<AkIndenLaras2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akIndenLaras2 end

        // function  json Create end

        // GET: AkIndenLaras/Edit/5
        [Authorize(Policy = "PI001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akIndenLaras = await _akIndenLarasRepo.GetById((int)id);

            if (akIndenLaras == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akIndenLaras);
            return View(akIndenLaras);
        }

        // get latest Index number in AkIndenLaras2
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
        // get all item from cart akBelian1 end

        private void PopulateCartFromDb(AkIndenLaras akIndenLaras)
        {
            List<AkIndenLaras1> akIndenLaras1Table = _context.AkIndenLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkIndenLarasId == akIndenLaras.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkIndenLaras1 item in akIndenLaras1Table)
            {
                _cart.AddItem1(item.AkIndenLarasId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkIndenLaras2> akIndenLaras2Table = _context.AkIndenLaras2
                .Where(b => b.AkIndenLarasId == akIndenLaras.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkIndenLaras2 item in akIndenLaras2Table)
            {
                _cart.AddItem2(item.AkIndenLarasId,
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

        // IndenST: AkIndenLaras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PI001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AkIndenLaras akIndenLaras, int JKWId, int AkIndenId, decimal JumlahPerihal, int JBahagianId)
        {
            if (id != akIndenLaras.Id)
            {
                return NotFound();
            }

            if (akIndenLaras.Jumlah == JumlahPerihal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                        AkIndenLaras dataAsal = await _akIndenLarasRepo.GetById(id);

                        // list of input that cannot be change
                        akIndenLaras.Tahun = dataAsal.Tahun;
                        akIndenLaras.JKWId = dataAsal.JKWId;
                        akIndenLaras.JBahagianId = dataAsal.JBahagianId;
                        akIndenLaras.NoRujukan = dataAsal.NoRujukan;
                        akIndenLaras.TarMasuk = dataAsal.TarMasuk;
                        akIndenLaras.UserId = dataAsal.UserId;
                        akIndenLaras.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                        akIndenLaras.FlCetak = 0;
                        // list of input that cannot be change end

                        foreach (AkIndenLaras1 item in dataAsal.AkIndenLaras1)
                        {
                            var model = _context.AkIndenLaras1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkIndenLaras2 item in dataAsal.AkIndenLaras2)
                        {
                            var model = _context.AkIndenLaras2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        decimal jumlahAsal = dataAsal.Jumlah;
                        _context.Entry(dataAsal).State = EntityState.Detached;

                        akIndenLaras.AkIndenLaras1 = _cart.Lines1.ToList();
                        akIndenLaras.AkIndenLaras2 = _cart.Lines2.ToList();

                        akIndenLaras.UserIdKemaskini = user.UserName;
                        akIndenLaras.TarKemaskini = DateTime.Now;
                        akIndenLaras.SuPekerjaKemaskiniId = pekerjaId;

                        _context.Update(akIndenLaras);

                        // insert applog
                        if (jumlahAsal != akIndenLaras.Jumlah)
                        {
                            await AddLogAsync("Ubah","RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" + 
                                Convert.ToDecimal(akIndenLaras.Jumlah).ToString("#,##0.00"), akIndenLaras.NoRujukan, id, akIndenLaras.Jumlah, pekerjaId);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", akIndenLaras.NoRujukan, id, akIndenLaras.Jumlah, pekerjaId);
                        }
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkIndenLarasExists(akIndenLaras.Id))
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
                    if (akIndenLaras.Jumlah != JumlahPerihal)
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
            return View(akIndenLaras);
        }

        // GET: AkIndenLaras/Delete/5
        [Authorize(Policy = "PI001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akIndenLaras = await _akIndenLarasRepo.GetByIdIncludeDeletedItems((int)id);


            if (akIndenLaras == null)
            {
                return NotFound();
            }

            AkIndenLarasViewModel viewModel = new AkIndenLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkIndenId = akIndenLaras.AkIndenId;
            viewModel.AkInden = akIndenLaras.AkInden;
            viewModel.Id = akIndenLaras.Id;
            viewModel.Tahun = akIndenLaras.Tahun;
            viewModel.NoRujukan = akIndenLaras.NoRujukan;
            viewModel.Tarikh = akIndenLaras.Tarikh;
            viewModel.Tajuk = akIndenLaras.Tajuk;
            viewModel.JKW = akIndenLaras.JKW;
            viewModel.JKWId = akIndenLaras.JKWId;
            viewModel.JBahagian = akIndenLaras.JBahagian;
            viewModel.JBahagianId = akIndenLaras.JBahagianId;
            viewModel.Jumlah = akIndenLaras.Jumlah;
            viewModel.TarikhPosting = akIndenLaras.TarikhPosting;
            viewModel.FlPosting = akIndenLaras.FlPosting;
            viewModel.FlHapus = akIndenLaras.FlHapus;
            viewModel.FlCetak = akIndenLaras.FlCetak;

            foreach (AkIndenLaras2 item in akIndenLaras.AkIndenLaras2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkIndenLaras1 = akIndenLaras.AkIndenLaras1;
            viewModel.AkIndenLaras2 = akIndenLaras.AkIndenLaras2;

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akIndenLaras);
            return View(viewModel);
        }

        // IndenST: AkIndenLaras/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PI001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akIndenLaras = await _context.AkIndenLaras.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            akIndenLaras.UserIdKemaskini = user.UserName;
            akIndenLaras.TarKemaskini = DateTime.Now;
            akIndenLaras.SuPekerjaKemaskiniId = pekerjaId;
            // check if already posting redirect back
            if (akIndenLaras.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            akIndenLaras.FlCetak = 0;
            _context.AkIndenLaras.Update(akIndenLaras);

            //insert applog
            await AddLogAsync("Hapus", akIndenLaras.NoRujukan, akIndenLaras.NoRujukan, 0, akIndenLaras.Jumlah, pekerjaId);
            //insert applog end

            _context.AkIndenLaras.Remove(akIndenLaras);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkIndenLarasExists(int id)
        {
            return _context.AkIndenLaras.Any(e => e.Id == id);
        }

        // posting function
        [Authorize(Policy = "PI001T")]
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

                AkIndenLaras akIndenLaras = await _akIndenLarasRepo.GetById((int)id);

                //check for print
                if (akIndenLaras.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AkIndenLaras1> akIndenLaras1 = akIndenLaras.AkIndenLaras1.ToList();

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akIndenLaras.NoRujukan)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here

                    foreach (AkIndenLaras1 item in akIndenLaras1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotPosting = new AbBukuVot()
                        {
                            Tahun = akIndenLaras.Tahun,
                            JKWId = akIndenLaras.JKWId,
                            JBahagianId = akIndenLaras.JBahagianId,
                            Tarikh = akIndenLaras.Tarikh,
                            Kod = akIndenLaras.AkInden.AkPembekal.KodSykt,
                            Penerima = akIndenLaras.AkInden.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = akIndenLaras.NoRujukan,
                            Tanggungan = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotPosting);
                        // insert into AbBukuVot end

                    }

                    //update posting status in akInden
                    akIndenLaras.FlPosting = 1;
                    akIndenLaras.TarikhPosting = DateTime.Now;
                    await _akIndenLarasRepo.Update(akIndenLaras);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", akIndenLaras.NoRujukan, (int)id, akIndenLaras.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "PI001UT")]
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

                AkIndenLaras akIndenLaras = await _akIndenLarasRepo.GetById((int) id);

                // cannot unposting cancelled document AkInden
                if (0 - akIndenLaras.Jumlah == akIndenLaras.AkInden.Jumlah && akIndenLaras.AkInden.FlBatal == 1)
                {
                    TempData[SD.Error] = "Data terkait dengan pembatalan Inden.";
                    return RedirectToAction(nameof(Index));
                }

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akIndenLaras.NoRujukan)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    AkBelian akBelian = _context.AkBelian.Where(x => x.AkIndenId == akIndenLaras.AkIndenId).FirstOrDefault();

                    if (akBelian != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada no Inbois " + akBelian.NoInbois.ToUpper() + ". Batal kelulusan tidak dibenarkan";
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

                        //update posting status in akIndenLaras
                        akIndenLaras.FlPosting = 0;
                        akIndenLaras.TarikhPosting = null;
                        await _akIndenLarasRepo.Update(akIndenLaras);

                        //insert applog
                        await AddLogAsync("UnPosting", "UnPosting Data", akIndenLaras.NoRujukan, (int)id, akIndenLaras.Jumlah, pekerjaId);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kelulusan.";
                        //unposting operation end
                    }

                }

            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        //// IndenST: AkIndenLaras/Cancel/5
        [Authorize(Policy = "PI001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var obj = await _akIndenLarasRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PI/" + obj.NoRujukan)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkIndenId == obj.AkIndenId && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    //}
                }
                else
                {

                    //unposting operation start here

                    //insert contra data into abBukuVot
                    foreach (AkIndenLaras1 item in obj.AkIndenLaras1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotCanceling = new AbBukuVot()
                        {
                            Tahun = obj.Tahun,
                            JKWId = obj.JKWId,
                            JBahagianId = obj.JBahagianId,
                            Tarikh = obj.Tarikh,
                            Kod = obj.AkInden.AkPembekal.KodSykt,
                            Penerima = obj.AkInden.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = obj.NoRujukan,
                            Tanggungan = 0 - item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotCanceling);
                        // insert into AbBukuVot end

                    }

                    //update AkInden

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akIndenLarasRepo.Update(obj);

                    //insert applog
                    await AddLogAsync("Batal", "Batal Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }
                

            }

            return RedirectToAction(nameof(Index));
        }

        //// IndenST: AkIndenLaras/Cancel/5
        [Authorize(Policy = "PI001B")]
        public async Task<IActionResult> CancelAll(int id)
        {
            var obj = await _akIndenLarasRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PI/" + obj.NoRujukan)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // check if already linked with AkBelian
                AkBelian Belian = _context.AkBelian.Where(x => x.AkIndenId == obj.AkIndenId && x.FlBatal == 0).FirstOrDefault();

                if (Belian != null)
                {

                    //linkage id error
                    TempData[SD.Error] = "Data terkait pada No Inbois " + Belian.NoInbois.ToUpper() + ". Batal tidak dibenarkan";
                    //}
                }
                else
                {

                    //unposting operation start here

                    // batal IndenLaras
                    //insert contra data into abBukuVot
                    foreach (AkIndenLaras1 item in obj.AkIndenLaras1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotCanceling = new AbBukuVot()
                        {
                            Tahun = obj.Tahun,
                            JKWId = obj.JKWId,
                            JBahagianId = obj.JBahagianId,
                            Tarikh = obj.Tarikh,
                            Kod = obj.AkInden.AkPembekal.KodSykt,
                            Penerima = obj.AkInden.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = obj.NoRujukan,
                            Tanggungan = 0 - item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotCanceling);
                        // insert into AbBukuVot end

                    }

                    //update AkIndenLaras

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;
                    await _akIndenLarasRepo.Update(obj);

                    // batal Inden
                    //insert contra data into abBukuVot
                    foreach (AkInden1 item in obj.AkInden.AkInden1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotCanceling = new AbBukuVot()
                        {
                            Tahun = obj.AkInden.Tahun,
                            JKWId = obj.AkInden.JKWId,
                            JBahagianId = obj.AkInden.JBahagianId,
                            Tarikh = obj.AkInden.Tarikh,
                            Kod = obj.AkInden.AkPembekal.KodSykt,
                            Penerima = obj.AkInden.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = "Inden/" + obj.AkInden.NoInden,
                            Tanggungan = 0 - item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotCanceling);
                        // insert into AbBukuVot end

                    }

                    //update AkInden

                    obj.AkInden.FlBatal = 1;
                    obj.AkInden.TarBatal = DateTime.Now;
                    await _akIndenRepo.Update(obj.AkInden);

                    //insert applog
                    await AddLogAsync("Batal", "Batal Semua Data", obj.NoRujukan, (int)id, obj.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));
        }

        // printing pelarasan Inden 
        [Authorize(Policy = "PI001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkIndenLaras akIndenLaras = await _akIndenLarasRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akIndenLaras.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - akIndenLaras.Jumlah)).ToUpper();
            }
            else 
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akIndenLaras.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            IndenLarasPrintModel data = new IndenLarasPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkIndenLaras = akIndenLaras;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            akIndenLaras.FlCetak = 1;
            await _akIndenLarasRepo.Update(akIndenLaras);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", akIndenLaras.NoRujukan, id, akIndenLaras.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("IndenLarasPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing pelarasan Inden end
    }
}
