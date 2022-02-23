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
    public class AkPOLarasController : Controller
    {
        
        public const string modul = "PT001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly IRepository<AkPOLaras, int, string> _akPOLarasRepo;
        private readonly ListViewIRepository<AkPOLaras1, int> _akPOLaras1Repo;
        private readonly ListViewIRepository<AkPOLaras2, int> _akPOLaras2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkPembekal, int, string> _akpembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JBank, int, string> _jbankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartPOLaras _cart;

        public AkPOLarasController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int, string> akPORepository,
            IRepository<AkPOLaras, int, string> akPOLarasRepository,
            ListViewIRepository<AkPOLaras1, int> akPOLaras1Repository,
            ListViewIRepository<AkPOLaras2, int> akPOLaras2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JBank, int, string> JBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CartPOLaras cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPORepo = akPORepository;
            _akPOLarasRepo = akPOLarasRepository;
            _akPOLaras1Repo = akPOLaras1Repository;
            _akPOLaras2Repo = akPOLaras2Repository;
            _akCartaRepo = akCartaRepository;
            _kwRepo = kwRepository;
            _akpembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _cart = cart;
        }

        // GET: AkPOLaras
        [Authorize(Policy = "PT001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No Rujukan", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Nama", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akPOLaras = await _akPOLarasRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akPOLaras = akPOLaras.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Pembekal")
                    {
                        akPOLaras = akPOLaras.Where(s => s.AkPO.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPOLaras = akPOLaras.Where(x => x.Tarikh >= date1
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

            var lastItem = akPOLaras.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem != null)
            {
                ViewData["lastItem"] = lastItem.NoRujukan;
            }
            else
            {
                ViewData["lastItem"] = "NaN";
            }


            return View(akPOLaras);
        }

        // GET: AkPOLaras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetById((int)id);

            var akPO = await _akPORepo.GetById(akPOLaras.AkPOId);

            if (akPOLaras == null)
            {
                return NotFound();
            }

            AkPOLarasViewModel viewModel = new AkPOLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPOId = akPOLaras.AkPOId;
            viewModel.AkPO = akPO;
            viewModel.Id = akPOLaras.Id;
            viewModel.Tahun = akPOLaras.Tahun;
            viewModel.NoRujukan = akPOLaras.NoRujukan;
            viewModel.Tarikh = akPOLaras.Tarikh;
            viewModel.Tajuk = akPOLaras.Tajuk;
            viewModel.JKW = akPOLaras.JKW;
            viewModel.JKWId = akPOLaras.JKWId;
            viewModel.Jumlah = akPOLaras.Jumlah;
            viewModel.TarikhPosting = akPOLaras.TarikhPosting;
            viewModel.FlPosting = akPOLaras.FlPosting;
            viewModel.FlHapus = akPOLaras.FlHapus;
            viewModel.FlCetak = akPOLaras.FlCetak;

            foreach (AkPOLaras2 item in akPOLaras.AkPOLaras2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkPOLaras1 = akPOLaras.AkPOLaras1;
            viewModel.AkPOLaras2 = akPOLaras.AkPOLaras2;

            // check if this data is the last one (for preventing batal purpose)
            var lastItem = _context.AkPOLaras.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem.Id == akPOLaras.Id)
            {
                ViewData["isLastItem"] = "Y";
            }
            else
            {
                ViewData["isLastItem"] = "N";
            }
            // check end

            PopulateTable(id);
            return View(viewModel);
        }

        private void PopulateTable(int? id)
        {
            List<AkPOLaras1> akPOLaras1Table = _context.AkPOLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOLarasId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPOLaras1 = akPOLaras1Table;

            List<AkPOLaras2> akPOLaras2Table = _context.AkPOLaras2
                .Where(b => b.AkPOLarasId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPOLaras2 = akPOLaras2Table;
        }

        // GET: AkPOLaras/Create
        [Authorize(Policy = "PT001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            string prefix = year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPOLaras
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

            List<AkPO> akPOList = _context.AkPO.Include(x=> x.AkPembekal).Where(x => x.FlPosting == 1).ToList();
            ViewBag.AkPO = akPOList;

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

        //Function Running Number
        private string RunningNumber(string year)
        {

            string prefix = year ;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPOLaras
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

        // on change no PO controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoPO(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkPO(id);
                var result = await _akPORepo.GetById(id);

                List<AkPO1> akPO1Table = await _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkPO1 item in akPO1Table)
                {
                    result.AkPO1.Add(item);
                }

                List<AkPO2> akPO2Table = await _context.AkPO2
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkPO2 item in akPO2Table)
                {
                    result.AkPO2.Add(item);
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkPO(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO1 item in akPO1Table)
            {

                item.AkPOId = 0;

                _cart.AddItem1(item.AkPOId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                .AsNoTracking()
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO2 item in akPO2Table)
            {
                item.AkPOId = 0;

                _cart.AddItem2(item.AkPOId,
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

        // POST: AkPOLaras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PT001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPOLaras akPOLaras, int JKWId, int AkPOId)
        {
            AkPOLaras m = new AkPOLaras();
            var user = await _userManager.GetUserAsync(User);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akPOLaras.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akPOLaras != null && JKWId != 0)
                {

                    m.JKWId = JKWId;
                    m.NoRujukan = "PT/" + noRujukan;
                    m.Tarikh = akPOLaras.Tarikh;
                    m.Tajuk = akPOLaras.Tajuk;
                    m.AkPOId = AkPOId;
                    m.TarikhPosting = akPOLaras.TarikhPosting;
                    m.Jumlah = akPOLaras.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.Tahun = akPOLaras.Tahun;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkPOLaras1 = _cart.Lines1.ToArray();
                    m.AkPOLaras2 = _cart.Lines2.ToArray();

                    await _akPOLarasRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Pelarasan Tanggungan - Tambah";
                    appLog.NoRujukan = "PT/" + noRujukan;
                    appLog.Jumlah = akPOLaras.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat Pelarasan Tanggungan berjaya ditambah. No Pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            return View(akPOLaras);
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

        public async Task<JsonResult> SaveAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                if (akPOLaras1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akPOLaras1.AkPOLarasId,
                                    akPOLaras1.AkCartaId,
                                    akPOLaras1.Amaun
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                if (akPOLaras1 != null)
                {

                    _cart.RemoveItem1(akPOLaras1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                if (akPOLaras2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akPOLaras2.AkPOLarasId,
                                   akPOLaras2.Indek,
                                   akPOLaras2.Baris,
                                   akPOLaras2.Bil,
                                   akPOLaras2.NoStok,
                                   akPOLaras2.Perihal,
                                   akPOLaras2.Kuantiti,
                                   akPOLaras2.Unit,
                                   akPOLaras2.Harga,
                                   akPOLaras2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                if (akPOLaras2 != null)
                {

                    _cart.RemoveItem2(akPOLaras2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akBelian1
        public JsonResult GetAnItemCartAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {
                AkPOLaras1 data = _cart.Lines1.Where(x => x.AkCartaId == akPOLaras1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian1 end

        //save cart akBelian1
        public JsonResult SaveCartAkPOLaras1(AkPOLaras1 akPOLaras1)
        {

            try
            {

                var akPOL1 = _cart.Lines1.Where(x => x.AkCartaId == akPOLaras1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPOL1 != null)
                {
                    _cart.RemoveItem1(akPOLaras1.AkCartaId);

                    _cart.AddItem1(akPOLaras1.AkPOLarasId,
                                    akPOLaras1.AkCartaId,
                                    akPOLaras1.Amaun
                                    );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akBelian1 end

        // get all item from cart akBelian1
        public JsonResult GetAllItemCartAkPOLaras1()
        {

            try
            {
                List<AkPOLaras1> data = _cart.Lines1.ToList();

                foreach (AkPOLaras1 item in data)
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
        // get all item from cart akBelian1 end

        // get an item from cart akBelian2
        public JsonResult GetAnItemCartAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {
                AkPOLaras2 data = _cart.Lines2.Where(x => x.Indek == akPOLaras2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian2 end

        //save cart akBelian2
        public JsonResult SaveCartAkPOLaras2(AkPOLaras2 akPOLaras2)
        {

            try
            {

                var akPOL2 = _cart.Lines2.Where(x => x.Indek == akPOLaras2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPOL2 != null)
                {
                    _cart.RemoveItem2(akPOLaras2.Indek);

                    _cart.AddItem2(akPOLaras2.AkPOLarasId,
                                   akPOLaras2.Indek,
                                   akPOLaras2.Baris,
                                   akPOLaras2.Bil,
                                   akPOLaras2.NoStok,
                                   akPOLaras2.Perihal,
                                   akPOLaras2.Kuantiti,
                                   akPOLaras2.Unit,
                                   akPOLaras2.Harga,
                                   akPOLaras2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akBelian2 end

        // get all item from cart akBelian2
        public JsonResult GetAllItemCartAkPOLaras2()
        {

            try
            {
                List<AkPOLaras2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akBelian2 end

        // function  json Create end

        // GET: AkPOLaras/Edit/5
        [Authorize(Policy = "PT001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetById((int)id);

            if (akPOLaras == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPOLaras);
            return View(akPOLaras);
        }

        // get latest Index number in AkPOLaras2
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

        private void PopulateCartFromDb(AkPOLaras akPOLaras)
        {
            List<AkPOLaras1> akPOLaras1Table = _context.AkPOLaras1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOLarasId == akPOLaras.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPOLaras1 item in akPOLaras1Table)
            {
                _cart.AddItem1(item.AkPOLarasId,
                               item.AkCartaId,
                               item.Amaun);
            }

            List<AkPOLaras2> akPOLaras2Table = _context.AkPOLaras2
                .Where(b => b.AkPOLarasId == akPOLaras.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPOLaras2 item in akPOLaras2Table)
            {
                _cart.AddItem2(item.AkPOLarasId,
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

        // POST: AkPOLaras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PT001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AkPOLaras akPOLaras, int JKWId, int AkPOId, decimal JumlahPerihal)
        {
            if (id != akPOLaras.Id)
            {
                return NotFound();
            }

            if (akPOLaras.Jumlah == JumlahPerihal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);

                        AkPOLaras akPOLarasAsal = await _akPOLarasRepo.GetById(id);

                        // list of input that cannot be change
                        akPOLaras.Tahun = akPOLarasAsal.Tahun;
                        akPOLaras.JKWId = akPOLarasAsal.JKWId;
                        akPOLaras.NoRujukan = akPOLarasAsal.NoRujukan;
                        akPOLaras.TarMasuk = akPOLarasAsal.TarMasuk;
                        akPOLaras.UserId = akPOLarasAsal.UserId;
                        akPOLaras.FlCetak = 0;
                        // list of input that cannot be change end

                        foreach (AkPOLaras1 item in akPOLarasAsal.AkPOLaras1)
                        {
                            var model = _context.AkPOLaras1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkPOLaras2 item in akPOLarasAsal.AkPOLaras2)
                        {
                            var model = _context.AkPOLaras2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        _context.Entry(akPOLarasAsal).State = EntityState.Detached;

                        akPOLaras.AkPOLaras1 = _cart.Lines1.ToList();
                        akPOLaras.AkPOLaras2 = _cart.Lines2.ToList();

                        akPOLaras.UserIdKemaskini = user.UserName;
                        akPOLaras.TarKemaskini = DateTime.Now;

                        _context.Update(akPOLaras);

                        //insert applog
                        AppLog appLog = new AppLog();

                        appLog.UserId = user.UserName;
                        appLog.LgModule = modul + "E";
                        appLog.LgOperation = "Ubah";
                        appLog.LgNote = modul + " Pelarasan Tanggungan - Ubah";
                        appLog.NoRujukan = akPOLaras.NoRujukan;
                        appLog.Jumlah = akPOLaras.Jumlah;

                        await _appLog.Insert(appLog);
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkPOLarasExists(akPOLaras.Id))
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
                    if (akPOLaras.Jumlah != JumlahPerihal)
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
            return View(akPOLaras);
        }

        // GET: AkPOLaras/Delete/5
        [Authorize(Policy = "PT001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPOLaras = await _akPOLarasRepo.GetById((int)id);

            var akPO = await _akPORepo.GetById(akPOLaras.AkPOId);

            if (akPOLaras == null)
            {
                return NotFound();
            }

            AkPOLarasViewModel viewModel = new AkPOLarasViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkPOId = akPOLaras.AkPOId;
            viewModel.AkPO = akPO;
            viewModel.Id = akPOLaras.Id;
            viewModel.Tahun = akPOLaras.Tahun;
            viewModel.NoRujukan = akPOLaras.NoRujukan;
            viewModel.Tarikh = akPOLaras.Tarikh;
            viewModel.Tajuk = akPOLaras.Tajuk;
            viewModel.JKW = akPOLaras.JKW;
            viewModel.JKWId = akPOLaras.JKWId;
            viewModel.Jumlah = akPOLaras.Jumlah;
            viewModel.TarikhPosting = akPOLaras.TarikhPosting;
            viewModel.FlPosting = akPOLaras.FlPosting;
            viewModel.FlHapus = akPOLaras.FlHapus;
            viewModel.FlCetak = akPOLaras.FlCetak;

            foreach (AkPOLaras2 item in akPOLaras.AkPOLaras2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkPOLaras1 = akPOLaras.AkPOLaras1;
            viewModel.AkPOLaras2 = akPOLaras.AkPOLaras2;

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPOLaras);
            return View(viewModel);
        }

        // POST: AkPOLaras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPOLaras = await _context.AkPOLaras.FindAsync(id);
            // check if already posting redirect back
            if (akPOLaras.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            _context.AkPOLaras.Remove(akPOLaras);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkPOLarasExists(int id)
        {
            return _context.AkPOLaras.Any(e => e.Id == id);
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PT001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var akPOLaras = await _context.AkPOLaras.FindAsync(id);
            // check if already posting redirect back
            if (akPOLaras.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            // check if this data is the last one (for preventing batal purpose)
            var lastItem = _context.AkPOLaras.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem.Id == akPOLaras.Id)
            {
                TempData[SD.Warning] = "Anda disarankan untuk hapus data ini. Operasi batal tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            // check end
            akPOLaras.FlHapus = 1;

            _context.AkPOLaras.Update(akPOLaras);

            //insert applog
            var user = await _userManager.GetUserAsync(User);

            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "B";
            appLog.LgOperation = "Batal";
            appLog.LgNote = modul + " Pelarasan Tanggungan - Batal";
            appLog.NoRujukan = akPOLaras.NoRujukan;
            appLog.Jumlah = akPOLaras.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dibatalkan..!";
            return RedirectToAction(nameof(Index));
        }

        // posting function
        [Authorize(Policy = "PT001T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AkPOLaras akPOLaras = await _akPOLarasRepo.GetById((int)id);

                //check for print
                if (akPOLaras.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<AkPOLaras1> akPOLaras1 = akPOLaras.AkPOLaras1.ToList();

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akPOLaras.NoRujukan)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here

                    foreach (AkPOLaras1 item in akPOLaras1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVotPosting = new AbBukuVot()
                        {
                            Tahun = akPOLaras.Tahun,
                            JKWId = akPOLaras.JKWId,
                            Tarikh = akPOLaras.Tarikh,
                            Kod = akPOLaras.AkPO.AkPembekal.KodSykt,
                            Penerima = akPOLaras.AkPO.AkPembekal.NamaSykt,
                            VotId = item.AkCartaId,
                            Rujukan = akPOLaras.NoRujukan,
                            Tanggungan = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVotPosting);
                        // insert into AbBukuVot end

                    }

                    //update posting status in akPO
                    akPOLaras.FlPosting = 1;
                    akPOLaras.TarikhPosting = DateTime.Now;
                    await _akPOLarasRepo.Update(akPOLaras);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "T";
                    appLog.LgOperation = "Posting";
                    appLog.LgNote = modul + " Pelarasan Tanggungan - Posting";
                    appLog.NoRujukan = akPOLaras.NoRujukan;
                    appLog.Jumlah = akPOLaras.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "PT001UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkPOLaras akPOLaras = await _context.AkPOLaras
                    .Include(x => x.AkPO)
                    .Include(x => x.AkPOLaras1).ThenInclude(x => x.AkCarta)
                    .Include(x => x.AkPOLaras2)
                    .FirstOrDefaultAsync(x => x.Id == id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akPOLaras.NoRujukan)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

                }
                else
                {
                    AkBelian akBelian = _context.AkBelian.Where(x => x.AkPOId == akPOLaras.AkPOId).FirstOrDefault();

                    if (akBelian != null)
                    {
                        //linkage id error
                        TempData[SD.Error] = "Data terkait pada no Inbois " + akBelian.NoInbois.ToUpper() + ". Batal posting tidak dibenarkan";
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
                        akPOLaras.FlPosting = 0;
                        akPOLaras.TarikhPosting = null;
                        await _akPOLarasRepo.Update(akPOLaras);

                        //insert applog
                        var user = await _userManager.GetUserAsync(User);

                        AppLog appLog = new AppLog();

                        appLog.UserId = user.UserName;
                        appLog.LgModule = modul + "UT";
                        appLog.LgOperation = "UnPosting";
                        appLog.LgNote = modul + " Pelarasan Tanggungan - UnPosting";
                        appLog.NoRujukan = akPOLaras.NoRujukan;
                        appLog.Jumlah = akPOLaras.Jumlah;

                        await _appLog.Insert(appLog);
                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kemaskini dari lejar.";
                        //unposting operation end
                    }

                }

            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // printing resit rasmi by akPO.Id
        [Authorize(Policy = "PT001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkPOLaras akPOLaras = await _akPOLarasRepo.GetById(id);

            string jumlahDalamPerkataan;

            if (akPOLaras.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - akPOLaras.Jumlah)).ToUpper();
            }
            else 
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akPOLaras.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);

            POLarasPrintModel data = new POLarasPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.AkPOLaras = akPOLaras;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            akPOLaras.FlCetak = 1;
            await _akPOLarasRepo.Update(akPOLaras);

            //insert applog
            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "P";
            appLog.LgOperation = "Cetak";
            appLog.LgNote = modul + " Pelarasan Tanggungan - Cetak";
            appLog.NoRujukan = akPOLaras.NoRujukan;
            appLog.Jumlah = akPOLaras.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("POLarasPrintPdf", data)
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
