using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using MSNK.Models.Modules.ViewModel;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;


namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AkPVController : Controller
    {
        public const string modul = "PV001";
        public const string namamodul = "Baucer Pembayaran";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly ListViewIRepository<AkPV1, int> _akPV1Repo;
        private readonly ListViewIRepository<AkPV2, int> _akPV2Repo;
        private readonly IRepository<AkBelian, int, string> _akBelianRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<SuAtlet, int, string> _suAtletRepo;
        private readonly IRepository<SuJurulatih, int, string> _suJurulatihRepo;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<AkTunaiLejar, int, string> _akTunaiLejarRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPPRepo;
        private readonly IRepository<SuProfil, int, string> _suProfilRepo;
        private readonly IRepository<JPenyemak, int, string> _penyemakRepo;
        private readonly IRepository<JPelulus, int, string> _pelulusRepo;
        private readonly IRepository<JBank, int, string> _jBankRepo;
        private readonly IRepository<JCaraBayar, int, string> _jCaraBayarRepo;
        private readonly UserService _userService;
        private CartPV _cart;

        public AkPVController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPV, int, string> akPVRepository,
            ListViewIRepository<AkPV1, int> akPV1Repository,
            ListViewIRepository<AkPV2, int> akPV2Repository,
            IRepository<AkBelian, int, string> akBelian,
            IRepository<AkPembekal, int, string> akPembekal,
            IRepository<SuPekerja, int, string> suPekerja,
            IRepository<SuJurulatih, int, string> suJurulatih,
            IRepository<SuAtlet, int, string> suAtlet,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository,
            IRepository<AkTunaiLejar, int, string> akTunaiLejarRepository,
            IRepository<JKW, int, string> kwRepo,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CustomIRepository<string, int> customRepo,
            IRepository<SpPendahuluanPelbagai, int, string> spPPRepo,
            IRepository<SuProfil, int, string> suProfilRepo,
            IRepository<JPenyemak, int, string> penyemakRepo,
            IRepository<JPelulus, int, string> pelulusRepo,
            IRepository<JBank, int, string> jBankRepo,
            IRepository<JCaraBayar, int, string> jCaraBayarRepo,
            UserService userService,
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
            _suPekerjaRepo = suPekerja;
            _suAtletRepo = suAtlet;
            _suJurulatihRepo = suJurulatih;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
            _akTunaiLejarRepo = akTunaiLejarRepository;
            _kwRepo = kwRepo;
            _akCartaRepo = akCartaRepository;
            _akBankRepo = akBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVotRepo = abBukuVotRepository;
            _customRepo = customRepo;
            _spPPRepo = spPPRepo;
            _suProfilRepo = suProfilRepo;
            _penyemakRepo = penyemakRepo;
            _pelulusRepo = pelulusRepo;
            _jBankRepo = jBankRepo;
            _jCaraBayarRepo= jCaraBayarRepo;
            _userService = userService;
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

        // GET: AkPV
        [Authorize(Policy = "PV001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No PV", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Nama", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akPV = await _akPVRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akPV = await _akPVRepo.GetAllIncludeDeletedItems();
            }

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
                        akPV = akPV.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akPV = akPV.Where(x => x.Tarikh >= date1
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
                akPV = akPV.OrderByDescending(b => b.Tarikh).Take(100);
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }

            List<AkPVViewModel> viewModel = new List<AkPVViewModel>();
            foreach (AkPV item in akPV)
            {
                decimal jumlahInbois = 0;
                foreach (AkPV2 item2 in item.AkPV2)
                {
                    jumlahInbois += item2.Amaun;
                }
                viewModel.Add(new AkPVViewModel
                {
                    Id = item.Id,
                    Tahun = item.Tahun,
                    NoPV = item.NoPV,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Penerima = item.Nama,
                    CaraBayar = item.JCaraBayar?.Perihal ?? "PELBAGAI",
                    FlHapus = item.FlHapus,
                    FlPosting = item.FlPosting,
                    FlCetak = item.FlCetak,
                    FlStatusSemak = item.FlStatusSemak,
                    FlStatusLulus = item.FlStatusLulus,
                    JumlahInbois = jumlahInbois,
                    FlKategoriPenerima = item.FlKategoriPenerima,
                    FlBatal = item.FlBatal
                }
                );
            }

            List<JPenyemak> penyemak = _context.JPenyemak
                .Include(x => x.SuPekerja)
                .Where(x => x.IsPV == true).OrderBy(b => b.SuPekerja.Nama).ToList();
            ViewBag.JPenyemak = penyemak;

            List<JPelulus> pelulus = _context.JPelulus
                .Include(x => x.SuPekerja)
                .Where(x => x.IsPV == true).OrderBy(b => b.SuPekerja.Nama).ToList();
            ViewBag.JPelulus = pelulus;

            return View(viewModel);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBank> bankList = _context.JBank.OrderBy(b => b.Nama).ToList();
            bankList.Add(new JBank
            {
                Id = 9999,
                Kod = "X",
                Nama = "BERGANDA"
            });
            ViewBag.JBank = bankList;

            List<SpPendahuluanPelbagai> spList = _context.SpPendahuluanPelbagai.Where(x => x.FlPosting == 1).OrderBy(b => b.NoPermohonan).ToList();

            List<SpPendahuluanPelbagai> spListUpdated = new List<SpPendahuluanPelbagai>();

            foreach (var item in spList)
            {
                var ExistAkPVWithSp = _context.AkPV.Any(b => b.SpPendahuluanPelbagaiId == item.Id);

                if (ExistAkPVWithSp == true)
                {
                    continue;
                }
                else
                {
                    spListUpdated.Add(item);
                }
            }

            ViewBag.SpPendahuluanPelbagai = spListUpdated;

            List<SuProfil> suProfilList = _context.SuProfil.Where(x => x.FlPosting == 1).OrderBy(b => b.NoRujukan).ToList();
            ViewBag.SuProfil = suProfilList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkTunaiRuncit> tunaiRuncitList = _context.AkTunaiRuncit
                .Include(c => c.AkTunaiPemegang)
                .ThenInclude(c => c.SuPekerja).ToList();
            ViewBag.AkTunaiRuncit = tunaiRuncitList;

            List<AkBelian> akBelianList = _context.AkBelian
                .Include(b => b.AkPO)
                .Where(b => b.FlPosting == 1)
                .OrderBy(b => b.Tarikh).ToList();

            List<AkBelian> akBelianListUpdated = new List<AkBelian>();

            foreach (var item in akBelianList)
            {
                var TotalAkPV = _context.AkPV2.Where(b => b.AkBelianId == item.Id).Sum(b => b.Amaun).CompareTo(item.Jumlah);
                if (TotalAkPV == 0 || TotalAkPV > 0)
                {
                    continue;
                }
                else
                {
                    if (item.NoInbois.Length > 9)
                    {
                        item.NoInbois = item.NoInbois.Substring(9);
                    }
                    else
                    {
                        item.NoInbois = "xx/xxxxx/" + item.NoInbois;
                    }

                    akBelianListUpdated.Add(item);
                }

            }
            ViewBag.AkBelian = akBelianListUpdated;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;

            List<SuJurulatih> suJurulatihList = _context.SuJurulatih
                .OrderBy(b => b.NoKp).Where(b => b.FlStatus == 1).ToList();
            ViewBag.SuJurulatih = suJurulatihList;

            List<SuAtlet> suAtletList = _context.SuAtlet
                .OrderBy(b => b.NoKp).Where(b => b.FlStatus == 1).ToList();
            ViewBag.SuAtlet = suAtletList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

            List<AkTunaiRuncit> akTunaiRuncitList = _context.AkTunaiRuncit.ToList();
            ViewBag.AkTunaiRuncit = akTunaiRuncitList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<JCaraBayar> jCaraBayarList = _context.JCaraBayar.Where(b => b.Kod == "C" || b.Kod == "E" || b.Kod == "JP").ToList();
            jCaraBayarList.Add(new JCaraBayar
            {
                Id = 9999,
                Kod = "X",
                Perihal = "BERGANDA"
            });
            ViewBag.JCaraBayar = jCaraBayarList;

        }

        private void PopulateCart()
        {
            List<AkPV1> lines1 = _cart.Lines1.ToList();

            foreach (AkPV1 item in lines1)
            {
                var carta = _context.AkCarta.Where(x => x.Id == item.AkCartaId).FirstOrDefault();
                item.AkCarta = carta;
            }

            ViewBag.akTerima1 = lines1;

            List<AkPV2> lines2 = _cart.Lines2.ToList();

            foreach (AkPV2 item in lines2)
            {
                var akBelian = _context.AkBelian
                    .Include(x => x.AkPO)
                    .Where(x => x.Id == item.AkBelianId).FirstOrDefault();
                item.AkBelian = akBelian;
            }

            ViewBag.akTerima2 = _cart.Lines2.ToList();
        }

        private void PopulateCartFromDb(AkPV akPV)
        {
            List<AkPV1> akPV1Table = _context.AkPV1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPVId == akPV.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPV1 akPV1 in akPV1Table)
            {
                _cart.AddItem1(akPV1.AkPVId,
                               akPV1.Amaun,
                               akPV1.AkCartaId);
            }

            ViewBag.akPV1 = akPV1Table;

            List<AkPV2> akPV2Table = _context.AkPV2
                .Include(b => b.AkBelian).ThenInclude(b => b.AkPO)
                .Where(b => b.AkPVId == akPV.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPV2 akPV2 in akPV2Table)
            {
                _cart.AddItem2(akPV2.AkPVId,
                               akPV2.AkBelianId,
                               akPV2.Amaun,
                               akPV2.HavePO);
            }

            ViewBag.akPV2 = akPV2Table;

            List<AkPVGanda> akPVGandaTable = _context.AkPVGanda
                .Include(b => b.SuAtlet)
                .Include(b => b.SuJurulatih)
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JBank)
                .Where(b => b.AkPVId == akPV.Id)
                .OrderBy(b => b.Indek)
                .ToList();

            foreach (AkPVGanda akPVGanda in akPVGandaTable)
            {
                _cart.AddItemGanda(akPVGanda.AkPVId,
                                akPVGanda.Indek,
                                akPVGanda.FlKategoriPenerima,
                                akPVGanda.SuPekerjaId,
                                akPVGanda.SuAtletId,
                                akPVGanda.SuJurulatihId,
                                akPVGanda.Nama,
                                akPVGanda.NoKp,
                                akPVGanda.NoAkaun,
                                akPVGanda.JBankId,
                                null,
                                akPVGanda.Amaun,
                                akPVGanda.NoCekAtauEFT,
                                akPVGanda.TarCekAtauEFT,
                                akPVGanda.JCaraBayarId,
                                null);
            }

            ViewBag.akPVGanda = akPVGandaTable;
        }
        // function json get no rujukan (running number)
        [HttpPost]
        public JsonResult JsonGetKod(int data, string year, string month)
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
                    if (year != null)
                    {
                        year = year.Substring(2, 2);

                    }
                    // get latest no rujukan running number  
                    result = GetNoRujukan(data, year, month);
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

        // function json get panjar 
        [HttpPost]
        public async Task<JsonResult> JsonGetPanjar(int data)
        {
            try
            {

                var akTunaiRuncit = await _context.AkTunaiRuncit
                    .Include(b => b.AkTunaiPemegang)
                    .ThenInclude(b => b.SuPekerja).ThenInclude(b => b.JBank)
                    .FirstOrDefaultAsync(b => b.Id == data);

                return Json(new { result = "OK", record = akTunaiRuncit });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        // function json get panjar end

        // function json get panjar 
        [HttpPost]
        public async Task<JsonResult> JsonGetNoRekup(int data)
        {
            try
            {
                CartEmpty();

                string noRekup = "";

                // cari latest no rekup
                var LatestTunaiLejarRekup = await _context.AkTunaiLejar
                    .Include(b => b.AkTunaiRuncit)
                    .Where(b => b.AkTunaiRuncit.Id == data && b.Rekup != "BAKI AWAL" && b.Rekup != null)
                    .OrderByDescending(b => b.Rekup).ThenByDescending(b => b.Tarikh)
                    .FirstOrDefaultAsync();

                // rekupan route
                if (LatestTunaiLejarRekup != null)
                {
                    noRekup = LatestTunaiLejarRekup.Rekup;

                    // find kod akaun, had maksimum in AkTunaiRuncit
                    AkTunaiRuncit akTunaiRuncit = _context.AkTunaiRuncit.FirstOrDefault(x => x.Id == data);

                    if (akTunaiRuncit.HadMaksimum != 0)
                    {
                        var rekupanList = (from tbl in _context.AkTunaiLejar
                                                       .Include(x => x.AkTunaiRuncit).ThenInclude(x => x.AkCarta)
                                                       .Where(x => x.AkTunaiRuncitId == data && x.Rekup == noRekup).ToList()
                                           select new
                                           {
                                               noRekup = tbl.Rekup,
                                               akCarta = tbl.AkTunaiRuncit.AkCarta,
                                               Debit = tbl.Debit,
                                               Kredit = tbl.Kredit
                                           }).GroupBy(x => x.noRekup).FirstOrDefault();


                        decimal JumlahDebit = 0;

                        decimal JumlahKredit = 0;

                        foreach (var item in rekupanList)
                        {
                            JumlahDebit = JumlahDebit + item.Debit;
                            JumlahKredit = JumlahKredit + item.Kredit;
                        }

                        if (JumlahDebit == akTunaiRuncit.HadMaksimum)
                        {
                            decimal JumlahRekupan = JumlahKredit;

                            var objRekup = rekupanList.Select(l => new
                            {
                                noRekup = l.noRekup,
                                akCarta = l.akCarta,
                                amaun = JumlahRekupan
                            }).FirstOrDefault();

                            PopulateCartFromAkTunaiRuncit(data, noRekup, objRekup.akCarta.Id, JumlahRekupan);


                            return Json(new { result = "OK", record = noRekup, objek = objRekup });
                        }

                    }

                }

                return Json(new { result = "OK", record = noRekup });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCartFromAkTunaiRuncit(int id, string noRekup, int akCartaId, decimal Amaun)
        {

            var akPVId = 0;

            _cart.AddItem1(akPVId,
                           Amaun,
                           akCartaId);

        }
        // function json get panjar end

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

        public async Task<JsonResult> SaveAkPV1(
            AkPV1 akPV1,
            string tahun,
            int jKWId,
            int jBahagianId,
            int FlKategoriPenerima,
            bool IsAKB,
            int AkBankId)
        {

            try
            {
                if (akPV1 != null)
                {
                    // check for baki peruntukan
                    // note :
                    // FlKategoriPenerima = 1 (pembekal)
                    // FlKategoriPenerima = 2 (pekerja)
                    // FlKategoriPenerima = 3 (panjar)
                    // FlKategoriPenerima = 0 (other than above)
                    //if (FlKategoriPenerima == 0 || FlKategoriPenerima == 2)
                    //{
                    if (IsAKB == true)
                    {
                        // check if this is the first year using the system
                        var AppInfo = _context.SiAppInfo.Where(b => b.TarMula.Year <= DateTime.Now.Year).FirstOrDefault();

                        // if system already run for after a year, check Waran Peruntukan for past year
                        if (AppInfo == null)
                        {
                            // check if akaun bank bypass peruntukan or not
                            var akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);
                            if (akBank != null)
                            {
                                // if akaun bank is in bajet, check peruntukan
                                if (akBank.IsBajet == true)
                                {
                                    // check AkCarta is it bypass peruntukan or not
                                    var CartaDgnPeruntukan = await _context.AkCarta
                                        .Where(d => d.Id == akPV1.AkCartaId && d.IsBajet == true)
                                        .FirstOrDefaultAsync();

                                    if (CartaDgnPeruntukan != null)
                                    {
                                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                        .Where(x => x.Tahun == tahun && x.VotId == akPV1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                        .AnyAsync();

                                        if (IsExistAbBukuVot == true)
                                        {
                                            if (FlKategoriPenerima == 0 || FlKategoriPenerima == 2)
                                            {
                                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPV1.AkCartaId, jKWId, jBahagianId);

                                                if (sum < akPV1.Amaun)
                                                {
                                                    return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak mencukupi." });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak wujud." });
                                        }

                                    }
                                    // check for baki peruntukan end
                                }
                            }
                            else
                            {
                                return Json(new { result = "ERROR", message = "Sila pilih akaun bank." });
                            }

                        }
                    }
                    else
                    {
                        // check if akaun bank bypass peruntukan or not
                        var akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);
                        if (akBank != null)
                        {
                            // if akaun bank is in bajet, check peruntukan
                            if (akBank.IsBajet == true)
                            {
                                // check AkCarta is it bypass peruntukan or not
                                var CartaDgnPeruntukan = await _context.AkCarta
                                    .Where(d => d.Id == akPV1.AkCartaId && d.IsBajet == true)
                                    .FirstOrDefaultAsync();

                                if (CartaDgnPeruntukan != null)
                                {
                                    bool IsExistAbBukuVot = await _context.AbBukuVot
                                    .Where(x => x.Tahun == tahun && x.VotId == akPV1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                    .AnyAsync();

                                    if (IsExistAbBukuVot == true)
                                    {
                                        if (FlKategoriPenerima == 0 || FlKategoriPenerima == 2)
                                        {
                                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPV1.AkCartaId, jKWId, jBahagianId);

                                            if (sum < akPV1.Amaun)
                                            {
                                                return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak mencukupi." });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { result = "ERROR", message = "Bajet untuk kod akaun ini tidak wujud." });
                                    }

                                }
                                // check for baki peruntukan end
                            }

                        }
                        else
                        {
                            return Json(new { result = "ERROR", message = "Sila pilih akaun bank." });
                        }

                    }

                    _cart.AddItem1(akPV1.AkPVId,
                                   akPV1.Amaun,
                                   akPV1.AkCartaId);
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
        public async Task<JsonResult> SaveCartAkPV1(
            AkPV1 akPV1,
            string tahun,
            int jKWId,
            int jBahagianId,
            int FlKategoriPenerima,
            bool IsAKB,
            int AkBankId)
        {

            try
            {

                var akT1 = _cart.Lines1.Where(x => x.AkCartaId == akPV1.AkCartaId).FirstOrDefault();

                if (akT1 != null)
                {
                    // check for baki peruntukan
                    // note :
                    // FlKategoriPenerima = 1 (pembekal)
                    // FlKategoriPenerima = 2 (pekerja)
                    // FlKategoriPenerima = 3 (panjar)
                    // FlKategoriPenerima = 0 (other than above)
                    // check if akaun bank bypass peruntukan or not
                    var akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);
                    if (akBank != null)
                    {
                        // if akaun bank is in bajet, check peruntukan
                        if (akBank.IsBajet == true)
                        {
                            if (IsAKB == true)
                            {
                                // check if this is the first year using the system
                                var AppInfo = _context.SiAppInfo.Where(b => b.TarMula.Year <= DateTime.Now.Year).FirstOrDefault();

                                // if system already run for after a year, check Waran Peruntukan for past year
                                if (AppInfo == null)
                                {

                                    // check AkCarta is it bypass peruntukan or not
                                    var CartaDgnPeruntukan = await _context.AkCarta
                                        .Where(d => d.Id == akPV1.AkCartaId && d.IsBajet == true)
                                        .FirstOrDefaultAsync();

                                    if (CartaDgnPeruntukan != null)
                                    {
                                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                        .Where(x => x.Tahun == tahun && x.VotId == akPV1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                        .AnyAsync();

                                        if (IsExistAbBukuVot == true)
                                        {
                                            if (FlKategoriPenerima == 0 || FlKategoriPenerima == 2)
                                            {
                                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPV1.AkCartaId, jKWId, jBahagianId);

                                                if (sum < akPV1.Amaun)
                                                {
                                                    return Json(new { result = "ERROR" });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return Json(new { result = "ERROR" });
                                        }

                                    }
                                    // check for baki peruntukan end

                                }
                            }
                            else
                            {

                                // check AkCarta is it bypass peruntukan or not
                                var CartaDgnPeruntukan = await _context.AkCarta
                                    .Where(d => d.Id == akPV1.AkCartaId && d.IsBajet == true)
                                    .FirstOrDefaultAsync();

                                if (CartaDgnPeruntukan != null)
                                {
                                    bool IsExistAbBukuVot = await _context.AbBukuVot
                                    .Where(x => x.Tahun == tahun && x.VotId == akPV1.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                    .AnyAsync();

                                    if (IsExistAbBukuVot == true)
                                    {
                                        if (FlKategoriPenerima == 0 || FlKategoriPenerima == 2)
                                        {
                                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, akPV1.AkCartaId, jKWId, jBahagianId);

                                            if (sum < akPV1.Amaun)
                                            {
                                                return Json(new { result = "ERROR" });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { result = "ERROR" });
                                    }

                                }
                                // check for baki peruntukan end


                            }
                        }

                    }
                    else
                    {
                        return Json(new { result = "ERROR", message = "Sila pilih akaun bank dahulu." });
                    }


                    _cart.RemoveItem1(akPV1.AkCartaId);

                    _cart.AddItem1(akPV1.AkPVId,
                                   akPV1.Amaun,
                                   akPV1.AkCartaId);
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
                //CartEmpty();

                var result = _context.AkBelian
                    .Include(b => b.AkPO)
                    .Include(b => b.AkBelian1).ThenInclude(b => b.AkCarta)
                    .Where(b => b.Id == akBelian.Id)
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

        //private void PopulateCartAkPV1(int id)
        //{
        //    var user = _userManager.GetUserName(User);

        //    List<AkBelian1> table1 = _context.AkBelian1
        //        .Include(b => b.AkCarta)
        //        .Where(b => b.AkBelianId == id)
        //        .OrderBy(b => b.Id)
        //        .ToList();

        //    foreach (AkBelian1 item in table1)
        //    {

        //        item.AkBelianId = 0;

        //        _cart.AddItem1(item.AkBelianId,
        //                        item.Amaun,
        //                        item.AkCartaId);
        //    }
        //}
        //on change no PO controller end

        public async Task<JsonResult> SaveAkPV2(
            AkPV2 akPV2,
            string tahun,
            int jKWId,
            int jBahagianId,
            int AkBankId)
        {

            try
            {
                if (akPV2 != null)
                {

                    // check if Inbois has PO or not
                    var po = _context.AkBelian.Include(x => x.AkBelian1).ThenInclude(x => x.AkCarta).Where(b => b.Id == akPV2.AkBelianId).FirstOrDefault();

                    if (po.AkPOId != null || po.AkIndenId != null)
                    {
                        akPV2.HavePO = true;
                    }

                    // check for baki peruntukan
                    if (akPV2.HavePO == false)
                    {
                        // check if akaun bank is in bajet or not
                        var akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);
                        if (akBank != null)
                        {
                            // if is in bajet, check peruntukan
                            if (akBank.IsBajet == true)
                            {
                                foreach (AkBelian1 item in po.AkBelian1)
                                {
                                    bool IsExistAbBukuVot = await _context.AbBukuVot
                                   .Where(x => x.Tahun == tahun && x.VotId == item.AkCartaId && x.JKWId == jKWId && x.JBahagianId == jBahagianId)
                                   .AnyAsync();

                                    if (IsExistAbBukuVot == true)
                                    {
                                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(tahun, item.AkCartaId, jKWId, jBahagianId);

                                        if (sum < akPV2.Amaun)
                                        {
                                            return Json(new { result = "ERROR" });
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { result = "ERROR" });
                                    }
                                }
                            }

                        }
                        else
                        {
                            return Json(new { result = "ERROR", message = "Sila pilih akaun bank dahulu." });
                        }

                    }

                    // check for baki peruntukan end
                    // add AkPV2 into cart lines2
                    _cart.AddItem2(akPV2.AkPVId,
                                   akPV2.AkBelianId,
                                   akPV2.Amaun,
                                   akPV2.HavePO);

                    // get kod akaun from akBelian1
                    foreach (var item in po.AkBelian1)
                    {
                        var akBelian1 = _cart.Lines1.Where(b => b.AkCartaId == item.AkCartaId).FirstOrDefault();

                        var amount = item.Amaun;
                        if (akBelian1 != null)
                        {
                            amount += akBelian1.Amaun;

                            _cart.RemoveItem1(akBelian1.Id);

                        }

                        _cart.AddItem1(akPV2.AkPVId,
                                        amount,
                                        item.AkCartaId);
                    }
                    // add to cart
                    // -- check if akCartaId already exist or not in _cart
                    // -- if exist, add the amount
                    // -- if not, add new kod akaun
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
        public async Task<JsonResult> GetAnItemCartAkPV2(AkPV2 akPV2)
        {

            try
            {
                AkPV2 data = _cart.Lines2.Where(x => x.AkBelianId == akPV2.AkBelianId).FirstOrDefault();

                if (data.AkBelianId != null)
                    data.AkBelian = await _akBelianRepo.GetById((int)data.AkBelianId);

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

                    if (akPV2.AkBelianId != 0)
                        akPV2.HavePO = true;

                    _cart.AddItem2(akPV2.AkPVId,
                                   akPV2.AkBelianId,
                                   akPV2.Amaun,
                                   akPV2.HavePO);
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

                string PoType = String.Empty;

                foreach (AkPV2 item in data)
                {
                    var akBelian = _context.AkBelian
                        .Include(d => d.AkPO)
                        .Include(d => d.AkInden)
                        .Where(d => d.Id == item.AkBelianId)
                        .FirstOrDefault();

                    item.AkBelian = akBelian;

                    if (akBelian.AkPO != null)
                    {
                        PoType = "PO";
                    }

                    if (akBelian.AkInden != null)
                    {
                        PoType = "Inden";
                    }

                }

                return Json(new { result = "OK", record = data, type = PoType });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPV2 end


        //function json Create akPV2 end

        // get latest Index number in AkPVGanda
        public JsonResult GetLatestIndexNumberGanda()
        {

            try
            {
                if (_cart.LinesGanda != null && _cart.LinesGanda.Count() > 0)
                {
                    var data = _cart.LinesGanda.Max(x => x.Indek);

                    bool IsGanda = _cart.LinesGanda.Count() > 0;

                    return Json(new { result = "OK", record = data, ganda = IsGanda });
                }
                else
                {
                    return Json(new { result = "OK", record = 0, ganda = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart akPVGanda
        public JsonResult GetAnItemCartAkPVGanda(AkPVGanda akPVGanda)
        {

            try
            {
                AkPVGanda data = _cart.LinesGanda.Where(x => x.Indek == akPVGanda.Indek).FirstOrDefault();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart akPVGanda end

        //save cart akPVGanda
        public async Task<JsonResult> SaveCartAkPVGanda(AkPVGanda akPVGanda)
        {

            try
            {

                var akT3 = _cart.LinesGanda.Where(x => x.Indek == akPVGanda.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akPVGanda.JBankId != 0)
                {
                    akPVGanda.JBank = await _jBankRepo.GetById((int)akPVGanda.JBankId);
                }

                if (akPVGanda.JCaraBayarId != 0)
                {
                    akPVGanda.JCaraBayar = await _jCaraBayarRepo.GetById((int)akPVGanda.JCaraBayarId);
                }
                if (akT3 != null)
                {
                    _cart.RemoveItemGanda(akPVGanda.Indek);

                    _cart.AddItemGanda(akPVGanda.AkPVId,
                                       akPVGanda.Indek,
                                       akPVGanda.FlKategoriPenerima,
                                       akPVGanda.SuPekerjaId,
                                       akPVGanda.SuAtletId,
                                       akPVGanda.SuJurulatihId,
                                       akPVGanda.Nama,
                                       akPVGanda.NoKp,
                                       akPVGanda.NoAkaun,
                                       akPVGanda.JBankId,
                                       akPVGanda.JBank,
                                       akPVGanda.Amaun,
                                       akPVGanda.NoCekAtauEFT,
                                       akPVGanda.TarCekAtauEFT,
                                       akPVGanda.JCaraBayarId,
                                       akPVGanda.JCaraBayar);
                }
                else
                {
                    _cart.AddItemGanda(akPVGanda.AkPVId,
                                       akPVGanda.Indek,
                                       akPVGanda.FlKategoriPenerima,
                                       akPVGanda.SuPekerjaId,
                                       akPVGanda.SuAtletId,
                                       akPVGanda.SuJurulatihId,
                                       akPVGanda.Nama,
                                       akPVGanda.NoKp,
                                       akPVGanda.NoAkaun,
                                       akPVGanda.JBankId,
                                       akPVGanda.JBank,
                                       akPVGanda.Amaun,
                                       akPVGanda.NoCekAtauEFT,
                                       akPVGanda.TarCekAtauEFT,
                                       akPVGanda.JCaraBayarId,
                                       akPVGanda.JCaraBayar);
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
        public JsonResult GetAllItemCartAkPVGanda()
        {

            try
            {
                List<AkPVGanda> data = _cart.LinesGanda.OrderBy(b => b.Nama).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart akPVGanda end

        // remove item from cart akPVGanda
        public JsonResult RemoveAkPVGanda(AkPVGanda akPVGanda)
        {

            try
            {
                if (akPVGanda != null)
                {

                    _cart.RemoveItemGanda(akPVGanda.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // remove item from cart akPVGanda end

        // GET: AkPV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPV = await _akPVRepo.GetByIdIncludeDeletedItems((int)id);

            // normal user access
            if (User.IsInRole("User"))
            {
                akPV = await _akPVRepo.GetById((int)id);
            }

            if (akPV == null)
            {
                return NotFound();
            }

            AkPVViewModel akPVView = new AkPVViewModel();

            //fill in view model AkPVViewModel from akPV
            akPVView.AkPembekalId = akPV.AkPembekalId;
            akPVView.SuPekerjaId = akPV.SuPekerjaId;
            akPVView.Id = akPV.Id;
            akPVView.Tahun = akPV.Tahun;
            akPVView.NoPV = akPV.NoPV;
            akPVView.Tarikh = akPV.Tarikh;
            akPVView.JKW = akPV.JKW;
            akPVView.JKWId = akPV.JKWId;
            akPVView.JBahagian = akPV.JBahagian;
            akPVView.JBahagianId = akPV.JBahagianId;
            akPVView.AkBank = akPV.AkBank;
            akPVView.Jumlah = akPV.Jumlah;
            akPVView.TarikhPosting = akPV.TarikhPosting;
            akPVView.IsAKB = akPV.IsAKB;
            akPVView.SebabHapus = akPV.SebabHapus;

            switch (akPV.FlKategoriPenerima)
            {
                //pembekal
                case KategoriPenerima.Pembekal:
                    akPVView.KodPenerima = akPV.AkPembekal.KodSykt;
                    akPVView.NoKP = akPV.AkPembekal.KodSykt ?? "-";
                    akPVView.Penerima = akPV.AkPembekal.NamaSykt;
                    akPVView.Alamat1 = akPV.AkPembekal.Alamat1;
                    akPVView.Alamat2 = akPV.AkPembekal.Alamat2;
                    akPVView.Alamat3 = akPV.AkPembekal.Alamat3;
                    akPVView.NoAkaunBank = akPV.AkPembekal.AkaunBank;
                    akPVView.Telefon = akPV.AkPembekal.Telefon1;
                    akPVView.Emel = akPV.AkPembekal.Emel;
                    break;
                //pekerja
                case KategoriPenerima.Pekerja:
                    akPVView.KodPenerima = akPV.SuPekerja.NoGaji;
                    akPVView.NoKP = akPV.SuPekerja?.NoKp ?? "-";
                    akPVView.Penerima = akPV.SuPekerja.Nama;
                    akPVView.Alamat1 = akPV.SuPekerja.Alamat1;
                    akPVView.Alamat2 = akPV.SuPekerja.Alamat2;
                    akPVView.Alamat3 = akPV.SuPekerja.Alamat3;
                    akPVView.NoAkaunBank = akPV.SuPekerja.NoAkaunBank;
                    akPVView.Telefon = akPV.SuPekerja.TelefonBimbit;
                    akPVView.Emel = akPV.SuPekerja.Emel;
                    akPVView.SpPendahuluanPelbagai = akPV.SpPendahuluanPelbagai;
                    break;
                // panjar
                case KategoriPenerima.PemegangPanjar:
                    akPVView.AkTunaiRuncit = akPV.AkTunaiRuncit;
                    akPVView.KodPenerima = akPV.AkTunaiRuncit.KaunterPanjar;
                    akPVView.NoKP = akPV.NoKP;
                    akPVView.Penerima = akPV.Nama;
                    akPVView.Alamat1 = akPV.Alamat1;
                    akPVView.Alamat2 = akPV.Alamat2;
                    akPVView.Alamat3 = akPV.Alamat3;
                    akPVView.NoAkaunBank = akPV.NoAkaunBank;
                    akPVView.Telefon = akPV.Telefon;
                    akPVView.Emel = akPV.Emel;
                    akPVView.NoRekup = akPV.NoRekup;
                    break;
                //Am
                default:
                    akPVView.denganTanggungan = akPV.denganTanggungan;
                    akPVView.KodPenerima = "-";
                    akPVView.NoKP = akPV.NoKP ?? "-";
                    akPVView.Penerima = akPV.Nama;
                    akPVView.Alamat1 = akPV.Alamat1;
                    akPVView.Alamat2 = akPV.Alamat2;
                    akPVView.Alamat3 = akPV.Alamat3;
                    akPVView.NoAkaunBank = akPV.NoAkaunBank;
                    akPVView.Telefon = akPV.Telefon;
                    akPVView.Emel = akPV.Emel;

                    break;
            }

            akPVView.NoCekAtauEFT = akPV.NoCekAtauEFT;
            akPVView.TarCekAtauEFT = akPV.TarCekAtauEFT;
            akPVView.Perihal = akPV.Perihal;
            akPVView.CaraBayar = akPV.JCaraBayar?.Perihal ?? "PELBAGAI";
            akPVView.BankPenerima = akPV.JBank?.Nama ?? "PELBAGAI";
            akPVView.FlPosting = akPV.FlPosting;
            akPVView.FlCetak = akPV.FlCetak;
            akPVView.FlHapus = akPV.FlHapus;
            akPVView.FlKategoriPenerima = akPV.FlKategoriPenerima;
            akPVView.FlJenisBaucer = akPV.FlJenisBaucer;
            akPVView.AkTunaiRuncitId = akPV.AkTunaiRuncitId;
            akPVView.SpPendahuluanPelbagaiId = akPV.SpPendahuluanPelbagaiId;
            akPVView.SpPendahuluanPelbagai = akPV.SpPendahuluanPelbagai;
            akPVView.SuProfilId = akPV.SuProfilId;
            akPVView.SuProfil = akPV.SuProfil;

            akPVView.AkPV1 = akPV.AkPV1;
            foreach (AkPV2 item in akPV.AkPV2)
            {
                akPVView.JumlahInbois += item.Amaun;
            }
            akPVView.AkPV2 = akPV.AkPV2;

            foreach (AkPVGanda item in akPV.AkPVGanda)
            {
                akPVView.JumlahGanda += item.Amaun;
            }
            akPVView.AkPVGanda = akPV.AkPVGanda.OrderBy(b => b.Nama).ToList();

            PopulateTable(id);
            PopulateList();
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
                .Include(b => b.AkBelian).ThenInclude(b => b.AkPO)
                .Where(b => b.AkPVId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPV2 = akPV2Table;

            List<AkPVGanda> akPVGandaTable = _context.AkPVGanda
                .Include(b => b.SuAtlet)
                .Include(b => b.SuJurulatih)
                .Include(b => b.SuPekerja)
                .Include(b => b.JCaraBayar)
                .Include(b => b.JBank)
                .Where(b => b.AkPVId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPVGanda = akPVGandaTable;
        }

        private string GetNoRujukan(int data, string year, string month)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data);

            var kumpulanWang = kw.Kod;

            string prefix = "PV/" + kumpulanWang + "-" + year + "/" + month + "/";
            int x = 1;
            string noRujukan = prefix + "000";
            int tahun = int.Parse("20" + year);
            int bulan = int.Parse(month);

            var LatestNoRujukan = _context.AkPV
                       .IgnoreQueryFilters()
                       .Where(x => x.Tarikh.Year ==  tahun && x.Tarikh.Month == bulan && x.JKW.Kod == kw.Kod)
                       .Max(x => x.NoPV);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(13));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            return noRujukan;
        }

        // GET: AkPV/Create
        [Authorize(Policy = "PV001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.Year.ToString();
            var data = 1;
            var month = DateTime.Now.Month.ToString();

            ViewBag.NoRujukan = GetNoRujukan(data, year, month);
            // get latest no rujukan running number end

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
                _cart.ClearGanda();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // on change pendahuluan
        [HttpPost]
        public async Task<JsonResult> JsonGetPendahuluan(int data, int AkPVId)
        {
            try
            {
                CartEmpty();
                var result = await _spPPRepo.GetById(data);

                _cart.AddItem1(AkPVId,
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

        // on change Profil
        [HttpPost]
        public async Task<JsonResult> JsonGetProfil(int data, int AkPVId)
        {
            try
            {
                CartEmpty();
                var result = await _suProfilRepo.GetById(data);

                _cart.AddItem1(AkPVId,
                               result.Jumlah,
                               result.AkCartaId);

                var indek = 0;
                var nama = "";
                var noKP = "";
                var noAkaun = "";
                KategoriPenerima kategoriPenerima = KategoriPenerima.Am; // null / 0
                int? bankId = 0;
                int? caraBayarId = 0;
                JBank jBank = new JBank();
                JCaraBayar jCaraBayar = new JCaraBayar();
                decimal jumGanda = 0;

                foreach (var i in result.SuProfil1)
                {
                    // atlet
                    if (result.FlKategori == 0)
                    {
                        nama = i.SuAtlet.Nama;
                        noKP = i.SuAtlet.NoKp;
                        noAkaun = i.SuAtlet.NoAkaunBank;
                        bankId = i.SuAtlet.JBankId;
                        jBank = i.SuAtlet.JBank;
                        caraBayarId = i.SuAtlet.JCaraBayarId;
                        jCaraBayar = i.SuAtlet.JCaraBayar;
                        kategoriPenerima = KategoriPenerima.Atlet; // refer pada kategori penerima table AkPVGanda
                    }
                    // jurulatih
                    else
                    {
                        nama = i.SuJurulatih.Nama;
                        noKP = i.SuJurulatih.NoKp;
                        noAkaun = i.SuJurulatih.NoAkaunBank;
                        bankId = i.SuJurulatih.JBankId;
                        jBank = i.SuJurulatih.JBank;
                        caraBayarId = i.SuJurulatih.JCaraBayarId;
                        jCaraBayar = i.SuJurulatih.JCaraBayar;
                        kategoriPenerima = KategoriPenerima.Jurulatih; // refer pada kategori penerima table AkPVGanda

                    }

                    jCaraBayar.SuProfil1 = null;

                    indek++;
                    jumGanda = jumGanda + i.Jumlah;

                    _cart.AddItemGanda(AkPVId,
                                    indek,
                                    kategoriPenerima,
                                    null,
                                    i.SuAtletId,
                                    i.SuJurulatihId,
                                    nama,
                                    noKP,
                                    noAkaun,
                                    bankId,
                                    jBank,
                                    i.Jumlah,
                                    "",
                                    null,
                                    caraBayarId,
                                    jCaraBayar);
                }

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change pendahuluan end


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
        public async Task<JsonResult> JsonGetInboisPembekal(int data, int jenisBaucer)
        {
            try
            {
                if (jenisBaucer == 0)
                {
                    return Json(new { result = "OK" });
                }
                var result = await _context.AkBelian.Include(b => b.AkPembekal).Where(x => x.AkPembekalId == data).ToListAsync();

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
        public async Task<JsonResult> JsonGetAkBelian(int data)
        {
            try
            {
                var result = await _akBelianRepo.GetById(data);

                var akPOLaras = _context.AkPOLaras
                    .Include(x => x.AkPOLaras1)
                    .Where(x => x.AkPOId == result.AkPOId && x.FlPosting == 1).FirstOrDefault();

                if (akPOLaras != null)
                {
                    result.AkPO.Jumlah += akPOLaras.Jumlah;
                }

                // if akBelian link with debitKreditBelian
                var akNota = _context.AkNotaDebitKreditBelian
                    .Where(b => b.AkBelianId == data).FirstOrDefault();

                if (akNota != null)
                {
                    // debit
                    if (akNota.FlJenis == 0)
                    {
                        result.Jumlah += akNota.Jumlah;
                        result.AkPO.Jumlah += akNota.Jumlah;
                    }
                    else
                    {
                        result.Jumlah -= akNota.Jumlah;
                        result.AkPO.Jumlah -= akNota.Jumlah;
                    }
                }
                // endif

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        //on change inbois controller end

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

        // on change kod Pekerja controller
        [HttpPost]
        public async Task<JsonResult> JsonGetAtlet(int data)
        {
            try
            {
                var result = await _suAtletRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod Pekerja controller end

        // on change kod Pekerja controller
        [HttpPost]
        public async Task<JsonResult> JsonGetJurulatih(int data)
        {
            try
            {
                var result = await _suJurulatihRepo.GetById(data);

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        //on change kod Pekerja controller end

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

        [Authorize(Policy = "PV001C")]
        public IActionResult CreateByJenis(string jenis)
        {
            // get latest no rujukan running number  
            var year = DateTime.Now.ToString("yy");
            var month = DateTime.Now.ToString("MM");
            var data = 1;

            ViewBag.NoRujukan = GetNoRujukan(data, year, month);
            // get latest no rujukan running number end

            PopulateList();
            CartEmpty();
            return View(jenis);
        }
        // POST: AkPV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PV001C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByJenis(
            AkPV akPV,
            int JKWId,
            int? AkPembekalId,
            int? SuPekerjaId,
            int AkBankId,
            int? JBankId,
            int JCaraBayarId,
            decimal JumlahInbois,
            int? AkTunaiRuncitId,
            int? SpPendahuluanPelbagaiId,
            int? SuProfilId,
            int JBahagianId,
            int FlJenisBaucer,
            bool IsAKB)
        {
            // note:
            // FlJenisBaucer = 0 ( Am )
            // FlJenisBaucer = 1 ( Inbois )
            // FlJenisBaucer = 2 ( Gaji )
            // FlJenisBaucer = 3 ( Pendahuluan )
            // FlJenisBaucer = 4 ( Rekupan )
            // FlJenisBaucer = 5 ( Tambah Had Panjar )
            // FlJenisBaucer = 6 ( Profil Atlet / Jurulatih )
            // ..
            // FlKategoriPenerima = 0 ( Am / Lain - lain )
            // FlKategoriPenerima = 1 ( pembekal )
            // FlKategoriPenerima = 2 ( pekerja )
            // FlKategoriPenerima = 3 ( pemegang panjar )

            // FlKategoriPenerima = 4 ( jurulatih )
            // FlKategoriPenerima = 5 ( atlet )
            // ..


            AkPV m = new AkPV();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var pembekal = await _context.AkPembekal.FirstOrDefaultAsync(x => x.Id == AkPembekalId);
            var pekerja = await _context.SuPekerja.FirstOrDefaultAsync(x => x.Id == SuPekerjaId);
            var tunaiRuncit = await _context.AkTunaiRuncit
                .Include(x => x.AkTunaiPemegang).ThenInclude(x => x.SuPekerja)
                .FirstOrDefaultAsync(x => x.Id == AkTunaiRuncitId);
            var spPendahuluan = await _context.SpPendahuluanPelbagai.FirstOrDefaultAsync(x => x.Id == SpPendahuluanPelbagaiId);

            var suProfil = await _context.SuProfil.FirstOrDefaultAsync(x => x.Id == SuProfilId);

            var jenis = "CreateAm";
            //check if user fil in both pekerja and pembekal
            //if (pembekal != null && pekerja != null)
            //{
            //    TempData[SD.Error] = "Maklumat gagal disimpan. Sila isi salah satu kod pekerja atau kod pembekal";
            //    //PopulateCart();
            //    CartEmpty();
            //    PopulateList();
            //    return View(akPV);
            //}            

            if (tunaiRuncit != null)
            {
                akPV.FlKategoriPenerima = KategoriPenerima.PemegangPanjar;
                akPV.AkTunaiRuncitId = AkTunaiRuncitId;
                jenis = "CreatePanjar";
            }

            if (FlJenisBaucer == 1)
            {
                if (pembekal != null)
                {
                    akPV.Nama = pembekal.NamaSykt;
                    akPV.Alamat1 = pembekal.Alamat1;
                    akPV.Alamat2 = pembekal.Alamat2;
                    akPV.Alamat3 = pembekal.Alamat3;
                    akPV.Telefon = pembekal.Telefon1;
                    akPV.Emel = pembekal.Emel;
                    akPV.NoAkaunBank = pembekal.AkaunBank;
                    akPV.FlJenisBaucer = JenisBaucer.Inbois;
                    akPV.FlKategoriPenerima = KategoriPenerima.Pembekal;
                    jenis = "CreateAm";

                    //check if PV dengan tanggungan or tanpa tanggungan
                    List<AkPV2> akPV2CartList = _cart.Lines2.ToList();

                    foreach (AkPV2 item in akPV2CartList)
                    {
                        AkBelian akBelian = _context.AkBelian.FirstOrDefault(b => b.Id == item.AkBelianId);

                        if (akBelian.FlTanggungan == "1")
                        {
                            item.HavePO = true;
                        }
                    }
                    //check if PV dengan tanggungan or tanpa tanggungan end
                }
            }

            if (pekerja != null)
            {

                akPV.FlKategoriPenerima = KategoriPenerima.Pekerja;
                jenis = "CreatePekerja";
            }

            if (FlJenisBaucer == 2)
            {
                akPV.FlKategoriPenerima = KategoriPenerima.Pekerja;
                jenis = "CreatePekerja";
            }

            if (FlJenisBaucer == 0 && pembekal != null)
            {
                akPV.FlKategoriPenerima = KategoriPenerima.Pembekal;
                jenis = "CreateAm";
            }
            // get latest no rujukan running number  
            var kw = _context.JKW.FirstOrDefault(x => x.Id == akPV.JKWId);

            var year = akPV.Tarikh.ToString("yy");
            var month = akPV.Tarikh.ToString("MM");

            var noRujukan = GetNoRujukan(akPV.JKWId, year, month);

            // get latest no rujukan running number end


            if (ModelState.IsValid)
            {
                if (akPV != null && JKWId != 0 && AkBankId != 0 && akPV.Nama != null && JBahagianId != 0)
                {
                    m.AkBankId = AkBankId;
                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;

                    if (FlJenisBaucer == 1)
                    {
                        if (AkPembekalId != null)
                        {
                            if (AkPembekalId != 0)
                            {
                                m.AkPembekalId = AkPembekalId;

                                foreach (var i in _cart.Lines2.ToList())
                                {
                                    if (i.HavePO == true)
                                    {
                                        akPV.denganTanggungan = true;
                                    }
                                }
                                // checking for jumlah objek & jumlah perihal
                                if (akPV.Jumlah != JumlahInbois)
                                {
                                    TempData[SD.Error] = "Maklumat gagal disimpan. Jumlah Objek tidak sama dengan jumlah Inbois";
                                    //PopulateCart();
                                    CartEmpty();
                                    PopulateList();
                                    return View(jenis, akPV);
                                }
                            }
                        }
                    }


                    m.SuPekerjaId = SuPekerjaId == 0 ? null : SuPekerjaId;

                    m.AkPembekalId = AkPembekalId == 0 ? null : AkPembekalId;
                    m.Tahun = akPV.Tahun;
                    m.NoPV = noRujukan;
                    m.Tarikh = akPV.Tarikh;
                    m.NoKP = akPV.NoKP;
                    m.Nama = akPV.Nama?.ToUpper() ?? null;
                    m.Alamat1 = akPV.Alamat1?.ToUpper() ?? null;
                    m.Alamat2 = akPV.Alamat2?.ToUpper() ?? null;
                    m.Alamat3 = akPV.Alamat3?.ToUpper() ?? null;
                    m.NoAkaunBank = akPV.NoAkaunBank;
                    m.Telefon = akPV.Telefon;
                    m.Emel = akPV.Emel;

                    m.JCaraBayarId = JCaraBayarId;
                    m.NoCekAtauEFT = akPV.NoCekAtauEFT;
                    m.TarCekAtauEFT = akPV.TarCekAtauEFT;
                    m.Perihal = akPV.Perihal?.ToUpper() ?? null;
                    m.Jumlah = akPV.Jumlah;
                    m.FlPosting = 0;
                    m.FlHapus = 0;
                    m.FlCetak = 0;
                    m.FlKategoriPenerima = akPV.FlKategoriPenerima;
                    m.FlJenisBaucer = akPV.FlJenisBaucer;
                    m.NoRekup = akPV.NoRekup;
                    m.denganTanggungan = akPV.denganTanggungan;
                    m.IsGanda = akPV.IsGanda;
                    m.IsAKB = IsAKB;
                    m.JBankId = JBankId;

                    if (tunaiRuncit != null)
                    {
                        m.AkTunaiRuncitId = AkTunaiRuncitId;
                    }
                    if (spPendahuluan != null)
                    {
                        m.SpPendahuluanPelbagaiId = SpPendahuluanPelbagaiId;
                    }
                    if (suProfil != null)
                    {
                        if (suProfil.FlKategori == 1)
                        {
                            m.FlKategoriPenerima = KategoriPenerima.Jurulatih;
                        }

                        if (suProfil.FlKategori == 0)
                        {
                            m.FlKategoriPenerima = KategoriPenerima.Atlet;
                        }
                        m.SuProfilId = SuProfilId;
                    }

                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AkPV1 = _cart.Lines1.ToArray();
                    m.AkPV2 = _cart.Lines2.ToArray();

                    if (akPV.IsGanda == true)
                    {
                        m.JCaraBayarId = null;
                        m.JBankId = null;
                    }

                    decimal ganda = 0;
                    foreach (var item in _cart.LinesGanda)
                    {
                        ganda = ganda + item.Amaun;
                        item.JCaraBayar = null;
                        item.JBank = null;
                    }

                    m.AkPVGanda = _cart.LinesGanda.ToArray();

                    // check for baki peruntukan
                    // if jenis baucer is Am / pembekal && its not pembekal,
                    // jenis baucer is Am / pembekal && its pembekal that have tanggungan(PO) ,
                    // jenis baucer is gaji / pekerja && it do not have pendahuluan pelbagai
                    // note :
                    // FlJenisBaucer = 0 ( Am )
                    // FlJenisBaucer = 1 ( Inbois )
                    // FlJenisBaucer = 2 ( Gaji )
                    // FlJenisBaucer = 3 ( Pendahuluan )
                    // FlJenisBaucer = 4 ( Panjar )
                    // ..
                    // FlKategoriPenerima = 0 ( Am / Lain - lain )
                    // FlKategoriPenerima = 1 ( pembekal )
                    // FlKategoriPenerima = 2 ( pekerja )
                    // FlKategoriPenerima = 3 ( pemegang panjar )
                    // FlKategoriPenerima = 4 ( jurulatih )
                    // FlKategoriPenerima = 5 ( atlet )
                    // ..

                    // check if akaun bank is in bajet or not
                    var akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);

                    if (akBank == null)
                    {
                        TempData[SD.Error] = "No Akaun Bank tidak diisi.";
                        PopulateList();
                        CartEmpty();
                        return View(jenis, akPV);
                    }
                    else
                    {
                        //if akaun bank is in bajet, then check peruntukan
                        if (akBank.IsBajet == true)
                        {
                            if (IsAKB == true)
                            {
                                var AppInfo = _context.SiAppInfo.Where(b => b.TarMula.Year <= DateTime.Now.Year).FirstOrDefault();

                                if (AppInfo == null)
                                {
                                    if ((m.FlJenisBaucer == 0 && m.FlKategoriPenerima == 0)
                                                            || (m.FlKategoriPenerima == KategoriPenerima.Pembekal && m.denganTanggungan == false)
                                                            || (m.FlJenisBaucer == JenisBaucer.Gaji) || (m.FlJenisBaucer == JenisBaucer.ProfilAtletJurulatih))
                                    {
                                        foreach (AkPV1 item in m.AkPV1)
                                        {

                                            // check 
                                            var CartaDgnPeruntukan = await _context.AkCarta
                                            .Where(d => d.Id == item.AkCartaId && d.IsBajet == true)
                                            .FirstOrDefaultAsync();

                                            if (CartaDgnPeruntukan != null)
                                            {
                                                bool IsExistAbBukuVot = await _context.AbBukuVot
                                                .Where(x => x.Tahun == m.Tahun && x.VotId == item.AkCartaId && x.JKWId == m.JKWId && x.JBahagianId == m.JBahagianId)
                                                .AnyAsync();

                                                var carta = _context.AkCarta.Find(item.AkCartaId);

                                                if (IsExistAbBukuVot == true)
                                                {
                                                    decimal sum = await _customRepo.GetBalanceFromAbBukuVot(m.Tahun, item.AkCartaId, m.JKWId, m.JBahagianId);

                                                    if (sum < item.Amaun)
                                                    {
                                                        TempData[SD.Error] = "Bajet untuk kod akaun " + carta.Kod + " tidak mencukupi.";
                                                        PopulateList();
                                                        CartEmpty();

                                                        return View(jenis, akPV);
                                                    }
                                                }
                                                else
                                                {
                                                    TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                                                    PopulateList();
                                                    CartEmpty();

                                                    return View(jenis, akPV);
                                                }
                                            }

                                        }

                                    }
                                }
                            }
                            else
                            {

                                if ((m.FlJenisBaucer == 0 && m.FlKategoriPenerima == 0)
                                                                                    || (m.FlKategoriPenerima == KategoriPenerima.Pembekal && m.denganTanggungan == false)
                                                                                    || (m.FlJenisBaucer == JenisBaucer.Gaji) || (m.FlJenisBaucer == JenisBaucer.ProfilAtletJurulatih))
                                {
                                    foreach (AkPV1 item in m.AkPV1)
                                    {

                                        // check 
                                        var CartaDgnPeruntukan = await _context.AkCarta
                                        .Where(d => d.Id == item.AkCartaId && d.IsBajet == true)
                                        .FirstOrDefaultAsync();

                                        if (CartaDgnPeruntukan != null)
                                        {
                                            bool IsExistAbBukuVot = await _context.AbBukuVot
                                            .Where(x => x.Tahun == m.Tahun && x.VotId == item.AkCartaId && x.JKWId == m.JKWId && x.JBahagianId == m.JBahagianId)
                                            .AnyAsync();

                                            var carta = _context.AkCarta.Find(item.AkCartaId);

                                            if (IsExistAbBukuVot == true)
                                            {
                                                decimal sum = await _customRepo.GetBalanceFromAbBukuVot(m.Tahun, item.AkCartaId, m.JKWId, m.JBahagianId);

                                                if (sum < item.Amaun)
                                                {
                                                    TempData[SD.Error] = "Bajet untuk kod akaun " + carta.Kod + " tidak mencukupi.";
                                                    PopulateList();
                                                    CartEmpty();

                                                    return View(jenis, akPV);
                                                }
                                            }
                                            else
                                            {
                                                TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + carta.Kod;
                                                PopulateList();
                                                CartEmpty();

                                                return View(jenis, akPV);
                                            }
                                        }

                                    }

                                }

                            }

                        }
                    }

                    // check for baki peruntukan end

                    await _akPVRepo.Insert(m);

                    //insert applog

                    //insert applog
                    await AddLogAsync("Tambah", m.NoPV, m.NoPV, 0, m.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akPV.NoPV;
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            CartEmpty();
            return View(jenis, akPV);
        }

        // GET: AkPV/Edit/5
        [Authorize(Policy = "PV001E")]
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
            AkPVViewModel akPVView = new AkPVViewModel();

            //fill in view model AkPVViewModel from akPV
            akPVView.AkPembekalId = akPV.AkPembekalId;
            akPVView.SuPekerjaId = akPV.SuPekerjaId;
            akPVView.Id = akPV.Id;
            akPVView.Tahun = akPV.Tahun;
            akPVView.NoPV = akPV.NoPV;
            akPVView.Tarikh = akPV.Tarikh;
            akPVView.JKWId = akPV.JKWId;
            akPVView.JKW = akPV.JKW;
            akPVView.JBahagianId = akPV.JBahagianId;
            akPVView.JBahagian = akPV.JBahagian;
            akPVView.AkBankId = akPV.AkBankId;
            akPVView.AkBank = akPV.AkBank;
            akPVView.Jumlah = akPV.Jumlah;
            akPVView.TarikhPosting = akPV.TarikhPosting;
            akPVView.JCaraBayarId = akPV.JCaraBayarId;
            akPVView.AkTunaiRuncitId = akPV.AkTunaiRuncitId;
            akPVView.NoRekup = akPV.NoRekup;
            akPVView.IsAKB = akPV.IsAKB;
            akPVView.JBankId = akPV.JBankId;
            akPVView.FlJenisBaucer = akPV.FlJenisBaucer;

            switch (akPV.FlKategoriPenerima)
            {
                //pembekal
                case KategoriPenerima.Pembekal:
                    akPVView.KodPenerima = akPV.AkPembekal.KodSykt;
                    akPVView.NoKP = "-";
                    akPVView.Nama = akPV.AkPembekal.NamaSykt;
                    akPVView.Alamat1 = akPV.AkPembekal.Alamat1;
                    akPVView.Alamat2 = akPV.AkPembekal.Alamat2;
                    akPVView.Alamat3 = akPV.AkPembekal.Alamat3;
                    akPVView.NoAkaunBank = akPV.AkPembekal.AkaunBank;
                    akPVView.Telefon = akPV.AkPembekal.Telefon1;
                    akPVView.Emel = akPV.AkPembekal.Emel;
                    break;
                //pekerja
                case KategoriPenerima.Pekerja:
                    akPVView.KodPenerima = akPV.SuPekerja.NoGaji;
                    akPVView.NoKP = akPV.SuPekerja.NoKp;
                    akPVView.Nama = akPV.SuPekerja.Nama;
                    akPVView.Alamat1 = akPV.SuPekerja.Alamat1;
                    akPVView.Alamat2 = akPV.SuPekerja.Alamat2;
                    akPVView.Alamat3 = akPV.SuPekerja.Alamat3;
                    akPVView.NoAkaunBank = akPV.SuPekerja.NoAkaunBank;
                    akPVView.Telefon = akPV.SuPekerja.TelefonBimbit;
                    akPVView.Emel = akPV.SuPekerja.Emel;
                    break;
                //Am
                default:
                    akPVView.denganTanggungan = akPV.denganTanggungan;
                    akPVView.KodPenerima = "-";
                    akPVView.NoKP = akPV.NoKP;
                    akPVView.Nama = akPV.Nama;
                    akPVView.Alamat1 = akPV.Alamat1;
                    akPVView.Alamat2 = akPV.Alamat2;
                    akPVView.Alamat3 = akPV.Alamat3;
                    akPVView.NoAkaunBank = akPV.NoAkaunBank;
                    akPVView.Telefon = akPV.Telefon;
                    akPVView.Emel = akPV.Emel;
                    break;
            }

            akPVView.NoCekAtauEFT = akPV.NoCekAtauEFT;
            akPVView.TarCekAtauEFT = akPV.TarCekAtauEFT;
            akPVView.Perihal = akPV.Perihal;
            akPVView.CaraBayar = akPV.JCaraBayar?.Perihal ?? "PELBAGAI";
            akPVView.FlPosting = akPV.FlPosting;
            akPVView.FlCetak = akPV.FlCetak;
            akPVView.FlHapus = akPV.FlHapus;
            akPVView.FlKategoriPenerima = akPV.FlKategoriPenerima;
            akPVView.FlJenisBaucer = akPV.FlJenisBaucer;
            akPVView.IsGanda = akPV.IsGanda;

            akPVView.AkPV1 = akPV.AkPV1;
            foreach (AkPV2 item in akPV.AkPV2)
            {
                akPVView.JumlahInbois += item.Amaun;
            }
            akPVView.AkPV2 = akPV.AkPV2;

            foreach (AkPVGanda item in akPV.AkPVGanda)
            {
                akPVView.JumlahGanda += item.Amaun;
            }

            akPVView.AkPVGanda = akPV.AkPVGanda.OrderBy(b => b.Nama).ToList();

            CartEmpty();
            PopulateTable(id);
            PopulateList();
            PopulateCartFromDb(akPV);
            return View(akPVView);
        }



        // POST: AkPV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PV001E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            AkPVViewModel akPV,
            int JKWId,
            string Penerima,
            int AkBankId,
            int JCaraBayarId,
            decimal JumlahInbois,
            int JBahagianId,
            bool IsAKB)
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
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                    var dataAsal = await _akPVRepo.GetById(id);
                    var jumlah = dataAsal.Jumlah;

                    switch (akPV.FlKategoriPenerima)
                    {
                        case KategoriPenerima.Pembekal:
                            var pembekal = dataAsal.AkPembekal;
                            akPV.SuPekerjaId = null;
                            akPV.Nama = pembekal.NamaSykt;
                            akPV.Alamat1 = pembekal.Alamat1;
                            akPV.Alamat2 = pembekal.Alamat2;
                            akPV.Alamat3 = pembekal.Alamat3;
                            akPV.Emel = pembekal.Emel;
                            akPV.Telefon = pembekal.Telefon1;
                            akPV.NoAkaunBank = pembekal.AkaunBank;
                            break;
                        case KategoriPenerima.Pekerja:
                            var pekerja = dataAsal.SuPekerja;
                            akPV.AkPembekalId = null;
                            akPV.Nama = pekerja.Nama;
                            akPV.Alamat1 = pekerja.Alamat1;
                            akPV.Alamat2 = pekerja.Alamat2;
                            akPV.Alamat3 = pekerja.Alamat3;
                            akPV.Emel = pekerja.Emel;
                            akPV.Telefon = pekerja.TelefonBimbit;
                            akPV.NoAkaunBank = pekerja.NoAkaunBank;
                            break;
                        default:
                            akPV.Nama = dataAsal.Nama;
                            akPV.AkPembekalId = null;
                            akPV.SuPekerjaId = null;
                            break;
                    }

                    // list of input that cannot be change
                    akPV.Tahun = dataAsal.Tahun;
                    akPV.Tarikh = dataAsal.Tarikh;
                    akPV.JKWId = dataAsal.JKWId;
                    akPV.JBahagianId = dataAsal.JBahagianId;
                    akPV.NoPV = dataAsal.NoPV;
                    akPV.SuPekerjaId = dataAsal.SuPekerjaId;
                    akPV.AkPembekalId = dataAsal.AkPembekalId;
                    akPV.FlJenisBaucer = dataAsal.FlJenisBaucer;
                    akPV.FlKategoriPenerima = dataAsal.FlKategoriPenerima;
                    akPV.AkTunaiRuncitId = dataAsal.AkTunaiRuncitId;
                    akPV.SpPendahuluanPelbagaiId = dataAsal.SpPendahuluanPelbagaiId;
                    akPV.SuProfilId = dataAsal.SuProfilId;
                    akPV.NoRekup = dataAsal.NoRekup;
                    akPV.TarMasuk = dataAsal.TarMasuk;
                    akPV.UserId = dataAsal.UserId;
                    akPV.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    akPV.FlCetak = 0;
                    akPV.IsAKB = dataAsal.IsAKB;
                    akPV.IsGanda = dataAsal.IsGanda;
                    // list of input that cannot be change end

                    foreach (AkPV1 item in dataAsal.AkPV1)
                    {
                        var model = _context.AkPV1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    foreach (AkPV2 item in dataAsal.AkPV2)
                    {
                        var model = _context.AkPV2.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    foreach (AkPVGanda item in dataAsal.AkPVGanda)
                    {
                        var model = _context.AkPVGanda.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    var jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akPV.AkPV1 = _cart.Lines1.ToList();
                    akPV.AkPV2 = _cart.Lines2.ToList();
                    akPV.AkPVGanda = _cart.LinesGanda.ToList();

                    akPV.TarSemak = null;
                    akPV.JPenyemakId = null;
                    akPV.FlStatusSemak = 0;

                    akPV.TarLulus = null;
                    akPV.JPelulusId = null;
                    akPV.FlStatusLulus = 0;

                    akPV.UserIdKemaskini = user.UserName;
                    akPV.TarKemaskini = DateTime.Now;
                    akPV.SuPekerjaKemaskiniId = pekerjaId;
                    akPV.Perihal = akPV.Perihal?.ToUpper() ?? null;
                    _context.Update(akPV);

                    //insert applog
                    if (jumlahAsal != akPV.Jumlah)
                    {
                        await AddLogAsync("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(akPV.Jumlah).ToString("#,##0.00"), akPV.NoPV, id, akPV.Jumlah, pekerjaId);

                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", akPV.NoPV, id, akPV.Jumlah, pekerjaId);
                    }
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
                // checking for jumlah objek & jumlah inbois (untuk pembekal)
                if (akPV.FlJenisBaucer == JenisBaucer.Inbois)
                {
                    if (akPV.Jumlah != JumlahInbois)
                    {
                        TempData[SD.Warning] = "Jumlah Objek tidak sama dengan Jumlah Inbois";
                        PopulateList();
                        PopulateTable(id);
                        PopulateCartFromDb(akPV);
                        return View(akPV);
                    }
                    else
                    {
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }

                    return RedirectToAction(nameof(Index));
                }

                TempData[SD.Success] = "Data berjaya diubah..!";

                return RedirectToAction(nameof(Index));
            }
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akPV);
            return View(akPV);
        }

        // GET: AkPV/Delete/5
        [Authorize(Policy = "PV001D")]
        public async Task<IActionResult> Delete(int? id)
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
            akPVView.AkPembekalId = akPV.AkPembekalId;
            akPVView.SuPekerjaId = akPV.SuPekerjaId;
            akPVView.Id = akPV.Id;
            akPVView.Tahun = akPV.Tahun;
            akPVView.NoPV = akPV.NoPV;
            akPVView.Tarikh = akPV.Tarikh;
            akPVView.JKW = akPV.JKW;
            akPVView.AkBank = akPV.AkBank;
            akPVView.Jumlah = akPV.Jumlah;
            akPVView.TarikhPosting = akPV.TarikhPosting;
            akPVView.JCaraBayarId = akPV.JCaraBayarId;
            akPVView.AkBankId = akPV.AkBankId;
            akPVView.JKWId = akPV.JKWId;
            akPVView.JBahagianId = akPV.JBahagianId;
            akPVView.JBahagian = akPV.JBahagian;
            akPVView.IsAKB = akPV.IsAKB;

            switch (akPV.FlKategoriPenerima)
            {
                //pembekal
                case KategoriPenerima.Pembekal:
                    akPVView.KodPenerima = akPV.AkPembekal.KodSykt;
                    akPVView.NoKP = "-";
                    akPVView.Nama = akPV.AkPembekal.NamaSykt;
                    akPVView.Alamat1 = akPV.AkPembekal.Alamat1;
                    akPVView.Alamat2 = akPV.AkPembekal.Alamat2;
                    akPVView.Alamat3 = akPV.AkPembekal.Alamat3;
                    akPVView.NoAkaunBank = akPV.AkPembekal.AkaunBank;
                    akPVView.Telefon = akPV.AkPembekal.Telefon1;
                    akPVView.Emel = akPV.AkPembekal.Emel;
                    break;
                //pekerja
                case KategoriPenerima.Pekerja:
                    akPVView.KodPenerima = akPV.SuPekerja.NoGaji;
                    akPVView.NoKP = akPV.SuPekerja.NoKp;
                    akPVView.Nama = akPV.SuPekerja.Nama;
                    akPVView.Alamat1 = akPV.SuPekerja.Alamat1;
                    akPVView.Alamat2 = akPV.SuPekerja.Alamat2;
                    akPVView.Alamat3 = akPV.SuPekerja.Alamat3;
                    akPVView.NoAkaunBank = akPV.SuPekerja.NoAkaunBank;
                    akPVView.Telefon = akPV.SuPekerja.TelefonBimbit;
                    akPVView.Emel = akPV.SuPekerja.Emel;
                    break;
                //Am
                default:
                    akPVView.denganTanggungan = akPV.denganTanggungan;
                    akPVView.KodPenerima = "-";
                    akPVView.NoKP = akPV.NoKP;
                    akPVView.Nama = akPV.Nama;
                    akPVView.Alamat1 = akPV.Alamat1;
                    akPVView.Alamat2 = akPV.Alamat2;
                    akPVView.Alamat3 = akPV.Alamat3;
                    akPVView.NoAkaunBank = akPV.NoAkaunBank;
                    akPVView.Telefon = akPV.Telefon;
                    akPVView.Emel = akPV.Emel;
                    break;
            }

            akPVView.NoCekAtauEFT = akPV.NoCekAtauEFT;
            akPVView.TarCekAtauEFT = akPV.TarCekAtauEFT;
            akPVView.Perihal = akPV.Perihal;
            akPVView.CaraBayar = akPV.JCaraBayar?.Perihal ?? "PELBAGAI";
            akPVView.BankPenerima = akPV.JBank?.Nama ?? "PELBAGAI";
            akPVView.FlPosting = akPV.FlPosting;
            akPVView.FlCetak = akPV.FlCetak;
            akPVView.FlHapus = akPV.FlHapus;
            akPVView.FlKategoriPenerima = akPV.FlKategoriPenerima;
            akPVView.FlJenisBaucer = akPV.FlJenisBaucer;
            akPVView.AkTunaiRuncitId = akPV.AkTunaiRuncitId;
            akPVView.SpPendahuluanPelbagaiId = akPV.SpPendahuluanPelbagaiId;
            akPVView.SpPendahuluanPelbagai = akPV.SpPendahuluanPelbagai;
            akPVView.SuProfilId = akPV.SuProfilId;
            akPVView.SuProfil = akPV.SuProfil;

            akPVView.AkPV1 = akPV.AkPV1;
            foreach (AkPV2 item in akPV.AkPV2)
            {
                akPVView.JumlahInbois += item.Amaun;
            }
            akPVView.AkPV2 = akPV.AkPV2;

            foreach (AkPVGanda item in akPV.AkPVGanda)
            {
                akPVView.JumlahGanda += item.Amaun;
            }
            akPVView.AkPVGanda = akPV.AkPVGanda;

            CartEmpty();
            PopulateTable(id);
            PopulateList();
            PopulateCartFromDb(akPV);
            return View(akPVView);
        }

        // POST: AkPV/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PV001D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string sebabHapus)
        {
            var akPV = await _context.AkPV.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
            akPV.UserIdKemaskini = user.UserName;
            akPV.TarKemaskini = DateTime.Now;
            akPV.SuPekerjaKemaskiniId = pekerjaId;

            akPV.SebabHapus = sebabHapus?.ToUpper() ?? "";

            // check if already posting redirect back
            if (akPV.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            akPV.FlCetak = 0;
            _context.AkPV.Update(akPV);

            _context.AkPV.Remove(akPV);

            //insert applog
            await AddLogAsync("Hapus", "Hapus Data : " + akPV.SebabHapus, akPV.NoPV, id, akPV.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkPVExists(int id)
        {
            return _context.AkPV.Any(e => e.Id == id);
        }

        [Authorize(Policy = "PV001P")]
        public async Task<IActionResult> PrintPdf(int id, int penyemakId, int pelulusId)
        {
            AkPV akPV = await _akPVRepo.GetByIdIncludeDeletedItems(id);

            PVPrintModel data = new PVPrintModel();
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            JBank bankPelbagai = new JBank()
            {
                Kod = "-",
                Nama = "-"
            };

            if (akPV.JBank == null)
            {
                akPV.JBank = bankPelbagai;
            }
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            var pekerja = _context.SuPekerja.FirstOrDefault(x => x.Id == namaUser.SuPekerjaId);
            var jawatan = "Super Admin";
            if (pekerja != null)
            {
                jawatan = pekerja.Jawatan;
            }
            var penyemak = await _penyemakRepo.GetById(penyemakId);
            var pelulus = await _pelulusRepo.GetById(pelulusId);

            string jumlahDalamPerkataan;

            if (akPV.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - akPV.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(akPV.Jumlah)).ToUpper();
            }

            var noAkaunBank = "";
            var namaBankPenerima = "";

            decimal jumlahInbois = 0;
            decimal jumlahPOInden = 0;

            CompanyDetails company = await _userService.GetCompanyDetails();
            data.Username = namaUser.Nama;
            data.Penyemak = penyemak;
            data.Pelulus = pelulus;
            data.AkPV = akPV;

            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.AkPV2 = akPV.AkPV2;
            data.IsAKB = akPV.IsAKB;

            switch (akPV.FlKategoriPenerima)
            {
                //pembekal
                case KategoriPenerima.Pembekal:
                    data.KodPenerima = akPV.AkPembekal.KodSykt;
                    namaBankPenerima = akPV.AkPembekal.JBank.Nama;
                    noAkaunBank = akPV.AkPembekal.AkaunBank;
                    data.Poskod = akPV.AkPembekal?.Poskod ?? "";

                    foreach (AkPV2 item in data.AkPV2)
                    {
                        jumlahInbois += item.Amaun;
                        if (item.AkBelian.AkPO != null)
                        {
                            jumlahPOInden += item.AkBelian.AkPO.Jumlah;
                        }

                        if (item.AkBelian.AkInden != null)
                        {
                            jumlahPOInden += item.AkBelian.AkInden.Jumlah;
                        }
                    }
                    data.jumlahInbois = jumlahInbois;
                    data.jumlahPOInden = jumlahPOInden;
                    break;
                //pekerja
                case KategoriPenerima.Pekerja:
                    var noGaji = akPV.SuPekerja == null ? "00000" : akPV.SuPekerja.NoGaji;
                    var noKP = akPV.SuPekerja == null ? "012345678901" : akPV.SuPekerja.NoKp;
                    var nama = akPV.SuPekerja == null ? "SuperAdmin" : akPV.SuPekerja.Nama;
                    var noAkaun = akPV.SuPekerja == null ? "019284719285" : akPV.SuPekerja.NoAkaunBank;
                    data.KodPenerima = noGaji + " - "+ noKP;
                    namaBankPenerima = akPV.SuPekerja == null ? "Testing Bank" : akPV.SuPekerja.JBank.Nama; ;
                    noAkaunBank = noAkaun;
                    data.Poskod = akPV.SuPekerja?.Poskod ?? "";

                    break;
                //am
                default:
                    data.KodPenerima = akPV.NoKP;
                    noAkaunBank = akPV.NoAkaunBank;
                    namaBankPenerima = akPV.JBank?.Nama ?? "PELBAGAI";
                    data.Poskod = "";
                    break;
            }

            data.denganTanggungan = akPV.denganTanggungan;
            data.FlKategoriPenerima = akPV.FlKategoriPenerima;
            data.Penerima = akPV.Nama;
            data.NoAkaunBankPenerima = noAkaunBank;
            data.NamaBankPenerima = namaBankPenerima;
            data.NoAkaunBank = akPV.AkBank.NoAkaun;
            data.NoKP = akPV.NoKP;
            data.CompanyDetail = company;

            if (akPV.TarCekAtauEFT != null)
            {
                data.TarikhCekAtauEFT = akPV.TarCekAtauEFT.ToString();
            }

            //update cetak -> 1
            akPV.FlCetak = 1;
            await _akPVRepo.Update(akPV);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", akPV.NoPV, id, akPV.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("PVPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // Semak function
        [HttpPost, ActionName("Semak")]
        [Authorize(Policy = "PV001T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Semak(int? id, int penyemakId, DateTime? tarikhSemak)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

                if (tarikhSemak == null)
                {
                    TempData[SD.Error] = "Tarikh Semak diperlukan.";
                    return RedirectToAction(nameof(Index));

                }

                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                var namaUser = _context.
                    applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.Email == user.UserName);
                //var pelulus = await _context.JPelulus.Include(x => x.SuPekerja).Where(x => x.IsPendahuluan == true).FirstOrDefaultAsync();
                //var sokong = Convert.ToDecimal(jumSokong);

                AkPV sp = await _akPVRepo.GetById((int)id);

                //check for print
                if (sp.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal disemak. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                //semak operation start here
                //update semak status
                sp.FlStatusSemak = 1;
                sp.TarSemak = tarikhSemak;

                sp.JPenyemakId = penyemakId;


                await _akPVRepo.Update(sp);

                //insert applog
                await AddLogAsync("Posting", "Semak Data", sp.NoPV, (int)id, sp.Jumlah, pekerjaId);

                //insert applog end

                await _context.SaveChangesAsync();

                TempData[SD.Success] = "Data berjaya disemak.";
            }

            return RedirectToAction(nameof(Index));

        }
        // Semak function end

        // posting function
        [Authorize(Policy = "PV001T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Posting(int? id, int pelulusId, DateTime? tarikhLulus)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                AkPV akPV = await _akPVRepo.GetById((int)id);

                var AppInfo = _context.SiAppInfo.Where(b => b.TarMula.Year <= DateTime.Now.Year).FirstOrDefault();

                //check for print
                if (akPV.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                if (tarikhLulus == null)
                {
                    TempData[SD.Error] = "Tarikh Lulus diperlukan.";
                    return RedirectToAction(nameof(Index));

                }

                List<AkPV1> akPV1 = akPV.AkPV1.ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akPV.NoPV).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan.";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //posting operation start here

                    var kod = "";
                    var penerima = "";
                    switch (akPV.FlKategoriPenerima)
                    {
                        //pembekal
                        case KategoriPenerima.Pembekal:
                            kod = akPV.AkPembekal.KodSykt;
                            penerima = akPV.AkPembekal.NamaSykt;
                            break;
                        //pekerja
                        case KategoriPenerima.Pekerja:
                            var noGaji = akPV.SuPekerja == null ? "00000" : akPV.SuPekerja.NoGaji;
                            var nama = akPV.SuPekerja == null ? "SuperAdmin" : akPV.SuPekerja.Nama;
                            kod = noGaji;
                            penerima = nama;

                            break;
                        //panjar
                        case KategoriPenerima.PemegangPanjar:
                            kod = akPV.NoKP;
                            penerima = akPV.Nama;
                            break;
                        //am
                        default:
                            kod = akPV.NoKP;
                            penerima = akPV.Nama;
                            break;
                    }


                    foreach (AkPV1 item in akPV1)
                    {
                        //insert into AbBukuVot
                        if (!item.AkCarta.Kod.Contains("L", StringComparison.OrdinalIgnoreCase))
                        {
                            // check if akaun bank bypass peruntukan or not
                            var akBank = _context.AkBank.FirstOrDefault(b => b.Id == akPV.AkBankId);
                            if (akBank != null)
                            {
                                // if akaun bank is in bajet, check peruntukan
                                if (akBank.IsBajet == true)
                                {
                                    // if baucer have invois, bypass
                                    if (akPV.FlJenisBaucer != JenisBaucer.Inbois) 
                                    {
                                        if (akPV.IsAKB == true)
                                        {

                                            if (AppInfo == null)
                                            {

                                                // check 
                                                var CartaDgnPeruntukan = await _context.AkCarta
                                                .Where(d => d.Id == item.AkCartaId && d.IsBajet == true)
                                                .FirstOrDefaultAsync();

                                                if (CartaDgnPeruntukan != null)
                                                {
                                                    // check for baki peruntukan
                                                    //if ((akPV.FlKategoriPenerima != 1) || (akPV.FlKategoriPenerima != 3) || (akPV.FlKategoriPenerima == 1 && akPV.denganTanggungan == false))
                                                    if ((akPV.FlJenisBaucer == 0 && akPV.FlKategoriPenerima == 0)
                                                || (akPV.FlKategoriPenerima == KategoriPenerima.Pembekal && akPV.denganTanggungan == false)
                                                || (akPV.FlJenisBaucer == JenisBaucer.Gaji))
                                                    {
                                                        bool IsExistAbBukuVot = await _context.AbBukuVot
                                                            .Where(x => x.Tahun == akPV.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPV.JKWId && x.JBahagianId == akPV.JBahagianId)
                                                            .AnyAsync();

                                                        if (IsExistAbBukuVot == true)
                                                        {
                                                            decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPV.Tahun, item.AkCartaId, akPV.JKWId, akPV.JBahagianId);

                                                            if (sum < item.Amaun)
                                                            {
                                                                TempData[SD.Error] = "Bajet untuk kod akaun " + item.AkCarta.Kod + " tidak mencukupi.";
                                                                return RedirectToAction(nameof(Index));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + item.AkCarta.Kod;
                                                            return RedirectToAction(nameof(Index));
                                                        }

                                                    }

                                                    // check for baki peruntukan end
                                                }

                                            }
                                        }
                                        else
                                        {
                                            // check 
                                            var CartaDgnPeruntukan = await _context.AkCarta
                                            .Where(d => d.Id == item.AkCartaId && d.IsBajet == true)
                                            .FirstOrDefaultAsync();

                                            if (CartaDgnPeruntukan != null)
                                            {
                                                // check for baki peruntukan
                                                //if ((akPV.FlKategoriPenerima != 1) || (akPV.FlKategoriPenerima != 3) || (akPV.FlKategoriPenerima == 1 && akPV.denganTanggungan == false))
                                                if ((akPV.FlJenisBaucer == 0 && akPV.FlKategoriPenerima == 0)
                                            || (akPV.FlKategoriPenerima == KategoriPenerima.Pembekal && akPV.denganTanggungan == false)
                                            || (akPV.FlJenisBaucer == JenisBaucer.Gaji))
                                                {
                                                    bool IsExistAbBukuVot = await _context.AbBukuVot
                                                        .Where(x => x.Tahun == akPV.Tahun && x.VotId == item.AkCartaId && x.JKWId == akPV.JKWId && x.JBahagianId == akPV.JBahagianId)
                                                        .AnyAsync();

                                                    if (IsExistAbBukuVot == true)
                                                    {
                                                        decimal sum = await _customRepo.GetBalanceFromAbBukuVot(akPV.Tahun, item.AkCartaId, akPV.JKWId, akPV.JBahagianId);

                                                        if (sum < item.Amaun)
                                                        {
                                                            TempData[SD.Error] = "Bajet untuk kod akaun " + item.AkCarta.Kod + " tidak mencukupi.";
                                                            return RedirectToAction(nameof(Index));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TempData[SD.Error] = "Tiada peruntukan untuk kod akaun " + item.AkCarta.Kod;
                                                        return RedirectToAction(nameof(Index));
                                                    }

                                                }

                                                // check for baki peruntukan end
                                            }

                                        }
                                    }

                                }
                            }
                            else
                            {
                                TempData[SD.Error] = "akaun bank tidak diisi.";
                                return RedirectToAction(nameof(Index));
                            }
                            // check peruntukan end

                            if (akPV.IsAKB == false)
                            {
                                AbBukuVot abBukuVot = new AbBukuVot();

                                decimal liabiliti = 0;
                                decimal tanggungan = 0;
                                bool havePO = false;

                                if (akPV.FlJenisBaucer == 0 && akPV.FlKategoriPenerima == KategoriPenerima.Pembekal)
                                {
                                    liabiliti = 0;
                                }
                                else
                                {
                                    liabiliti = 0 - item.Amaun;
                                }

                                if (akPV.FlKategoriPenerima == KategoriPenerima.Pembekal)
                                {
                                    foreach (var akPV2 in akPV.AkPV2)
                                    {
                                        if (akPV2.HavePO == true)
                                        {
                                            havePO = true;
                                        }
                                    }

                                    if (havePO == true)
                                    {
                                        tanggungan = 0 - item.Amaun;
                                    }
                                    else
                                    {
                                        tanggungan = 0;
                                    }

                                    //dengan tanggungan
                                    abBukuVot = new AbBukuVot()
                                    {
                                        Tahun = akPV.Tahun,
                                        JKWId = akPV.JKWId,
                                        JBahagianId = akPV.JBahagianId,
                                        Tarikh = akPV.Tarikh,
                                        Kod = kod,
                                        Penerima = penerima,
                                        VotId = item.AkCartaId,
                                        Rujukan = akPV.NoPV,
                                        Debit = item.Amaun,
                                        Tanggungan = tanggungan,
                                        Liabiliti = liabiliti

                                    };

                                    await _abBukuVotRepo.Insert(abBukuVot);
                                }
                                else if (akPV.FlKategoriPenerima == KategoriPenerima.Pekerja)
                                {
                                    if (akPV.FlJenisBaucer == JenisBaucer.Pendahuluan)
                                    {
                                        abBukuVot = new AbBukuVot()
                                        {
                                            Tahun = akPV.Tahun,
                                            JKWId = akPV.JKWId,
                                            JBahagianId = akPV.JBahagianId,
                                            Tarikh = akPV.Tarikh,
                                            Kod = kod,
                                            Penerima = penerima,
                                            VotId = item.AkCartaId,
                                            Rujukan = akPV.NoPV,
                                            Debit = item.Amaun,
                                            Tanggungan = 0 - item.Amaun
                                        };

                                    }
                                    else
                                    {
                                        abBukuVot = new AbBukuVot()
                                        {
                                            Tahun = akPV.Tahun,
                                            JKWId = akPV.JKWId,
                                            JBahagianId = akPV.JBahagianId,
                                            Tarikh = akPV.Tarikh,
                                            Kod = kod,
                                            Penerima = penerima,
                                            VotId = item.AkCartaId,
                                            Rujukan = akPV.NoPV,
                                            Debit = item.Amaun
                                        };

                                    }

                                    await _abBukuVotRepo.Insert(abBukuVot);
                                }
                                else
                                {
                                    //tanpa tanggungan
                                    abBukuVot = new AbBukuVot()
                                    {
                                        Tahun = akPV.Tahun,
                                        JKWId = akPV.JKWId,
                                        JBahagianId = akPV.JBahagianId,
                                        Tarikh = akPV.Tarikh,
                                        Kod = kod,
                                        Penerima = penerima,
                                        VotId = item.AkCartaId,
                                        Rujukan = akPV.NoPV,
                                        Debit = item.Amaun
                                    };

                                    await _abBukuVotRepo.Insert(abBukuVot);
                                }


                                // insert into AbBukuVot end
                            }
                        }
                        //insert into AbBukuVot end

                        // insert into tunai lejar
                        if (akPV.FlKategoriPenerima == KategoriPenerima.PemegangPanjar)
                        {
                            //insert akTunaiLejar
                            // kalau tambah had maksimum untuk kaunter panjar
                            if (akPV.FlJenisBaucer == JenisBaucer.TambahHadPanjar)
                            {
                                // update Had maksimum di AkTunaiRuncit
                                AkTunaiRuncit akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)akPV.AkTunaiRuncitId);

                                akTunaiRuncit.HadMaksimum = akTunaiRuncit.HadMaksimum + item.Amaun;

                                await _akTunaiRuncitRepo.Update(akTunaiRuncit);

                                var tunaiLejar = await _context.AkTunaiLejar.Include(x => x.AkTunaiRuncit)
                                    .Where(x => x.AkTunaiRuncitId == akPV.AkTunaiRuncitId && x.Rekup == null)
                                    .OrderByDescending(x => x.Tarikh).FirstOrDefaultAsync();

                                decimal bakiAkhir = 0;

                                if (tunaiLejar != null)
                                {
                                    bakiAkhir = tunaiLejar.Baki;
                                }
                                //insert into AkTunaiLejar
                                AkTunaiLejar akTunaiLejar = new AkTunaiLejar()
                                {
                                    JKWId = akPV.JKWId,
                                    JBahagianId = akPV.JBahagianId,
                                    AkTunaiRuncitId = (int)akPV.AkTunaiRuncitId,
                                    Tarikh = akPV.Tarikh,
                                    AkCartaId = item.AkCartaId,
                                    NoRujukan = akPV.NoPV,
                                    Debit = item.Amaun,
                                    Kredit = 0,
                                    Baki = bakiAkhir + item.Amaun
                                };
                                // insert into AkTunaiLejar end

                                await _akTunaiLejarRepo.Insert(akTunaiLejar);
                            }


                            if (akPV.FlJenisBaucer == JenisBaucer.Rekupan)
                            {
                                //find latest baki with noRekup and AkTunaiRuncitId
                                var rekupanList = (from tbl in _context.AkTunaiLejar
                                                        .Include(x => x.AkTunaiRuncit).ThenInclude(x => x.AkCarta)
                                                        .Where(x => x.AkTunaiRuncitId == akPV.AkTunaiRuncitId && x.Rekup == akPV.NoRekup).ToList()
                                                   select new
                                                   {
                                                       noRekup = tbl.Rekup,
                                                       akCarta = tbl.AkTunaiRuncit.AkCarta,
                                                       Debit = tbl.Debit,
                                                       Kredit = tbl.Kredit
                                                   }).GroupBy(x => x.noRekup).FirstOrDefault();


                                decimal JumlahDebit = 0;

                                decimal JumlahKredit = 0;

                                foreach (var i in rekupanList)
                                {
                                    JumlahDebit = JumlahDebit + i.Debit;
                                    JumlahKredit = JumlahKredit + i.Kredit;
                                }

                                if (JumlahDebit == akPV.AkTunaiRuncit.HadMaksimum)
                                {
                                    decimal JumlahRekupan = JumlahKredit;

                                    var objRekup = rekupanList.Select(l => new
                                    {
                                        noRekup = l.noRekup,
                                        akCarta = l.akCarta,
                                        amaun = JumlahRekupan
                                    }).FirstOrDefault();

                                    decimal bakiAkhir = JumlahDebit - JumlahKredit;

                                    //insert into AkTunaiLejar
                                    AkTunaiLejar akTunaiLejar = new AkTunaiLejar()
                                    {
                                        JKWId = akPV.JKWId,
                                        JBahagianId = akPV.JBahagianId,
                                        AkTunaiRuncitId = (int)akPV.AkTunaiRuncitId,
                                        Tarikh = akPV.Tarikh,
                                        AkCartaId = item.AkCartaId,
                                        NoRujukan = akPV.NoPV,
                                        Debit = item.Amaun,
                                        Kredit = 0,
                                        Baki = bakiAkhir + item.Amaun
                                    };
                                    // insert into AkTunaiLejar end

                                    await _akTunaiLejarRepo.Insert(akTunaiLejar);

                                    List<AkTunaiLejar> TunaiLejarPaid = await _context.AkTunaiLejar
                                        .Where(x => x.AkTunaiRuncitId == akPV.AkTunaiRuncitId && x.Rekup == akPV.NoRekup)
                                        .ToListAsync();

                                    foreach (var list in TunaiLejarPaid)
                                    {
                                        list.IsPaid = true;

                                        await _akTunaiLejarRepo.Update(list);
                                    }
                                }

                            }
                        }
                        // insert into tunai lejar end

                        // insert into akAkaun
                        if (akPV.AkPV2 != null && akPV.AkPV2.Count > 0 )
                        {
                            int kodAkaunPerdangangan = 0;
                            
                            foreach(var item2 in akPV.AkPV2)
                            {
                                kodAkaunPerdangangan = item2.AkBelian.KodObjekAPId;
                            }
                                //insert into akAkaun dengan tanggungan
                                AkAkaun akAKodBank = new()
                                {
                                    NoRujukan = akPV.NoPV,
                                    JKWId = akPV.JKWId,
                                    JBahagianId = akPV.JBahagianId,
                                    AkCartaId1 = akPV.AkBank.AkCartaId,
                                    AkCartaId2 = kodAkaunPerdangangan,
                                    Tarikh = akPV.Tarikh,
                                    Tahun = akPV.Tahun,
                                    Kredit = item.Amaun,
                                    AkPembekalId = akPV.AkPembekalId
                                };

                                await _akAkaunRepo.Insert(akAKodBank);

                                AkAkaun akAObjek = new()
                                {
                                    NoRujukan = akPV.NoPV,
                                    JKWId = akPV.JKWId,
                                    JBahagianId = akPV.JBahagianId,
                                    AkCartaId1 = kodAkaunPerdangangan,
                                    AkCartaId2 = akPV.AkBank.AkCartaId,
                                    Tarikh = akPV.Tarikh,
                                    Tahun = akPV.Tahun,
                                    Debit = item.Amaun,
                                    AkPembekalId = akPV.AkPembekalId
                                };

                                await _akAkaunRepo.Insert(akAObjek);

                        }
                        else
                        {
                            //insert into akAkaun
                            AkAkaun akAKodBank = new AkAkaun()
                            {
                                NoRujukan = akPV.NoPV,
                                JKWId = akPV.JKWId,
                                JBahagianId = akPV.JBahagianId,
                                AkCartaId1 = akPV.AkBank.AkCartaId,
                                AkCartaId2 = item.AkCartaId,
                                Tarikh = akPV.Tarikh,
                                Tahun = akPV.Tahun,
                                Kredit = item.Amaun,
                                AkPembekalId = akPV.AkPembekalId,
                                JSukanId = akPV.SpPendahuluanPelbagai?.JSukanId
                            };

                            await _akAkaunRepo.Insert(akAKodBank);

                            AkAkaun akAObjek = new AkAkaun()
                            {
                                NoRujukan = akPV.NoPV,
                                JKWId = akPV.JKWId,
                                JBahagianId = akPV.JBahagianId,
                                AkCartaId1 = item.AkCartaId,
                                AkCartaId2 = akPV.AkBank.AkCartaId,
                                Tarikh = akPV.Tarikh,
                                Tahun = akPV.Tahun,
                                Debit = item.Amaun,
                                AkPembekalId = akPV.AkPembekalId,
                                JSukanId = akPV.SpPendahuluanPelbagai?.JSukanId
                            };

                            await _akAkaunRepo.Insert(akAObjek);
                        }
                        // insert into akAkaun end

                    }

                    akPV.FlStatusLulus = 1;
                    akPV.TarLulus = tarikhLulus;
                    akPV.JPelulusId = pelulusId;

                    akPV.FlPosting = 1;
                    akPV.TarikhPosting = tarikhLulus ?? DateTime.Now;
                    akPV.TarKemaskini = DateTime.Now;

                    //insert applog
                    await AddLogAsync("Posting", "Posting Data", akPV.NoPV, (int)id, akPV.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya diluluskan.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

        // unposting function
        [Authorize(Policy = "PV001UT")]
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

                AkPV akPV = await _akPVRepo.GetById((int)id);

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == akPV.NoPV).ToList();

                List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan == akPV.NoPV).ToList();

                List<AkTunaiLejar> akTunaiLejar = _context.AkTunaiLejar.Where(x => x.NoRujukan == akPV.NoPV).ToList();

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

                    //delete data from abBukuVot
                    foreach (AbBukuVot item in abBukuVot)
                    {
                        await _abBukuVotRepo.Delete(item.Id);
                    }
                    //delete data from abBukuVot

                    //delete data from akTunaiLejar
                    foreach (AkTunaiLejar item in akTunaiLejar)
                    {
                        await _akTunaiLejarRepo.Delete(item.Id);

                        if (akPV.FlJenisBaucer == JenisBaucer.Rekupan)
                        {
                            List<AkTunaiLejar> TunaiLejarPaid = await _context.AkTunaiLejar
                                        .Where(x => x.AkTunaiRuncitId == akPV.AkTunaiRuncitId && x.Rekup == akPV.NoRekup)
                                        .ToListAsync();

                            foreach (var list in TunaiLejarPaid)
                            {
                                list.IsPaid = false;

                                await _akTunaiLejarRepo.Update(list);
                            }
                        }
                    }
                    //delete data from akTunaiLejar

                    //update posting status in akTerima
                    akPV.FlPosting = 0;
                    akPV.TarikhPosting = null;

                    // AK CODE 19/03/2022
                    akPV.FlStatusLulus = 0;
                    akPV.TarLulus = null;
                    akPV.JPelulusId = null;
                    // AK CODE 19/03/2022

                    await _akPVRepo.Update(akPV);

                    //insert applog
                    await AddLogAsync("UnPosting", "UnPosting Data", akPV.NoPV, (int)id, akPV.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kelulusan.";
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // unposting function end

        //// POST: AkPOLaras/Cancel/5
        [Authorize(Policy = "PV001B")]
        public async Task<IActionResult> Cancel(int id)
        {
            var obj = await _akPVRepo.GetById(id);
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // check if not posting redirect back
            if (obj.FlPosting == 0)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            List<AbBukuVot> abBukuVot = _context.AbBukuVot.Where(x => x.Rujukan.EndsWith("PV/" + obj.NoPV)).ToList();
            if (abBukuVot == null)
            {
                //duplicate id error
                TempData[SD.Error] = "Data belum diluluskan.";
                return RedirectToAction(nameof(Index));
            }
            else
            {

                List<AkPV1> akPV1 = obj.AkPV1.ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == obj.NoPV).FirstOrDefaultAsync();
                if (akAkaun == null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dibatalkan.";

                }
                else
                {

                    obj.FlBatal = 1;
                    obj.TarBatal = DateTime.Now;

                    await _akPVRepo.Update(obj);

                    //insert applog
                    await AddLogAsync("Batal", "Batal Data", obj.NoPV, (int)id, obj.Jumlah, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya dibatalkan.";
                }

            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PV001R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            var obj = await _akPVRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // rollback operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            obj.TarSemak = null;
            obj.JPenyemakId = null;
            obj.FlStatusSemak = 0;

            obj.TarLulus = null;
            obj.JPelulusId = null;
            obj.FlStatusLulus = 0;

            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkPV.Update(obj);
            // rollback operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoPV, id, obj.Jumlah, pekerjaId);
            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // -- unused --
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
                    var akT2 = await _context.AkPV2.Include(b => b.AkBelian).ThenInclude(b => b.AkPO).FirstOrDefaultAsync(x => x.AkBelianId == akPV2.AkBelianId && x.AkPVId == akPV2.AkPVId);
                    var user = await _userManager.GetUserAsync(User);

                    _context.AkPV2.Remove(akT2);

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
    }
}
