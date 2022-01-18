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

namespace MSNK.Controllers
{
    [Authorize]
    public class AkTerimaController : Controller
    {
        public const string modul = "PR001";

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
            _cart = cart;
        }

        [Authorize(Policy = "PR001")]
        // GET: AkTerima
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

            var akTerima = await _akTerimaRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            { 
                    // searching with '%like%' condition
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (searchColumn == "NoRujukan")
                        {
                            akTerima = akTerima.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                        }
                        else if (searchColumn == "Nama")
                        {
                            akTerima = akTerima.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akTerima = akTerima.Where(x => x.Tarikh >= date1
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
                    FlBatal = item.FlBatal,
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

            var akTerima = await _akTerimaRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akTerima.JKWId);
            akTerima.JKW = kw;
            var negeri = await _negeriRepo.GetById(akTerima.JNegeriId);
            akTerima.JNegeri = negeri;
            var akBank = await _akBankRepo.GetById(akTerima.AkBankId);
            akTerima.AkBank = akBank;
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

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

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
                               akTerima2.TarSlip);
            }

            ViewBag.akTerima2 = akTerima2Table;
        }

        // GET: AkTerima/Create
        [Authorize(Policy = "PR001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Kod == "100");

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            string prefix = kumpulanWang + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTerima
                        .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(12));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;
            PopulateList();
            CartEmpty();
            return View();
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
                    var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

                    var kumpulanWang = kw.Kod;
                    string prefix = kumpulanWang + year;
                    int x = 1;
                    string noRujukan = prefix + "000000";

                    var LatestNoRujukan = _context.AkTerima
                        .Where(x=> x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);

                    if (LatestNoRujukan == null)
                    {
                        noRujukan = string.Format("{0:" + prefix + "000000}", x);
                    }
                    else
                    {
                        x = int.Parse(LatestNoRujukan.Substring(12));
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
                                   akTerima2.TarSlip);
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
                    if (item.JenisCek == null)
                    {
                        item.JenisCek = "";
                    }
                    else
                    {
                        switch (item.JenisCek)
                        {
                            case "1":
                                item.JenisCek = "Cek Cawangan Ini";
                                break;
                            case "2":
                                item.JenisCek = "Cek Tempatan";
                                break;
                            case "3":
                                item.JenisCek = "Cek Luar";
                                break;
                            case "4":
                                item.JenisCek = "Cek Antarabangsa";
                                break;

                        }
                    }
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akTerima2 end

        // POST: AkTerima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "PR001C")]
        public async Task<IActionResult> Create(AkTerima akTerima, int JKWId, int JNegeriId, int AkBankId, decimal JumlahUrusniaga)
        {
            
            AkTerima m = new AkTerima();
            var user = await _userManager.GetUserAsync(User);

            // checking for jumlah objek & jumlah perihal
            if (akTerima.Jumlah != JumlahUrusniaga)
            {
                TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Perihal";
                //PopulateCart();
                CartEmpty();
                PopulateList();
                return View(akTerima);
            }

            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == akTerima.JKWId);

            var kumpulanWang = kw.Kod;
            var year = akTerima.Tahun;
            string prefix = "RR/" + kumpulanWang + year;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTerima
                        .Where(x => x.Tahun == year && x.JKW.Kod == kw.Kod)
                        .Max(x => x.NoRujukan);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(12));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akTerima != null && JNegeriId != 0 && JKWId != 0 && JNegeriId != 0)
                {
                    
                    m.JKWId = JKWId;
                    m.JNegeriId = JNegeriId;
                    m.AkBankId = AkBankId;
                    m.Tahun = akTerima.Tahun;
                    m.NoRujukan = noRujukan;
                    m.Tarikh = akTerima.Tarikh;
                    m.Jumlah = akTerima.Jumlah;
                    m.FlCetak = 0;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
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
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    //m.TarKemaskini = akTerima.TarKemaskini;

                    m.AkTerima1 = _cart.Lines1.ToArray();
                    m.AkTerima2 = _cart.Lines2.ToArray();

                    await _akTerimaRepo.Insert(m);

                    //insert applog

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Penerimaan - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akTerima.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akTerima);
        }

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

            var kw = await _kwRepo.GetById(akTerima.JKWId);
            akTerima.JKW = kw;
            var negeri = await _negeriRepo.GetById(akTerima.JNegeriId);
            akTerima.JNegeri = negeri;
            var akBank = await _akBankRepo.GetById(akTerima.AkBankId);
            akTerima.AkBank = akBank;
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
        public async Task<IActionResult> Edit(int id, AkTerima akTerima, int JKWId, int JNegeriId, int AkBankId, decimal JumlahUrusniaga)
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
                        AkTerima akTerimaAsal = await _akTerimaRepo.GetById(id);

                        // list of input that cannot be change
                        akTerima.Tahun = akTerimaAsal.Tahun;
                        akTerima.JKWId = akTerimaAsal.JKWId;
                        akTerima.NoRujukan = akTerimaAsal.NoRujukan;
                        akTerima.Nama = akTerimaAsal.Nama;
                        akTerima.TarMasuk = akTerimaAsal.TarMasuk;
                        akTerima.UserId = akTerimaAsal.UserId;
                        // list of input that cannot be change end

                        foreach (AkTerima1 item in akTerimaAsal.AkTerima1)
                        {
                            var model = _context.AkTerima1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (AkTerima2 item in akTerimaAsal.AkTerima2)
                        {
                            var model = _context.AkTerima2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        _context.Entry(akTerimaAsal).State = EntityState.Detached;

                        akTerima.AkTerima1 = _cart.Lines1.ToList();
                        akTerima.AkTerima2 = _cart.Lines2.ToList();

                        akTerima.UserIdKemaskini = user.UserName;
                        akTerima.TarKemaskini = DateTime.Now;

                        _context.Update(akTerima);

                        //insert applog
                        AppLog appLog = new AppLog();

                        appLog.UserId = user.UserName;
                        appLog.LgModule = modul + "E";
                        appLog.LgOperation = "Ubah";
                        appLog.LgNote = modul + " Penerimaan - Ubah";
                        appLog.NoRujukan = akTerima.NoRujukan;
                        appLog.Jumlah = akTerima.Jumlah;

                        await _appLog.Insert(appLog);
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

            TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Urusniaga";
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

            var akTerima = await _context.AkTerima
                .Include(a => a.AkBank)
                .ThenInclude(a => a.JBank)
                .Include(a => a.JKW)
                .Include(a => a.JNegeri)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTerima = await _context.AkTerima.FindAsync(id);
            // check if already posting redirect back
            if (akTerima.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            _context.AkTerima.Remove(akTerima);

            //insert applog
            var user = await _userManager.GetUserAsync(User);

            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "D";
            appLog.LgOperation = "Hapus";
            appLog.LgNote = modul + " Penerimaan - Hapus";
            appLog.NoRujukan = akTerima.NoRujukan;
            appLog.Jumlah = akTerima.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        // POST: AkTerima/Cancel/5
        [Authorize(Policy = "PR001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var akTerima = await _context.AkTerima.FindAsync(id);
            // check if already posting redirect back
            if (akTerima.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            akTerima.FlBatal = 1;

            _context.AkTerima.Update(akTerima);

            //insert applog
            var user = await _userManager.GetUserAsync(User);

            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "B";
            appLog.LgOperation = "Batal";
            appLog.LgNote = modul + " Penerimaan - Batal";
            appLog.NoRujukan = akTerima.NoRujukan;
            appLog.Jumlah = akTerima.Jumlah;

            await _appLog.Insert(appLog);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dibatalkan..!";
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
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveAkTerima1(AkTerima1 akTerima1)
        {

            try
            {
                if (akTerima1 != null )
                {
                    var user = await _userManager.GetUserAsync(User);


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
                if (akTerima2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);


                    _cart.AddItem2(akTerima2.AkTerimaId,
                                   akTerima2.JCaraBayarId,
                                   akTerima2.Amaun,
                                   akTerima2.NoCek,
                                   akTerima2.JenisCek,
                                   akTerima2.KodBankCek,
                                   akTerima2.TempatCek,
                                   akTerima2.NoSlip,
                                   akTerima2.TarSlip);
                }

                return Json(new { result = "OK" });
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

                    //insert applog
                    var akCarta = await _akCartaRepo.GetById(akT1.AkCartaId);

                    AppLog appLog = new AppLog();
                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Penerimaan - Hapus Objek";
                    appLog.NoRujukan = akTerima.NoRujukan + "/" + akCarta.Kod;
                    appLog.Jumlah = akT1.Amaun;

                    await _appLog.Insert(appLog);
                    //insert applog end

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

                //insert applog
                if (akTerima1.Amaun != originalAmount)
                {
                    var akCarta = await _akCartaRepo.GetById(akT1.AkCartaId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "EE";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Penerimaan - Ubah Objek";
                    appLog.NoRujukan = akTerima.NoRujukan + "/" + akCarta.Kod + " Dari Amaun RM" + originalAmount.ToString() + " ke RM" + akTerima1.Amaun.ToString();
                    appLog.Jumlah = akT1.Amaun;

                    await _appLog.Insert(appLog);
                }
                //insert applog end

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

                    //insert applog
                    var akTerima = await _akTerimaRepo.GetById(akTerima2.AkTerimaId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "ED";
                    appLog.LgOperation = "Hapus";
                    appLog.LgNote = modul + " Penerimaan - Hapus Perihal";
                    appLog.NoRujukan = akTerima.NoRujukan + "/" + akTerima2.JCaraBayar.Kod;
                    appLog.Jumlah = akTerima2.Amaun;

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

                //insert applog
                if (akTerima2.Amaun != originalAmount)
                {
                    var akTerima = await _akTerimaRepo.GetById(akTerima2.AkTerimaId);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "EE";
                    appLog.LgOperation = "Ubah";
                    appLog.LgNote = modul + " Penerimaan - Ubah Perihal";
                    appLog.NoRujukan = akTerima.NoRujukan + "/" + akT2.JCaraBayar.Kod + " Dari Amaun RM" + originalAmount.ToString() + " ke RM" + akTerima2.Amaun.ToString();
                    appLog.Jumlah = akTerima2.Amaun;

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
                                   item.TarSlip);
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
                
                AkTerima akTerima = await _context.AkTerima
                    .Include(x => x.AkBank)
                    .Include(x => x.AkTerima1).ThenInclude(x => x.AkCarta)
                    .Include(x => x.AkTerima2).ThenInclude(x=> x.JCaraBayar)
                    .FirstOrDefaultAsync(x => x.Id == id);

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
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar. Jumlah Objek tidak sama dengan Jumlah Perihal.";
                    return RedirectToAction(nameof(Index));
                }
                // checking end

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";
                   
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
                            AkCartaId1 = akTerima.AkBank.AkCartaId,
                            AkCartaId2 = item.AkCartaId,
                            Tarikh = akTerima.Tarikh,
                            Debit = item.Amaun
                        };
                        await _akAkaunRepo.Insert(akAKodBank);

                        AkAkaun akAObjek = new AkAkaun()
                        {
                            NoRujukan = akTerima.NoRujukan,
                            JKWId = akTerima.JKWId,
                            AkCartaId1 = item.AkCartaId,
                            AkCartaId2 = akTerima.AkBank.AkCartaId,
                            Tarikh = akTerima.Tarikh,
                            Kredit = item.Amaun
                        };

                        await _akAkaunRepo.Insert(akAObjek);
                    }
                    
                    //update posting status in akTerima
                    akTerima.FlPosting = 1;
                    akTerima.TarikhPosting = DateTime.Now;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    var user = await _userManager.GetUserAsync(User);
                    
                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "T";
                    appLog.LgOperation = "Posting";
                    appLog.LgNote = modul + " Penerimaan - Posting";
                    appLog.NoRujukan = akTerima.NoRujukan;
                    appLog.Jumlah = akTerima.Jumlah;

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
        [Authorize(Policy = "PR001UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkTerima akTerima = await _context.AkTerima.Include(x => x.AkTerima1).ThenInclude(x => x.AkCarta).FirstOrDefaultAsync(x => x.Id == id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akTerima.NoRujukan).ToList();
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

                    //update posting status in akTerima
                    akTerima.FlPosting = 0;
                    akTerima.TarikhPosting = null;
                    //akTerima.TarikhPosting = null;
                    await _akTerimaRepo.Update(akTerima);

                    //insert applog
                    var user = await _userManager.GetUserAsync(User);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "UT";
                    appLog.LgOperation = "UnPosting";
                    appLog.LgNote = modul + " Penerimaan - UnPosting";
                    appLog.NoRujukan = akTerima.NoRujukan;
                    appLog.Jumlah = akTerima.Jumlah;

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

        // printing resit rasmi by akTerima.Id
        [Authorize(Policy = "PR001P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            AkTerima akTerima = await _akTerimaRepo.GetById(id);

            var jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akTerima.Jumlah)).ToUpper();

            var user = await _userManager.GetUserAsync(User);

            ResitPrintModel data = new ResitPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.AkTerima = akTerima;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.username = user.UserName;

            //update cetak -> 1
            akTerima.FlCetak = 1;
            await _akTerimaRepo.Update(akTerima);

            //insert applog
            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.LgModule = modul + "P";
            appLog.LgOperation = "Cetak";
            appLog.LgNote = modul + " Penerimaan - Cetak";
            appLog.NoRujukan = akTerima.NoRujukan;
            appLog.Jumlah = akTerima.Jumlah;

            await _appLog.Insert(appLog);
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
            };
        }
        // printing resit rasmi end

    }
}
