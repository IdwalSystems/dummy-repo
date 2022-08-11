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
    public class AkPenyataPemungutController : Controller
    {
        public const string modul = "PR002";
        public const string namamodul = "Penyata Pemungut";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPenyataPemungut, int, string> _akPungutRepo;
        private readonly ListViewIRepository<AkPenyataPemungut1, int> _akPungut1Repo;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartPenyataPemungut _cart;

        public AkPenyataPemungutController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPenyataPemungut, int, string> akPungutRepo,
            ListViewIRepository<AkPenyataPemungut1, int> akPungut1Repo,
            IRepository<AkTerima, int, string> akTerimaRepo,
            ListViewIRepository<AkTerima2, int> akTerima2Repo,
            IRepository<AkBank, int, string> akBankRepo,
            CartPenyataPemungut cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akPungutRepo = akPungutRepo;
            _akPungut1Repo = akPungut1Repo;
            _akTerimaRepo = akTerimaRepo;
            _akTerima2Repo = akTerima2Repo;
            _akBankRepo = akBankRepo;
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

        // GET: AkPenyataPemungut
        [Authorize(Policy = "PR002")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No Dokumen", Value = "NoDokumen" });
            columnList.Add(new SelectListItem() { Text = "Tahun", Value = "Tahun" });

            var akPungut = await _akPungutRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akPungut = await _akPungutRepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoDokumen")
                    {
                        akPungut = akPungut.Where(s => s.NoDokumen.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Tahun")
                    {
                        akPungut = akPungut.Where(s => s.Tahun.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPungut = akPungut.Where(x => x.Tarikh >= date1
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

            return View(akPungut);
        }

        // GET: AkPenyataPemungut/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPungut = await _akPungutRepo.GetByIdIncludeDeletedItems((int)id);
            if (akPungut == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(akPungut);
        }

        private void PopulateTable(int? id)
        {
            List<AkPenyataPemungut2> akPungut2 = _context.AkPenyataPemungut2
                .Include(b => b.AkTerima2)
                .ThenInclude(b => b.AkTerima)
                .ToList();

            List<AkPenyataPemungut2ViewModel> vm = new List<AkPenyataPemungut2ViewModel>();

            foreach (var item in akPungut2)
            {

                vm.Add(new AkPenyataPemungut2ViewModel
                {
                    Id = item.Id,
                    Indek = item.Indek,
                    AkTerima2Id = item.AkTerima2Id,
                    NoResit = item.AkTerima2.AkTerima.NoRujukan,
                    Tarikh = item.AkTerima2.AkTerima.Tarikh,
                    Amaun = item.Amaun
                });
            }

            ViewBag.AkPenyataPemungut2 = vm;
        }

        // GET: AkPenyataPemungut/Create
        [Authorize(Policy = "PR002C")]
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            var noRujukan = GetNoRujukan(DateTime.Now.ToString("yyyy"));

            ViewBag.NoDokumen = noRujukan;

            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akPenyataPemungut1 = new List<int>();
                ViewBag.akPenyataPemungut2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        private void PopulateList()
        {
            List<AkBank> bankList = _context.AkBank
                .Include(x => x.JKW)
                .Include(x => x.JBank)
                .Include(x => x.JBahagian)
                .Include(x => x.AkCarta)
                .ToList();

            List<JCaraBayar> caraBayarList = _context.JCaraBayar
                .OrderBy(b => b.Perihal)
                .ToList();

            ViewBag.AkBank = bankList;
            ViewBag.JCaraBayar = caraBayarList;
        }

        [HttpPost]
        public async Task<JsonResult> JsonGetCaraBayar(int id)
        {
            try
            {

                var caraBayar =await _context.JCaraBayar.FirstOrDefaultAsync(b => b.Id == id);

                if (caraBayar.Perihal.Contains("CEK")){
                    return Json(new { result = "CEK" });
                }
                else
                {
                    return Json(new { result = "LAIN" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult JsonGetKod(string tahun)
        {
            try
            {
                if (tahun != null)
                {
                    var noDokumen = GetNoRujukan(tahun);

                    return Json(new { result = "OK", record = noDokumen });
                }
                else
                {
                    return Json(new { result = "OK", record = "" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult JsonGetTerima(DateTime tarDari, DateTime tarHingga, int JCaraBayarId, int JenisCek)
        {
            try
            {
                CartEmpty();

                // find all terimaan within date range where cetak = 1 and posting = 1
                // case Gandaan :
                // get all information from AkTerima2
                // return
                // 1. data

                // get all AkTerima where posting = 1
                List<AkTerima> terima = _context.AkTerima
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.SpPendahuluanPelbagai)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.SpPendahuluanPelbagai)
                        .ThenInclude(b => b.SuPekerja)
                    .Include(b => b.AkPenghutang)
                        .ThenInclude(x => x.JBank)
                    .Include(b => b.AkBank)
                        .ThenInclude(b => b.JBank)
                    .Include(b => b.AkTerima1)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkTerima2)
                        .ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.AkTerima2)
                        .ThenInclude( b => b.AkPenyataPemungut2)
                    .Where(b => b.FlPosting == 1 )
                    .OrderBy(b => b.NoRujukan)
                    .ToList();

                // get all Resit within date range
                terima = terima.Where(x => x.Tarikh >= tarDari
                    && x.Tarikh <= tarHingga.AddHours(23.99)).ToList();

                List<ListItemViewModel> AkTerimaIdList = new List<ListItemViewModel>();

                List<AkPenyataPemungut1ViewModel> terima1Table = new List<AkPenyataPemungut1ViewModel>();

                List<AkPenyataPemungut2ViewModel> terima2Table = new List<AkPenyataPemungut2ViewModel>();

                // individual
                int indek = 0;
                foreach (var item in terima)
                {
                    
                    item.AkTerima2 = item.AkTerima2.Where(b => b.JCaraBayarId == JCaraBayarId).ToList();

                    var caraBayar = _context.JCaraBayar.FirstOrDefault(b => b.Id == JCaraBayarId);

                    if (caraBayar.Perihal.Contains("CEK"))
                    {
                        item.AkTerima2 = item.AkTerima2.Where(b => b.JenisCek == JenisCek).ToList();
                    }

                    foreach (var item2 in item.AkTerima2)
                    {
                        //checked if already jana Terima in AkPenyataPemungut1
                        bool isExistTerima = _context.AkPenyataPemungut2
                            .Include(b => b.AkPenyataPemungut)
                            .Where(b => b.AkTerima2Id == item2.Id && b.AkPenyataPemungut.FlHapus == 0)
                            .Any();

                        if (isExistTerima == true)
                        {
                            continue;
                        }
                        // foreach AkTerima2, check if equal to AkTerima1 Through AkTerima
                        var AkTerima2 = _context.AkTerima2
                            .Include(b => b.AkTerima)
                            .Where(b => b.Id == item2.Id).FirstOrDefault();

                        if (AkTerima2!= null)
                        {
                            AkTerimaIdList.Add(
                                new ListItemViewModel
                                {
                                    id = AkTerima2.AkTerimaId
                                });
                        }

                        indek++;
                        terima2Table.Add(
                            new AkPenyataPemungut2ViewModel
                            {
                                Id = 0,
                                Indek = indek,
                                AkTerima2Id = item2.Id,
                                Tarikh = item.Tarikh,
                                NoResit = item.NoRujukan,
                                Amaun = item2.Amaun
                            });
                    }
                }
                // AkPenyataPemungut1
                indek = 0;

                foreach (var i in AkTerimaIdList
                    .GroupBy(grp => grp.id)
                    .Select(g => new { id = g.FirstOrDefault().id})
                    .ToList())
                {

                    var AkTerima = _context.AkTerima
                        .Include(b => b.JBahagian)
                        .Include(b => b.AkTerima1).ThenInclude(b => b.AkCarta)
                        .FirstOrDefault(b => b.Id == i.id);

                    foreach (var item1 in AkTerima.AkTerima1)
                    {
                        indek++;
                        terima1Table.Add(
                            new AkPenyataPemungut1ViewModel
                            {
                                Id = 0,
                                AkTerimaId = AkTerima.Id,
                                Indek = indek,
                                JBahagianId = (int)AkTerima.JBahagianId,
                                JBahagian = AkTerima.JBahagian,
                                AkCartaId = item1.AkCartaId,
                                AkCarta = item1.AkCarta,
                                Amaun = item1.Amaun
                            });
                    }
                }

                // add to cart first
                PopulateCart(terima, terima1Table, JCaraBayarId);

                return Json(new { result = "OK", table = terima2Table });

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCart(List<AkTerima> terima, List<AkPenyataPemungut1ViewModel> terima1Table, int JCaraBayarId)
        {
            int indek = 0;

            // individual
            foreach (var item in terima)
            {

                foreach (var item2 in item.AkTerima2)
                {
                    //checked if already jana Terima in AkPenyataPemungut2
                    bool isExistTerima = _context.AkPenyataPemungut2
                        .Include(b => b.AkPenyataPemungut)
                        .Where(b => b.AkTerima2Id == item2.Id && b.AkPenyataPemungut.FlHapus == 0)
                        .Any();

                    if (isExistTerima == true)
                    {
                        continue;
                    }

                    indek++;
                    _cart.AddItem2(0,
                                    indek,
                                    item2.Id,
                                    item2.Amaun
                                    );
                }
            }

            // AkPenyataPemungut1
            indek = 0;

            // group by JBahagian, AkCarta first to get sum of Amaun for each AkCarta
            var groupTerima1List = terima1Table
                    .GroupBy(b => new
                    {
                        b.JBahagianId,
                        b.AkCartaId
                    })
                    .Select(grp => new
                    {
                        JBahagianId = grp.FirstOrDefault().JBahagianId,
                        AkCartaId = grp.FirstOrDefault().AkCartaId,
                        Amaun = grp.Sum(g => g.Amaun)

                    })
                    .ToList();

            foreach (var i in groupTerima1List)
            {
                indek++;
                _cart.AddItem1(0,
                                indek,
                                i.JBahagianId,
                                i.AkCartaId,
                                i.Amaun
                                );
            }
        }

        private string GetNoRujukan(string year)
        {
            string prefix = "";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkPenyataPemungut
                       .IgnoreQueryFilters()
                       .Where(x => x.Tahun ==  year)
                       .Max(x => x.NoDokumen);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(1));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            return noRujukan;
        }


        // POST: AkPenyataPemungut/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PR002C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPenyataPemungut akPenyataPemungut, int AkBankId)
        {
            AkPenyataPemungut m = new AkPenyataPemungut();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var namaUser = _context.applicationUsers.FirstOrDefault(x => x.Email == user.Email);
            var pekerja = _context.SuPekerja.FirstOrDefault(x => x.Id == namaUser.SuPekerjaId);
            var jawatan = "Super Admin";
            if (pekerja != null)
            {
                jawatan = pekerja.Jawatan;
            }

            // get latest no rujukan running number  
            var noRujukan = GetNoRujukan(akPenyataPemungut.Tahun);
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akPenyataPemungut != null && AkBankId != 0)
                {

                    m.NoDokumen = noRujukan;
                    m.JCaraBayarId = akPenyataPemungut.JCaraBayarId;
                    m.Tahun = akPenyataPemungut.Tahun;
                    m.Tarikh = akPenyataPemungut.Tarikh;
                    m.NoSlip = akPenyataPemungut.NoSlip;
                    m.TarSlip = akPenyataPemungut.TarSlip;
                    m.Jumlah = akPenyataPemungut.Jumlah;
                    m.BilTerima = akPenyataPemungut.BilTerima;
                    m.FlJenisCek = akPenyataPemungut.FlJenisCek;
                    m.FlHapus = 0;
                    m.AkBankId = AkBankId;
                    m.SuPekerjaId = namaUser.SuPekerjaId;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = akPenyataPemungut.SuPekerjaMasukId;

                    m.AkPenyataPemungut1 = _cart.Lines1.ToArray();
                    m.AkPenyataPemungut2 = _cart.Lines2.ToArray();

                    await _akPungutRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoDokumen, m.NoDokumen, 0, m.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat Penyata Pemungut berjaya ditambah. No Dokumen adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            CartEmpty();
            ViewBag.NoDokumen = noRujukan;

            return View(akPenyataPemungut);
        }

        // GET: AkPenyataPemungut/Edit/5
        [Authorize(Policy = "PR002E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPenyataPemungut = await _akPungutRepo.GetByIdIncludeDeletedItems((int)id);
            if (akPenyataPemungut == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPenyataPemungut);
            return View(akPenyataPemungut);
        }

        private void PopulateCartFromDb(AkPenyataPemungut akPenyataPemungut)
        {
            List<AkPenyataPemungut2> akPenyataPemungut2 = _context.AkPenyataPemungut2
                .Include(b => b.AkTerima2)
                    .ThenInclude(b => b.AkTerima)
                .Where(b => b.AkPenyataPemungutId == akPenyataPemungut.Id)
                .OrderBy(b => b.Indek)
                .ToList();

            foreach (AkPenyataPemungut2 item in akPenyataPemungut2)
            {
                _cart.AddItem2(item.Id,
                                item.Indek,
                                item.AkTerima2Id,
                                item.Amaun
                                );
            }
        }

        //save cart akPenyataPemungut1
        public JsonResult SaveCartAkPenyataPemungut2(AkPenyataPemungut2 akPenyataPemungut2)
        {

            try
            {

                var akPP2 = _cart.Lines2.Where(x => x.Indek == akPenyataPemungut2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPP2 != null)
                {
                    _cart.RemoveItem2(akPenyataPemungut2.Indek);

                    _cart.AddItem2(0,
                                akPP2.Indek,
                                akPP2.AkTerima2Id,
                                akPP2.Amaun
                                );

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akNotaMinta2 end

        // POST: AkPenyataPemungut/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PR002E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AkPenyataPemungut akPenyataPemungut)
        {
            if (id != akPenyataPemungut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    AkPenyataPemungut dataAsal = await _akPungutRepo.GetById(id);

                    // list of input that cannot be change
                    akPenyataPemungut.Tarikh = dataAsal.Tarikh;
                    akPenyataPemungut.NoDokumen = dataAsal.NoDokumen;
                    akPenyataPemungut.NoSlip = dataAsal.NoSlip;
                    akPenyataPemungut.SuPekerjaId = dataAsal.SuPekerjaId;
                    akPenyataPemungut.JCaraBayarId = dataAsal.JCaraBayarId;
                    akPenyataPemungut.AkBankId = dataAsal.AkBankId;
                    akPenyataPemungut.FlJenisCek = dataAsal.FlJenisCek;

                    akPenyataPemungut.TarMasuk = dataAsal.TarMasuk;
                    akPenyataPemungut.UserId = dataAsal.UserId;
                    akPenyataPemungut.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    foreach (AkPenyataPemungut1 item in dataAsal.AkPenyataPemungut1)
                    {
                        var model = _context.AkPenyataPemungut1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akPenyataPemungut.AkPenyataPemungut1 = _cart.Lines1.ToList();
                    akPenyataPemungut.AkPenyataPemungut2 = _cart.Lines2.ToList();

                    akPenyataPemungut.UserIdKemaskini = user.UserName;
                    akPenyataPemungut.TarKemaskini = DateTime.Now;
                    akPenyataPemungut.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akPenyataPemungut);

                    //insert applog
                    await AddLogAsync("Ubah", "Ubah Data", akPenyataPemungut.NoDokumen, id, akPenyataPemungut.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPenyataPemungutExists(akPenyataPemungut.Id))
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
            return View(akPenyataPemungut);
        }

        // GET: AkPenyataPemungut/Delete/5
        [Authorize(Policy = "PR002D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPenyataPemungut = await _akPungutRepo.GetByIdIncludeDeletedItems((int)id);
            if (akPenyataPemungut == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(akPenyataPemungut);
        }

        // POST: AkPenyataPemungut/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PR002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _context.AkPenyataPemungut.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            //insert applog
            await AddLogAsync("Hapus", obj.NoDokumen, obj.NoDokumen, id, obj.Jumlah, pekerjaId);
            //insert applog end

            _context.AkPenyataPemungut.Remove(obj);

            var akPP = await _akPungutRepo.GetById(id);

            foreach (var item in akPP.AkPenyataPemungut2)
            {
                var akTerima2 = await _akTerima2Repo.GetById(item.AkTerima2Id);

                akTerima2.NoSlip = "";
                akTerima2.TarSlip = null;

                await _akTerima2Repo.Update(akTerima2);

            }

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PR002R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akPungutRepo.GetByIdIncludeDeletedItems(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Rollback operation

            obj.FlHapus = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;
            _context.AkPenyataPemungut.Update(obj);

            // Rollback operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoDokumen, (int)id, obj.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkPenyataPemungutExists(int id)
        {
            return _context.AkPenyataPemungut.Any(e => e.Id == id);
        }
    }
}
