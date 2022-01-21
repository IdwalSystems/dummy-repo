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
    [Authorize]
    public class AkTunaiCVController : Controller
    {

        public const string modul = "TR002";

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
            _cart = cart;
        }
        // GET: AkTunaiCV
        [Authorize(Policy = "TR002")]
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
                    FlBatal = item.FlBatal
                });
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

            var akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

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

        public async Task<JsonResult> SaveAkTunaiCV1(AkTunaiCV1 akTunaiCV1)
        {

            try
            {
                if (akTunaiCV1 != null)
                {
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

            //check if user fil in both pekerja and pembekal
            if (pembekal != null && pekerja != null)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Sila isi salah satu kod pekerja atau kod pembekal";
                //PopulateCart();
                CartEmpty();
                PopulateList();
                return View(akTunaiCV);
            }

            var user = await _userManager.GetUserAsync(User);

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
                akTunaiCV.Penerima = pekerja.Nama;
                akTunaiCV.Alamat1 = pekerja.Alamat1;
                akTunaiCV.Alamat2 = pekerja.Alamat2;
                akTunaiCV.Alamat3 = pekerja.Alamat3;
                akTunaiCV.KategoriPenerima = 2;
            }

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
                    m.FlBatal = 0;
                    m.FlCetak = 0;
                    m.KategoriPenerima = akTunaiCV.KategoriPenerima;

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkTunaiCV1 = _cart.Lines1.ToArray();

                    await _akTunaiCVRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Tunai Keluar - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akTunaiCV.Jumlah;

                    await _appLog.Insert(appLog);
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
        [Authorize(Policy = "TR002E")]
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
        [Authorize(Policy = "TR002E")]
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
                    var akTunaiCVAsal = await _akTunaiCVRepo.GetById(id);
                    var jumlah = akTunaiCVAsal.Jumlah;

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
                    akTunaiCV.Tahun = akTunaiCVAsal.Tahun;
                    akTunaiCV.AkTunaiRuncitId = akTunaiCVAsal.AkTunaiRuncitId;
                    akTunaiCV.NoCV = akTunaiCVAsal.NoCV;
                    akTunaiCV.Tarikh = akTunaiCVAsal.Tarikh;
                    akTunaiCV.TarMasuk = akTunaiCVAsal.TarMasuk;
                    akTunaiCV.UserId = akTunaiCVAsal.UserId;
                    // list of input that cannot be change end

                    foreach (AkTunaiCV1 item in akTunaiCVAsal.AkTunaiCV1)
                    {
                        var model = _context.AkTunaiCV1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    _context.Entry(akTunaiCVAsal).State = EntityState.Detached;

                    akTunaiCV.AkTunaiCV1 = _cart.Lines1.ToList();

                    akTunaiCV.UserIdKemaskini = user.UserName;
                    akTunaiCV.TarKemaskini = DateTime.Now;
                    if (akTunaiCV.Catatan == null)
                    {
                        akTunaiCV.Catatan = "";
                    }

                    _context.Update(akTunaiCV);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "E";
                    appLog.LgOperation = "Ubah";
                    if (jumlah != akTunaiCV.Jumlah)
                    {
                        appLog.LgNote = modul + " Tunai Keluar - Ubah Jumlah dari RM" + jumlah + " ke RM" + akTunaiCV.Jumlah;
                    }
                    else
                    {
                        appLog.LgNote = modul + " Tunai Keluar - Ubah";
                    }

                    appLog.NoRujukan = akTunaiCV.NoCV;
                    appLog.Jumlah = akTunaiCV.Jumlah;

                    await _appLog.Insert(appLog);
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
        [Authorize(Policy = "TR002D")]
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
        [Authorize(Policy = "TR002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            _context.AkTunaiCV.Remove(akTunaiCV);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiCVExists(int id)
        {
            return _context.AkTunaiCV.Any(e => e.Id == id);
        }

        // posting function
        [Authorize(Policy = "TR002T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AkTunaiCV akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

                List<AkTunaiCV1> akTunaiCV1 = akTunaiCV.AkTunaiCV1.ToList();

                var akTunaiLejarDuplicate = await _context.AkTunaiLejar.Where(x => x.NoRujukan == akTunaiCV.NoCV).FirstOrDefaultAsync();
                if (akTunaiLejarDuplicate != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";
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
                    }
                    else
                    {
                        TempData[SD.Warning] = "Baki awal belum dimasukkan ke dalam lejar tunai bagi kod kaunter panjar " + akTunaiCV.AkTunaiRuncit.KaunterPanjar+ ". Anda diminta untuk membuat baucer pembayaran melalui paparan ini.";
                        return RedirectToAction(nameof(AkPVController.Create), "AkPV");
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
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "T";
                    appLog.LgOperation = "Posting";
                    appLog.LgNote = modul + " Tunai Keluar - Posting";
                    appLog.NoRujukan = akTunaiCV.NoCV;
                    appLog.Jumlah = akTunaiCV.Jumlah;

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
        [Authorize(Policy = "TR002UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkTunaiCV akTunaiCV = await _akTunaiCVRepo.GetById((int)id);

                List<AkTunaiLejar> akTunaiLejar = _context.AkTunaiLejar.Where(x => x.NoRujukan == akTunaiCV.NoCV).ToList();

                if (akTunaiLejar == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

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
                    var user = await _userManager.GetUserAsync(User);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "UT";
                    appLog.LgOperation = "UnPosting";
                    appLog.LgNote = modul + " Tunai Keluar - UnPosting";
                    appLog.NoRujukan = akTunaiCV.NoCV;
                    appLog.Jumlah = akTunaiCV.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kemaskini dari lejar.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end
    }
}
