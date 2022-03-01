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
    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class AkBelianController : Controller
    {

        public const string modul = "TG002";
        public const string namamodul = "Inbois Pembekal";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkPO, int, string> _akPORepo;
        private readonly ListViewIRepository<AkBelian1, int> _akBelian1Repo;
        private readonly ListViewIRepository<AkBelian2, int> _akBelian2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private CartBelian _cart;

        public AkBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int, string> akBelian,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<JKW, int, string> kwRepo,
            IRepository<AkPO, int, string> akPORepo,
            ListViewIRepository<AkBelian1, int> akBelian1Repository,
            ListViewIRepository<AkBelian2, int> akBelian2Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AkPV, int, string> akPVRepository,
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
            _akPVRepo = akPVRepository;
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

        // GET: AkBelian
        [Authorize(Policy = "TG002")]
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

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akBelian = await _akBelianRepo.GetAllIncludeDeletedItems();
            }
            
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
                        FlHapus = item.FlHapus,
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

            // admin access
            var akBelian = await _akBelianRepo.GetByIdIncludeDeletedItems((int)id);

            var kodObjekAkaunPemiutang = await _akCartaRepo.GetByIdIncludeDeletedItems(akBelian.KodObjekAPId);

            var akPO = new AkPO();
            if (akBelian.AkPOId != null)
            {
                akPO = await _akPORepo.GetByIdIncludeDeletedItems((int)akBelian.AkPOId);
            } else
            {
                akPO = new AkPO()
                {
                    NoPO = "-"
                };     
            }

            var pembekal = await _akPembekalRepo.GetByIdIncludeDeletedItems(akBelian.AkPembekalId);

            if (akBelian == null)
            {
                return NotFound();
            }
            // admin access end

            // normal user access
            if (User.IsInRole("User"))
            {
                kodObjekAkaunPemiutang = await _akCartaRepo.GetById(akBelian.KodObjekAPId);

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

                pembekal = await _akPembekalRepo.GetById(akBelian.AkPembekalId);

                if (akBelian == null)
                {
                    return NotFound();
                }
            }
            //normal user access end

            AkBelianViewModel akBelianView = new AkBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            akBelianView.AkPembekalId = akBelian.AkPembekalId;
            akBelianView.AkPO = akPO;
            akBelianView.AkPembekal = pembekal;
            akBelianView.Id = akBelian.Id;
            akBelianView.Tahun = akBelian.Tahun;
            akBelianView.NoInbois = akBelian.NoInbois;
            akBelianView.Tarikh = akBelian.Tarikh;
            akBelianView.TarikhTerima = akBelian.TarikhTerima;
            akBelianView.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
            akBelianView.JKWId = akBelian.JKWId;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlHapus = akBelian.FlHapus;

            foreach (AkBelian2 item in akBelian.AkBelian2)
            {
                akBelianView.JumlahPerihal += item.Amaun;
            }
            akBelianView.AkBelian1 = akBelian.AkBelian1;
            akBelianView.AkBelian2 = akBelian.AkBelian2;

            PopulateTable(id);
            return View(akBelianView);
        }

        // GET: AkBelian/Create
        [Authorize(Policy = "TG002C")]
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

                var akPOLaras = _context.AkPOLaras
                    .Include(x=> x.AkPOLaras1)
                    .Where(x => x.AkPOId == id && x.FlPosting == 1).FirstOrDefault();

                List<AkPO1> akPO1Table = await _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkPO1 item in akPO1Table)
                {
                    if(item.Amaun != 0)
                    {
                        result.AkPO1.Add(item);
                    }
                }

                List<AkPO2> akPO2Table = await _context.AkPO2
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkPO2 item in akPO2Table)
                {
                    if (akPOLaras != null)
                    {
                        item.Amaun = 0;
                        item.Harga = 0;
                        item.Kuantiti = 0;
                    }

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

            AkPOLaras akPOLaras = _context.AkPOLaras
                    .Include(x => x.AkPOLaras1)
                    .Where(x => x.AkPOId == id && x.FlPosting == 1).FirstOrDefault();

            List<AkPO1> akPO1Table =  _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO1 item in akPO1Table)
            {

                item.AkPOId = 0;

                //if there is pelarasan PO
                if(akPOLaras != null)
                {
                    foreach(var laras in akPOLaras.AkPOLaras1)
                    {
                        if(laras.AkCartaId == item.AkCartaId)
                        {
                            item.Amaun += laras.Amaun;
                        }
                    }
                }
                
                if(item.Amaun != 0)
                {
                    _cart.AddItem1(item.AkPOId,
                                   item.Amaun,
                                   item.AkCartaId);
                }
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                .AsNoTracking()
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkPO2 item in akPO2Table)
            {
                item.AkPOId = 0;
                item.Amaun = 0;
                item.Kuantiti = 0;
                item.Harga = 0;

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
        [Authorize(Policy = "TG002C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBelian akBelian, int JKWId, int AkPOId, int AkPembekalId, int KodObjekAPId, string NamaPembekal, decimal JumlahPerihal)
        {
            AkBelian m = new AkBelian();
            var user = await _userManager.GetUserAsync(User);

            var pembekal = await _akPembekalRepo.GetById(AkPembekalId);
            if (pembekal == null)
            {
                TempData[SD.Error] = "Pembekal tidak wujud..!";
                //PopulateCart();
                PopulateList();
                CartEmpty();
                return View(akBelian);
            }

            var noRujukan = "IN/"+ pembekal.KodSykt.ToUpper() + "/" + akBelian.NoInbois.ToUpper();

            var akPo = await _akPORepo.GetById(AkPOId);
            // checking for existing no rujukan
            var countNoRujukan = _context.AkBelian.Where(x => x.NoInbois == noRujukan && x.AkPembekalId == AkPembekalId).Count();

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
                    m.TarikhTerima = akBelian.TarikhTerima;
                    m.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
                    m.Jumlah = akBelian.Jumlah;
                    m.FlPosting = 0;
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
                    await AddLogAsync("Tambah", m.NoInbois, m.NoInbois, 0, m.Jumlah);
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
        [Authorize(Policy = "TG002E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _akBelianRepo.GetById((int)id);

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

        // get latest Index number in AkBelian2
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
        [Authorize(Policy = "TG002E")]
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
                        if (akBelianAsal.AkPOId != null)
                        {
                            akBelian.AkPOId = akBelianAsal.AkPOId;
                            akBelian.AkPembekalId = akBelianAsal.AkPembekalId;
                        }
                        akBelian.TarMasuk = akBelianAsal.TarMasuk;
                        akBelian.UserId = akBelianAsal.UserId;
                        akBelian.KodObjekAPId = akBelianAsal.KodObjekAPId;
                        decimal jumlahAsal = akBelianAsal.Jumlah;
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
                        if(jumlahAsal != akBelian.Jumlah)
                        {
                            await AddLogAsync("Ubah","RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                                Convert.ToDecimal(akBelian.Jumlah).ToString("#,##0.00"), akBelian.NoInbois, id, akBelian.Jumlah);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", akBelian.NoInbois, id, akBelian.Jumlah);
                        }

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
        [Authorize(Policy = "TG002D")]
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
            akBelianView.TarikhTerima = akBelian.TarikhTerima;
            akBelianView.TarikhKewanganTerima = akBelian.TarikhKewanganTerima;
            akBelianView.JKW = akBelian.JKW;
            akBelianView.KodObjekAP = kodObjekAkaunPemiutang;
            akBelianView.Jumlah = akBelian.Jumlah;
            akBelianView.TarikhPosting = akBelian.TarikhPosting;
            akBelianView.FlPosting = akBelian.FlPosting;
            akBelianView.FlHapus = akBelian.FlHapus;

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
        [Authorize(Policy = "TG002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBelian = await _context.AkBelian.FindAsync(id);
            // check if already posting redirect back
            if (akBelian.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // check if already link with akPV, Batal akPV included
            var akPV = await _akPVRepo.GetAll();
            var akPV2 = _context.AkPV2.ToList();
            var result = (from tbl2 in akPV2
                          join tbl in akPV
                          on tbl2.AkPVId equals tbl.Id into tbl2Tbl
                          from tbl2_tbl in tbl2Tbl
                          select new
                          {
                              Id = tbl2.Id,
                              AkPVId = tbl2.AkPVId,
                              AkBelianId = tbl2.AkBelianId

                          }).Where(x => x.AkBelianId == id).FirstOrDefault();

            if (result != null)
            {
                AkPV akPVItem = await _akPVRepo.GetById(result.AkPVId);
                //duplicate id error
                TempData[SD.Error] = "Data terkait dengan no baucer " + akPVItem.NoPV + ".";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            akBelian.UserIdKemaskini = user.UserName;
            akBelian.TarKemaskini = DateTime.Now;

            _context.AkBelian.Remove(akBelian);
            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", akBelian.NoInbois, id, akBelian.Jumlah);
            //insert applog end
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkBelianExists(int id)
        {
            return _context.AkBelian.Any(e => e.Id == id);
        }

        private bool CurrentAkBelianExists(int akPembekalId,string noRujukan)
        {
            return _context.AkBelian.Any(e => e.AkPembekalId == akPembekalId && e.NoInbois == noRujukan);
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
        [Authorize(Policy = "TG002T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkBelian akBelian = await _akBelianRepo.GetById((int)id);

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
                    if(akBelian.TarikhTerima != null || akBelian.TarikhKewanganTerima != null)
                    {
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
                            AbBukuVot abBukuVot = new AbBukuVot();
                            if (akBelian.AkPO != null)
                            {
                                //dengan tanggungan
                                abBukuVot = new AbBukuVot()
                                {
                                    Tahun = akBelian.Tahun,
                                    JKWId = akBelian.JKWId,
                                    Tarikh = akBelian.Tarikh,
                                    Kod = kodPembekal,
                                    Penerima = penerima,
                                    VotId = item.AkCartaId,
                                    Rujukan = akBelian.NoInbois,
                                    Tanggungan = 0 - item.Amaun,
                                    Liabiliti = item.Amaun

                                };
                            }
                            else
                            {
                                //tanpa tanggungan
                                abBukuVot = new AbBukuVot()
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

                            }

                            await _abBukuVotRepo.Insert(abBukuVot);

                            // insert into AbBukuVot end

                            //insert into akAkaun
                            AkAkaun akALiabiliti = new AkAkaun()
                            {
                                NoRujukan = akBelian.NoInbois,
                                JKWId = akBelian.JKWId,
                                AkCartaId1 = akBelian.KodObjekAPId,
                                AkCartaId2 = item.AkCartaId,
                                Tarikh = akBelian.Tarikh,
                                Kredit = item.Amaun
                            };

                            await _akAkaunRepo.Insert(akALiabiliti);

                            AkAkaun akAObjek = new AkAkaun()
                            {
                                NoRujukan = akBelian.NoInbois,
                                JKWId = akBelian.JKWId,
                                AkCartaId1 = item.AkCartaId,
                                AkCartaId2 = akBelian.KodObjekAPId,
                                Tarikh = akBelian.Tarikh,
                                Debit = item.Amaun
                            };

                            await _akAkaunRepo.Insert(akAObjek);
                        }

                        //update posting status in akTerima
                        akBelian.FlPosting = 1;
                        akBelian.TarikhPosting = DateTime.Now;
                        await _akBelianRepo.Update(akBelian);

                        //insert applog
                        await AddLogAsync("Posting", "Posting Data", akBelian.NoInbois, (int)id, akBelian.Jumlah);

                        //insert applog end

                        await _context.SaveChangesAsync();


                        TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                    }
                    else
                    {
                        //duplicate id error
                        TempData[SD.Error] = "Sila isi tarikh terima / tarikh kewangan terima untuk meneruskan operasi ini.";
                    }
                    
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
                AkBelian akBelian = await _akBelianRepo.GetById((int) id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akBelian.NoInbois).ToList();

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akBelian.NoInbois).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

                }
                else
                {
                    var akPV = await _akPVRepo.GetAll();
                    var akPV2 = _context.AkPV2.ToList();
                    var result = (from tbl2 in akPV2
                                   join tbl in akPV
                                   on tbl2.AkPVId equals tbl.Id into tbl2Tbl
                                   from tbl2_tbl in tbl2Tbl
                                   select new
                                   {
                                       Id = tbl2.Id,
                                       AkPVId = tbl2.AkPVId,
                                       AkBelianId = tbl2.AkBelianId

                                   }).Where(x=> x.AkBelianId == id).FirstOrDefault();

                    if (result != null)
                    {
                        AkPV akPVItem = await _akPVRepo.GetById(result.AkPVId);
                        //duplicate id error
                        TempData[SD.Error] = "Data terkait dengan no baucer " + akPVItem.NoPV + ".";
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
                        await AddLogAsync("UnPosting", "Batal Posting Data", akBelian.NoInbois, (int)id, akBelian.Jumlah);

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
        // POST: AkPV/Cancel/5
        //[Authorize(Policy = "TG002B")]
        //public async Task<IActionResult> Cancel(int id)
        //{
        //    var akBelian = await _context.AkBelian.FindAsync(id);
        //    // check if already posting redirect back
        //    if (akBelian.FlPosting == 1)
        //    {
        //        TempData[SD.Error] = "Akses tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    // Batal operation

        //    akBelian.FlHapus = 1;
        //    akBelian.TarHapus = DateTime.Now;
        //    _context.AkBelian.Update(akBelian);

        //    // Batal operation end

        //    //insert applog
        //    var user = await _userManager.GetUserAsync(User);

        //    AppLog appLog = new AppLog();

        //    appLog.UserId = user.UserName;
        //    appLog.LgModule = modul + "B";
        //    appLog.LgOperation = "Batal";
        //    appLog.LgNote = modul + " Inbois Pembekal - Batal";
        //    appLog.NoRujukan = akBelian.NoInbois;
        //    appLog.Jumlah = akBelian.Jumlah;

        //    await _appLog.Insert(appLog);
        //    //insert applog end

        //    await _context.SaveChangesAsync();
        //    TempData[SD.Success] = "Data berjaya dibatalkan..!";
        //    return RedirectToAction(nameof(Index));
        //}

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "TG002R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akBelianRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if(CurrentAkBelianExists(obj.AkPembekalId,obj.NoInbois) == false)
            {
                // Batal operation

                obj.FlHapus = 0;
                _context.AkBelian.Update(obj);

                // Batal operation end

                //insert applog
                await AddLogAsync("Rollback", "Rollback Data", obj.NoInbois, id, obj.Jumlah);
                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dikembalikan..!";
                
            }
            else
            {
                TempData[SD.Error] = "No Inbois telah wujud..!";
                
            }
            return RedirectToAction(nameof(Index));

        }

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetKod(int data, string noInbois)
        {
            try
            {
                var result = await _context.AkBelian.FirstOrDefaultAsync(x=>x.NoInbois == "IN/"+ data +"/"+noInbois);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end
    }
}
