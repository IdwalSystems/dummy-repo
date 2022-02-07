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
    [Authorize]
    public class AkNotaMintaController : Controller
    {
        public const string modul = "NM001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkNotaMinta, int, string> _akNotaMintaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartNotaMinta _cart;

        public AkNotaMintaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkNotaMinta, int, string> akNotaMintaRepository,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<JKW, int, string> kwRepo,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            CartNotaMinta cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akNotaMintaRepo = akNotaMintaRepository;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _abBukuVotRepo = abBukuVotRepository;
            _akCartaRepo = akCartaRepository; 
            _cart = cart;
        }

        // GET: AkNotaMinta
        [Authorize(Policy = "NM001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No Nota Minta", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Nama", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akNotaMinta = await _akNotaMintaRepo.GetAll();

            //var akNotaMinta = await _context.akNotaMinta.ToListAsync();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akNotaMinta = akNotaMinta.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akNotaMinta = akNotaMinta.Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akNotaMinta = akNotaMinta.Where(x => x.Tarikh >= date1
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

            List<AkNotaMintaViewModel> viewModel = new List<AkNotaMintaViewModel>();


            foreach (AkNotaMinta item in akNotaMinta)
            {
                var namaSykt = "";
                var alamat1 = "";

                namaSykt = item.AkPembekal.NamaSykt;
                alamat1 = item.AkPembekal.Alamat1;

                decimal jumlahPerihal = 0;
                foreach (AkNotaMinta2 item2 in item.AkNotaMinta2)
                {
                    jumlahPerihal += item2.Amaun;
                }
                viewModel.Add(new AkNotaMintaViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoRujukan = item.NoRujukan,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    NamaSykt = namaSykt,
                    Alamat1 = alamat1,
                    FlBatal = item.FlBatal,
                    FlPosting = item.FlPosting,
                    JumlahPerihal = jumlahPerihal
                }
                );
            }

            var lastItem = akNotaMinta.OrderByDescending(x => x.Id).FirstOrDefault();

            ViewData["lastItem"] = lastItem.NoRujukan;

            return View(viewModel);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4" && (b.Kod.Substring(0, 1) == "B" || b.Kod.Substring(0, 1) == "A"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {
            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akNotaMinta1 = akNotaMinta1Table;

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akNotaMinta2 = akNotaMinta2Table;
        }
        private void PopulateCart()
        {
            List<AkNotaMinta1> lines1 = _cart.Lines1.ToList();

            foreach (AkNotaMinta1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkNotaMinta2> lines2 = _cart.Lines2.ToList();

            ViewBag.akNotaMinta1 = lines1;
            ViewBag.akNotaMinta2 = lines2;
        }

        private void PopulateCartFromDb(AkNotaMinta akNotaMinta)
        {
            List<AkNotaMinta1> akNotaMinta1Table = _context.AkNotaMinta1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaMintaId == akNotaMinta.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkNotaMinta1 item in akNotaMinta1Table)
            {
                _cart.AddItem1(item.AkNotaMintaId,
                               item.AkCartaId,
                               item.Amaun
                               );
            }

            List<AkNotaMinta2> akNotaMinta2Table = _context.AkNotaMinta2
                .Where(b => b.AkNotaMintaId == akNotaMinta.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkNotaMinta2 item in akNotaMinta2Table)
            {
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

        // GET: AkNotaMinta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int) id);

            // check if this data is the last one (for preventing batal purpose)
            var lastItem = _context.AkNotaMinta.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem.Id == akNotaMinta.Id)
            {
                ViewData["isLastItem"] = "Y";
            }
            else
            {
                ViewData["isLastItem"] = "N";
            }
            // check end

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlBatal = akNotaMinta.FlBatal;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            PopulateList();
            return View(viewModel);
        }

        // GET: AkNotaMinta/Create
        [Authorize(Policy = "NM001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Kod == "100");

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            string prefix = kumpulanWang + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkNotaMinta
                        .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(10));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;

            PopulateList();
            CartEmpty();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akNotaMinta1 = new List<int>();
                ViewBag.akNotaMinta2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

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

        // get an item from cart akNotaMinta1
        public JsonResult GetAnItemCartAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                AkNotaMinta1 data = _cart.Lines1.Where(x => x.AkCartaId == akNotaMinta1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaMinta1 end

        //save cart akNotaMinta1
        public JsonResult SaveCartAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akNotaMinta1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akNotaMinta1.AkCartaId);

                    _cart.AddItem1(akNotaMinta1.AkNotaMintaId,
                                    akNotaMinta1.AkCartaId,
                                    akNotaMinta1.Amaun
                                    );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaMinta1 end

        // get all item from cart akNotaMinta1
        public JsonResult GetAllItemCartAkNotaMinta1()
        {

            try
            {
                List<AkNotaMinta1> data = _cart.Lines1.ToList();

                foreach (AkNotaMinta1 item in data)
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
        // get all item from cart akNotaMinta1 end

        // get an item from cart akNotaMinta2
        public JsonResult GetAnItemCartAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {
                AkNotaMinta2 data = _cart.Lines2.Where(x => x.Indek == akNotaMinta2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaMinta2 end

        //save cart akNotaMinta2
        public JsonResult SaveCartAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akNotaMinta2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akNotaMinta2.Indek);

                    _cart.AddItem2(akNotaMinta2.AkNotaMintaId,
                                   akNotaMinta2.Indek,
                                   akNotaMinta2.Baris,
                                   akNotaMinta2.Bil,
                                   akNotaMinta2.NoStok,
                                   akNotaMinta2.Perihal,
                                   akNotaMinta2.Kuantiti,
                                   akNotaMinta2.Unit,
                                   akNotaMinta2.Harga,
                                   akNotaMinta2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaMinta2 end

        // get all item from cart akNotaMinta2
        public JsonResult GetAllItemCartAkNotaMinta2()
        {

            try
            {
                List<AkNotaMinta2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akNotaMinta2 end

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
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw.Kod;
                    string prefix = kumpulanWang + year;
                    int x = 1;
                    string noRujukan = prefix + "000000";

                    var LatestNoRujukan = _context.AkNotaMinta
                        .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);
                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "000000}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(10));
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

        public async Task<JsonResult> SaveAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                if (akNotaMinta1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akNotaMinta1.AkNotaMintaId,
                                    akNotaMinta1.AkCartaId,
                                    akNotaMinta1.Amaun
                                    );

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaMinta1(AkNotaMinta1 akNotaMinta1)
        {

            try
            {
                if (akNotaMinta1 != null)
                {

                    _cart.RemoveItem1(akNotaMinta1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {
                if (akNotaMinta2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akNotaMinta2.AkNotaMintaId,
                                   akNotaMinta2.Indek,
                                   akNotaMinta2.Baris,
                                   akNotaMinta2.Bil,
                                   akNotaMinta2.NoStok,
                                   akNotaMinta2.Perihal,
                                   akNotaMinta2.Kuantiti,
                                   akNotaMinta2.Unit,
                                   akNotaMinta2.Harga,
                                   akNotaMinta2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaMinta2(AkNotaMinta2 akNotaMinta2)
        {

            try
            {
                if (akNotaMinta2 != null)
                {

                    _cart.RemoveItem2(akNotaMinta2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // function  json Create end

        // POST: AkNotaMinta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "NM001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkNotaMinta akNotaMinta,int JKWId, int AkPembekalId, string NamaPembekal, decimal JumlahPerihal)
        {
            AkNotaMinta m = new AkNotaMinta();
            var user = await _userManager.GetUserAsync(User);

            // checking for jumlah objek & jumlah perihal
            //if (akNotaMinta.Jumlah != JumlahPerihal)
            //{
            //    TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
            //    CartEmpty();
            //    PopulateList();
            //    return View(akNotaMinta);
            //}

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == akNotaMinta.JKWId);

            var kumpulanWang = kw.Kod;
            var year = akNotaMinta.Tahun;
            string prefix = "NM/" + kumpulanWang + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkNotaMinta
                        .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(10));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end


            if (ModelState.IsValid)
            {
                if (akNotaMinta != null && JKWId != 0 && AkPembekalId != 0)
                {
                    m.JKWId = JKWId;
                    m.Tahun = akNotaMinta.Tahun;
                    m.Tajuk = akNotaMinta.Tajuk;
                    m.AkPembekalId = akNotaMinta.AkPembekalId;
                    m.NoRujukan = noRujukan;
                    m.Tarikh = akNotaMinta.Tarikh;
                    m.Jumlah = akNotaMinta.Jumlah;
                    m.FlPosting = 0;
                    m.FlCetak = 0;
                    m.FlBatal = 0;

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkNotaMinta1 = _cart.Lines1.ToArray();
                    m.AkNotaMinta2 = _cart.Lines2.ToArray();

                    await _akNotaMintaRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Nota Minta - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akNotaMinta.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akNotaMinta.NoRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            CartEmpty();
            PopulateList();
            return View(akNotaMinta);
        }

        // GET: AkNotaMinta/Edit/5
        [Authorize(Policy = "NM001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlBatal = akNotaMinta.FlBatal;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateTable(id);
            PopulateList();
            PopulateCartFromDb(akNotaMinta);
            return View(viewModel);
        }

        // POST: AkNotaMinta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "NM001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkNotaMinta akNotaMinta, int JKWId, int AkPembekalId, string NamaPembekal, decimal JumlahPerihal)
        {
            if (id != akNotaMinta.Id)
            {
                return NotFound();
            }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);

                        AkNotaMinta dataAsal = await _akNotaMintaRepo.GetById(id);

                        // list of input that cannot be change
                        akNotaMinta.Tahun = dataAsal.Tahun;
                        akNotaMinta.JKWId = dataAsal.JKWId;
                        akNotaMinta.NoRujukan = dataAsal.NoRujukan;
                        akNotaMinta.NoCAS = dataAsal.NoCAS;
                        akNotaMinta.TarikhSeksyenKewangan = dataAsal.TarikhSeksyenKewangan;
                        akNotaMinta.NoSiri = dataAsal.NoSiri;
                        akNotaMinta.TarMasuk = dataAsal.TarMasuk;
                        akNotaMinta.UserId = dataAsal.UserId;
                        // list of input that cannot be change end

                        foreach (AkNotaMinta2 item in dataAsal.AkNotaMinta2)
                        {
                            var model = _context.AkNotaMinta2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        _context.Entry(dataAsal).State = EntityState.Detached;

                        akNotaMinta.AkNotaMinta1 = dataAsal.AkNotaMinta1;
                        akNotaMinta.AkNotaMinta2 = _cart.Lines2.ToList();

                        akNotaMinta.UserIdKemaskini = user.UserName;
                        akNotaMinta.TarKemaskini = DateTime.Now;

                        _context.Update(akNotaMinta);

                        //insert applog
                        AppLog appLog = new AppLog();

                        appLog.UserId = user.UserName;
                        appLog.LgModule = modul + "E";
                        appLog.LgOperation = "Ubah";
                        appLog.LgNote = modul + " Nota Minta - Ubah";
                        appLog.NoRujukan = akNotaMinta.NoRujukan;
                        appLog.Jumlah = akNotaMinta.Jumlah;

                        await _appLog.Insert(appLog);
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkNotaMintaExists(akNotaMinta.Id))
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
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi";
            PopulateList();
            PopulateTable(id);
            return View(akNotaMinta);
        }

        // GET: AkNotaMinta/Edit/5
        [Authorize(Policy = "NM001E1")]
        public async Task<IActionResult> EditKewangan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlBatal = akNotaMinta.FlBatal;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            // get latest no rujukan running number if not existed 
            if (akNotaMinta.NoSiri == null)
            {
                var year = DateTime.Now.Year.ToString().Substring(2);
                var month = DateTime.Now.Month.ToString();
                string prefix = "/" + month + "/" + year;
                int x = 1;
                string noRujukan = "0000" + prefix;

                var LatestNoRujukan = _context.AkNotaMinta.Where(x => x.NoSiri.EndsWith(prefix))
                            .Max(x => x.NoSiri);

                if (LatestNoRujukan == null)
                {
                    noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                }
                else
                {
                    x = int.Parse(LatestNoRujukan.Substring(0, 4));
                    x++;
                    noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                }
                ViewBag.NoSiri = noRujukan;
            }
            else
            {
                ViewBag.NoSiri = akNotaMinta.NoSiri;
            }
            
            // get latest no rujukan running number end
            

            CartEmpty();
            PopulateTable(id);
            PopulateList();
            PopulateCartFromDb(akNotaMinta);
            return View(viewModel);
        }

        // POST: AkNotaMinta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "NM001E1")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKewangan(int id, AkNotaMinta akNotaMinta, int JKWId, int AkPembekalId, string NamaPembekal, decimal JumlahPerihal)
        {
            if (id != akNotaMinta.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    AkNotaMinta dataAsal = await _akNotaMintaRepo.GetById(id);

                    // get latest no rujukan running number if not existed 
                    if (dataAsal.NoSiri == null)
                    {
                        var year = DateTime.Now.Year.ToString().Substring(2);
                        var month = DateTime.Now.Month.ToString();
                        string prefix = "/" + month + "/" + year;
                        int x = 1;
                        string noRujukan = "0000" + prefix;

                        var LatestNoRujukan = _context.AkNotaMinta.Where(x => x.NoSiri.EndsWith(prefix))
                                    .Max(x => x.NoSiri);

                        if (LatestNoRujukan == null)
                        {
                            noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                        }
                        else
                        {
                            x = int.Parse(LatestNoRujukan.Substring(0, 4));
                            x++;
                            noRujukan = string.Format("{0:" + "0000}", x) + prefix;
                        }
                        akNotaMinta.NoSiri = noRujukan;
                    }
                    else
                    {
                        akNotaMinta.NoSiri = dataAsal.NoSiri;
                    }

                    // get latest no rujukan running number end

                    // list of input that cannot be change
                    akNotaMinta.Tahun = dataAsal.Tahun;
                    akNotaMinta.JKWId = dataAsal.JKWId;
                    akNotaMinta.NoRujukan = dataAsal.NoRujukan;
                    akNotaMinta.AkPembekalId = dataAsal.AkPembekalId;
                    akNotaMinta.Tajuk = dataAsal.Tajuk;
                    akNotaMinta.NoCAS = dataAsal.NoCAS;
                    akNotaMinta.TarikhSeksyenKewangan = dataAsal.TarikhSeksyenKewangan;
                    akNotaMinta.TarMasuk = dataAsal.TarMasuk;
                    akNotaMinta.UserId = dataAsal.UserId;
                    // list of input that cannot be change end

                    foreach (AkNotaMinta1 item in dataAsal.AkNotaMinta1)
                    {
                        var model = _context.AkNotaMinta1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akNotaMinta.AkNotaMinta1 = _cart.Lines1.ToList();
                    akNotaMinta.AkNotaMinta2 = dataAsal.AkNotaMinta2;

                    akNotaMinta.UserIdKemaskini = user.UserName;
                    akNotaMinta.TarKemaskini = DateTime.Now;

                    _context.Update(akNotaMinta);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "E";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Nota Minta - Ubah Bahagian Kewangan";
                    appLog.NoRujukan = akNotaMinta.NoRujukan;
                    appLog.Jumlah = akNotaMinta.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkNotaMintaExists(akNotaMinta.Id))
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
            TempData[SD.Warning] = "Data tidak lengkap. Sila cuba sekali lagi";
            PopulateList();
            PopulateTable(id);
            return View(akNotaMinta);
        }

        // GET: AkNotaMinta/Delete/5
        [Authorize(Policy = "NM001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

            AkNotaMintaViewModel viewModel = new AkNotaMintaViewModel();

            viewModel.Id = akNotaMinta.Id;
            viewModel.AkPembekalId = akNotaMinta.AkPembekalId;
            viewModel.AkPembekal = akNotaMinta.AkPembekal;
            viewModel.Tahun = akNotaMinta.Tahun;
            viewModel.Tarikh = akNotaMinta.Tarikh;
            viewModel.JKW = akNotaMinta.JKW;
            viewModel.JKWId = akNotaMinta.JKWId;
            viewModel.NoRujukan = akNotaMinta.NoRujukan.Substring(3);
            viewModel.Tajuk = akNotaMinta.Tajuk;
            viewModel.NoSiri = akNotaMinta.NoSiri;
            viewModel.NoCAS = akNotaMinta.NoCAS;
            viewModel.TarikhSeksyenKewangan = akNotaMinta.TarikhSeksyenKewangan;
            viewModel.FlPosting = akNotaMinta.FlPosting;
            viewModel.FlCetak = akNotaMinta.FlCetak;
            viewModel.FlBatal = akNotaMinta.FlBatal;

            viewModel.Jumlah = akNotaMinta.Jumlah;
            viewModel.AkNotaMinta1 = akNotaMinta.AkNotaMinta1;
            foreach (AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaMinta2 = akNotaMinta.AkNotaMinta2;

            if (akNotaMinta == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            PopulateList();
            return View(viewModel);
        }

        // POST: AkNotaMinta/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "NM001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akNotaMinta = await _context.AkNotaMinta.FindAsync(id);
            _context.AkNotaMinta.Remove(akNotaMinta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // posting function
        [Authorize(Policy = "NM001T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetById((int)id);

                List<AkNotaMinta1> akNM1 = akNotaMinta.AkNotaMinta1.ToList();

                var akPO = await _context.AkPO.Where(x => x.AkNotaMintaId == id).FirstOrDefaultAsync();
                if (akPO != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke Pesanan Tempatan.";

                }
                else
                {
                    //posting operation start here
                    
                    //update posting status in akTerima
                    akNotaMinta.FlPosting = 1;
                    akNotaMinta.TarikhPosting = DateTime.Now;
                    await _akNotaMintaRepo.Update(akNotaMinta);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "T";
                    appLog.LgOperation = "Posting";
                    appLog.LgNote = modul + " NotaMinta - Posting";
                    appLog.NoRujukan = akNotaMinta.NoRujukan;
                    appLog.Jumlah = akNotaMinta.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya dikemaskini ke Pesanan Tempatan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "TG002UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetById((int) id);

                List<AkPO> akPO = _context.AkPO.Where(x => x.AkNotaMintaId == id).ToList();

                if (akPO == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke Pesanan Tempatan.";

                }
                else
                {
                    //unposting operation start here

                    //update posting status in akTerima
                    akNotaMinta.FlPosting = 0;
                    akNotaMinta.TarikhPosting = null;
                    await _akNotaMintaRepo.Update(akNotaMinta);

                    //insert applog
                    var user = await _userManager.GetUserAsync(User);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "UT";
                    appLog.LgOperation = "UnPosting";
                    appLog.LgNote = modul + " Nota Minta - UnPosting";
                    appLog.NoRujukan = akNotaMinta.NoRujukan;
                    appLog.Jumlah = akNotaMinta.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kemaskini dari Pesanan Tempatan.";
                    //unposting operation end
                }

            }
            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // POST: AkNotaMinta/Cancel/5
        [Authorize(Policy = "NM001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var akNotaMinta = await _context.AkNotaMinta.FindAsync(id);
            // check if already posting redirect back
            if (akNotaMinta.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // check if this data is the last one (for preventing batal purpose)
            var lastItem = _context.AkNotaMinta.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastItem.Id == akNotaMinta.Id)
            {
                TempData[SD.Warning] = "Anda disarankan untuk hapus data ini. Operasi batal tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            // check end
            akNotaMinta.FlBatal = 1;

            _context.AkNotaMinta.Update(akNotaMinta);

            //insert applog
            var user = await _userManager.GetUserAsync(User);

            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "B";
            appLog.LgOperation = "Batal";
            appLog.LgNote = modul + " Nota Minta - Batal";
            appLog.NoRujukan = akNotaMinta.NoRujukan;
            appLog.Jumlah = akNotaMinta.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dibatalkan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkNotaMintaExists(int id)
        {
            return _context.AkNotaMinta.Any(e => e.Id == id);
        }

        // printing Nota Minta
        [Authorize(Policy = "NM001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkNotaMinta akNotaMinta = await _akNotaMintaRepo.GetById(id);

            var jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akNotaMinta.Jumlah)).ToUpper();

            var user = await _userManager.GetUserAsync(User);

            NotaMintaPrintModel data = new NotaMintaPrintModel();


            if (akNotaMinta.TarikhSeksyenKewangan != null)
            {
                data.TarikhKewangan= akNotaMinta.TarikhSeksyenKewangan.ToString();
            }
            else
            {
                data.TarikhKewangan = "";
            }

            CompanyDetails company = new CompanyDetails();

            foreach( AkNotaMinta2 item in akNotaMinta.AkNotaMinta2)
            {
                data.JumlahPerihal += item.Amaun;
            }
            data.CompanyDetail = company;
            data.AkNotaMinta = akNotaMinta;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.username = user.UserName;

            //update cetak -> 1
            akNotaMinta.FlCetak = 1;
            await _akNotaMintaRepo.Update(akNotaMinta);

            //insert applog
            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "P";
            appLog.LgOperation = "Cetak";
            appLog.LgNote = modul + " Nota Minta - Cetak";
            appLog.NoRujukan = akNotaMinta.NoRujukan;
            appLog.Jumlah = akNotaMinta.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("NotaMintaPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing Nota Minta end

    }
}
