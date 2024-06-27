using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using Rotativa.AspNetCore;
using MSNK.Infrastructure;
using MSNK.Models.Modules.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Models.Operations;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkTerimaController : Controller
    {
        public const string modul = "PR001";
        public const string namamodul = "Penerimaan";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPPRepo;
        private readonly IRepository<AkPenghutang, int, string> _akPenghutangRepo;
        private readonly IRepository<AkInvois, int, string> _akInvoisRepo;
        private readonly UserService _userService;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartTerima _cart;

        public AkTerimaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int, string> akTerimaRepository,
            ListViewIRepository<AkTerima1, int> akTerima1Repository,
            ListViewIRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<SpPendahuluanPelbagai, int, string> spPPRepo,
            IRepository<AkPenghutang, int, string> akPenghutangRepo,
            IRepository<AkInvois, int, string> akInvoisRepo,
            UserService userService,
            IRepository<AbBukuVot, int, string> abBukuVotRepo,
            CartTerima cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _spPPRepo = spPPRepo;
            _akPenghutangRepo = akPenghutangRepo;
            _akInvoisRepo = akInvoisRepo;
            _userService = userService;
            _abBukuVotRepo = abBukuVotRepo;
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

        [Authorize(Policy = "PR001")]
        // GET: AkTerima
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

            var akTerima = new List<AkTerima>().AsEnumerable();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akTerima = await _akTerimaRepo.GetAllIncludeDeletedItemsFiltered(searchString,searchDate1,searchDate2,searchColumn);
            }
            else
            {
                akTerima = await _akTerimaRepo.GetAllFiltered(searchString, searchDate1,searchDate2,searchColumn);
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

            List<AkTerimaViewModel> viewModel = new List<AkTerimaViewModel>();
            foreach (AkTerima item in akTerima)
            {
                decimal jumlahUrusniaga = 0;
                foreach (AkTerima2 item2 in item.AkTerima2)
                {
                    jumlahUrusniaga += item2.Amaun;
                }
                viewModel.Add(new AkTerimaViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoRujukan = item.NoRujukan,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Nama = item.Nama,
                    AkBank = item.AkBank,
                    FlHapus = item.FlHapus,
                    FlPosting = item.FlPosting,
                    FlCetak = item.FlCetak,
                    JumlahUrusniaga = jumlahUrusniaga
                });
            }

            return View(viewModel);
        }

        // GET: AkTerima/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetByIdIncludeDeletedItems((int)id);

            // normal user access
            if (User.IsInRole("User"))
            {
                akTerima = await _akTerimaRepo.GetById((int)id);
            }

            if (akTerima == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akTerima);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<SpPendahuluanPelbagai> spList = _context.SpPendahuluanPelbagai.Where(b => b.FlPosting == 1).OrderBy(b => b.NoPermohonan).ToList();

            List<SpPendahuluanPelbagai> spListUpdated = new List<SpPendahuluanPelbagai>();

            foreach (var item in spList)
            {
                var ExistAkTerimaWithSp = _context.AkTerima.Any(b => b.SpPendahuluanPelbagaiId == item.Id && b.FlPosting == 0);

                if (ExistAkTerimaWithSp == true)
                {
                    continue;
                    
                }
                else
                {
                    var ExistAkPVWithSp = _context.AkPV.Any(b => b.SpPendahuluanPelbagaiId == item.Id && b.FlPosting == 1);
                    if (ExistAkPVWithSp == true)
                    {
                        spListUpdated.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            ViewBag.SpPendahuluanPelbagai = spListUpdated;

            List<AkBank> akBankList = _context.AkBank.Include(b=> b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b=>b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.OrderBy(b => b.Kod).ToList();
            ViewBag.JCaraBayar = jCaraBayarList;

            List<AkPenghutang> akPenghutangList = _context.AkPenghutang
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPenghutang = akPenghutangList;

            List<AkInvois> akInvoisList = _context.AkInvois
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();

            foreach (var item in akInvoisList)
            {
                item.NoInbois = item.NoInbois.Substring(3);
            }
            ViewBag.AkInvois = akInvoisList;

            //var jenisCek = new Dictionary<string, string>
            //{
            //    { "1",   "Cek Cawangan Ini"},
            //    { "2",    "Cek Tempatan" },
            //    { "3", "Cek Luar" },
            //    { "4", "Cek Antarabangsa"}
            //};

            //ViewBag.JenisCek = jenisCek;

        }

        private void PopulateTable(int? id)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b =>b.AkCarta)
                .Where(b=>b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima1 = akTerima1Table;

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima2 = akTerima2Table;

            List<AkTerima3> akTerima3Table = _context.AkTerima3
                .Include(b => b.AkInvois)
                .Where(b => b.AkTerimaId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTerima3 = akTerima3Table;
        }
        private void PopulateCart()
        {
            List<AkTerima1> lines1 = _cart.Lines1.ToList();

            foreach (AkTerima1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            ViewBag.akTerima1 = lines1;

            List<AkTerima2> lines2 = _cart.Lines2.ToList();

            foreach (AkTerima2 item in lines2)
            {
                var jCaraBayar = _context.JCaraBayar.Where(x => x.Id == item.JCaraBayarId).FirstOrDefault();
                item.JCaraBayar = jCaraBayar;
            }

            ViewBag.akTerima2 = _cart.Lines2.ToList();
        }

        private void PopulateCartFromDb(AkTerima akTerima)
        {
            List<AkTerima1> akTerima1Table = _context.AkTerima1
                .Include(b => b.AkCarta)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkTerima1 akTerima1 in akTerima1Table)
            {
                _cart.AddItem1(akTerima1.AkTerimaId,
                               akTerima1.Amaun,
                               akTerima1.AkCartaId);
            }

            ViewBag.akTerima1 = akTerima1Table;

            List<AkTerima2> akTerima2Table = _context.AkTerima2
                .Include(b => b.JCaraBayar)
                .Where(b => b.AkTerimaId == akTerima.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach(AkTerima2 akTerima2 in akTerima2Table)
            {
                _cart.AddItem2(akTerima2.AkTerimaId,
                               akTerima2.JCaraBayarId,
                               akTerima2.Amaun,
                               akTerima2.NoCek,
                               akTerima2.JenisCek,
                               akTerima2.KodBankCek,
                               akTerima2.TempatCek,
                               akTerima2.NoSlip,
                               akTerima2.TarSlip,
                               akTerima2.AkPenyataPemungutId);
            }

            ViewBag.akTerima2 = akTerima2Table;
        }

        private string GetNoRujukan(int data, string year)
        {
            var kodBank = _context.AkBank.FirstOrDefault(x => x.Id == data)?.Kod ?? "AK1";

            string prefix = kodBank + year ;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTerima
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun == year && x.AkBank.Kod == kodBank)
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
            return noRujukan;
        }

        // AK COMMENT 17-03-2022
        // GET: AkTerima/Create
        //[Authorize(Policy = "PR001C")]
        //public IActionResult Create()
        //{
        //    // get latest no rujukan running number 
        //    var year = DateTime.Now.Year.ToString();
        //    var data = 1;

        //    ViewBag.NoRujukan = GetNoRujukan(data,year);
        //    // get latest no rujukan running number end

        //    PopulateList();
        //    CartEmpty();
        //    return View();
        //}
        // function json get no rujukan (running number)
        // AK COMMENT 17-03-2022 END

        [Authorize(Policy = "PR001C")]
        public IActionResult CreateByJenis(string jenis)
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            var data = 2;

            ViewBag.NoRujukan = GetNoRujukan(data, year);
            // get latest no rujukan running number end

            PopulateList();
            CartEmpty();
            return View(jenis);
        }

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
                    result = GetNoRujukan(data,year);
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

        // on change NoKP controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoKP(string data)
        {
            try
            {
                var result = await _context.AkTerima
                    .Where(x=> x.NoKp == data)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    result = new AkTerima
                    {
                        Nama = "",
                        Alamat1 = "",
                        Alamat2 = "",
                        Alamat3 = "",
                        Poskod = "",
                        Bandar = "",
                        Tel = "",
                        Emel = ""
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

        // on change CaraBayar controller
        [HttpPost]
        public async Task<JsonResult> JsonGetCaraBayar(int data)
        {
            try
            {
                var result = await _context.JCaraBayar.FindAsync(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change CaraBayar controller end

        // on change pendahuluan
        [HttpPost]
        public async Task<JsonResult> JsonGetPendahuluan(int data, int AkTerimaId)
        {
            try
            {
                CartEmpty();
                var result = await _spPPRepo.GetById(data);

                _cart.AddItem1(AkTerimaId,
                               result.JumLulus,
                               (int)result.AkCartaId);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change pendahuluan end

        // get an item from cart akTerima1
        public JsonResult GetAnItemCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                AkTerima1 data = _cart.Lines1.Where(x => x.AkCartaId == akTerima1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima1 end

        //save cart akTerima1
        public JsonResult SaveCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akTerima1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akTerima1.AkCartaId);

                    _cart.AddItem1(akTerima1.AkTerimaId,
                                    akTerima1.Amaun,
                                    akTerima1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima1 end

        // get all item from cart akTerima1
        public JsonResult GetAllItemCartAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                List<AkTerima1> data = _cart.Lines1.ToList();

                foreach (AkTerima1 item in data)
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
        // get all item from cart akTerima1 end

        // get an item from cart akTerima2
        public JsonResult GetAnItemCartAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                AkTerima2 data = _cart.Lines2.Where(x => x.JCaraBayarId == akTerima2.JCaraBayarId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima2 end

        //save cart akTerima2
        public JsonResult SaveCartAkTerima2(AkTerima2 akTerima2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.JCaraBayarId == akTerima2.JCaraBayarId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akTerima2.JCaraBayarId);


                    _cart.AddItem2(akTerima2.AkTerimaId,
                                   akTerima2.JCaraBayarId,
                                   akTerima2.Amaun,
                                   akTerima2.NoCek,
                                   akTerima2.JenisCek,
                                   akTerima2.KodBankCek,
                                   akTerima2.TempatCek,
                                   akTerima2.NoSlip,
                                   akTerima2.TarSlip, 
                                   akTerima2.AkPenyataPemungutId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima2 end

        // get all item from cart akTerima2
        public JsonResult GetAllItemCartAkTerima2()
        {

            try
            {
                List<AkTerima2> data = _cart.Lines2.ToList();

                foreach (AkTerima2 item in data)
                {
                    var jCaraBayar = _context.JCaraBayar.Find(item.JCaraBayarId);

                    item.JCaraBayar = jCaraBayar;
                    
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akTerima2 end

        // get an item from cart akTerima3
        public JsonResult GetAnItemCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                AkTerima3 data = _cart.Lines3.Where(x => x.AkInvoisId == akTerima3.AkInvoisId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akTerima3 end

        //save cart akTerima3
        public JsonResult SaveCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {

                var akT3 = _cart.Lines3.Where(x => x.AkInvoisId == akTerima3.AkInvoisId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT3 != null)
                {
                    _cart.RemoveItem3((int)akTerima3.AkInvoisId);

                    _cart.AddItem3(akTerima3.AkTerimaId,
                                    akTerima3.AkInvoisId,
                                    akTerima3.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akTerima3 end

        // get all item from cart akTerima3
        public JsonResult GetAllItemCartAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                List<AkTerima3> data = _cart.Lines3.ToList();

                foreach (AkTerima3 item in data)
                {
                    var akInvois = _context.AkInvois.Find(item.AkInvoisId);

                    item.AkInvois = akInvois;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akTerima3 end


        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001C")]
        public async Task<IActionResult> CreateByJenis(
            AkTerima akTerima,
            int JKWId,
            int JNegeriId,
            int AkBankId,
            decimal JumlahUrusniaga,
            int JBahagianId,
            int? SpPendahuluanPelbagaiId,
            int FlJenisTerima,
            int FlKategoriPenerima = 0)
        {

            // note:
            // FlJenisTerima = 0 ( Am )
            // FlJenisTerima = 1 ( Inbois )
            // FlJenisTerima = 2 ( Gaji )
            // FlJenisTerima = 3 ( Pendahuluan )
            // FlJenisTerima = 4 ( Panjar )
            // ..
            // FlKategoriPenerima = 0 ( Am / Lain - lain )
            // FlKategoriPenerima = 1 ( pembekal )
            // FlKategoriPenerima = 2 ( pekerja )
            // ..

            AkTerima m = new AkTerima();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            var spPendahuluan = _context.SpPendahuluanPelbagai.Find(SpPendahuluanPelbagaiId);

            var jenis = "CreateAm";

            if (FlJenisTerima == 1)
            {
                akTerima.FlKategoriPembayar = KategoriPembayar.Penghutang;
            }

            if (spPendahuluan != null)
            {
                var pekerja = _context.SuPekerja.Find(spPendahuluan.SuPekerjaId);
                jenis = "CreatePekerja";
                akTerima.FlKategoriPembayar = KategoriPembayar.Pekerja;
            }
            

            // checking for jumlah objek & jumlah perihal
            if (akTerima.Jumlah != JumlahUrusniaga)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                //PopulateCart();
                CartEmpty();
                PopulateList();
                return View(jenis, akTerima);
            }

            // get latest no rujukan running number  
            var kodBank = _context.AkBank.FirstOrDefault(x => x.Id == akTerima.AkBankId)?.Kod ?? "AK1";

            var year = akTerima.Tahun;
            string prefix = "RR/" + kodBank + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTerima
                .IgnoreQueryFilters()
                        .Where(x => x.Tahun == year && x.AkBank.Kod == kodBank)
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
                if (akTerima != null && JNegeriId != 0 && JKWId != 0 && JNegeriId != 0 && JBahagianId != 0)
                {

                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.JNegeriId = JNegeriId;
                    m.AkBankId = AkBankId;
                    m.Tahun = akTerima.Tahun;
                    m.NoRujukan = noRujukan;
                    m.Tarikh = akTerima.Tarikh;
                    m.Jumlah = akTerima.Jumlah;
                    m.FlCetak = 0;
                    m.FlPosting = 0;
                    m.KodPembayar = akTerima.KodPembayar;
                    m.NoKp = akTerima.NoKp;
                    m.Nama = akTerima.Nama;
                    m.Alamat1 = akTerima.Alamat1;
                    m.Alamat2 = akTerima.Alamat2;
                    m.Alamat3 = akTerima.Alamat3;
                    m.Poskod = akTerima.Poskod;
                    m.Bandar = akTerima.Bandar;
                    m.Tel = akTerima.Tel;
                    m.Emel = akTerima.Emel;
                    m.Sebab = akTerima.Sebab;
                    m.FlPostingBukuVot = akTerima.FlPostingBukuVot;

                    m.FlKategoriPembayar = akTerima.FlKategoriPembayar;
                    
                    if (akTerima.FlKategoriPembayar == KategoriPembayar.Penghutang)
                    {
                        m.AkPenghutangId = akTerima.AkPenghutangId;
                    }
                    
                    m.FlJenisTerima = akTerima.FlJenisTerima;
                    if (spPendahuluan != null)
                    {
                        m.SpPendahuluanPelbagaiId = SpPendahuluanPelbagaiId;
                    }

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;
                    //m.TarKemaskini = akTerima.TarKemaskini;

                    m.AkTerima1 = _cart.Lines1.ToArray();
                    m.AkTerima2 = _cart.Lines2.ToArray();
                    m.AkTerima3 = _cart.Lines3.ToArray();

                    await _akTerimaRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(jenis, akTerima);
        }

        // AK COMMENT 17-03-2022
        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "PR001C")]
        //public async Task<IActionResult> Create(
        //    AkTerima akTerima, 
        //    int JKWId, 
        //    int JNegeriId, 
        //    int AkBankId, 
        //    decimal JumlahUrusniaga,
        //    int JBahagianId)
        //{

        //    AkTerima m = new AkTerima();
        //    var user = await _userManager.GetUserAsync(User);

        //    // checking for jumlah objek & jumlah perihal
        //    if (akTerima.Jumlah != JumlahUrusniaga)
        //    {
        //        TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
        //        //PopulateCart();
        //        CartEmpty();
        //        PopulateList();
        //        return View(akTerima);
        //    }

        //    // get latest no rujukan running number  
        //    var kw = _context.JKW.FirstOrDefault(x => x.Id == akTerima.JKWId);

        //    var kumpulanWang = kw.Kod;
        //    var year = akTerima.Tahun;
        //    string prefix = "RR/" + kumpulanWang + year;
        //    int x = 1;
        //    string noRujukan = prefix + "000000";

        //    var LatestNoRujukan = _context.AkTerima
        //        .IgnoreQueryFilters()
        //                .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
        //                .Max(x => x.NoRujukan);

        //    if (LatestNoRujukan == null)
        //    {
        //        noRujukan = string.Format("{0:" + prefix + "000000}", x);
        //    }
        //    else
        //    {
        //        x = int.Parse(LatestNoRujukan.Substring(10));
        //        x++;
        //        noRujukan = string.Format("{0:" + prefix + "000000}", x);
        //    }

        //    // get latest no rujukan running number end

        //    if (ModelState.IsValid)
        //    {
        //        if (akTerima != null && JNegeriId != 0 && JKWId != 0 && JNegeriId != 0 && JBahagianId != 0)
        //        {

        //            m.JKWId = JKWId;
        //            m.JBahagianId = JBahagianId;
        //            m.JNegeriId = JNegeriId;
        //            m.AkBankId = AkBankId;
        //            m.Tahun = akTerima.Tahun;
        //            m.NoRujukan = noRujukan;
        //            m.Tarikh = akTerima.Tarikh;
        //            m.Jumlah = akTerima.Jumlah;
        //            m.FlCetak = 0;
        //            m.FlPosting = 0;
        //            m.KodPembayar = akTerima.KodPembayar;
        //            m.NoKp = akTerima.NoKp;
        //            m.Nama = akTerima.Nama;
        //            m.Alamat1 = akTerima.Alamat1;
        //            m.Alamat2 = akTerima.Alamat2;
        //            m.Alamat3 = akTerima.Alamat3;
        //            m.Poskod = akTerima.Poskod;
        //            m.Bandar = akTerima.Bandar;
        //            m.Tel = akTerima.Tel;
        //            m.Emel = akTerima.Emel;
        //            m.Sebab = akTerima.Sebab;
        //            m.UserId = user.UserName;
        //            m.TarMasuk = DateTime.Now;
        //            //m.TarKemaskini = akTerima.TarKemaskini;

        //            m.AkTerima1 = _cart.Lines1.ToArray();
        //            m.AkTerima2 = _cart.Lines2.ToArray();

        //            await _akTerimaRepo.Insert(m);

        //            //insert applog
        //            await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah);
        //            //insert applog end

        //            await _context.SaveChangesAsync();

        //            CartEmpty();
        //            TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    PopulateList();
        //    return View(akTerima);
        //}
        // AK COMMENT 17-03-2022 END

        // GET: AkTerima/Edit/5
        [Authorize(Policy = "PR001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int)id);

            // check if already posting redirect back
            if (akTerima.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if (akTerima == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTerima);
            return View(akTerima);
        }

        // POST: AkTerima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001E")]
        public async Task<IActionResult> Edit(
            int id, 
            AkTerima akTerima, 
            int JKWId, 
            int JNegeriId, 
            int AkBankId, 
            decimal JumlahUrusniaga,
            int JBahagianId)
        {
            if (id != akTerima.Id)
            {
                return NotFound();
            }

            if (akTerima.Jumlah == JumlahUrusniaga)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                        AkTerima dataAsal = await _akTerimaRepo.GetById(id);

                        // list of input that cannot be change
                        akTerima.Tahun = dataAsal.Tahun;
                        akTerima.JKWId = dataAsal.JKWId;
                        akTerima.FlJenisTerima = dataAsal.FlJenisTerima;
                        akTerima.FlKategoriPembayar = dataAsal.FlKategoriPembayar;
                        akTerima.AkPenghutangId = dataAsal.AkPenghutangId;
                        //akTerima.JBahagianId = dataAsal.JBahagianId;
                        akTerima.NoRujukan = dataAsal.NoRujukan;
                        akTerima.Nama = dataAsal.Nama;
                        akTerima.TarMasuk = dataAsal.TarMasuk;
                        akTerima.UserId = dataAsal.UserId;
                        akTerima.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                        akTerima.FlCetak = 0;
                        // list of input that cannot be change end

                        foreach (AkTerima1 item in dataAsal.AkTerima1)
                        {
                            var model = _context.AkTerima1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkTerima2 item in dataAsal.AkTerima2)
                        {
                            var model = _context.AkTerima2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkTerima3 item in dataAsal.AkTerima3)
                        {
                            var model = _context.AkTerima3.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        decimal jumlahAsal = dataAsal.Jumlah;
                        _context.Entry(dataAsal).State = EntityState.Detached;

                        akTerima.AkTerima1 = _cart.Lines1.ToList();
                        akTerima.AkTerima2 = _cart.Lines2.ToList();
                        akTerima.AkTerima3 = _cart.Lines3.ToList();

                        akTerima.UserIdKemaskini = user.UserName;
                        akTerima.TarKemaskini = DateTime.Now;
                        akTerima.SuPekerjaKemaskiniId = pekerjaId;

                        _context.Update(akTerima);

                        //insert applog
                        if (jumlahAsal != akTerima.Jumlah)
                        {
                            await AddLogAsync("Ubah","RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" + 
                                Convert.ToDecimal(akTerima.Jumlah).ToString("#,##0.00"), akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId);
                        }
                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkTerimaExists(akTerima.Id))
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
                    if (akTerima.Jumlah != JumlahUrusniaga)
                    {
                        TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
                    }
                    else
                    {
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            TempData[SD.Warning] = "Berlaku Ralat ketika operasi simpan!";
            PopulateList();
            PopulateTable(id);
            //PopulateCart();
            return View(akTerima);
        }

        // GET: AkTerima/Delete/5
        [Authorize(Policy = "PR001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTerima = await _akTerimaRepo.GetById((int) id);

            if (akTerima == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akTerima);
            return View(akTerima);
        }

        // POST: AkTerima/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001D")]
        public async Task<IActionResult> DeleteConfirmed(int id, string sebabHapus)
        {
            var akTerima = await _context.AkTerima.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            akTerima.UserIdKemaskini = user.UserName;
            akTerima.TarKemaskini = DateTime.Now;
            akTerima.SuPekerjaKemaskiniId = pekerjaId;

            akTerima.SebabHapus = sebabHapus?.ToUpper() ?? "";
            // check if already posting redirect back
            if (akTerima.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            akTerima.FlCetak = 0;
            _context.AkTerima.Update(akTerima);

            _context.AkTerima.Remove(akTerima);

            //insert applog
            await AddLogAsync("Hapus", "Hapus Data : " + akTerima.SebabHapus, akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        // POST: AkTerima/Cancel/5
        //[Authorize(Policy = "PR001B")]
        //public async Task<IActionResult> Cancel(int id)
        //{
        //    var akTerima = await _context.AkTerima.FindAsync(id);
        //    // check if already posting redirect back
        //    if (akTerima.FlPosting == 1)
        //    {
        //        TempData[SD.Error] = "Akses tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    // check if this data is the last one (for preventing batal purpose)
        //    var lastItem = _context.AkTerima.OrderByDescending(x => x.Id).FirstOrDefault();

        //    if (lastItem.Id == akTerima.Id)
        //    {
        //        TempData[SD.Warning] = "Anda disarankan untuk hapus data ini. Operasi batal tidak dibenarkan..!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    // check end
        //    akTerima.FlHapus = 1;

        //    _context.AkTerima.Update(akTerima);

        //    //insert applog
        //    var user = await _userManager.GetUserAsync(User);

        //    AppLog appLog = new AppLog();

        //    appLog.UserId = user.UserName;
        //    appLog.LgModule = modul + "B";
        //    appLog.LgOperation = "Batal";
        //    appLog.LgNote = modul + " Penerimaan - Batal";
        //    appLog.NoRujukan = akTerima.NoRujukan;
        //    appLog.Jumlah = akTerima.Jumlah;

        //    await _appLog.Insert(appLog);
        //    //insert applog end

        //    await _context.SaveChangesAsync();
        //    TempData[SD.Success] = "Data berjaya dibatalkan..!";
        //    return RedirectToAction(nameof(Index));
        //}

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PR001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var obj = await _akTerimaRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // Rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkTerima.Update(obj);

            // Rollback operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoRujukan, id, obj.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkTerimaExists(int id)
        {
            return _context.AkTerima.Any(e => e.Id == id);
        }

        public JsonResult GetCaraBayar(JCaraBayar jCaraBayar)
        {
            try
            {
                var result = _context.JCaraBayar.Where(b => b.Id == jCaraBayar.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

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
                ViewBag.akTerima1 = new List<int>();
                ViewBag.akTerima2 = new List<int>();
                ViewBag.akTerima3 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();
                _cart.Clear3();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null )
                {
                    _cart.AddItem1(akTerima1.AkTerimaId,
                                   akTerima1.Amaun,
                                   akTerima1.AkCartaId);    
                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                bool isCek = false;

                if (akTerima2 != null)
                {
                    var caraBayar = _context.JCaraBayar.FirstOrDefault(b => b.Id == akTerima2.JCaraBayarId);

                    isCek = caraBayar.Perihal.Contains("CEK");

                    if (isCek == true)
                    {
                        if (akTerima2.JenisCek == 0)
                        {
                            return Json(new { result = "ERRORCEK", message = "Sila pilih jenis cek." });
                        }

                    }
                    var user = await _userManager.GetUserAsync(User);


                    _cart.AddItem2(akTerima2.AkTerimaId,
                                   akTerima2.JCaraBayarId,
                                   akTerima2.Amaun,
                                   akTerima2.NoCek,
                                   akTerima2.JenisCek,
                                   akTerima2.KodBankCek,
                                   akTerima2.TempatCek,
                                   akTerima2.NoSlip,
                                   akTerima2.TarSlip,
                                   null);
                }

                return Json(new { result = "OK", isCek = isCek });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {

                    _cart.RemoveItem1(akTerima1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null)
                {

                    _cart.RemoveItem2(akTerima2.JCaraBayarId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                if (akTerima3 != null)
                {

                    _cart.RemoveItem3((int)akTerima3.AkInvoisId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // Ubah AkTerima1
        public async Task<JsonResult> UpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                AkTerima1 data = await _akTerima1Repo.GetBy2Id(akTerima1.AkTerimaId, akTerima1.AkCartaId);

                return Json(new { result = "OK" , record = data});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null || akTerima1.Amaun != 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akTerima1.AkCartaId);
                    akTerima1.AkCarta = akCarta;

                    await _akTerima1Repo.Insert(akTerima1);

                    decimal total = 0;

                    AkTerima akTerima = await _akTerimaRepo.GetById(akTerima1.AkTerimaId);

                    total = akTerima.Jumlah + akTerima1.Amaun;

                    akTerima.Jumlah = total;
                    akTerima.UserIdKemaskini = user.UserName;
                    //akTerima.TarKemaskini = DateTime.Now;

                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    //AppLog appLog = new AppLog();

                    //appLog.UserId = user.UserName;
                    //appLog.LgModule = modul + "EC";
                    //appLog.LgOperation = "Tambah";
                    //appLog.LgNote = modul + " Penerimaan - Tambah Objek";
                    //appLog.NoRujukan = akTerima.NoRujukan + "/" + akTerima1.AkCarta.Kod;
                    //appLog.Jumlah = akTerima1.Amaun;

                    //await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    _cart.AddItem1(akTerima1.AkTerimaId,
                                   akTerima1.Amaun,
                                   akTerima1.AkCartaId);

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var akT1 = await _context.AkTerima1.FirstOrDefaultAsync(x=> x.AkCartaId == akTerima1.AkCartaId && x.AkTerimaId == akTerima1.AkTerimaId);
                    _context.AkTerima1.Remove(akT1);

                    decimal total = 0;

                    AkTerima akTerima = await _akTerimaRepo.GetById(akTerima1.AkTerimaId);

                    total = akTerima.Jumlah - akT1.Amaun;

                    akTerima.Jumlah = total;
                    akTerima.UserIdKemaskini = user.UserName;
                    akTerima.TarKemaskini = DateTime.Now;
                    await _akTerimaRepo.Update(akTerima);

                    await _context.SaveChangesAsync();

                    _cart.RemoveItem1(akTerima1.AkCartaId);

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                _cart.Clear1();

                AkTerima1 akT1 = await _akTerima1Repo.GetById(akTerima1.Id);

                decimal originalAmount = akT1.Amaun;
                var user = await _userManager.GetUserAsync(User);

                akT1.Amaun = akTerima1.Amaun;
                _context.AkTerima1.Update(akT1);

                // update total akTerima with date updated and userUpdated
                var akTerima = await _akTerimaRepo.GetById(akTerima1.AkTerimaId);
                decimal total = 0;

                total = akTerima.Jumlah - originalAmount + akT1.Amaun;
                akTerima.Jumlah = total;
                akTerima.UserIdKemaskini = user.UserName;
                akTerima.TarKemaskini = DateTime.Now;
                await _akTerimaRepo.Update(akTerima);
                // update total akTerima with date updated and userUpdated end

                await _context.SaveChangesAsync();

                return Json(new { result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart1(AkTerima1 akTerima1)
        {
            try
            {
                AkTerima data = await _context.AkTerima.Include(x => x.AkTerima1).ThenInclude(x=>x.AkCarta).FirstOrDefaultAsync(x => x.Id == akTerima1.AkTerimaId);

                List<AkTerima1> akT1 = data.AkTerima1.ToList();

                foreach (AkTerima1 item in akT1)
                {
                    _cart.AddItem1(item.AkTerimaId,
                                   item.Amaun,
                                   item.AkCartaId);
                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // Ubah AkTerima1 End

        // Ubah AkTerima2
        public async Task<JsonResult> UpdateAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                AkTerima2 data = await _akTerima2Repo.GetBy2Id(akTerima2.AkTerimaId, akTerima2.JCaraBayarId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null || akTerima2.Amaun != 0)
                {
                    var jCaraBayar = _context.JCaraBayar.FirstOrDefault(x => x.Id == akTerima2.JCaraBayarId);
                    var user = await _userManager.GetUserAsync(User);

                    akTerima2.JCaraBayar = jCaraBayar;
                    await _akTerima2Repo.Insert(akTerima2);

                    await _context.SaveChangesAsync();
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                if (akTerima2 != null)
                {
                    var akT2 = await _context.AkTerima2.FirstOrDefaultAsync(x => x.JCaraBayarId == akTerima2.JCaraBayarId && x.AkTerimaId == akTerima2.AkTerimaId);
                    var user = await _userManager.GetUserAsync(User);

                    _context.AkTerima2.Remove(akT2);

                    await _context.SaveChangesAsync();

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkTerima2(AkTerima2 akTerima2)
        {

            try
            {
                _cart.Clear2();

                AkTerima2 akT2 = await _akTerima2Repo.GetById(akTerima2.Id);
                var user = await _userManager.GetUserAsync(User);
                decimal originalAmount = akT2.Amaun;

                akT2.Amaun = akTerima2.Amaun;
                akT2.NoCek = akTerima2.NoCek;
                akT2.JenisCek = akTerima2.JenisCek;
                akT2.KodBankCek = akTerima2.KodBankCek;
                akT2.TempatCek = akTerima2.TempatCek;
                akT2.NoSlip = akTerima2.NoSlip;
                akT2.TarSlip = akTerima2.TarSlip;;

                _context.AkTerima2.Update(akT2);

                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart2(AkTerima2 akTerima2)
        {
            try
            {
                AkTerima data = await _context.AkTerima.Include(x => x.AkTerima2).ThenInclude(x => x.JCaraBayar).FirstOrDefaultAsync(x => x.Id == akTerima2.AkTerimaId);

                List<AkTerima2> akT2 = data.AkTerima2.ToList();

                foreach (AkTerima2 item in akT2)
                {
                    _cart.AddItem2(item.AkTerimaId,
                                   item.JCaraBayarId,
                                   item.Amaun,
                                   item.NoCek,
                                   item.JenisCek,
                                   item.KodBankCek,
                                   item.TempatCek,
                                   item.NoSlip,
                                   item.TarSlip,
                                   item.AkPenyataPemungutId);
                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Ubah AkTerima2 end

        // posting function
        [Authorize(Policy = "PR001T")]
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

                AkTerima akTerima = await _akTerimaRepo.GetById((int)id);

                //check if data print status is printed or not
                if (akTerima.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila Cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                // check if data print status is printed or not end

                List<AkTerima1> akT1 = akTerima.AkTerima1.ToList();
                List<AkTerima2> akT2 = akTerima.AkTerima2.ToList();

                //checking if jumlah objek equal to jumlah perihal 
                decimal jumlahPerihal = 0;
                foreach (AkTerima2 item in akT2)
                {
                    jumlahPerihal += item.Amaun;
                }
                if (akTerima.Jumlah != jumlahPerihal)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Jumlah Objek tidak sama dengan Jumlah Perihal.";
                    return RedirectToAction(nameof(Index));
                }
                // checking end

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                   
                }
                else
                {
                    //posting operation start here
                    //insert into akAkaun
                    
                    foreach(AkTerima1 item in akT1)
                    {
                        AkAkaun akAKodBank = new AkAkaun()
                        {
                            NoRujukan = akTerima.NoRujukan,
                            JKWId = akTerima.JKWId,
                            JBahagianId = akTerima.JBahagianId,
                            AkCartaId1 = akTerima.AkBank.AkCartaId,
                            AkCartaId2 = item.AkCartaId,
                            Tarikh = akTerima.Tarikh,
                            Debit = item.Amaun,
                            AkPenghutangId = akTerima.AkPenghutangId,
                            JSukanId = akTerima.SpPendahuluanPelbagai?.JSukanId
                        };
                        await _akAkaunRepo.Insert(akAKodBank);

                        AkAkaun akAObjek = new AkAkaun()
                        {
                            NoRujukan = akTerima.NoRujukan,
                            JKWId = akTerima.JKWId,
                            JBahagianId = akTerima.JBahagianId,
                            AkCartaId1 = item.AkCartaId,
                            AkCartaId2 = akTerima.AkBank.AkCartaId,
                            Tarikh = akTerima.Tarikh,
                            Kredit = item.Amaun,
                            AkPenghutangId = akTerima.AkPenghutangId,
                            JSukanId = akTerima.SpPendahuluanPelbagai?.JSukanId
                        };

                        await _akAkaunRepo.Insert(akAObjek);

                        if (akTerima.FlPostingBukuVot == 1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVotPosting = new AbBukuVot()
                            {
                                Tahun = akTerima.Tahun,
                                JKWId = akTerima.JKWId,
                                JBahagianId = akTerima.JBahagianId,
                                Tarikh = akTerima.Tarikh,
                                //Kod = "",
                                Penerima = akTerima.Nama,
                                VotId = item.AkCartaId,
                                Rujukan = akTerima.NoRujukan,
                                Belanja = -item.Amaun,
                                Debit = -item.Amaun
                            };
                            await _abBukuVotRepo.Insert(abBukuVotPosting);
                        }
                        
                    }
                    
                    //update posting status in akTerima
                    akTerima.FlPosting = 1;
                    akTerima.TarikhPosting = DateTime.Now;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", akTerima.NoRujukan, (int)id, akTerima.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }

                
            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "PR001UT")]
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

                AkTerima akTerima = await _akTerimaRepo.GetById((int)id);

                // if already exist in Penyata Pemungut, declare error
                foreach(var akTerima2 in akTerima.AkTerima2)
                {
                    if (!string.IsNullOrEmpty(akTerima2.NoSlip) || !string.IsNullOrEmpty(akTerima2.TarSlip?.ToString("dd/MM/yyyy")))
                    {
                        bool IsExistPenyataPemungut = await _context.AkPenyataPemungut.AnyAsync(b => b.NoDokumen == akTerima2.NoSlip);
                        if (IsExistPenyataPemungut == true)
                        {
                            TempData[SD.Error] = "Data terlibat dengan Penyata Pemungut " + akTerima2.NoSlip + ". Batal Posting tidak dibenarkan.";
                            return RedirectToAction(nameof(Index));
                        }
                        
                    } ;
                }
                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    //unposting operation start here
                    //delete data from akAkaun
                    foreach (AkAkaun item in akAkaun)
                    {
                        await _akAkaunRepo.Delete(item.Id);
                    }

                    if (akTerima.FlPostingBukuVot == 1)
                    {
                        List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(akTerima.NoRujukan)).ToList();
                        if (abBukuVot == null)
                        {

                            //duplicate id error
                            TempData[SD.Error] = "Data belum diluluskan.";

                        }
                        else
                        {
                            //delete data from abBukuVot
                            foreach (AbBukuVot item in abBukuVot)
                            {
                                await _abBukuVotRepo.Delete(item.Id);
                            }
                            //delete data from abBukuVot end
                        }

                    }

                    //update posting status in akTerima
                    akTerima.FlPosting = 0;
                    akTerima.TarikhPosting = null;
                    //akTerima.TarikhPosting = null;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    await AddLogAsync("UnPosting", "UnPosting Data", akTerima.NoRujukan, (int)id, akTerima.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        // printing resit rasmi by akTerima.Id
        [Authorize(Policy = "PR001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkTerima akTerima = await _akTerimaRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (akTerima.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - akTerima.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akTerima.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            string penyedia = "SuperAdmin";

            if (akTerima.SuPekerjaMasukId != null)
            {
                penyedia = _context.SuPekerja.FirstOrDefault(b => b.Id == akTerima.SuPekerjaMasukId).Nama;

            }

            ResitPrintModel data = new ResitPrintModel();

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.CompanyDetail = company;
            data.AkTerima = akTerima;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.penyedia = penyedia;
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            data.username = namaUser.Nama;

            //update cetak -> 1
            akTerima.FlCetak = 1;
            await _akTerimaRepo.Update(akTerima);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", akTerima.NoRujukan, id, akTerima.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("ResitPrintPdf",data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            }  ;
            
        }
        // printing resit rasmi end

        // on change kod pembekal controller
        [HttpPost]
        public async Task<JsonResult> JsonGetPenghutang(int data)
        {
            try
            {
                var result = await _akPenghutangRepo.GetById(data);

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
        public async Task<JsonResult> JsonGetInboisDikeluarkan(int data)
        {
            try
            {
                var result = await _context.AkInvois.Include(b => b.AkPenghutang).Where(x => x.AkPenghutangId == data).ToListAsync();

                if (result.Count() == 0)
                {
                    return Json(new { result = "Error" });
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod pembekal controller end

        // on change inbois controller
        [HttpPost]
        public async Task<JsonResult> JsonGetAkInvois(int data)
        {
            try
            {
                //_cart.Clear3();
                var result = await _akInvoisRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        //on change inbois controller end

        // json empty Cart controller
        [HttpPost]
        public JsonResult JsonEmptyCart()
        {
            try
            {
                CartEmpty();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // json empty cart end

        //function json Create akTerima3
        public JsonResult GetAkInvois(AkInvois akInvois)
        {
            try
            {

                var result = _context.AkInvois
                    .Include(b => b.AkInvois1).ThenInclude(b => b.AkCarta)
                    .Where(b => b.Id == akInvois.Id)
                    .FirstOrDefault();

                //if (result!= null)
                //{
                //    PopulateCartAkPV1(result.Id);
                //}
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        public JsonResult SaveAkTerima3(AkTerima3 akTerima3)
        {

            try
            {
                if (akTerima3 != null)
                {

                    // add akTerima3 into cart lines3
                    _cart.AddItem3(akTerima3.AkTerimaId,
                                   akTerima3.AkInvoisId,
                                   akTerima3.Amaun);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
    }
}
