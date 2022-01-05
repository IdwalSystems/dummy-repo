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
    public class AkPVController : Controller
    {
        public const string modul = "PV001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPV, int> _akPVRepo;
        private readonly ListViewIRepository<AkPV1, int> _akPV1Repo;
        private readonly ListViewIRepository<AkPV2, int> _akPV2Repo;
        private readonly IRepository<AkBelian, int> _akBelianRepo;
        private readonly IRepository<AkPembekal, int> _akPembekalRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<AkAkaun, int> _akAkaunRepo;
        private CartPV _cart;

        public AkPVController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPV, int> akPVRepository,
            ListViewIRepository<AkPV1, int> akPV1Repository,
            ListViewIRepository<AkPV2, int> akPV2Repository,
            IRepository<AkBelian, int> akBelian,
            IRepository<AkPembekal, int> akPembekal,
            IRepository<JKW, int> kwRepo,
            IRepository<AkCarta, int> akCartaRepository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<AkAkaun, int> akAkaunRepository,
            CartPV cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPVRepo = akPVRepository;
            _akPV1Repo = akPV1Repository;
            _akPV2Repo = akPV2Repository;
            _akBelianRepo = akBelian;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _akCartaRepo = akCartaRepository;
            _akBankRepo = akBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _cart = cart;
        }

        // GET: AkPV
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            var akPV = await _akPVRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akPV = akPV.Where(s => s.NoPV.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akPV = akPV.Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }


                    ViewBag.SearchString = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2))
                {
                    if (searchColumn == "Tarikh")
                    {
                        DateTime date1 = DateTime.Parse(searchDate1);
                        DateTime date2 = DateTime.Parse(searchDate2).AddHours(23.99);
                        akPV = akPV.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = searchColumn;
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = "Tarikh";
            }

            List<AkPVViewModel> viewModel = new List<AkPVViewModel>();
            foreach(AkPV item in akPV)
            {

                viewModel.Add(new AkPVViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoPV = item.NoPV,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Penerima = item.Nama,
                    CaraBayar = item.JCaraBayar.Perihal,
                    FlBatal = item.FlBatal,
                    FlPosting = item.FlPosting,
                    FlCetak = item.FlCetak
                }
                );
            }
            return View(viewModel);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkBelian> aBelianList = _context.AkBelian
                .Include(b => b.AkPO)
                .OrderBy(b => b.Tarikh).ToList();
            ViewBag.AkBelian = aBelianList;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.ToList();
            ViewBag.JCaraBayar = jCaraBayarList;

        }

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

        public async Task<JsonResult> SaveAkPV1(AkPV1 akPV1)
        {

            try
            {
                if (akPV1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    akPV1.UserId = user.UserName;
                    akPV1.TarMasuk = DateTime.Now;

                    _cart.AddItem1(akPV1.AkPVId,
                                   akPV1.Amaun,
                                   akPV1.AkCartaId,
                                   akPV1.UserId,
                                   akPV1.TarMasuk,
                                   akPV1.UserIdKemaskini,
                                   akPV1.TarKemaskini);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPV1(AkPV1 akPV1)
        {

            try
            {
                if (akPV1 != null)
                {

                    _cart.RemoveItem1(akPV1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akPV1
        public JsonResult GetAnItemCartAkPV1(AkPV1 akPV1)
        {

            try
            {
                AkPV1 data = _cart.Lines1.Where(x => x.AkCartaId == akPV1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPV1 end

        //save cart akPV1
        public JsonResult SaveCartAkPV1(AkPV1 akPV1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akPV1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akPV1.AkCartaId);

                    akPV1.UserId = user;
                    akPV1.TarMasuk = DateTime.Now;

                    _cart.AddItem1(akPV1.AkPVId,
                                   akPV1.Amaun,
                                   akPV1.AkCartaId,
                                   akPV1.UserId,
                                   akPV1.TarMasuk,
                                   akPV1.UserIdKemaskini,
                                   akPV1.TarKemaskini);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPV1 end

        // get all item from cart akPV1
        public JsonResult GetAllItemCartAkPV1()
        {

            try
            {
                List<AkPV1> data = _cart.Lines1.ToList();

                foreach (AkPV1 item in data)
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
        // function json Create akPV1 end

        //function json Create akPV2
        public JsonResult GetAkBelian(AkBelian akBelian)
        {
            try
            {
                var result = _context.AkBelian
                    .Include(b=>b.AkPO)
                    .Include(b=>b.AkBelian1).ThenInclude(b=>b.AkCarta)
                    .Where(b => b.Id == akBelian.Id)
                    .FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public async Task<JsonResult> SaveAkPV2(AkPV2 akPV2)
        {

            try
            {
                if (akPV2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    akPV2.UserId = user.UserName;
                    akPV2.TarMasuk = DateTime.Now;

                    _cart.AddItem2(akPV2.AkPVId,
                                   akPV2.AkBelianId,
                                   akPV2.Amaun,
                                   akPV2.UserId,
                                   akPV2.TarMasuk,
                                   akPV2.UserIdKemaskini,
                                   akPV2.TarKemaskini);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPV2(AkPV2 akPV2)
        {

            try
            {
                if (akPV2 != null)
                {

                    _cart.RemoveItem2(akPV2.AkBelianId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akPV2
        public JsonResult GetAnItemCartAkPV2(AkPV2 akPV2)
        {

            try
            {
                AkPV2 data = _cart.Lines2.Where(x => x.AkBelianId == akPV2.AkBelianId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPV2 end

        //save cart akPV2
        public JsonResult SaveCartAkPV2(AkPV2 akPV2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.AkBelianId == akPV2.AkBelianId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akPV2.AkBelianId);

                    akPV2.UserId = user;
                    akPV2.TarMasuk = DateTime.Now;

                    _cart.AddItem2(akPV2.AkPVId,
                                   akPV2.AkBelianId,
                                   akPV2.Amaun,
                                   akPV2.UserId,
                                   akPV2.TarMasuk,
                                   akPV2.UserIdKemaskini,
                                   akPV2.TarKemaskini);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akPV2 end

        // get all item from cart akPV2
        public JsonResult GetAllItemCartAkPV2()
        {

            try
            {
                List<AkPV2> data = _cart.Lines2.ToList();

                foreach (AkPV2 item in data)
                {
                    var akBelian = _context.AkBelian
                        .Include(d => d.AkPO)
                        .Where(d => d.Id == item.AkBelianId)
                        .FirstOrDefault();

                    item.AkBelian = akBelian;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPV2 end

        //function json Create akPV2 end

        // GET: AkPV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPV = await _akPVRepo.GetById((int)id);
            

            if (akPV == null)
            {
                return NotFound();
            }
            AkPVViewModel akPVView = new AkPVViewModel();

            //fill in view model AkPVViewModel from akPV
            akPVView.Id = akPV.Id;
            akPVView.Tahun = akPV.Tahun;
            akPVView.NoPV = akPV.NoPV;
            akPVView.Tarikh = akPV.Tarikh;
            akPVView.TarikhTerima = akPV.TarikhTerima;
            akPVView.JKW = akPV.JKW;
            akPVView.AkBank = akPV.AkBank;
            akPVView.Jumlah = akPV.Jumlah;
            akPVView.TarikhPosting = akPV.TarikhPosting;

            if (akPV.AkPembekalId == null)
            {
                akPVView.denganTanggungan = false;
                akPVView.KodPenerima = "-";
                akPVView.NoKP = akPV.NoKP;
                akPVView.Penerima = akPV.Nama;
                akPVView.Alamat1 = akPV.Alamat1;
                akPVView.Alamat2 = akPV.Alamat2;
                akPVView.Alamat3 = akPV.Alamat3;
                akPVView.NoAkaunBank = akPV.NoAkaunBank;
                akPVView.Telefon = akPV.Telefon;
                akPVView.Emel = akPV.Emel;
            }
            else
            {
                akPVView.denganTanggungan = true;
                akPVView.KodPenerima = akPV.AkPembekal.KodSykt;
                akPVView.NoKP = "-";
                akPVView.Penerima = akPV.AkPembekal.NamaSykt;
                akPVView.Alamat1 = akPV.AkPembekal.Alamat1;
                akPVView.Alamat2 = akPV.AkPembekal.Alamat2;
                akPVView.Alamat3 = akPV.AkPembekal.Alamat3;
                akPVView.NoAkaunBank = akPV.AkPembekal.AkaunBank;
                akPVView.Telefon = akPV.AkPembekal.Telefon1;
                akPVView.Emel = akPV.AkPembekal.Emel;
            }
            akPVView.NoCekAtauEFT = akPV.NoCekAtauEFT;
            akPVView.TarCekAtauEFT = akPV.TarCekAtauEFT;
            akPVView.Perihal = akPV.Perihal;
            akPVView.CaraBayar = akPV.JCaraBayar.Perihal;
            akPVView.FlPosting = akPV.FlPosting;
            akPVView.FlCetak = akPV.FlCetak;
            akPVView.FlBatal = akPV.FlBatal;
            akPVView.AkPV1 = akPV.AkPV1;
            foreach(AkPV2 item in akPV.AkPV2)
            {
                akPVView.JumlahInbois += item.Amaun;
            }
            akPVView.AkPV2 = akPV.AkPV2;

            PopulateTable(id);
            return View(akPVView);
        }

        private void PopulateTable(int? id)
        {
            List<AkPV1> akPV1Table = _context.AkPV1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPVId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPV1 = akPV1Table;

            List<AkPV2> akPV2Table = _context.AkPV2
                .Include(b=> b.AkBelian).ThenInclude(b=> b.AkPO)
                .Where(b => b.AkPVId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPV2 = akPV2Table;
        }

        // GET: AkPV/Create
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

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetAkBelian(int data)
        {
            try
            {
                var result = await _akBelianRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // POST: AkPV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPV akPV, int JKWId, int? AkPembekalId, int AkBankId, int JCaraBayarId)
        {
            AkPV m = new AkPV();
            var user = await _userManager.GetUserAsync(User);
            var pembekal = new AkPembekal();
            pembekal = _context.AkPembekal.Find(AkPembekalId);
            if (pembekal != null)
            {
                akPV.Nama = pembekal.NamaSykt;
                akPV.Alamat1 = pembekal.Alamat1;
                akPV.Alamat2 = pembekal.Alamat2;
                akPV.Alamat3 = pembekal.Alamat3;
                akPV.Telefon = pembekal.Telefon1;
                akPV.Emel = pembekal.Emel;
                akPV.NoAkaunBank = pembekal.AkaunBank;
            }

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == akPV.JKWId);

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.Month.ToString();
            string prefix = "PV/" + kumpulanWang + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPV.Max(x => x.NoPV);
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
                if (akPV != null && JKWId != 0 && AkBankId != 0 && akPV.Nama != null)
                {
                    m.AkBankId = AkBankId;
                    m.JKWId = JKWId;
                    m.AkPembekalId = AkPembekalId;

                    m.Tahun = akPV.Tahun;
                    m.NoPV = noRujukan;
                    m.Tarikh = akPV.Tarikh;
                    m.TarikhTerima = akPV.TarikhTerima;
                    m.NoKP = akPV.NoKP;
                    m.Nama = akPV.Nama;
                    m.Alamat1 = akPV.Alamat1;
                    m.Alamat2 = akPV.Alamat2;
                    m.Alamat3 = akPV.Alamat3;
                    m.NoAkaunBank = akPV.NoAkaunBank;
                    m.Telefon = akPV.Telefon;
                    m.Emel = akPV.Emel;

                    m.JCaraBayarId = JCaraBayarId;
                    m.NoCekAtauEFT = akPV.NoCekAtauEFT;
                    m.TarCekAtauEFT = akPV.TarCekAtauEFT;
                    m.Perihal = akPV.Perihal;
                    m.Jumlah = akPV.Jumlah;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
                    m.FlCetak = 0;

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.AkPV1 = _cart.Lines1.ToArray();
                    m.AkPV2 = _cart.Lines2.ToArray();

                    await _akPVRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Baucer Pembayaran - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akPV.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akPV.NoPV;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            return View(akPV);
        }

        // GET: AkPV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPV = await _akPVRepo.GetById((int)id);
            if (akPV == null)
            {
                return NotFound();
            }

            if(akPV.AkPV2.Any())
            {
                akPV.denganTanggungan = true;
            }

            PopulateList();
            PopulateTable(id);
            return View(akPV);
        }

        // update add akPV1
        public async Task<JsonResult> InsertUpdateAkPV1(AkPV1 akPV1)
        {

            try
            {
                if (akPV1 != null || akPV1.Amaun != 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akPV1.AkCartaId);
                    akPV1.AkCarta = akCarta;
                    akPV1.UserId = user.UserName;
                    akPV1.TarMasuk = DateTime.Now;

                    await _akPV1Repo.Insert(akPV1);

                    decimal total = 0;

                    AkPV akPV = await _akPVRepo.GetById(akPV1.AkPVId);

                    total = akPV.Jumlah + akPV1.Amaun;

                    akPV.Jumlah = total;
                    akPV.UserIdKemaskini = user.UserName;

                    await _akPVRepo.Update(akPV);

                    await _context.SaveChangesAsync();

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update add akPV1 end

        // update remove akPV1
        public async Task<JsonResult> RemoveUpdateAkPV1(AkPV1 akPV1)
        {

            try
            {
                if (akPV1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akB1 = await _context.AkPV1.FirstOrDefaultAsync(x => x.AkCartaId == akPV1.AkCartaId && x.AkPVId == akPV1.AkPVId);
                    _context.AkPV1.Remove(akB1);

                    decimal total = 0;

                    AkPV akPV = await _akPVRepo.GetById(akPV1.AkPVId);

                    total = akPV.Jumlah - akB1.Amaun;

                    akPV.Jumlah = total;
                    akPV.UserIdKemaskini = user.UserName;
                    akPV.TarKemaskini = DateTime.Now;
                    await _akPVRepo.Update(akPV);

                    //insert applog
                    var akCarta = await _akCartaRepo.GetById(akB1.AkCartaId);

                    AppLog appLog = new AppLog();
                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Baucer Pembayaran - Hapus Objek";
                    appLog.NoRujukan = akPV.NoPV + "/" + akCarta.Kod;
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
        // update remove akPV1 end

        // update update akPV1
        public async Task<JsonResult> UpdateAkPV1(AkPV1 akPV1)
        {

            try
            {
                AkPV1 data = await _akPV1Repo.GetBy2Id(akPV1.AkPVId, akPV1.AkCartaId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkPV1(AkPV1 akPV1)
        {

            try
            {

                AkPV1 akB1 = await _akPV1Repo.GetById(akPV1.Id);

                decimal originalAmount = akB1.Amaun;
                var user = await _userManager.GetUserAsync(User);

                akB1.Amaun = akPV1.Amaun;
                akB1.UserIdKemaskini = user.UserName;
                akB1.TarKemaskini = DateTime.Now;
                _context.AkPV1.Update(akB1);

                // update total akBelian with date updated and userUpdated
                var akPV = await _akPVRepo.GetById(akPV1.AkPVId);
                decimal total = 0;

                total = akPV.Jumlah - originalAmount + akB1.Amaun;
                akPV.Jumlah = total;
                akPV.UserIdKemaskini = user.UserName;
                akPV.TarKemaskini = DateTime.Now;
                await _akPVRepo.Update(akPV);
                // update total akTerima with date updated and userUpdated end

                //insert applog
                if (akPV1.Amaun != originalAmount)
                {
                    var akCarta = await _akCartaRepo.GetById(akB1.AkCartaId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "EE";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Baucer Pembayaran - Ubah Objek";
                    appLog.NoRujukan = akPV.NoPV + "/" + akCarta.Kod + " Dari Amaun RM" + originalAmount.ToString() + " ke RM" + akPV1.Amaun.ToString();
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
        public async Task<JsonResult> GetAkPV1(AkPV1 akPV1)
        {
            try
            {
                AkPV data = await _context.AkPV
                    .Include(x => x.AkPV1).ThenInclude(x => x.AkCarta)
                    .FirstOrDefaultAsync(x => x.Id == akPV1.AkPVId);

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get cart for updated akBelian1 end

        // update create akPV2
        public async Task<JsonResult> InsertUpdateAkPV2(AkPV2 akPV2)
        {

            try
            {
                if (akPV2 != null || akPV2.Amaun != 0)
                {
                    var akBelian = _context.AkBelian.FirstOrDefault(x => x.Id == akPV2.AkBelianId);
                    var user = await _userManager.GetUserAsync(User);

                    akPV2.AkBelian = akBelian;
                    akPV2.UserId = user.UserName;
                    akPV2.TarMasuk = DateTime.Now;
                    await _akPV2Repo.Insert(akPV2);

                    await _context.SaveChangesAsync();
                }




                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // update create akPV2 end

        // update remove akPV2
        public async Task<JsonResult> RemoveUpdateAkPV2(AkPV2 akPV2)
        {

            try
            {
                if (akPV2 != null)
                {
                    var akT2 = await _context.AkPV2.Include(b=>b.AkBelian).ThenInclude(b=>b.AkPO).FirstOrDefaultAsync(x => x.AkBelianId == akPV2.AkBelianId && x.AkPVId == akPV2.AkPVId);
                    var user = await _userManager.GetUserAsync(User);

                    _context.AkPV2.Remove(akT2);

                    //insert applog
                    var akPV = await _akPVRepo.GetById(akPV2.AkPVId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Penerimaan - Hapus Perihal";
                    appLog.NoRujukan = akPV.NoPV + "/" + akT2.AkBelian.NoInbois;
                    appLog.Jumlah = akPV2.Amaun;

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

        // POST: AkPV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkPV akPV, int JKWId, int AkPembekalId, int AkBankId, int JCaraBayarId)
        {
            if (id != akPV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    akPV.UserIdKemaskini = user.UserName;
                    akPV.TarKemaskini = DateTime.Now;
                    _context.Update(akPV);

                    //insert applog
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "E";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Baucer Pembayaran - Ubah";
                    appLog.NoRujukan = akPV.NoPV;
                    appLog.Jumlah = akPV.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPVExists(akPV.Id))
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
            return View(akPV);
        }

        // GET: AkPV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPV = await _context.AkPV
                .Include(a => a.AkBank)
                .Include(a => a.AkPembekal)
                .Include(a => a.JCaraBayar)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPV == null)
            {
                return NotFound();
            }

            return View(akPV);
        }

        // POST: AkPV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPV = await _context.AkPV.FindAsync(id);
            _context.AkPV.Remove(akPV);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPVExists(int id)
        {
            return _context.AkPV.Any(e => e.Id == id);
        }
    }
}
