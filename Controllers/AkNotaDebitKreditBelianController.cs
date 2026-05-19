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
    //[Authorize(Roles = "SuperAdmin,Supervisor,User")]
    [Authorize(Roles = "SuperAdmin")]
    public class AkNotaDebitKreditBelianController : Controller
    {
        public const string modul = "ND001";
        public const string namamodul = "Nota Debit Kredit Belian";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkNotaDebitKreditBelian, int, string> _akNotaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<JBahagian, int, string> _bahagianRepo;
        private readonly ListViewIRepository<AkNotaDebitKreditBelian1, int> _akNota1Repo;
        private readonly ListViewIRepository<AkNotaDebitKreditBelian2, int> _akNota2Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private CartNotaDebitKreditBelian _cart;

        public AkNotaDebitKreditBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkNotaDebitKreditBelian, int, string> akNota,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<JBahagian, int, string> bahagian,
            ListViewIRepository<AkNotaDebitKreditBelian1, int> akNota1,
            ListViewIRepository<AkNotaDebitKreditBelian2, int> akNota2,
            IRepository<AkCarta, int ,string> akCarta,
            IRepository<AbBukuVot, int, string> abBukuVot,
            IRepository<AkAkaun, int, string> akAkaun,
            IRepository<AkBelian, int, string> akBelian,
            IRepository<AkPV, int, string> akPV,
            CartNotaDebitKreditBelian cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akNotaRepo = akNota;
            _akPembekalRepo = akPembekal;
            _abBukuVotRepo = abBukuVot;
            _bahagianRepo = bahagian;
            _akNota1Repo = akNota1;
            _akNota2Repo = akNota2;
            _akCartaRepo = akCarta;
            _akAkaunRepo = akAkaun;
            _akBelianRepo = akBelian;
            _akPVRepo = akPV;
            _cart = cart;
        }

        private async Task AddLogAsync(
            string operasi,
            string nota,
            string rujukan,
            int idRujukan,
            decimal jumlah,
            int? suPekerjaId)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = suPekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        // GET: AkNotaDebitKreditBelian
        [Authorize(Policy = "ND001")]
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

            var model = new List<AkNotaDebitKreditBelian>().AsEnumerable();
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                model = await _akNotaRepo.GetAllIncludeDeletedItemsFiltered(searchString,searchDate1,searchDate2,searchColumn);
            }
            else
            {
                model = await _akNotaRepo.GetAllFiltered(searchString,searchDate1, searchDate2, searchColumn);  
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

            List<AkNotaDebitKreditBelianViewModel> viewModel = new List<AkNotaDebitKreditBelianViewModel>();

            foreach (AkNotaDebitKreditBelian item in model)
            {
                var namaSykt = "";
                var alamat1 = "";

                if (item.AkBelian.AkPOId == null)
                {
                    namaSykt = item.AkBelian.AkPembekal.NamaSykt;
                    alamat1 = item.AkBelian.AkPembekal.Alamat1;
                }
                else
                {
                    namaSykt = item.AkBelian.AkPO.AkPembekal.NamaSykt;
                    alamat1 = item.AkBelian.AkPO.AkPembekal.Alamat1;
                }

                decimal jumlahPerihal = 0;
                foreach (AkNotaDebitKreditBelian2 item2 in item.AkNotaDebitKreditBelian2)
                {
                    jumlahPerihal += item2.Amaun;
                }
                viewModel.Add(new AkNotaDebitKreditBelianViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoRujukan = item.NoRujukan,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    NamaSykt = namaSykt,
                    Alamat1 = alamat1,
                    FlJenis = item.FlJenis,
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
            List<JKW> jkwList = _context.JKW.ToList();
            ViewBag.JKW = jkwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkBelian> akBelianList = _context.AkBelian
                .Include(b => b.AkPembekal).ThenInclude(b => b.JBank)
                .Include(b => b.JKW)
                .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                .Include(b => b.AkBelian2)
                .Include(b => b.AkPV2)
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();

            List<AkBelian> updatedList = new List<AkBelian>();

            foreach (var item in akBelianList)
            {
                if (item.AkPV2.Count() > 0 )
                {
                    decimal jumlah = 0;

                    foreach (var item2 in item.AkPV2)
                    {
                        jumlah =+item2.Amaun;
                    }

                    if (jumlah >= item.Jumlah)
                    {
                        continue;
                    }
                }
                updatedList.Add(item);
            }

            ViewBag.AkBelian = updatedList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4" && (b.Kod.Substring(0, 1) == "B" || b.Kod.Substring(0, 1) == "A"))
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {
            List<AkNotaDebitKreditBelian1> table1 = _context.AkNotaDebitKreditBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaDebitKreditBelianId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.table1 = table1;

            List<AkNotaDebitKreditBelian2> table2 = _context.AkNotaDebitKreditBelian2
                .Where(b => b.AkNotaDebitKreditBelianId == id)
                .OrderBy(b => b.Bil)
                .ToList();
            ViewBag.table2 = table2;
        }
        private void PopulateCart()
        {
            List<AkNotaDebitKreditBelian1> lines1 = _cart.Lines1.ToList();

            foreach (AkNotaDebitKreditBelian1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            List<AkNotaDebitKreditBelian2> lines2 = _cart.Lines2.ToList();

            ViewBag.table1 = lines1;
            ViewBag.table2 = lines2;
        }

        private void PopulateCartFromDb(AkNotaDebitKreditBelian model)
        {
            List<AkNotaDebitKreditBelian1> table1 = _context.AkNotaDebitKreditBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkNotaDebitKreditBelianId == model.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkNotaDebitKreditBelian1 item in table1)
            {
                _cart.AddItem1(item.AkNotaDebitKreditBelianId,
                               item.Amaun,
                               item.AkCartaId);
            }

            List<AkNotaDebitKreditBelian2> table2 = _context.AkNotaDebitKreditBelian2
                .Where(b => b.AkNotaDebitKreditBelianId == model.Id)
                .OrderBy(b => b.Bil)
                .ToList();
            foreach (AkNotaDebitKreditBelian2 item in table2)
            {
                _cart.AddItem2(item.AkNotaDebitKreditBelianId,
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

        // GET: AkNotaDebitKreditBelian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // admin access
            var model = await _akNotaRepo.GetByIdIncludeDeletedItems((int)id);
            
            if (model == null)
            {
                return NotFound();
            }

            // admin access end

            AkNotaDebitKreditBelianViewModel viewModel = new AkNotaDebitKreditBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkBelian = model.AkBelian;

            viewModel.Id = model.Id;
            viewModel.Tahun = model.Tahun;
            viewModel.NoRujukan = model.NoRujukan;
            viewModel.Tarikh = model.Tarikh;
            viewModel.JBahagianId = model.JBahagianId;
            viewModel.JBahagian = model.JBahagian;
            viewModel.Jumlah = model.Jumlah;
            viewModel.TarikhPosting = model.TarikhPosting;
            viewModel.FlJenis = model.FlJenis;
            viewModel.FlPosting = model.FlPosting;
            viewModel.FlHapus = model.FlHapus;

            foreach (AkNotaDebitKreditBelian2 item in model.AkNotaDebitKreditBelian2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaDebitKreditBelian1 = model.AkNotaDebitKreditBelian1;
            viewModel.AkNotaDebitKreditBelian2 = model.AkNotaDebitKreditBelian2;

            PopulateTable(id);
            return View(viewModel);
        }

        // GET: AkNotaDebitKreditBelian/Create
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var data = _context.JBahagian.FirstOrDefault(x => x.Id == 1);

            AkNotaDebitKreditBelian model = new AkNotaDebitKreditBelian();

            model.JBahagian = data;
            model.JBahagianId = 1;
            model.Tahun = year;
            ViewBag.NoRujukan = RunningNumber(model);
            // get latest no rujukan running number end

            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.table1 = new List<int>();
                ViewBag.table2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

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

        // on change no Invois controller
        [HttpPost]
        public async Task<JsonResult> JsonGetNoInvois(int id)
        {
            try
            {
                CartEmpty();
                PopulateCartFromAkBelian(id);
                var result = await _akBelianRepo.GetById(id);

                // get latest no rujukan running number 
                var year = DateTime.Now.Year.ToString();
                var data = _context.JBahagian.FirstOrDefault(x => x.Id == result.JBahagianId);

                AkNotaDebitKreditBelian model = new AkNotaDebitKreditBelian();

                model.JBahagian = data;
                model.JBahagianId = (int)result.JBahagianId;
                model.Tahun = year;
                var NoRujukan = RunningNumber(model);
                // get latest no rujukan running number end

                List<AkBelian1> akBelian1Table = await _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Id)
                .ToListAsync();

                foreach (AkBelian1 item in akBelian1Table)
                {
                    if (item.Amaun != 0)
                    {
                        result.AkBelian1.Add(item);
                    }
                }

                List<AkBelian2> akBelian2Table = await _context.AkBelian2
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Bil)
                .ToListAsync();

                foreach (AkBelian2 item in akBelian2Table)
                {
                    result.AkBelian2.Add(item);
                }

                result.AkBelian2 = result.AkBelian2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = result, rujukan = NoRujukan });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        private void PopulateCartFromAkBelian(int id)
        {
            var user = _userManager.GetUserName(User);

            List<AkBelian1> akBelian1Table = _context.AkBelian1
                .Include(b => b.AkCarta)
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (AkBelian1 item in akBelian1Table)
            {

                item.AkBelianId = 0;

                if (item.Amaun != 0)
                {
                    _cart.AddItem1(item.AkBelianId,
                                   item.Amaun,
                                   item.AkCartaId);
                }
            }

            List<AkBelian2> akBelian2Table = _context.AkBelian2
                .AsNoTracking()
                .Where(b => b.AkBelianId == id)
                .OrderBy(b => b.Bil)
                .ToList();

            foreach (AkBelian2 item in akBelian2Table)
            {
                item.AkBelianId = 0;

                _cart.AddItem2(item.AkBelianId,
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
        //on change no Invois controller end

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

        public async Task<JsonResult> SaveAkNotaDebitKreditBelian1(AkNotaDebitKreditBelian1 akNota1)
        {

            try
            {
                if (akNota1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(akNota1.AkNotaDebitKreditBelianId,
                                    akNota1.Amaun,
                                    akNota1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkNotaDebitKreditBelian2(AkNotaDebitKreditBelian2 akNota2)
        {

            try
            {
                if (akNota2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(akNota2.AkNotaDebitKreditBelianId,
                                   akNota2.Indek,
                                   akNota2.Bil,
                                   akNota2.NoStok,
                                   akNota2.Perihal?.ToUpper()?? "",
                                   akNota2.Kuantiti,
                                   akNota2.Unit?.ToUpper()?? "",
                                   akNota2.Harga,
                                   akNota2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaDebitKreditBelian1(AkNotaDebitKreditBelian1 akNota1)
        {

            try
            {
                if (akNota1 != null)
                {

                    _cart.RemoveItem1(akNota1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkNotaDebitKreditBelian2(AkNotaDebitKreditBelian2 akNota2)
        {

            try
            {
                if (akNota2 != null)
                {

                    _cart.RemoveItem2(akNota2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akNotaDebitKreditBelian1
        public JsonResult GetAnItemCartAkNotaDebitKreditBelian1(AkNotaDebitKreditBelian1 akNota1)
        {

            try
            {
                AkNotaDebitKreditBelian1 data = _cart.Lines1.Where(x => x.AkCartaId == akNota1.AkCartaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaDebitKreditBelian1 end

        //save cart akNotaDebitKreditBelian1
        public JsonResult SaveCartAkNotaDebitKreditBelian1(AkNotaDebitKreditBelian1 akNota1)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akNota1.AkCartaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT1 != null)
                {
                    _cart.RemoveItem1(akNota1.AkCartaId);

                    _cart.AddItem1(akNota1.AkNotaDebitKreditBelianId,
                                    akNota1.Amaun,
                                    akNota1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaDebitKreditBelian1 end

        // get all item from cart akNotaDebitKreditBelian1
        public JsonResult GetAllItemCartAkNotaDebitKreditBelian1()
        {

            try
            {
                List<AkNotaDebitKreditBelian1> data = _cart.Lines1.ToList();

                foreach (AkNotaDebitKreditBelian1 item in data)
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
        // get all item from cart akNotaDebitKreditBelian1 end

        // get an item from cart akNotaDebitKreditBelian2
        public JsonResult GetAnItemCartAkNotaDebitKreditBelian2(AkNotaDebitKreditBelian2 akNota2)
        {

            try
            {
                AkNotaDebitKreditBelian2 data = _cart.Lines2.Where(x => x.Indek == akNota2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akNotaDebitKreditBelian2 end

        //save cart akNotaDebitKreditBelian2
        public JsonResult SaveCartAkNotaDebitKreditBelian2(AkNotaDebitKreditBelian2 akNota2)
        {

            try
            {

                var akT2 = _cart.Lines2.Where(x => x.Indek == akNota2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem2(akNota2.Indek);

                    _cart.AddItem2(akNota2.AkNotaDebitKreditBelianId,
                                   akNota2.Indek,
                                   akNota2.Bil,
                                   akNota2.NoStok,
                                   akNota2.Perihal?.ToUpper()?? "",
                                   akNota2.Kuantiti,
                                   akNota2.Unit?.ToUpper()?? "",
                                   akNota2.Harga,
                                   akNota2.Amaun);
                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaDebitKreditBelian2 end

        // get all item from cart akNotaDebitKreditBelian2
        public JsonResult GetAllItemCartAkNotaDebitKreditBelian2()
        {

            try
            {
                List<AkNotaDebitKreditBelian2> data = _cart.Lines2.OrderBy(b => b.Bil).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akNotaDebitKreditBelian2 end

        //Function Running Number
        private string RunningNumber(AkNotaDebitKreditBelian data)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data.JBahagian.JKWId);

            var bahagian = data.JBahagian.Kod;
            var year = data.Tahun;
            string prefix = "NB/" + year + "/" + bahagian + "/";
            int x = 1;
            string noRujukan = prefix + "0000";

            var LatestNoRujukan = _context.AkNotaDebitKreditBelian
                .IgnoreQueryFilters()
                        .Where(x => x.Tahun == year && x.JBahagianId == data.JBahagianId)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(15));
                x++;
                noRujukan = string.Format("{0:" + prefix + "0000}", x);
            }
            return noRujukan;
        }

        // POST: AkNotaDebitKreditBelian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "ND001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkNotaDebitKreditBelian akNotaDebitKreditBelian, int FlJenis,int AkBelianId, int JBahagianId)
        {
            AkNotaDebitKreditBelian m = new AkNotaDebitKreditBelian();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            if (user.Email == "superadmin@idwal.com.my")
            {
                akNotaDebitKreditBelian.SuPekerjaMasukId = 1;
            }
            else
            {
                akNotaDebitKreditBelian.SuPekerjaMasukId = pekerjaId;
            }

            var belian = await _akBelianRepo.GetById(AkBelianId);

            if (belian == null)
            {
                TempData[SD.Error] = "no invois tidak wujud..!";
                //PopulateCart();
                PopulateList();
                CartEmpty();
                return View(akNotaDebitKreditBelian);
            }

            var pembekal = await _akPembekalRepo.GetById(belian.AkPembekalId);

            if (pembekal == null)
            {
                TempData[SD.Error] = "Pembekal tidak wujud..!";
                //PopulateCart();
                PopulateList();
                CartEmpty();
                return View(akNotaDebitKreditBelian);
            }
            var JBahagian = _context.JBahagian.Include(b => b.JKW)
                .FirstOrDefault(b => b.Id == akNotaDebitKreditBelian.JBahagianId);

            akNotaDebitKreditBelian.JBahagian = JBahagian;

            // get latest no rujukan running number  
            var noRujukan = RunningNumber(akNotaDebitKreditBelian);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akNotaDebitKreditBelian != null && JBahagianId != 0 && AkBelianId != 0)
                {
                    m.JBahagianId = JBahagianId;
                    m.Tahun = akNotaDebitKreditBelian.Tahun;
                    m.NoRujukan = noRujukan;
                    m.Tarikh = akNotaDebitKreditBelian.Tarikh;
                    m.Jumlah = akNotaDebitKreditBelian.Jumlah;
                    m.FlJenis = FlJenis;
                    m.Perihal = akNotaDebitKreditBelian.Perihal?.ToUpper();
                    m.AkBelianId = AkBelianId;
                    m.FlPosting = 0;
                }
                m.UserId = user.UserName;
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                m.AkNotaDebitKreditBelian1 = _cart.Lines1.ToArray();
                m.AkNotaDebitKreditBelian2 = _cart.Lines2.ToArray();

                await _akNotaRepo.Insert(m);

                //insert applog
                await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, m.Jumlah, pekerjaId);
                //insert applog end

                await _context.SaveChangesAsync();

                CartEmpty();
                TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                return RedirectToAction(nameof(Index));
            }

            CartEmpty();
            PopulateList();
            return View(akNotaDebitKreditBelian);
        }

        // GET: AkNotaDebitKreditBelian/Edit/5.
        [Authorize(Policy = "ND001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _akNotaRepo.GetById((int)id);

            if (model == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(model);
            return View(model);
        }

        // get latest Index number in AkNotaDebitKreditBelian2
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

        // POST: AkNotaDebitKreditBelian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "ND001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkNotaDebitKreditBelian model, decimal JumlahPerihal, int JBahagianId)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (model.Jumlah == JumlahPerihal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                        AkNotaDebitKreditBelian modelAsal = await _akNotaRepo.GetById(id);

                        // list of input that cannot be change
                        model.Tahun = modelAsal.Tahun;
                        model.JBahagianId = modelAsal.JBahagianId;
                        model.NoRujukan = modelAsal.NoRujukan;

                        model.AkBelianId = modelAsal.AkBelianId;

                        model.TarMasuk = modelAsal.TarMasuk;
                        model.UserId = modelAsal.UserId;
                        model.SuPekerjaMasukId = modelAsal.SuPekerjaMasukId;
                        
                        decimal jumlahAsal = modelAsal.Jumlah;
                        // list of input that cannot be change end

                        foreach (AkNotaDebitKreditBelian1 item in modelAsal.AkNotaDebitKreditBelian1)
                        {
                            var child = _context.AkNotaDebitKreditBelian1.FirstOrDefault(b => b.Id == item.Id);
                            if (child != null)
                            {
                                _context.Remove(child);
                            }
                        }

                        foreach (AkNotaDebitKreditBelian2 item in modelAsal.AkNotaDebitKreditBelian2)
                        {
                            var child = _context.AkNotaDebitKreditBelian2.FirstOrDefault(b => b.Id == item.Id);
                            if (child != null)
                            {
                                _context.Remove(child);
                            }
                        }
                        _context.Entry(modelAsal).State = EntityState.Detached;

                        model.AkNotaDebitKreditBelian1 = _cart.Lines1.ToList();
                        model.AkNotaDebitKreditBelian2 = _cart.Lines2.ToList();

                        model.UserIdKemaskini = user.UserName;
                        model.TarKemaskini = DateTime.Now;
                        model.SuPekerjaKemaskiniId = pekerjaId;

                        _context.Update(model);

                        //insert applog
                        if (jumlahAsal != model.Jumlah)
                        {
                            await AddLogAsync("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                                Convert.ToDecimal(model.Jumlah).ToString("#,##0.00"), model.NoRujukan, id, model.Jumlah, pekerjaId);

                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", model.NoRujukan, id, model.Jumlah, pekerjaId);
                        }

                        //insert applog end

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkNotaDebitKreditBelianExists(model.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Perihal";
            PopulateList();
            PopulateTable(id);
            return View(model);
        }

        // posting function
        [Authorize(Policy = "ND001T")]
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

                AkNotaDebitKreditBelian model = await _akNotaRepo.GetById((int)id);

                List<AkNotaDebitKreditBelian1> akB1 = model.AkNotaDebitKreditBelian1.ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == model.NoRujukan).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";

                }
                else
                {
                    //posting operation start here
                    if (model.Tahun != null)
                    {
                        var kodPembekal = "";
                        var penerima = "";

                        kodPembekal = model.AkBelian.AkPembekal.KodSykt;
                        penerima = model.AkBelian.AkPembekal.NamaSykt;

                        foreach (AkNotaDebitKreditBelian1 item in akB1)
                        {
                            //insert into AbBukuVot
                            AbBukuVot abBukuVot = new AbBukuVot();
                            
                            // debit 
                            if (model.FlJenis == 0)
                            {
                                abBukuVot = new AbBukuVot()
                                {
                                    Tahun = model.Tahun,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    Tarikh = model.Tarikh,
                                    Kod = kodPembekal,
                                    Penerima = penerima,
                                    VotId = item.AkCartaId,
                                    Rujukan = model.NoRujukan,
                                    Liabiliti = item.Amaun
                                };
                            }
                            else
                            {
                                //kredit
                                abBukuVot = new AbBukuVot()
                                {
                                    Tahun = model.Tahun,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    Tarikh = model.Tarikh,
                                    Kod = kodPembekal,
                                    Penerima = penerima,
                                    VotId = item.AkCartaId,
                                    Rujukan = model.NoRujukan,
                                    Liabiliti = 0 - item.Amaun
                                };

                            }

                            await _abBukuVotRepo.Insert(abBukuVot);

                            // insert into AbBukuVot end

                            AkAkaun akALiabiliti = new AkAkaun();
                            AkAkaun akAObjek = new AkAkaun();

                            // debit
                            if (model.FlJenis == 0)
                            {
                                //insert into akAkaun
                                akALiabiliti = new AkAkaun()
                                {
                                    NoRujukan = model.NoRujukan,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    AkCartaId1 = model.AkBelian.KodObjekAPId,
                                    AkCartaId2 = item.AkCartaId,
                                    Tarikh = model.Tarikh,
                                    Kredit = item.Amaun,
                                    AkPembekalId = model.AkBelian.AkPembekalId
                                };

                                akAObjek = new AkAkaun()
                                {
                                    NoRujukan = model.NoRujukan,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    AkCartaId1 = item.AkCartaId,
                                    AkCartaId2 = model.AkBelian.KodObjekAPId,
                                    Tarikh = model.Tarikh,
                                    Debit = item.Amaun,
                                    AkPembekalId = model.AkBelian.AkPembekalId
                                };
                            }
                            else
                            {
                                // kredit
                                //insert into akAkaun
                                akALiabiliti = new AkAkaun()
                                {
                                    NoRujukan = model.NoRujukan,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    AkCartaId1 = model.AkBelian.KodObjekAPId,
                                    AkCartaId2 = item.AkCartaId,
                                    Tarikh = model.Tarikh,
                                    Debit = item.Amaun,
                                    AkPembekalId = model.AkBelian.AkPembekalId
                                };

                                akAObjek = new AkAkaun()
                                {
                                    NoRujukan = model.NoRujukan,
                                    JKWId = model.JBahagian.JKWId,
                                    JBahagianId = model.JBahagianId,
                                    AkCartaId1 = item.AkCartaId,
                                    AkCartaId2 = model.AkBelian.KodObjekAPId,
                                    Tarikh = model.Tarikh,
                                    Kredit = item.Amaun,
                                    AkPembekalId = model.AkBelian.AkPembekalId
                                };
                            }
                           
                            await _akAkaunRepo.Insert(akALiabiliti);

                            await _akAkaunRepo.Insert(akAObjek);

                        }

                        //update posting status in akTerima
                        model.FlPosting = 1;
                        model.TarikhPosting = DateTime.Now;
                        await _akNotaRepo.Update(model);

                        //insert applog
                        await AddLogAsync("Posting", "Posting Data", model.NoRujukan, (int)id, model.Jumlah, pekerjaId);

                        //insert applog end

                        await _context.SaveChangesAsync();


                        TempData[SD.Success] = "Data berjaya diluluskan.";
                    }
                    else
                    {
                        //duplicate id error
                        TempData[SD.Error] = "Sila isi tahun untuk meneruskan operasi ini.";
                    }

                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "ND001UT")]
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

                AkNotaDebitKreditBelian model = await _akNotaRepo.GetById((int)id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == model.NoRujukan).ToList();

                // check if already link with AkNotaDebitKreditBelian
                var akNota = await _context.AkNotaDebitKreditBelian.FirstOrDefaultAsync(b => b.AkBelianId == id);

                if (akNota != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data terkait dengan nota debit/kredit " + akNota.NoRujukan + ".";
                    return RedirectToAction(nameof(Index));
                }

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == model.NoRujukan).ToList();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum diluluskan.";

                }
                else
                {
                    var akPV = await _akPVRepo.GetAll(null);
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

                                  }).Where(x => x.AkBelianId == model.AkBelianId).FirstOrDefault();

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
                        model.FlPosting = 0;
                        model.TarikhPosting = null;
                        await _akNotaRepo.Update(model);

                        //insert applog
                        await AddLogAsync("UnPosting", "Batal Posting Data", model.NoRujukan, (int)id, model.Jumlah, pekerjaId);

                        //insert applog end

                        await _context.SaveChangesAsync();

                        TempData[SD.Success] = "Data berjaya batal kelulusan.";
                        //unposting operation end
                    }

                }


            }

            return RedirectToAction(nameof(Index));

        }

        // GET: AkNotaDebitKreditBelian/Delete/5
        [Authorize(Policy = "ND001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // admin access
            var model = await _akNotaRepo.GetByIdIncludeDeletedItems((int)id);

            if (model == null)
            {
                return NotFound();
            }

            // admin access end

            AkNotaDebitKreditBelianViewModel viewModel = new AkNotaDebitKreditBelianViewModel();

            //fill in view model AkPVViewModel from akPV
            viewModel.AkBelian = model.AkBelian;

            viewModel.Id = model.Id;
            viewModel.Tahun = model.Tahun;
            viewModel.NoRujukan = model.NoRujukan;
            viewModel.Tarikh = model.Tarikh;
            viewModel.JBahagianId = model.JBahagianId;
            viewModel.JBahagian = model.JBahagian;
            viewModel.Jumlah = model.Jumlah;
            viewModel.TarikhPosting = model.TarikhPosting;
            viewModel.FlJenis = model.FlJenis;
            viewModel.FlPosting = model.FlPosting;
            viewModel.FlHapus = model.FlHapus;

            foreach (AkNotaDebitKreditBelian2 item in model.AkNotaDebitKreditBelian2)
            {
                viewModel.JumlahPerihal += item.Amaun;
            }
            viewModel.AkNotaDebitKreditBelian1 = model.AkNotaDebitKreditBelian1;
            viewModel.AkNotaDebitKreditBelian2 = model.AkNotaDebitKreditBelian2;

            PopulateTable(id);
            return View(viewModel);
        }

        // POST: AkNotaDebitKreditBelian/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "ND001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.AkNotaDebitKreditBelian.FindAsync(id);
            // check if already posting redirect back
            if (model.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // check if already link with akPV, Batal akPV included
            var akPV = await _akPVRepo.GetAll(null);
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

                          }).Where(x => x.AkBelianId == model.AkBelianId).FirstOrDefault();

            if (result != null)
            {
                AkPV akPVItem = await _akPVRepo.GetById(result.AkPVId);
                //duplicate id error
                TempData[SD.Error] = "Data terkait dengan no baucer " + akPVItem.NoPV + ".";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            model.UserIdKemaskini = user.UserName;
            model.TarKemaskini = DateTime.Now;
            model.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkNotaDebitKreditBelian.Remove(model);
            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", model.NoRujukan, id, model.Jumlah, pekerjaId);
            //insert applog end
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "ND001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akNotaRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            if (CurrentAkNotaExists(obj.Tahun, obj.NoRujukan) == false)
            {
                // Batal operation

                obj.FlHapus = 0;
                obj.UserIdKemaskini = user.UserName;
                obj.TarKemaskini = DateTime.Now;
                obj.SuPekerjaKemaskiniId = pekerjaId;
                _context.AkNotaDebitKreditBelian.Update(obj);

                // Batal operation end

                //insert applog
                await AddLogAsync("Rollback", "Rollback Data", obj.NoRujukan, id, obj.Jumlah, pekerjaId);
                //insert applog end

                await _context.SaveChangesAsync();
                TempData[SD.Success] = "Data berjaya dikembalikan..!";

            }
            else
            {
                TempData[SD.Error] = "No Rujukan telah wujud..!";

            }
            return RedirectToAction(nameof(Index));

        }

        private bool CurrentAkNotaExists(string tahun, string noRujukan)
        {
            return _context.AkNotaDebitKreditBelian.Any(e => e.Tahun == tahun && e.NoRujukan == noRujukan);
        }

        private bool AkNotaDebitKreditBelianExists(int id)
        {
            return _context.AkNotaDebitKreditBelian.Any(e => e.Id == id);
        }
    }
}
