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
    public class AkBelianController : Controller
    {

        public const string modul = "TG002";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int> _akBelianRepo;
        private readonly IRepository<AkPembekal, int> _akPembekalRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkPO, int> _akPORepo;
        private readonly ListViewIRepository<AkBelian1, int> _akBelian1Repo;
        private readonly ListViewIRepository<AkBelian2, int> _akBelian2Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int> _abBukuVotRepo;
        private readonly IRepository<AkAkaun, int> _akAkaunRepo;
        private CartBelian _cart;

        public AkBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int> akBelian,
            IRepository<AkPembekal, int> akPembekal,
            IRepository<JKW, int> kwRepo,
            IRepository<AkPO, int> akPORepo,
            ListViewIRepository<AkBelian1, int> akBelian1Repository,
            ListViewIRepository<AkBelian2, int> akBelian2Repository,
            IRepository<AkCarta, int> akCartaRepository,
            IRepository<AbBukuVot, int> abBukuVotRepository,
            IRepository<AkAkaun, int> akAkaunRepository,
            CartBelian cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelian;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _akPORepo = akPORepo;
            _akBelian1Repo = akBelian1Repository;
            _akBelian2Repo = akBelian2Repository;
            _akCartaRepo = akCartaRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _akAkaunRepo = akAkaunRepository;
            _cart = cart;
        }

        // GET: AkBelian
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No Inbois", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Nama", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akBelian = await _akBelianRepo.GetAll();

            //var akBelian = await _context.AkBelian.ToListAsync();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akBelian = akBelian.Where(s => s.NoInbois.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akBelian = akBelian.Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akBelian = akBelian.Where(x => x.Tarikh >= date1
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

            List<AkBelianViewModel> viewModel = new List<AkBelianViewModel>();

            foreach (AkBelian item in akBelian)
            {
                var namaSykt = "";
                var alamat1 = "";

                if(item.AkPOId == null)
                {
                    namaSykt = item.AkPembekal.NamaSykt;
                    alamat1 = item.AkPembekal.Alamat1;
                }
                else
                {
                    namaSykt = item.AkPO.AkPembekal.NamaSykt;
                    alamat1 = item.AkPO.AkPembekal.Alamat1;
                }

                decimal jumlahPerihal = 0;
                foreach (AkBelian2 item2 in item.AkBelian2)
                {
                    jumlahPerihal += item2.Amaun;
                }
                viewModel.Add( new AkBelianViewModel
                    {
                        Id = item.Id,
                        Tahun = item.Tahun,
                        NoInbois = item.NoInbois,
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

            return View(viewModel);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPO> akPOList = _context.AkPO
                .Include(b => b.AkPembekal).ThenInclude(b=>b.JBank)
                .Include(b => b.JKW)
                .Include(b => b.AkPO1).ThenInclude(b=> b.AkCarta)
                .Include(b => b.AkPO2)
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();
            ViewBag.AkPO = akPOList;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4" && (b.Kod.Substring(0, 1) == "B" || b.Kod.Substring(0,1) == "A"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

            List<AkCarta> KodObjekAPList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4" && (b.Kod.Substring(0, 1) == "L"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.KodObjekAP = KodObjekAPList;

        }

        private void PopulateTable(int? id)
        {
            List<AkBelian1> akBelian1Table = _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akBelian1 = akBelian1Table;

            List<AkBelian2> akBelian2Table = _context.AkBelian2
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akBelian2 = akBelian2Table;
        }
        private void PopulateCart()
        {
            List<AkBelian1> lines1 = _cart.Lines1.ToList();

            foreach(AkBelian1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkBelian2> lines2 = _cart.Lines2.ToList();

            ViewBag.akBelian1 = lines1;
            ViewBag.akBelian2 = lines2;
        }

        private void PopulateCartFromDb(AkBelian akBelian)
        {
            List<AkBelian1> akBelian1Table = _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == akBelian.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkBelian1 item in akBelian1Table)
            {
                _cart.AddItem1(item.AkBelianId,
                               item.Amaun,
                               item.AkCartaId);
            }

            List<AkBelian2> akBelian2Table = _context.AkBelian2
                .Where(b => b.AkBelianId == akBelian.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkBelian2 item in akBelian2Table)
            {
                _cart.AddItem2(item.AkBelianId,
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

        // GET: AkBelian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akBelian.JKWId);

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
            } else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };     
            }

            var pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);

            if (akBelian == null)
            {
                return NotFound();
            }

            AkBelianViewModel akBelianView = new AkBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            akBelianView.AkPembekalId = akBelian.AkPembekalId;
            akBelianView.AkPO = akPO;
            akBelianView.AkPembekal = pembekal;
            akBelianView.JKW = kw;
            akBelianView.Id = akBelian.Id;
            akBelianView.Tahun = akBelian.Tahun;
            akBelianView.NoInbois = akBelian.NoInbois;
            akBelianView.Tarikh = akBelian.Tarikh;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlBatal = akBelian.FlBatal;

            foreach (AkBelian2 item in akBelian.AkBelian2)
            {
                akBelianView.JumlahPerihal += item.Amaun;
            }
            akBelianView.AkBelian2 = akBelian.AkBelian2;

            PopulateTable(id);
            return View(akBelianView);
        }

        // GET: AkBelian/Create
        public IActionResult Create()
        {
            
            PopulateList();
            CartEmpty();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akBelian1 = new List<int>();
                ViewBag.akBelian2 = new List<int>();
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

            List<AkPO1> akPO1Table =  _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO1 item in akPO1Table)
            {

                item.AkPOId = 0;
                item.UserId = user;
                item.TarMasuk = DateTime.Now;

                _cart.AddItem1(item.AkPOId,
                               item.Amaun,
                               item.AkCartaId);
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                .AsNoTracking()
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO2 item in akPO2Table)
            {
                item.AkPOId = 0;
                item.UserId = user;
                item.TarMasuk = DateTime.Now;

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

        // get an item from cart akBelian1
        public JsonResult GetAnItemCartAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                AkBelian1 data = _cart.Lines1.Where(x => x.AkCartaId == akBelian1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian1 end

        //save cart akBelian1
        public JsonResult SaveCartAkBelian1(AkBelian1 akBelian1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akBelian1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akBelian1.AkCartaId);

                    _cart.AddItem1(akBelian1.AkBelianId,
                                    akBelian1.Amaun,
                                    akBelian1.AkCartaId);
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
        public JsonResult GetAllItemCartAkBelian1()
        {

            try
            {
                List<AkBelian1> data = _cart.Lines1.ToList();

                foreach (AkBelian1 item in data)
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
        public JsonResult GetAnItemCartAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                AkBelian2 data = _cart.Lines2.Where(x => x.Indek == akBelian2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akBelian2 end

        //save cart akBelian2
        public JsonResult SaveCartAkBelian2(AkBelian2 akBelian2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akBelian2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akBelian2.Indek);

                    _cart.AddItem2(akBelian2.AkBelianId,
                                   akBelian2.Indek,
                                   akBelian2.Baris,
                                   akBelian2.Bil,
                                   akBelian2.NoStok,
                                   akBelian2.Perihal,
                                   akBelian2.Kuantiti,
                                   akBelian2.Unit,
                                   akBelian2.Harga,
                                   akBelian2.Amaun);
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
        public JsonResult GetAllItemCartAkBelian2()
        {

            try
            {
                List<AkBelian2> data = _cart.Lines2.OrderBy(b=> b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akBelian2 end

        // POST: AkBelian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBelian akBelian, int JKWId, int AkPOId, int AkPembekalId, int KodObjekAPId, string NamaPembekal, decimal JumlahPerihal)
        {
            AkBelian m = new AkBelian();
            var user = await _userManager.GetUserAsync(User);

            var noRujukan = "IN/" + akBelian.NoInbois;

            var akPo = await _akPORepo.GetById(AkPOId);
            // checking for existing no rujukan
            var countNoRujukan = _context.AkBelian.Where(x => x.NoInbois == noRujukan).Count();

            if (countNoRujukan > 0)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. No rujukan pendaftaran " + akBelian.NoInbois + " telah wujud";
                //PopulateCart();
                PopulateList();
                CartEmpty();
                return View(akBelian);
            }

            // checking for jumlah objek & jumlah perihal
            if ( akBelian.Jumlah != JumlahPerihal)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                CartEmpty();
                PopulateList();
                return View(akBelian);
            }
            if (ModelState.IsValid)
            {
                if (akBelian != null && JKWId != 0 && AkPembekalId != 0 && KodObjekAPId != 0)
                {
                    m.KodObjekAPId = KodObjekAPId;
                    m.JKWId = JKWId;
                    m.Tahun = akBelian.Tahun;
                    m.NoInbois = noRujukan;
                    m.Tarikh = akBelian.Tarikh;
                    m.Jumlah = akBelian.Jumlah;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
                    if (akPo != null)
                    {
                        m.FlPO = "1";
                        m.AkPOId = AkPOId;
                        m.AkPembekalId = akPo.AkPembekalId;
                    }
                    else
                    {
                        m.FlPO = "0";
                        m.AkPembekalId = AkPembekalId;
                    }

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkBelian1 = _cart.Lines1.ToArray();
                    m.AkBelian2 = _cart.Lines2.ToArray();

                    await _akBelianRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Inbois Pembekal - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akBelian.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akBelian.NoInbois;
                    return RedirectToAction(nameof(Index));
                }
            }

            CartEmpty();
            PopulateList();
            return View(akBelian);
        }

        // GET: AkBelian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akBelian.JKWId);
            akBelian.JKW = kw;
            var KodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);
            akBelian.KodObjekAP = KodObjekAkaunPemiutang;
            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
            }
            else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };
            }

            akBelian.AkPO = akPO;

            var pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);
            akBelian.AkPembekal = pembekal;

            if (akBelian == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akBelian);
            return View(akBelian);
        }

        // update add akBelian1
        public async Task<JsonResult> InsertUpdateAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null || akBelian1.Amaun != 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akBelian1.AkCartaId);
                    akBelian1.AkCarta = akCarta;

                    await _akBelian1Repo.Insert(akBelian1);

                    decimal total = 0;

                    AkBelian akBelian = await _akBelianRepo.GetById(akBelian1.AkBelianId);

                    total = akBelian.Jumlah + akBelian1.Amaun;

                    akBelian.Jumlah = total;
                    akBelian.UserIdKemaskini = user.UserName;

                    await _akBelianRepo.Update(akBelian);

                    await _context.SaveChangesAsync();

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update add akBelian1 end

        // update add akBelian2
        public async Task<JsonResult> InsertUpdateAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null || akBelian2.Amaun != 0)
                {
                    var user = await _userManager.GetUserAsync(User);

                    await _akBelian2Repo.Insert(akBelian2);

                    await _context.SaveChangesAsync();

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update add akBelian2 end

        // update remove akBelian1
        public async Task<JsonResult> RemoveUpdateAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akB1 = await _context.AkBelian1.FirstOrDefaultAsync(x => x.AkCartaId == akBelian1.AkCartaId && x.AkBelianId == akBelian1.AkBelianId);
                    _context.AkBelian1.Remove(akB1);

                    decimal total = 0;

                    AkBelian akBelian = await _akBelianRepo.GetById(akBelian1.AkBelianId);

                    total = akBelian.Jumlah - akB1.Amaun;

                    akBelian.Jumlah = total;
                    akBelian.UserIdKemaskini = user.UserName;
                    akBelian.TarKemaskini = DateTime.Now;
                    await _akBelianRepo.Update(akBelian);

                    //insert applog
                    var akCarta = await _akCartaRepo.GetById(akB1.AkCartaId);

                    AppLog appLog = new AppLog();
                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Inbois Pembekal - Hapus Objek";
                    appLog.NoRujukan = akBelian.NoInbois + "/" + akCarta.Kod;
                    appLog.Jumlah = akB1.Amaun;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();


                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update remove akBelian1 end

        // update remove akBelian2
        public async Task<JsonResult> RemoveUpdateAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akB2 = await _context.AkBelian2.FirstOrDefaultAsync(x => x.Indek == akBelian2.Indek && x.AkBelianId == akBelian2.AkBelianId);
                    _context.AkBelian2.Remove(akB2);

                    AkBelian akBelian = await _akBelianRepo.GetById(akBelian2.AkBelianId);

                    //insert applog
                    AppLog appLog = new AppLog();
                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Inbois Pembekal - Hapus Objek";
                    appLog.NoRujukan = akBelian.NoInbois + "/" + akBelian2.Indek;
                    appLog.Jumlah = akB2.Amaun;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update remove akBelian2 end

        // update update akBelian1
        public async Task<JsonResult> UpdateAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                AkBelian1 data = await _akBelian1Repo.GetBy2Id(akBelian1.AkBelianId, akBelian1.AkCartaId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkBelian1(AkBelian1 akBelian1)
        {

            try
            {

                AkBelian1 akB1 = await _akBelian1Repo.GetById(akBelian1.Id);

                decimal originalAmount = akB1.Amaun;
                var user = await _userManager.GetUserAsync(User);

                akB1.Amaun = akBelian1.Amaun;
                _context.AkBelian1.Update(akB1);

                // update total akBelian with date updated and userUpdated
                var akBelian = await _akBelianRepo.GetById(akBelian1.AkBelianId);
                decimal total = 0;

                total = akBelian.Jumlah - originalAmount + akB1.Amaun;
                akBelian.Jumlah = total;
                akBelian.UserIdKemaskini = user.UserName;
                akBelian.TarKemaskini = DateTime.Now;
                await _akBelianRepo.Update(akBelian);
                // update total akTerima with date updated and userUpdated end

                //insert applog
                if (akBelian1.Amaun != originalAmount)
                {
                    var akCarta = await _akCartaRepo.GetById(akB1.AkCartaId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "EE";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Invois Pembekal - Ubah Objek";
                    appLog.NoRujukan = akBelian.NoInbois + "/" + akCarta.Kod + " Dari Amaun RM" + originalAmount.ToString() + " ke RM" + akBelian1.Amaun.ToString();
                    appLog.Jumlah = akB1.Amaun;

                    await _appLog.Insert(appLog);
                }
                //insert applog end

                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update update akBelian1 end

        // get cart for updated akBelian1
        public async Task<JsonResult> GetAkBelian1(AkBelian1 akBelian1)
        {
            try
            {
                AkBelian data = await _context.AkBelian
                    .Include(x => x.AkBelian1).ThenInclude(x => x.AkCarta)
                    .FirstOrDefaultAsync(x => x.Id == akBelian1.AkBelianId);

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get cart for updated akBelian1 end

        // update update akBelian2
        public async Task<JsonResult> UpdateAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                AkBelian2 data = await _akBelian2Repo.GetBy2Id(akBelian2.AkBelianId, akBelian2.Indek);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkBelian2(AkBelian2 akBelian2)
        {

            try
            {

                AkBelian2 akB2 = await _akBelian2Repo.GetById(akBelian2.Id);

                decimal originalAmount = akB2.Amaun;
                var user = await _userManager.GetUserAsync(User);

                akB2.Baris = akBelian2.Baris;
                akB2.Bil = akBelian2.Bil;
                akB2.NoStok = akBelian2.NoStok;
                akB2.Perihal = akBelian2.Perihal;
                akB2.Kuantiti = akBelian2.Kuantiti;
                akB2.Unit = akBelian2.Unit;
                akB2.Harga = akBelian2.Harga;
                akB2.Amaun = akBelian2.Amaun;
                _context.Update(akB2);

                var akBelian = await _akBelianRepo.GetById(akBelian2.AkBelianId);

                //insert applog
                if (akBelian2.Amaun != originalAmount)
                {
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "EE";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Invois Pembekal - Ubah Objek";
                    appLog.NoRujukan = akBelian.NoInbois + "/" + akBelian2.Indek + " Dari Amaun RM" + originalAmount.ToString() + " ke RM" + akBelian2.Amaun.ToString();
                    appLog.Jumlah = akBelian2.Amaun;

                    await _appLog.Insert(appLog);
                }
                //insert applog end

                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update update akBelian1 end

        // get cart for updated akBelian1
        public async Task<JsonResult> GetAkBelian2(AkBelian2 akBelian2)
        {
            try
            {
                AkBelian data = await _context.AkBelian
                    .Include(x => x.AkBelian2)
                    .FirstOrDefaultAsync(x => x.Id == akBelian2.AkBelianId);

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get cart for updated akBelian1 end

        // POST: AkBelian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBelian akBelian, int JKWId, int AkBankId, int KodObjekAPId, decimal JumlahPerihal)
        {
            if (id != akBelian.Id)
            {
                return NotFound();
            }

            if (akBelian.Jumlah == JumlahPerihal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);

                        AkBelian akBelianAsal = await _akBelianRepo.GetById(id);

                        // list of input that cannot be change
                        akBelian.Tahun = akBelianAsal.Tahun;
                        akBelian.JKWId = akBelianAsal.JKWId;
                        akBelian.NoInbois = akBelianAsal.NoInbois;
                        akBelian.AkPembekalId = akBelianAsal.AkPembekalId;
                        akBelian.TarMasuk = akBelianAsal.TarMasuk;
                        akBelian.UserId = akBelianAsal.UserId;
                        akBelian.KodObjekAPId = akBelianAsal.KodObjekAPId;
                        // list of input that cannot be change end

                        foreach (AkBelian1 item in akBelianAsal.AkBelian1)
                        {
                            var model = _context.AkBelian1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkBelian2 item in akBelianAsal.AkBelian2)
                        {
                            var model = _context.AkBelian2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        _context.Entry(akBelianAsal).State = EntityState.Detached;

                        akBelian.AkBelian1 = _cart.Lines1.ToList();
                        akBelian.AkBelian2 = _cart.Lines2.ToList();

                        akBelian.UserIdKemaskini = user.UserName;
                        akBelian.TarKemaskini = DateTime.Now;

                        _context.Update(akBelian);

                        //insert applog
                        AppLog appLog = new AppLog();

                        appLog.UserId = user.UserName;
                        appLog.LgModule = modul + "E";
                        appLog.LgOperation = "Ubah";
                        appLog.LgNote = modul + " Inbois Pembekal - Ubah";
                        appLog.NoRujukan = akBelian.NoInbois;
                        appLog.Jumlah = akBelian.Jumlah;

                        await _appLog.Insert(appLog);
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkBelianExists(akBelian.Id))
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
                    if (akBelian.Jumlah != JumlahPerihal)
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
            return View(akBelian);
        }

        // GET: AkBelian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akBelian.JKWId);

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetById((int)akBelian.AkPOId);
            }
            else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };
            }

            var pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);

            if (akBelian == null)
            {
                return NotFound();
            }

            AkBelianViewModel akBelianView = new AkBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            akBelianView.AkPembekalId = akBelian.AkPembekalId;
            akBelianView.AkPO = akPO;
            akBelianView.AkPembekal = pembekal;
            akBelianView.JKW = kw;
            akBelianView.Id = akBelian.Id;
            akBelianView.Tahun = akBelian.Tahun;
            akBelianView.NoInbois = akBelian.NoInbois;
            akBelianView.Tarikh = akBelian.Tarikh;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlBatal = akBelian.FlBatal;

            foreach (AkBelian2 item in akBelian.AkBelian2)
            {
                akBelianView.JumlahPerihal += item.Amaun;
            }
            akBelianView.AkBelian2 = akBelian.AkBelian2;

            PopulateTable(id);
            return View(akBelianView);
        }

        // POST: AkBelian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBelian = await _context.AkBelian.FindAsync(id);
            _context.AkBelian.Remove(akBelian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBelianExists(int id)
        {
            return _context.AkBelian.Any(e => e.Id == id);
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

        public async Task<JsonResult> SaveAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akBelian1.AkBelianId,
                                    akBelian1.Amaun,
                                    akBelian1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {

                    _cart.RemoveItem1(akBelian1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akBelian2.AkBelianId,
                                   akBelian2.Indek,
                                   akBelian2.Baris,
                                   akBelian2.Bil,
                                   akBelian2.NoStok,
                                   akBelian2.Perihal,
                                   akBelian2.Kuantiti,
                                   akBelian2.Unit,
                                   akBelian2.Harga,
                                   akBelian2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkBelian2(AkBelian2 akBelian2)
        {

            try
            {
                if (akBelian2 != null)
                {

                    _cart.RemoveItem2(akBelian2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // function  json Create end

        // posting function
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                AkBelian akBelian = await _context.AkBelian
                    .Include(x=> x.KodObjekAP)
                    .Include(x=> x.AkPembekal)
                    .Include(x => x.AkBelian1).ThenInclude(x => x.AkCarta)
                    .FirstOrDefaultAsync(x => x.Id == id);

                List<AkBelian1> akB1 = akBelian.AkBelian1.ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akBelian.NoInbois).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here

                    var kodPembekal = "";
                    var penerima = "";

                    if (akBelian.AkPembekalId != 0)
                    {
                        kodPembekal = akBelian.AkPembekal.KodSykt;
                        penerima = akBelian.AkPembekal.NamaSykt;
                    }

                    foreach (AkBelian1 item in akB1)
                    {
                        //insert into AbBukuVot
                        AbBukuVot abBukuVot = new AbBukuVot()
                        {
                            Tahun = akBelian.Tahun,
                            JKWId = akBelian.JKWId,
                            Tarikh = akBelian.Tarikh,
                            Kod = kodPembekal,
                            Penerima = penerima,
                            VotId = item.AkCartaId,
                            Rujukan = akBelian.NoInbois,
                            Liabiliti = item.Amaun
                        };

                        await _abBukuVotRepo.Insert(abBukuVot);
                        // insert into AbBukuVot end

                        //insert into akAkaun
                        AkAkaun akAKredit = new AkAkaun()
                        {
                            NoRujukan = akBelian.NoInbois,
                            JKWId = akBelian.JKWId,
                            AkCartaId1 = akBelian.KodObjekAPId,
                            AkCartaId2 = item.AkCartaId,
                            Tarikh = akBelian.Tarikh,
                            Kredit = item.Amaun
                        };

                        await _akAkaunRepo.Insert(akAKredit);

                        AkAkaun akADebit = new AkAkaun()
                        {
                            NoRujukan = akBelian.NoInbois,
                            JKWId = akBelian.JKWId,
                            AkCartaId1 = item.AkCartaId,
                            AkCartaId2 = akBelian.KodObjekAPId,
                            Tarikh = akBelian.Tarikh,
                            Debit = item.Amaun
                        };

                        await _akAkaunRepo.Insert(akADebit);
                    }
                    
                    //update posting status in akTerima
                    akBelian.FlPosting = 1;
                    akBelian.TarikhPosting = DateTime.Now;
                    await _akBelianRepo.Update(akBelian);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "T";
                    appLog.LgOperation = "Posting";
                    appLog.LgNote = modul + " Inbois Pembekal - Posting";
                    appLog.NoRujukan = akBelian.NoInbois;
                    appLog.Jumlah = akBelian.Jumlah;

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
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkBelian akBelian = await _context.AkBelian
                    .Include(x=> x.KodObjekAP)
                    .Include(x => x.AkBelian1).ThenInclude(x => x.AkCarta)
                    .FirstOrDefaultAsync(x => x.Id == id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akBelian.NoInbois).ToList();

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akBelian.NoInbois).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

                }
                else
                {
                    //unposting operation start here
                    //delete data from akAkaun
                    foreach (AkAkaun item in akAkaun)
                    {
                        await _akAkaunRepo.Delete(item.Id);
                    }

                    //delete data from abBukuVot
                    foreach (AbBukuVot item in abBukuVot)
                    {
                        await _abBukuVotRepo.Delete(item.Id);
                    }
                    //delete data from abBukuVot

                    //update posting status in akTerima
                    akBelian.FlPosting = 0;
                    akBelian.TarikhPosting = null;
                    await _akBelianRepo.Update(akBelian);

                    //insert applog
                    var user = await _userManager.GetUserAsync(User);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "UT";
                    appLog.LgOperation = "UnPosting";
                    appLog.LgNote = modul + " Inbois Pembekal - UnPosting";
                    appLog.NoRujukan = akBelian.NoInbois;
                    appLog.Jumlah = akBelian.Jumlah;

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
