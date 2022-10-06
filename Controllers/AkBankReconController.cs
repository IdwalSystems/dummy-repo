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
    [Authorize(Policy = "PB001")]
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkBankReconController : Controller
    {
        public const string modul = "PB001";
        public const string namamodul = "Penyesuaian Bank";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBankRecon, int, string> _akReconRepo;
        private readonly ListViewIRepository<AkBankReconPenyataBank, int> _akReconPenyataRepo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly ListViewIRepository<AkPVGanda, int> _akPVGandaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkJurnal, int, string> _akJurnalRepo;
        //private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartBankRecon _cart;


        public AkBankReconController(
            ApplicationDbContext context, 
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBankRecon, int, string> akReconRepo,
            ListViewIRepository<AkBankReconPenyataBank, int> akReconPenyataRepo,
            IRepository<AkPV, int, string> akPVRepo,
            ListViewIRepository<AkPVGanda, int> akPVGandaRepo,
            ListViewIRepository<AkTerima2, int> akTerima2Repo,
            IRepository<AkJurnal, int, string> akJurnalRepo,
            CartBankRecon cart)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akReconRepo = akReconRepo;
            _akReconPenyataRepo = akReconPenyataRepo;
            _cart = cart;
            _akTerima2Repo=akTerima2Repo;
            _akPVRepo=akPVRepo;
            _akPVGandaRepo=akPVGandaRepo;
            _akJurnalRepo = akJurnalRepo;
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

        // GET: AkBankRecon
        [Authorize(Policy = "PB001")]
        public IActionResult Index()
        {
            try
            {
                PopulateList();
                return View();
            } catch
            {
                return BadRequest();
            }
        }

        private void PopulateList()
        {
            List<AkBank> bankList = _context.AkBank
                .Include(b => b.JBank)
                .Include(b => b.JBahagian)
                    .ThenInclude(b => b.JKW)
                .OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = bankList;

            ViewBag.AkBankId = 1;

            ViewData["Tahun"] = DateTime.Now.ToString("yyyy");

        }

        private void PopulateCartFromDb(AkBankRecon akBankRecon)
        {

            CartEmpty();

            foreach (AkBankReconPenyataBank item in akBankRecon.AkBankReconPenyataBank)
            {
                _cart.AddItem1(item.Id,
                        item.Indek,
                        item.AkBankReconId,
                        item.NoAkaunBank,
                        item.Tarikh,
                        item.KodTransaksi,
                        item.PerihalTransaksi,
                        item.NoDokumen,
                        item.Debit,
                        item.Kredit,
                        item.Baki,
                        item.IsPadan);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Index(
            int AkBankId,
            string Tahun,
            string Bulan)
        {
            List<AkBankRecon> akRecon = await _context.AkBankRecon
                .Where(b => b.AkBankId == AkBankId).ToListAsync();

            if (!string.IsNullOrEmpty(Tahun))
            {
                akRecon = akRecon.Where(b => b.Tahun == Tahun).ToList();
            }

            if (!string.IsNullOrEmpty(Bulan))
            {
                akRecon = akRecon.Where(b => b.Bulan == Bulan).ToList();
            }

            PopulateList();
            ViewData["Tahun"] = Tahun;
            ViewData["Bulan"] = Bulan;

            return View(akRecon.OrderBy(b => b.Tahun).ThenBy(b => b.Bulan).ThenBy(b => b.AkBank.NoAkaun).ToList());
        }
            // GET: AkBankRecon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);

            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // GET: AkBankRecon/Create
        [Authorize(Policy = "PB001C")]
        public IActionResult Create()
        {

            PopulateList();
            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkBankRecon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PB001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBankRecon akBankRecon)
        {
            AkBankRecon m = new AkBankRecon();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            bool IsExistAkRecon = _context.AkBankRecon
                .Any(b => b.Tahun == akBankRecon.Tahun &&
                b.Bulan == akBankRecon.Bulan &&
                b.AkBankId == akBankRecon.AkBankId);

            if (IsExistAkRecon == true)
            {
                TempData[SD.Error] = "Data bagi Tahun, Bulan bagi Akaun Bank ini telah wujud.";

                PopulateList();
                return View(akBankRecon);
            }
            if (ModelState.IsValid)
            {
                m.Tahun = akBankRecon.Tahun;
                m.Bulan = akBankRecon.Bulan;
                m.AkBankId = akBankRecon.AkBankId;
                m.BakiPenyata = akBankRecon.BakiPenyata;
                m.UserId = user.UserName;
                m.TarMasuk = DateTime.Now;
                m.SuPekerjaMasukId = pekerjaId;

                await _akReconRepo.Insert(m);

                //insert applog
                await AddLogAsync("Tambah", m.Tahun + "/" + m.Bulan + "/" + m.AkBankId, m.Tahun + "/" + m.Bulan + "/" + m.AkBankId, 0, m.BakiPenyata, pekerjaId);
                //insert applog end
                await _akReconRepo.Save();
                TempData[SD.Success] = "Maklumat berjaya ditambah.";

                return RedirectToAction(nameof(Index));
            }

            PopulateList(); 
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Upload/5
        public async Task<IActionResult> Upload(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        public class Thing
        {
            public int Id { get; set; }
            public string Color { get; set; }
        }

        [HttpPost]
        public JsonResult SaveAkBankReconPenyataBank(
           [FromBody] List<AkBankReconPenyataBankViewModel> akBankReconPenyataBankViewModel)
        {
            CartEmpty();

            if (akBankReconPenyataBankViewModel != null)
            {
                var bil = 1;

                foreach (var i in akBankReconPenyataBankViewModel)
                {
                    _cart.AddItem1(0,
                            i.Indek,
                            i.AkBankReconId,
                            i.NoAkaunBank,
                            i.Tarikh,
                            i.KodTransaksi,
                            i.PerihalTransaksi,
                            i.NoDokumen,
                            i.Debit,
                            i.Kredit,
                            i.Baki,
                            false);
                    bil++;
                }
                

            }
            return Json(new { Result = "OK" });
        }

        // get all item from cart akPO1
        public JsonResult GetAllItemCartAkBankReconPenyataBank(int Id)
        {

            try
            {
                List<AkBankReconPenyataBank> data = _cart.Lines1.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPO1 end

        // get list of bank matched statement search result
        public JsonResult GetBankMatchedStatementList(DateTime tarDari, DateTime tarHingga, int? padananId)
        {

            try
            {
                List<AkBankReconPenyataBank> data = _cart.Lines1.Where(b => b.IsPadan == true).OrderBy(b => b.Indek).ToList();

                if (padananId != null )
                {
                    data = data.Where(b => b.Id == padananId).ToList();
                }
                else
                {
                    tarHingga = tarHingga.AddHours(23.99);
                    data = data.Where(x => x.Tarikh >= tarDari && x.Tarikh <= tarHingga).ToList();
                }
                

                return Json(new { result = "OK", record = data.OrderBy(b => b.Tarikh) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get list of bank matched statement search result end

        // get list of system matched statement search result
        public async Task<JsonResult> GetSystemMatchedStatementList(int idBank)
        {

            try
            {
                List<AkBankReconPenyataSistemViewModel> data = new List<AkBankReconPenyataSistemViewModel>();

                // select single pv
                List<AkPadananPenyata> padanan = await _context.AkPadananPenyata
                    .Where(b => b.AkBankReconPenyataBankId == idBank).ToListAsync();
                
                if (padanan.Count > 0)
                {
                    foreach(var i in padanan)
                    {
                        // PV --
                        if (i.AkPVId != null)
                        {
                            var pv = await _context.AkPV.Include(b => b.AkPadananPenyata)
                                                        .FirstOrDefaultAsync(b => b.Id == i.AkPVId);
                            if (i.AkPVGandaId == null)
                            {
                                if (pv.AkPadananPenyata.Count() == 0 )
                                {
                                    continue;
                                }

                                data.Add(new AkBankReconPenyataSistemViewModel
                                {
                                    Id = i.Id,
                                    Indek = pv.Id,
                                    Tarikh = pv.Tarikh,
                                    NoRujukan = pv.NoPV,
                                    Perihal = pv.Nama,
                                    NoSlip = pv.NoCekAtauEFT,
                                    Debit = pv.Jumlah,
                                    Kredit = 0,
                                    IsGanda = false,

                                });
                            }
                            else
                            {
                                // select multiple pv
                                List<AkPVGanda> multiplePV = await _context.AkPVGanda
                                    .Include(b => b.AkPadananPenyata)
                                    .Where(b => b.Id == i.AkPVGandaId)
                                    .ToListAsync();

                                foreach (var row in multiplePV)
                                {

                                    if (row.AkPadananPenyata.Count() == 0)
                                    {
                                        continue;
                                    }

                                    data.Add(new AkBankReconPenyataSistemViewModel
                                    {
                                        Id = i.Id,
                                        Indek = row.Id,
                                        Tarikh = pv.Tarikh,
                                        NoRujukan = pv.NoPV,
                                        Perihal = row.Nama,
                                        NoSlip = row.NoCekAtauEFT,
                                        Debit = row.Amaun,
                                        Kredit = 0,
                                        IsGanda = true,

                                    });
                                }
                            }
                        }

                        // PV END --

                        // RESIT --
                        if (i.AkTerima2Id != null)
                        {
                            var terima2 = await _context.AkTerima2.Include(b => b.AkPadananPenyata)
                                            .Include(b => b.AkTerima)
                                            .FirstOrDefaultAsync(b => b.Id == i.AkTerima2Id);

                            if (terima2.AkPadananPenyata.Count() == 0)
                            {
                                continue;
                            }

                            data.Add(new AkBankReconPenyataSistemViewModel
                            {
                                Id = i.Id,
                                Indek = terima2.Id,
                                Tarikh = terima2.AkTerima.Tarikh,
                                NoRujukan = terima2.AkTerima.NoRujukan,
                                Perihal = terima2.AkTerima.Nama,
                                NoSlip = terima2.NoSlip,
                                Debit = terima2.Amaun,
                                Kredit = 0,
                                IsGanda = false,

                            });
                        }
                        // RESIT END --

                        // JURNAL --
                        if (i.AkJurnalId != null)
                        {
                            var jurnal = await _context.AkJurnal.Include(b => b.AkPadananPenyata)
                                                        .FirstOrDefaultAsync(b => b.Id == i.AkJurnalId);

                            if (jurnal.AkPadananPenyata.Count() == 0)
                            {
                                continue;
                            }

                            data.Add(new AkBankReconPenyataSistemViewModel
                            {
                                Id = i.Id,
                                Indek = jurnal.Id,
                                Tarikh = jurnal.Tarikh,
                                NoRujukan = jurnal.NoJurnal,
                                Perihal = jurnal.Catatan1,
                                NoSlip = jurnal.Catatan2,
                                Debit = jurnal.JumDebit,
                                Kredit = jurnal.JumKredit,
                                IsGanda = false,

                            });
                        }
                        // JURNAL END --
                    }
                }

                return Json(new { result = "OK", record = data.OrderBy(b => b.Tarikh) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get list of system matched statement search result end

        // get list of bank statement search result
        public JsonResult GetBankStatementList(DateTime tarDari, DateTime tarHingga)
        {

            try
            {
                List<AkBankReconPenyataBank> data = _cart.Lines1.Where(b => b.IsPadan == false).OrderBy(b => b.Indek).ToList();

                tarHingga = tarHingga.AddHours(23.99);
                data = data.Where(x => x.Tarikh >= tarDari && x.Tarikh <= tarHingga).ToList();

                return Json(new { result = "OK", record = data.OrderBy(b => b.Tarikh) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get list of bank statement search result end

        // get list of system statement search result
        public async Task<JsonResult> GetSystemStatementList(DateTime tarDari, DateTime tarHingga)
        {

            try
            {
                List<AkBankReconPenyataSistemViewModel> data = new List<AkBankReconPenyataSistemViewModel>();

                // PV --
                // select single pv
                var singlePV = await _context.AkPV.Include(b => b.AkPadananPenyata)
                    .Where(b => b.IsGanda == false && b.FlPosting == 1 && b.FlBatal == 0 && b.FlHapus == 0)
                    .ToListAsync();

                foreach (var row in singlePV)
                {
                    if (row.AkPadananPenyata.Count() > 0)
                    {
                        continue;
                    }

                    data.Add( new AkBankReconPenyataSistemViewModel
                    {
                        Id = row.Id,
                        Tarikh = row.Tarikh,
                        NoRujukan = row.NoPV,
                        Perihal = row.Nama,
                        NoSlip = row.NoCekAtauEFT,
                        Debit = row.Jumlah,
                        Kredit = 0,
                        Indek = row.Id,
                        IsGanda = false

                    });
                }

                // select multiple pv
                List<AkPV> multiplePV = await _context.AkPV
                    .Include(b => b.AkPVGanda).ThenInclude(b => b.AkPadananPenyata)
                    .Where(b => b.IsGanda == true && b.FlPosting == 1 && b.FlBatal == 0 && b.FlHapus == 0)
                    .ToListAsync();

                foreach (var akPV in multiplePV)
                {
                    foreach (var row in akPV.AkPVGanda)
                    {

                        if (row.AkPadananPenyata.Count() == 0)
                        {
                            data.Add(new AkBankReconPenyataSistemViewModel
                            {
                                Id = akPV.Id,
                                Indek = row.Id,
                                Tarikh = akPV.Tarikh,
                                NoRujukan = akPV.NoPV,
                                Perihal = row.Nama,
                                NoSlip = row.NoCekAtauEFT,
                                Debit = row.Amaun,
                                Kredit = 0,
                                IsGanda = true

                            });
                        }
                    }
                    
                }
                // PV END --
                // RESIT --
                // select terima2
                List<AkTerima> multipleReceipt = await _context.AkTerima
                    .Include(b => b.AkTerima2).ThenInclude(b => b.AkPadananPenyata)
                    .Where( b => b.FlPosting == 1 && b.FlHapus == 0)
                    .ToListAsync();

                foreach (var akTerima in multipleReceipt)
                {
                    foreach (var row in akTerima.AkTerima2)
                    {

                        if (row.AkPadananPenyata.Count() == 0)
                        {
                            data.Add(new AkBankReconPenyataSistemViewModel
                            {
                                Id = akTerima.Id,
                                Indek = row.Id,
                                Tarikh = akTerima.Tarikh,
                                NoRujukan = akTerima.NoRujukan,
                                Perihal = akTerima.Nama,
                                NoSlip = row.NoSlip,
                                Debit = 0,
                                Kredit = row.Amaun,
                                IsGanda = false

                            });
                        }
                    }

                }
                // RESIT END --
                // JURNAL --
                var jurnal = await _context.AkJurnal.Include(b => b.AkPadananPenyata)
                    .Where(b => b.Posting == 1 && b.FlHapus == 0)
                    .ToListAsync();

                foreach (var row in jurnal)
                {
                    if (row.AkPadananPenyata.Count() > 0)
                    {
                        continue;
                    }

                    data.Add(new AkBankReconPenyataSistemViewModel
                    {
                        Id = row.Id,
                        Tarikh = row.Tarikh,
                        NoRujukan = row.NoJurnal,
                        Perihal = row.Catatan1,
                        NoSlip = row.Catatan2,
                        Debit = row.JumDebit,
                        Kredit = row.JumKredit,
                        Indek = row.Id,
                        IsGanda = false

                    });
                }
                // JURNAL END --

                tarHingga = tarHingga.AddHours(23.99);
                data = data.Where(x => x.Tarikh >= tarDari && x.Tarikh <= tarHingga).ToList();

                return Json(new { result = "OK", record = data.OrderBy(b => b.Tarikh) });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get list of system statement search result end

        // match bank and system statement
        public async Task<JsonResult> MatchStatementList(
            int idBank,
            int indekBank, 
            decimal amaunBank,
            List<ListItemViewModel> arrayOfValues)
        {
            // insert 
            try
            {
                var type = "";

                AkPV pv = new AkPV();
                AkPVGanda pvGanda = new AkPVGanda();
                AkTerima2 terima2 = new AkTerima2();
                AkJurnal jurnal = new AkJurnal();

                foreach (var item in arrayOfValues)
                {
                    type = item.perihal.Substring(0, 2);
                    switch (type)
                    {
                        case "PV":
                            if (item.isGanda == false)
                            {
                                pv = await _akPVRepo.GetByIdIncludeDeletedItems(item.id);

                                pv.AkPadananPenyata.Add(new AkPadananPenyata
                                {
                                    AkBankReconPenyataBankId = idBank,
                                    FlJenis = 1,
                                    AkPVId = item.id
                                });

                                pv.FlTunai = 1;
                                pv.TarTunai = DateTime.Now;

                                await _akPVRepo.Update(pv);
                            }
                            else
                            {
                                pvGanda = await _akPVGandaRepo.GetById(item.indek);

                                pvGanda.AkPadananPenyata.Add(new AkPadananPenyata
                                {
                                    AkBankReconPenyataBankId = idBank,
                                    FlJenis = 1,
                                    AkPVId = item.id,
                                    AkPVGandaId = item.indek
                                });

                                pvGanda.FlTunai = 1;
                                pvGanda.TarTunai = DateTime.Now;

                                await _akPVGandaRepo.Update(pvGanda);
                            }
                            

                            break;
                        case "RR":
                            terima2 = await _akTerima2Repo.GetById(item.indek);

                            terima2.AkPadananPenyata.Add(new AkPadananPenyata
                            {
                                AkBankReconPenyataBankId = idBank,
                                FlJenis = 1,
                                AkTerima2Id = item.indek
                            });

                            terima2.FlTunai = 1;
                            terima2.TarTunai = DateTime.Now;

                            await _akTerima2Repo.Update(terima2);
                            break;
                        case "JU":
                            jurnal = await _akJurnalRepo.GetById(item.indek);

                            jurnal.AkPadananPenyata.Add(new AkPadananPenyata
                            {
                                AkBankReconPenyataBankId = idBank,
                                FlJenis = 1,
                                AkJurnalId = item.indek
                            });

                            jurnal.FlTunai = 1;
                            jurnal.TarTunai = DateTime.Now;

                            await _akJurnalRepo.Update(jurnal);
                            break;
                    }



                }

                AkBankReconPenyataBank penyataBank = await _context.AkBankReconPenyataBank.FirstOrDefaultAsync(b => b.Id == idBank);
                if (penyataBank != null)
                {
                    penyataBank.IsPadan = true;
                }

                _context.AkBankReconPenyataBank.Update(penyataBank);

                await _context.SaveChangesAsync();

                // update cart
                _cart.RemoveItem1(indekBank);

                _cart.AddItem1(penyataBank.Id,
                            penyataBank.Indek,
                            penyataBank.AkBankReconId,
                            penyataBank.NoAkaunBank,
                            penyataBank.Tarikh,
                            penyataBank.KodTransaksi,
                            penyataBank.PerihalTransaksi,
                            penyataBank.NoDokumen,
                            penyataBank.Debit,
                            penyataBank.Kredit,
                            penyataBank.Baki,
                            true);

                return Json(new { result = "OK", dataBank = "OK", dataSistem = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // match bank and system statement end

        // unmatch bank and system statement
        public async Task<JsonResult> UnMatchStatementList(
            int idBank,
            int indekBank,
            decimal amaunBank,
            List<ListItemViewModel> arrayOfValues,
            int rowSystemCount)
        {
            // insert 
            try
            {
                var type = "";
                decimal amaun = 0;

                AkPV pv = new AkPV();
                AkPVGanda pvGanda = new AkPVGanda();
                AkTerima2 terima2 = new AkTerima2();
                AkJurnal jurnal = new AkJurnal();

                AkPadananPenyata padanan = new AkPadananPenyata();

                foreach (var item in arrayOfValues)
                {
                    type = item.perihal.Substring(0, 2);
                    switch (type)
                    {
                        case "PV":
                            
                            if (item.isGanda == false)
                            {
                                pv = await _akPVRepo.GetByIdIncludeDeletedItems(item.indek);

                                pv.FlTunai = 0;
                                pv.TarTunai = null;

                                amaun += pv.Jumlah;

                                await _akPVRepo.Update(pv);
                            }
                            else
                            {
                                pvGanda = await _akPVGandaRepo.GetById(item.indek);

                                pvGanda.FlTunai = 0;
                                pvGanda.TarTunai = null;

                                amaun += pvGanda.Amaun;

                                await _akPVGandaRepo.Update(pvGanda);
                            }

                            break;
                        case "RR":
                            terima2 = await _akTerima2Repo.GetById(item.indek);

                            terima2.FlTunai = 0;
                            terima2.TarTunai = null;

                            amaun += terima2.Amaun;

                            await _akTerima2Repo.Update(terima2);
                            break;
                        case "JU":
                            jurnal = await _akJurnalRepo.GetById(item.indek);

                            jurnal.FlTunai = 0;
                            jurnal.TarTunai = null;

                            amaun += jurnal.JumDebit;

                            await _akJurnalRepo.Update(jurnal);
                            break;
                    }

                    padanan = await _context.AkPadananPenyata.FirstOrDefaultAsync(b => b.Id == item.id);

                    _context.AkPadananPenyata.Remove(padanan);


                }

                if (rowSystemCount == 1)
                {
                    AkBankReconPenyataBank penyataBank = await _context.AkBankReconPenyataBank.FirstOrDefaultAsync(b => b.Id == idBank);
                    if (penyataBank != null)
                    {
                        penyataBank.IsPadan = false;
                    }

                    _context.AkBankReconPenyataBank.Update(penyataBank);

                    // update cart
                    _cart.RemoveItem1(indekBank);

                    _cart.AddItem1(penyataBank.Id,
                                penyataBank.Indek,
                                penyataBank.AkBankReconId,
                                penyataBank.NoAkaunBank,
                                penyataBank.Tarikh,
                                penyataBank.KodTransaksi,
                                penyataBank.PerihalTransaksi,
                                penyataBank.NoDokumen,
                                penyataBank.Debit,
                                penyataBank.Kredit,
                                penyataBank.Baki,
                                false);
                }
                

                await _context.SaveChangesAsync();

                

                return Json(new { result = "OK", dataBank = "OK", dataSistem = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // unmatch bank and system statement end

        // GET: AkBankRecon/Edit/5
        [Authorize(Policy = "PB001E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            PopulateList();
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Edit/5
        [Authorize(Policy = "PB001E")]
        public async Task<IActionResult> BankStatement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);
            if (akBankRecon == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateCartFromDb(akBankRecon);

            decimal bakiBukuTunai = 0;

            // get baki buku tunai

            foreach (AkBankReconPenyataBank item in akBankRecon.AkBankReconPenyataBank)
            {
                if (item.IsPadan == true)
                {
                    if(item.Debit != 0)
                    {
                        bakiBukuTunai += item.Debit;
                    }
                    else
                    {
                        bakiBukuTunai -= item.Kredit;
                    }
                }
            }

            ViewBag.BakiBukuTunai = bakiBukuTunai;

            return View(akBankRecon);
        }

        // POST: AkBankRecon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PB001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkBankRecon akBankRecon)
        {
            if (id != akBankRecon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    AkBankRecon dataAsal = await _akReconRepo.GetByIdIncludeDeletedItems(id);

                    // list of input that cannot be change
                    //suJurulatih.Emel = dataAsal.Emel;
                    akBankRecon.TarMasuk = dataAsal.TarMasuk;
                    akBankRecon.UserId = dataAsal.UserId;
                    akBankRecon.Tahun = dataAsal.Tahun;
                    akBankRecon.Bulan = dataAsal.Bulan;
                    akBankRecon.AkBankId = dataAsal.AkBankId;
                    var bakiPenyata = dataAsal.BakiPenyata;
                    akBankRecon.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    foreach (AkBankReconPenyataBank item in dataAsal.AkBankReconPenyataBank)
                    {
                        var model = _context.AkBankReconPenyataBank.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akBankRecon.AkBankReconPenyataBank = _cart.Lines1.ToList();

                    akBankRecon.UserIdKemaskini = user.UserName;
                    akBankRecon.TarKemaskini = DateTime.Now;
                    akBankRecon.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akBankRecon);

                    await AddLogAsync("Ubah", bakiPenyata + " -> " + akBankRecon.BakiPenyata
                            , akBankRecon.Tahun + "/" + akBankRecon.Bulan + "/" + akBankRecon.AkBankId
                            , id, akBankRecon.BakiPenyata, pekerjaId);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBankReconExists(akBankRecon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData[SD.Success] = "Data berjaya diubah..!";
                return RedirectToAction(nameof(Index));
            }

            PopulateList();
            return View(akBankRecon);
        }

        // GET: AkBankRecon/Delete/5
        [Authorize(Policy = "PB001D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBankRecon = await _akReconRepo.GetByIdIncludeDeletedItems((int)id);

            if (akBankRecon == null)
            {
                return NotFound();
            }

            return View(akBankRecon);
        }

        // POST: AkBankRecon/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PB001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBankRecon = await _context.AkBankRecon.FindAsync(id);
            // check if already posting redirect back
            if (!string.IsNullOrEmpty(akBankRecon.TarKunci?.ToString("yyyy/MM/dd")))
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            akBankRecon.UserIdKemaskini = user.UserName;
            akBankRecon.TarKemaskini = DateTime.Now;
            akBankRecon.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkBankRecon.Remove(akBankRecon);
            //insert applog
            await AddLogAsync("Hapus", "Hapus Data", akBankRecon.Tahun + "/" + akBankRecon.Bulan + "/" + akBankRecon.AkBankId, id, akBankRecon.BakiPenyata, pekerjaId);
            //insert applog end
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBankReconExists(int id)
        {
            return _context.AkBankRecon.Any(e => e.Id == id);
        }
    }
}
