using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class SpPendahuluanPelbagaiController : Controller


    {

        public const string modul = "SP001";
        public const string namamodul = "Pendahuluan Pelbagai";

        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPendahuluanPelbagaiRepo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai1, int> _spPendahuluanPelbagai1Repo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai2, int> _spPendahuluanPelbagai2Repo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JSukan, int, string> _sukanRepo;
        private readonly IRepository<JJantina, int, string> _jantinaRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<JTahapAktiviti, int, string> _tahapAktivitiRepo;
        private readonly IRepository<JBahagian, int, string> _bahagianRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly IRepository<JPelulus, int, string> _pelulusRepo;
        private CartPendahuluan _cart;

        public SpPendahuluanPelbagaiController(
           ApplicationDbContext context,
           AppLogIRepository<AppLog, int> appLog,
           IRepository<SpPendahuluanPelbagai, int, string> SpPendahuluanPelbagaiRepository,
           ListViewIRepository<SpPendahuluanPelbagai1, int> SpPendahuluanPelbagai1Repository,
           ListViewIRepository<SpPendahuluanPelbagai2, int> SpPendahuluanPelbagai2Repository,
           IRepository<JNegeri, int, string> negeriRepository,
           IRepository<JSukan, int, string> sukanRepository,
           IRepository<JJantina, int, string> jantinaRepository,
           IRepository<SuPekerja, int, string> suPekerjaRepository,
           IRepository<JTahapAktiviti, int, string> tahapAktivitiRepository,
           IRepository<JBahagian, int, string> bahagianRepository,
           IRepository<AkCarta, int, string> akCartaRepository,
           IRepository<JKW, int, string> kwRepository,
           IRepository<AbBukuVot, int, string> abBukuVotRepository,
           UserManager<IdentityUser> userManager,
           CustomIRepository<string, int> customRepo,
           IRepository<JPelulus, int, string> pelulusRepo,
           CartPendahuluan cart
           )
        {
            _appLog = appLog;
            _spPendahuluanPelbagaiRepo = SpPendahuluanPelbagaiRepository;
            _spPendahuluanPelbagai1Repo = SpPendahuluanPelbagai1Repository;
            _spPendahuluanPelbagai2Repo = SpPendahuluanPelbagai2Repository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _context = context;
            _negeriRepo = negeriRepository;
            _sukanRepo = sukanRepository;
            _jantinaRepo = jantinaRepository;
            _suPekerjaRepo = suPekerjaRepository;
            _tahapAktivitiRepo = tahapAktivitiRepository;
            _bahagianRepo = bahagianRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _userManager = userManager;
            _customRepo = customRepo;
            _pelulusRepo = pelulusRepo;
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

        //Function Running Number
        private string RunningNumber(SpPendahuluanPelbagai data)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data.JKWId);

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            //var year = data.Tahun;
            string prefix = year + "/" + kumpulanWang + "/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.SpPendahuluanPelbagai
                .IgnoreQueryFilters()
                .Where(x => x.NoPermohonan.Substring(0, 9) == prefix)
                .Max(x => x.NoPermohonan);
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
            return noRujukan;
        }
        [HttpPost]
        public JsonResult JsonGetKod(SpPendahuluanPelbagai data)
        {
            try
            {
                var result = "";
                if (data == null)
                {
                    result = "";
                }
                else
                {
                    result = RunningNumber(data);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //End Function Running Number

        //Start Function Get Baki Vot
        [HttpPost]
        public async Task<JsonResult> GetBakiVot(SpPendahuluanPelbagai spPendahuluanPelbagai, 
            int jKWId,
            int jBahagianId)
        {

                try
                {
                // check for baki peruntukan
                var tahun = DateTime.Now.Year.ToString();
                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                .Where(x => x.Tahun == tahun && x.VotId == spPendahuluanPelbagai.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                .AnyAsync();

                        if (IsExistAbBukuVot == true)
                        {
                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, spPendahuluanPelbagai.AkCartaId, jKWId, jBahagianId);

                            if (sum < spPendahuluanPelbagai.JumKeseluruhan)
                            {
                                return Json(new { result = "ERROR" });
                            }
                        }
                        else
                        {
                            return Json(new { result = "ERROR" });
                        }
                        // check for baki peruntukan end

                    return Json(new { result = "OK" });
                }
                catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Baki Vot

        //Start Function Get Id Bahagian
        [HttpPost]
        public JsonResult GetBahagian(JBahagian jBahagian)
        {
            try
            {
                var result = _context.JBahagian.Where(b => b.Id == jBahagian.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Id Bahagian

        //Start Function Get Id Pemohon/Penyedia
        [HttpPost]
        public JsonResult GetPekerja(SuPekerja suPekerja)
        {
            try
            {
                var result = _context.SuPekerja.Where(b => b.Id == suPekerja.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Id Pemohon/Penyedia

        //Start Function Get Jantina Id
        [HttpPost]
        public JsonResult GetJantina(JJantina jJantina)
        {
            try
            {
                var result = _context.JJantina.Where(b => b.Id == jJantina.Id).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //End Function Get Jantina Id

        //Function Cart Empty
        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.spPendahuluanPelbagai1 = new List<int>();
                ViewBag.spPendahuluanPelbagai2 = new List<int>();
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Function Cart Empty end


        //Function PopulateCartfromDb guna utk Edit
        private void PopulateCartFromDb(SpPendahuluanPelbagai spPendahuluanPelbagai)
        {
            List<SpPendahuluanPelbagai1> spPendahuluanPelbagai1Table = _context.SpPendahuluanPelbagai1
                .Include(b => b.JJantina)
                .Where(b => b.SpPendahuluanPelbagaiId == spPendahuluanPelbagai.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (SpPendahuluanPelbagai1 spPendahuluanPelbagai1 in spPendahuluanPelbagai1Table)
            {
                _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                               spPendahuluanPelbagai1.JJantinaId,
                               spPendahuluanPelbagai1.BilAtl,
                               spPendahuluanPelbagai1.BilJul,
                               spPendahuluanPelbagai1.BilPeg,
                               spPendahuluanPelbagai1.BilTek,
                               spPendahuluanPelbagai1.BilUru,
                               spPendahuluanPelbagai1.Jumlah);
            }

            ViewBag.spPendahuluanPelbagai1 = spPendahuluanPelbagai1Table;

            List<SpPendahuluanPelbagai2> spPendahuluanPelbagai2Table = _context.SpPendahuluanPelbagai2
                //.Include(b => b.Indek)
                .Where(b => b.SpPendahuluanPelbagaiId == spPendahuluanPelbagai.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (SpPendahuluanPelbagai2 spPendahuluanPelbagai2 in spPendahuluanPelbagai2Table)
            {
                _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                               spPendahuluanPelbagai2.Indek,
                               spPendahuluanPelbagai2.Baris,
                               spPendahuluanPelbagai2.Perihal,
                               spPendahuluanPelbagai2.Kadar,
                               spPendahuluanPelbagai2.Bil,
                               spPendahuluanPelbagai2.Bulan,
                               spPendahuluanPelbagai2.Jumlah);
            }

            ViewBag.spPendahuluanPelbagai2 = spPendahuluanPelbagai2Table;
        }
        //Function PopulateCartfromDb end

        // GET: SpPermohonanAktiviti
        public async Task<IActionResult> Index(
             string searchString,
             string searchDate1,
             string searchDate2,
             string searchColumn)
        {
            var searchResult = await _spPendahuluanPelbagaiRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoPermohonan")
                    {
                        searchResult = searchResult.Where(s => s.NoPermohonan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    //else if (searchColumn == "Pembekal")
                    //{
                    //    spPermohonanAktiviti = spPermohonanAktiviti.Where(s => s.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
                    //}

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
                        searchResult = searchResult.Where(x => x.TarSedia >= date1
                            && x.TarSedia <= date2).ToList();
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

            return View(searchResult);
        }

        // GET: SpPermohonanAktiviti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _spPendahuluanPelbagaiRepo.GetById((int)id);
            //var kw = await _kwRepo.GetById(spPendahuluanPelbagai.JKWId);
            //spPendahuluanPelbagai.JKW = kw;
            int jumlahPeserta = 0;
            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai1)
            {
                jumlahPeserta += item.Jumlah;
            }

            ViewData["jumlahPeserta"] = jumlahPeserta;
            PopulateList();
            PopulateTable(id);
            return View(spPendahuluanPelbagai);
        }

        //public async Task<IActionResult> Lulus(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var negeri = await _negeriRepo.GetById((int)id);
        //    var spPermohonanAktiviti = await _context.SpPermohonanAktiviti
        //        .Include(s => s.JNegeri)
        //        .Include(s => s.JSukan)
        //        .Include(s => s.JTahapAktiviti)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    var spPermohonanAktiviti = await _context.SpPermohonanAktiviti
        //        .Include(s => s.JNegeri)
        //        .Include(s => s.JSukan)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    spPermohonanAktiviti.JTahapAktiviti = await _tahapAktivitiRepo.GetById(spPermohonanAktiviti.JTahapId);
        //    if (spPermohonanAktiviti == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(spPermohonanAktiviti);
        //}

        // GET: SpPermohonanAktiviti/Create
        public IActionResult Create()
        {

            PopulateList();
            CartEmpty();
            return View();
        }

        // POST: SpPermohonanAktiviti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpPendahuluanPelbagai spPendahuluanPelbagai, int JKWId, int AkCartaId, int SuPekerjaId)
        {
            //var user = "";
            //if (spPendahuluanPelbagai.UserIdKemaskini == "" || spPendahuluanPelbagai.UserIdKemaskini == null)
            //{
            //    user = spPendahuluanPelbagai.Penyedia;
            //}
            //else
            //{
            //    user = spPendahuluanPelbagai.UserIdKemaskini;
            //}

            SpPendahuluanPelbagai m = new SpPendahuluanPelbagai();
            var bahagian = _context.JBahagian.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JBahagianId);
            var tahap = _context.JTahapAktiviti.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JTahapAktivitiId);
            var sukan = _context.JSukan.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JSukanId);
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (spPendahuluanPelbagai != null && JKWId != 0)
                {

                    m.JKWId = JKWId;
                    m.JenisPermohonan = spPendahuluanPelbagai.JenisPermohonan;
                    m.NoPermohonan = RunningNumber(spPendahuluanPelbagai);
                    m.Tarikh = spPendahuluanPelbagai.Tarikh;
                    m.Penyertaan = spPendahuluanPelbagai.Penyertaan;
                    m.Pertandingan = spPendahuluanPelbagai.Pertandingan;
                    m.Pengelolaan = spPendahuluanPelbagai.Pengelolaan;
                    m.ProgramBinaan = spPendahuluanPelbagai.ProgramBinaan;
                    m.JNegeriId = spPendahuluanPelbagai.JNegeriId;
                    m.JSukan = sukan;
                    m.Tarikh = spPendahuluanPelbagai.Tarikh;
                    m.Aktiviti = spPendahuluanPelbagai.Aktiviti;
                    m.Tempat = spPendahuluanPelbagai.Tempat;
                    m.JTahapAktiviti = tahap;
                    m.AkCartaId = spPendahuluanPelbagai.AkCartaId;
                    m.JumKeseluruhan = spPendahuluanPelbagai.JumKeseluruhan;
                    m.FlPosting = 0;
                    //m.TarikhPosting = spPendahuluanPelbagai.TarikhPosting;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.SuPekerjaId = SuPekerjaId;
                    m.TarMasuk = DateTime.Now;
                    m.JBahagian = bahagian;
                    m.UserId = user.UserName;

                    m.SpPendahuluanPelbagai1 = _cart.Lines1.ToArray();
                    m.SpPendahuluanPelbagai2 = _cart.Lines2.ToArray();

                    await _spPendahuluanPelbagaiRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoPermohonan, m.NoPermohonan, 0, m.JumKeseluruhan);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    //CartEmpty();
                    TempData[SD.Success] = "Maklumat Borang Permohonan berjaya ditambah";
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(spPendahuluanPelbagai);
        }

        // GET: SpPermohonanAktiviti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _spPendahuluanPelbagaiRepo.GetById((int)id);
            int jumlahPeserta = 0;

            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }

            foreach (var item in spPendahuluanPelbagai.SpPendahuluanPelbagai1)
            {
                jumlahPeserta += item.Jumlah;
            }

            ViewData["jumlahPeserta"] = jumlahPeserta;
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(spPendahuluanPelbagai);
            return View(spPendahuluanPelbagai);
        }

        // POST: SpPermohonanAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpPendahuluanPelbagai spPendahuluanPelbagai, int JKWId, int JNegeriId, int JJantinaId, decimal JumKeseluruhan, int JBahagian)
        {
            if (id != spPendahuluanPelbagai.Id)
            {
                return NotFound();
            }

            if (spPendahuluanPelbagai.JumKeseluruhan == JumKeseluruhan)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        SpPendahuluanPelbagai spPendahuluanPelbagaiAsal = await _spPendahuluanPelbagaiRepo.GetById(id);
                        //var user = UserManager.GetUserAsync(User);
                        //var namaUser = _context.applicationUsers.FirstOrDefault(x => x.Email == user.Result.Email);
                        // list of input that cannot be change
                        //spPendahuluanPelbagai.Tahun = spPendahuluanPelbagaiAsal.Tahun;
                        spPendahuluanPelbagai.JKWId = spPendahuluanPelbagaiAsal.JKWId;
                        //spPendahuluanPelbagai.NoRujukan = spPendahuluanPelbagaiAsal.NoRujukan;
                        //spPendahuluanPelbagai.Nama = spPendahuluanPelbagaiAsal.Nama;
                        spPendahuluanPelbagai.TarMasuk = spPendahuluanPelbagaiAsal.TarMasuk;
                        spPendahuluanPelbagai.UserId = spPendahuluanPelbagaiAsal.UserId;
                        spPendahuluanPelbagai.FlCetak = 0;
                        // list of input that cannot be change end

                        foreach (SpPendahuluanPelbagai1 item in spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai1)
                        {
                            var model = _context.SpPendahuluanPelbagai1.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }

                        foreach (SpPendahuluanPelbagai2 item in spPendahuluanPelbagaiAsal.SpPendahuluanPelbagai2)
                        {
                            var model = _context.SpPendahuluanPelbagai2.FirstOrDefault(b => b.Id == item.Id);
                            if (model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        // AK CODE
                        decimal jumSeluruhAsal = spPendahuluanPelbagaiAsal.JumKeseluruhan;
                        decimal jumSokongAsal = spPendahuluanPelbagaiAsal.JumSokong;
                        decimal jumLulusAsal = spPendahuluanPelbagaiAsal.JumLulus;
                        var logSeluruh = "";
                        var logSokong = "";
                        var logLulus = "";
                        //AK CODE END
                        _context.Entry(spPendahuluanPelbagaiAsal).State = EntityState.Detached;

                        spPendahuluanPelbagai.SpPendahuluanPelbagai1 = _cart.Lines1.ToList();
                        spPendahuluanPelbagai.SpPendahuluanPelbagai2 = _cart.Lines2.ToList();

                        spPendahuluanPelbagai.UserIdKemaskini = user.UserName;
                        spPendahuluanPelbagai.TarKemaskini = DateTime.Now;

                        _context.Update(spPendahuluanPelbagai);

                        // AK CODE
                        //insert applog
                        if (jumSeluruhAsal != spPendahuluanPelbagai.JumKeseluruhan)
                        {
                            logSeluruh = "Kredit : RM " + jumSeluruhAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumKeseluruhan;
                        }

                        if (jumSokongAsal != spPendahuluanPelbagai.JumSokong)
                        {
                            logSokong = "Debit : RM " + jumSokongAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumSokong;
                        }

                        if (jumLulusAsal != spPendahuluanPelbagai.JumLulus)
                        {
                            logLulus = "Debit : RM " + jumLulusAsal.ToString() + " -> RM " + spPendahuluanPelbagai.JumLulus;
                        }

                        if (logSeluruh != "" || logSokong != "" || logLulus != "")
                        {
                            await AddLogAsync("Ubah", "Ubah Data : " + logSeluruh + ", " + logSokong + ", " + logLulus, spPendahuluanPelbagai.NoPermohonan, id, spPendahuluanPelbagai.JumKeseluruhan);
                        }
                        else
                        {
                            await AddLogAsync("Ubah", "Ubah Data", spPendahuluanPelbagai.NoPermohonan, id, spPendahuluanPelbagai.JumKeseluruhan);
                        }
                        //insert applog end
                        //AK CODE END

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SpPendahuluanPelbagaiExists(spPendahuluanPelbagai.Id))
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
                    if (spPendahuluanPelbagai.JumKeseluruhan != JumKeseluruhan)
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
            return View(spPendahuluanPelbagai);
        }

        // GET: SpPermohonanAktiviti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPendahuluanPelbagai = await _context.SpPendahuluanPelbagai
                .Include(s => s.SuPekerja)
                .Include(s => s.JBahagian)
                .Include(s => s.JNegeri)
                .Include(s => s.JSukan)
                .Include(s => s.JTahapAktiviti)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(spPendahuluanPelbagai);
        }

        // POST: SpPermohonanAktiviti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spPendahuluanPelbagai = await _context.SpPendahuluanPelbagai.FindAsync(id);
            _context.SpPendahuluanPelbagai.Remove(spPendahuluanPelbagai);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Sokong function
        [HttpPost, ActionName("Sokong")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sokong(int? id, decimal jumSokong)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                //var user = UserManager.GetUserAsync(User);
                //var namaUser = _context.applicationUsers.FirstOrDefault(x => x.Email == user.Result.Email);
                //var pelulus = await _context.JPelulus.Include(x => x.SuPekerja).Where(x => x.IsPendahuluan == true).FirstOrDefaultAsync();
                //var sokong = Convert.ToDecimal(jumSokong);

                SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                //check for print
                if (sp.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("SP/" + sp.NoPermohonan)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here

                    //update posting status in SPPENDAHULUANPELBAGAI
                    sp.FlPosting = 1;
                    sp.TarikhPosting = DateTime.Now;
                    sp.JumSokong = jumSokong;
                    //sp.Pelulus = penyokong.SuPekerja.Nama;

                    await _spPendahuluanPelbagaiRepo.Update(sp);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", sp.NoPermohonan, (int)id, sp.JumKeseluruhan);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // Sokong function end

        // posting function
        [HttpPost, ActionName("Posting")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Posting(int? id, string jumSokong, decimal jumLulus)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                var pelulus = await _context.JPelulus.Include(x=>x.SuPekerja).Where(x => x.IsPendahuluan == true).FirstOrDefaultAsync();
                var sokong = Convert.ToDecimal(jumSokong);

                SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                //check for print
                if (sp.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                var abBukuVot = await _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("SP/" + sp.NoPermohonan)).FirstOrDefaultAsync();
                if (abBukuVot != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here


                    //insert into AbBukuVot
                    AbBukuVot abBukuVotPosting = new AbBukuVot()
                    {
                        Tahun = sp.TarMasuk.Year.ToString(),
                        JKWId = sp.JKWId,
                        Tarikh = sp.TarMasuk,
                        Kod = sp.SuPekerja.NoGaji, // tak pasti tarik dari id pekerja ke?
                        Penerima = sp.SuPekerja.Nama,
                        VotId = sp.AkCartaId,
                        Rujukan = "SP/" + sp.NoPermohonan,
                        Tanggungan = jumLulus,
                        JBahagianId = sp.JBahagianId
                    };

                    await _abBukuVotRepo.Insert(abBukuVotPosting);
                    // insert into AbBukuVot end

                    //update posting status in SPPENDAHULUANPELBAGAI
                    sp.FlPosting = 1;
                    sp.TarikhPosting = DateTime.Now;
                    sp.JumSokong = sokong;
                    sp.JumLulus = jumLulus;
                    sp.Pelulus = pelulus.SuPekerja.Nama;
    
                    await _spPendahuluanPelbagaiRepo.Update(sp);

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", sp.NoPermohonan, (int)id, sp.JumKeseluruhan);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "SP001UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                SpPendahuluanPelbagai obj = await _spPendahuluanPelbagaiRepo.GetById((int)id);

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith(obj.NoPermohonan)).ToList();
                if (abBukuVot == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";

                }
                else
                {

                    //unposting operation start here
                    //delete data from abBukuVot
                    foreach (AbBukuVot item in abBukuVot)
                    {
                        await _abBukuVotRepo.Delete(item.Id);
                    }
                    //delete data from abBukuVot end

                    //update posting status in SPPENDAHULUANPELBAGAI
                    obj.FlPosting = 0;
                    obj.TarikhPosting = null;
                    obj.JumSokong = 0;
                    obj.JumLulus = 0;
                    await _spPendahuluanPelbagaiRepo.Update(obj);

                    //insert applog
                    await AddLogAsync("UnPosting", "UnPosting Data", obj.NoPermohonan, (int)id, obj.JumKeseluruhan);

                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kemaskini dari lejar.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end
        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKW = kwList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<JSukan> sukanList = _context.JSukan.OrderBy(b => b.Id).ToList();
            ViewBag.JSukan = sukanList;

            List<JJantina> jantinaList = _context.JJantina.OrderBy(b => b.Id).ToList();
            ViewBag.JJantina = jantinaList;

            List<JTahapAktiviti> tahapAktivitiList = _context.JTahapAktiviti.OrderBy(b => b.Id).ToList();
            ViewBag.JTahapAktiviti = tahapAktivitiList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Id).ToList();
            ViewBag.JBahagian = bahagianList;

            var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (User.IsInRole("SuperAdmin"))
            {
                ViewBag.NamaPekerja = "SuperAdmin";
            }
            else
            {
                ViewBag.IdPekerja = user.SuPekerjaId;
                ViewBag.NamaPekerja = user.SuPekerja.Nama;
            }

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;
        }

        private void PopulateTable(int? id)
        {

            List<SpPendahuluanPelbagai1> spPendahuluanPelbagai1 = _context.SpPendahuluanPelbagai1
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.spPendahuluanPelbagai1 = spPendahuluanPelbagai1;

            List<SpPendahuluanPelbagai2> spPendahuluanPelbagai2 = _context.SpPendahuluanPelbagai2
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.spPendahuluanPelbagai2 = spPendahuluanPelbagai2;
        }

        public async Task<JsonResult> SaveSpPendahuluanPelbagai1 (SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                if (spPendahuluanPelbagai1 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                         spPendahuluanPelbagai1.JJantinaId,
                         spPendahuluanPelbagai1.BilAtl,
                         spPendahuluanPelbagai1.BilJul,
                         spPendahuluanPelbagai1.BilPeg,
                         spPendahuluanPelbagai1.BilTek,
                         spPendahuluanPelbagai1.BilUru,
                         spPendahuluanPelbagai1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public async Task<JsonResult> SaveSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                if (spPendahuluanPelbagai2 != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                         spPendahuluanPelbagai2.Indek,
                         spPendahuluanPelbagai2.Baris,
                         spPendahuluanPelbagai2.Perihal,
                         spPendahuluanPelbagai2.Kadar,
                         spPendahuluanPelbagai2.Bil,
                         spPendahuluanPelbagai2.Bulan,
                         spPendahuluanPelbagai2.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        public JsonResult RemoveSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                if (spPendahuluanPelbagai1 != null)
                {

                    _cart.RemoveItem1(spPendahuluanPelbagai1.JJantinaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                if (spPendahuluanPelbagai2 != null)
                {

                    _cart.RemoveItem2(spPendahuluanPelbagai2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        //save cart SpPendahuluanPelbagai1
        public JsonResult SaveCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {

                var akP1 = _cart.Lines1.Where(x => x.JJantinaId == spPendahuluanPelbagai1.JJantinaId).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akP1 != null)
                {
                    _cart.RemoveItem1(spPendahuluanPelbagai1.JJantinaId);

                    _cart.AddItem1(spPendahuluanPelbagai1.SpPendahuluanPelbagaiId,
                                    spPendahuluanPelbagai1.JJantinaId,
                                    spPendahuluanPelbagai1.BilAtl,
                                    spPendahuluanPelbagai1.BilJul,
                                    spPendahuluanPelbagai1.BilPeg,
                                    spPendahuluanPelbagai1.BilTek,
                                    spPendahuluanPelbagai1.BilUru,
                                    spPendahuluanPelbagai1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart SpPendahuluanPelbagai1 end

        // get an item from cart SpPendahuluanPelbagai1
        public JsonResult GetAnItemCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                SpPendahuluanPelbagai1 data = _cart.Lines1.Where(x => x.JJantinaId == spPendahuluanPelbagai1.JJantinaId).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart SpPendahuluanPelbagai1 end

        // get all item from cart SpPendahuluanPelbagai1
        public JsonResult GetAllItemCartSpPendahuluanPelbagai1(SpPendahuluanPelbagai1 spPendahuluanPelbagai1)
        {

            try
            {
                List<SpPendahuluanPelbagai1> data = _cart.Lines1.ToList();

                foreach (SpPendahuluanPelbagai1 item in data)
                {
                       var jJantina = _context.JJantina.Find(item.JJantinaId);

                      item.JJantina = jJantina;
                }

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart SpPendahuluanPelbagai1 end 

        //save cart SpPendahuluanPelbagai2
        public JsonResult SaveCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {

                var akP1 = _cart.Lines2.Where(x => x.Indek == spPendahuluanPelbagai2.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akP1 != null)
                {
                    _cart.RemoveItem2(spPendahuluanPelbagai2.Indek);

                    _cart.AddItem2(spPendahuluanPelbagai2.SpPendahuluanPelbagaiId,
                                    spPendahuluanPelbagai2.Indek,
                                    spPendahuluanPelbagai2.Baris,
                                    spPendahuluanPelbagai2.Perihal,
                                    spPendahuluanPelbagai2.Kadar,
                                    spPendahuluanPelbagai2.Bil,
                                    spPendahuluanPelbagai2.Bulan,
                                    spPendahuluanPelbagai2.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart SpPendahuluanPelbagai2 end

        // get an item from cart SpPendahuluanPelbagai2
        public JsonResult GetAnItemCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                SpPendahuluanPelbagai2 data = _cart.Lines2.Where(x => x.Indek == spPendahuluanPelbagai2.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart SpPendahuluanPelbagai2 end

        // get all item from cart SpPendahuluanPelbagai2
        public JsonResult GetAllItemCartSpPendahuluanPelbagai2(SpPendahuluanPelbagai2 spPendahuluanPelbagai2)
        {

            try
            {
                List<SpPendahuluanPelbagai2> data = _cart.Lines2.OrderBy(b => b.Indek).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart SpPendahuluanPelbagai1 end 

        private bool SpPendahuluanPelbagaiExists(int id)
        {
            return _context.SpPendahuluanPelbagai.Any(e => e.Id == id);
        }

        public async Task<IActionResult> PrintPdf(int id)
        {
            SpPendahuluanPelbagai sp = await _spPendahuluanPelbagaiRepo.GetByIdIncludeDeletedItems(id);

            var jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(sp.JumKeseluruhan)).ToUpper();

            var user = await _userManager.GetUserAsync(User);

            PendahuluanPelbagaiPrintModel data = new PendahuluanPelbagaiPrintModel();

            foreach (SpPendahuluanPelbagai1 item in sp.SpPendahuluanPelbagai1)
            {
                data.BilAtl += item.BilAtl;
                data.BilJul += item.BilJul;
                data.BilPeg += item.BilPeg;
                data.BilTek += item.BilTek;
                data.BilUru += item.BilUru;
                data.Jumlah += item.Jumlah;
            }

            foreach (SpPendahuluanPelbagai2 item in sp.SpPendahuluanPelbagai2)
            {
                data.JumlahPerihal += item.Jumlah;
            }

            List<JTahapAktiviti> list = _context.JTahapAktiviti.ToList();

            data.Tahap = list;

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.SpPendahuluanPelbagai = sp;
            //data.spPermohonanAktiviti.JNegeri = negeri;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            //update cetak -> 1
            sp.FlCetak = 1;
            await _spPendahuluanPelbagaiRepo.Update(sp);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", sp.NoPermohonan, id, sp.JumKeseluruhan);
            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("PendahuluanPelbagaiPrintPDF", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
    }
}

