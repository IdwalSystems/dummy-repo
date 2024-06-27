using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
using MSNK.Models.Operations;
using Newtonsoft.Json;
using static MSNK.Infrastructure.Tools;

namespace MSNK.Controllers
{

    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class AkCimbEFTController : Controller
    {

        public const string modul = "PV002";
        public const string namamodul = "Biz Channel";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkCimbEFT, int, string> _akCimbEFTRepo;
        private readonly ListViewIRepository<AkCimbEFT1, int> _akCimbEFT1Repo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartCimbEFT _cart;

        public AkCimbEFTController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkCimbEFT, int, string> akCimbEFTRepo,
            ListViewIRepository<AkCimbEFT1, int> akCimbEFT1Repo,
            IRepository<AkPV, int, string> akPVRepo,
            IRepository<AkBank, int, string> akBankRepo,
            CartCimbEFT cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akCimbEFTRepo = akCimbEFTRepo;
            _akCimbEFT1Repo = akCimbEFT1Repo;
            _akPVRepo = akPVRepo;
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

        // GET: AkCimbEFT
        [Authorize(Policy = "PV002")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new()
            {
                new SelectListItem() { Text = "Tar Jana", Value = "TarJana" },
                new SelectListItem() { Text = "No PBI", Value = "NoPBI" },
                new SelectListItem() { Text = "Penjana", Value = "Penjana" }
            };

            var akCimbEFT = new List<AkCimbEFT>().AsEnumerable();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akCimbEFT = await _akCimbEFTRepo.GetAllIncludeDeletedItemsFiltered(searchString,searchDate1,searchDate2,searchColumn);
            }
            else
            {
                akCimbEFT = await _akCimbEFTRepo.GetAllFiltered(searchString,searchDate1,searchDate2,searchColumn);
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
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "TarJana");
            }

            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _akCimbEFTRepo.GetByIdIncludeDeletedItems((int)id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(akCimbEFT);
        }

        private void PopulateTable(int? id)
        {
            List<AkCimbEFT1> akCimbEFT1 = _context.AkCimbEFT1
                .Include(b => b.JBank)
                .Include(b => b.AkPV)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Where(b => b.AkCimbEFTId == id)
                .OrderBy(b => b.Indek)
                .ToList();

            List<AkCimbEFT1ViewModel> vm = new List<AkCimbEFT1ViewModel>();

            foreach (var item in akCimbEFT1)
            {
                var noKP = "";
                var noAkaunBank = "";
                var nama = "";
                var jBank = "";

                switch (item.FlPenerimaEFT)
                {
                    case KategoriPenerima.Pembekal:
                        noKP = "";
                        noAkaunBank = item.AkPV.NoAkaunBank;
                        nama = item.AkPV.Nama;
                        jBank = item.AkPV.JBank.KodEFT;
                        break;
                    case KategoriPenerima.Pekerja:
                        noKP = "";
                        noAkaunBank = item.AkPV.NoAkaunBank;
                        nama = item.AkPV.Nama;
                        jBank = item.JBank.KodEFT;
                        break;
                    case KategoriPenerima.Jurulatih:
                        noKP = item.SuJurulatih.NoKp;
                        noAkaunBank = item.SuJurulatih.NoAkaunBank;
                        nama = item.SuJurulatih.Nama;
                        jBank = item.SuJurulatih.JBank.KodEFT;
                        break;
                    case KategoriPenerima.Atlet:
                        noKP = item.SuAtlet.NoKp;
                        noAkaunBank = item.SuAtlet.NoAkaunBank;
                        nama = item.SuAtlet.Nama;
                        jBank = item.SuAtlet.JBank.KodEFT;
                        break;
                    default:
                        noKP = "";
                        noAkaunBank = item.AkPV.NoAkaunBank;
                        nama = item.AkPV.Nama;
                        jBank = item.AkPV.JBank.KodEFT;
                        break;
                }
                vm.Add(new AkCimbEFT1ViewModel
                {
                    Id = item.Id,
                    Indek = item.Indek,
                    AkPVId = item.AkPVId,
                    FlPenerimaEFT = item.FlPenerimaEFT,
                    AkPembekalId = item.AkPembekalId == null ? null : item.AkPembekalId,
                    SuPekerjaId = item.SuPekerjaId == null ? null : item.SuPekerjaId,
                    SuAtletId = item.SuAtletId == null ? null : item.SuAtletId,
                    SuJurulatihId = item.SuJurulatihId == null ? null : item.SuJurulatihId,
                    NoPV = item.AkPV.NoPV,
                    NoKP = noKP,
                    NoAkaun = noAkaunBank,
                    Penerima = nama,
                    NoCekAtauEFT = item.AkPV.NoCekAtauEFT,
                    Tarikh = item.AkPV.Tarikh,
                    KodBank = jBank,
                    Amaun = item.Amaun,
                    FlStatus = item.FlStatus
                });
            }
            
            ViewBag.AkCimbEFT1 = vm;
        }

        // GET: AkCimbEFT/Create
        [Authorize(Policy = "PV002C")]
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            var noRujukan = GetNoRujukan(DateTime.Now, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

            ViewBag.NoPBI = noRujukan;
            ViewBag.NamaFail = noRujukan + ".txt";

            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akCimbEFT1 = new List<int>();
                _cart.Clear1();

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

            ViewBag.AkBank = bankList;

        }

        [HttpPost]
        public JsonResult JsonGetKod(DateTime tarJana, string noPBI)
        {
            try
            {
                if (noPBI != "Invalid date")
                {
                    noPBI = GetNoRujukan(tarJana, tarJana.ToString("yyyy"), tarJana.ToString("MM"));

                    return Json(new { result = "OK", record = noPBI });
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
        public async Task<JsonResult> JsonGetBaucer(DateTime tarDari, DateTime tarHingga)
        {
            try
            {
                CartEmpty();

                var caraBayar = _context.JCaraBayar.Where(b => b.Perihal.Contains("EFT")).FirstOrDefault();
                // find all pv within date range where it is not jenis baucer panjar or jenis baucer gaji and posting = 1
                // switch by jenis baucer, differentiate by individu or gandaan
                // case individual : 
                    // get all information on AkPV only
                // case Gandaan :
                    // get all information from SuProfil
                // return
                    // 1. data
                    // 2. type of data (Individual or Gandaan) 
                
                // get all PV where posting = 1
                List<AkPV> pv = await _context.AkPV
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.JBank)
                    .Include(b => b.AkTunaiRuncit).ThenInclude(b => b.AkCarta)
                    .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b.AkCarta)
                    .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b.SuPekerja)
                    .Include(b => b.SuProfil)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPembekal).ThenInclude(x => x.JBank)
                    .Include(b => b.SuPekerja).ThenInclude(x => x.JBank)
                    .Include(b => b.AkBank).ThenInclude(b => b.JBank)
                    .Include(b => b.JCaraBayar)
                    .Include(b => b.AkPV1)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2)
                        .ThenInclude(b => b.AkBelian)
                            .ThenInclude(b => b.AkPO)
                    .Include(b => b.AkPVGanda)
                        .ThenInclude(b => b.JBank)
                    .Include(b => b.AkPVGanda)
                        .ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.AkPVGanda)
                        .ThenInclude(b => b.SuAtlet)
                    .Include(b => b.AkPVGanda)
                        .ThenInclude(b => b.SuJurulatih)
                    .Include(b => b.AkPVGanda)
                        .ThenInclude(b => b.SuPekerja)
                    .Where(b => b.FlPosting == 1 )
                    .OrderBy(b => b.NoPV)
                    .ToListAsync();

                // get all PV within date range
                pv = pv.Where(x => x.Tarikh >= tarDari
                    && x.Tarikh <= tarHingga.AddHours(23.99)).ToList();

                // get all PV where it is not jenis baucer panjar or jenis baucer gaji
                pv = pv.Where(x => x.FlJenisBaucer != JenisBaucer.Gaji 
                                || x.FlJenisBaucer != JenisBaucer.Rekupan 
                                || x.FlJenisBaucer != JenisBaucer.TambahHadPanjar)
                                .ToList();

                List<AkCimbEFT1ViewModel> pvTable = new List<AkCimbEFT1ViewModel>();

                int indek = 0;
                foreach (var item in pv)
                {
                    //checked if already jana PV in AkCimbEFT1
                    bool isExistPV = _context.AkCimbEFT1
                        .Include(b => b.AkCimbEFT)
                        .Where(b => b.AkPVId == item.Id && b.FlStatus == 1 && b.AkCimbEFT.FlHapus == 0)
                        .Any();

                    // individu
                    if (item.IsGanda == false)
                    {
                        if (isExistPV == true)
                        {
                            continue;
                        }

                        // check if have no akaun bank or not
                        bool isNotExistNoAkaun = _context.AkPV.Where(b => b.Id == item.Id && string.IsNullOrEmpty(b.NoAkaunBank)).Any();
                        if (isNotExistNoAkaun == true)
                        {
                            continue;
                        }

                        if (item.JBank.KodEFT == null)
                        {
                            return Json(new { result = "ERROR" , bank = item.JBank.Nama });
                        }

                        indek++;
                        if (item.JCaraBayarId == caraBayar.Id)
                        {
                            pvTable.Add(
                            new AkCimbEFT1ViewModel
                            {
                                Id = 0,
                                Indek = indek,
                                AkPVId = item.Id,
                                FlPenerimaEFT = item.FlKategoriPenerima,
                                AkPembekalId = item.AkPembekalId == null ? null : item.AkPembekalId,
                                SuPekerjaId = item.SuPekerjaId == null ? null : item.SuPekerjaId,
                                SuAtletId = null,
                                SuJurulatihId = null,
                                NoPV = item.NoPV,
                                NoKP = item.NoKP == null ? "" : item.NoKP,
                                NoAkaun = item.NoAkaunBank,
                                Penerima = item.Nama,
                                NoCekAtauEFT = item.NoCekAtauEFT,
                                Tarikh = item.Tarikh,
                                KodBank = item.JBank.KodEFT,
                                Amaun = item.Jumlah
                            });
                        }
                    }
                    // gandaan
                    else
                    {
                        var pvGanda = item.AkPVGanda;

                        if (pvGanda != null)
                        {
                            foreach (var itemGanda in pvGanda)
                            {

                                if (itemGanda.JBank.KodEFT == null)
                                {
                                    return Json(new { result = "ERROR", bank = itemGanda.JBank.Nama });
                                }

                                // check if have no akaun bank or not
                                bool isNotExistNoAkaun = _context.AkPVGanda.Where(b => b.Id == item.Id && string.IsNullOrEmpty(b.NoAkaun)).Any();
                                
                                if (isNotExistNoAkaun == true)
                                {
                                    continue;
                                }

                                // check if already succeed jana txt in previous data
                                if (!string.IsNullOrEmpty(itemGanda.NoCekAtauEFT) || !string.IsNullOrEmpty(itemGanda.TarCekAtauEFT?.ToString("dd/MM/yyyy")))
                                {
                                    continue;
                                } ;
                                
                                if (itemGanda.JCaraBayarId == caraBayar.Id)
                                {

                                    indek++;
                                    pvTable.Add(
                                   new AkCimbEFT1ViewModel
                                   {
                                       Id = 0,
                                       Indek = indek,
                                       AkPVId = item.Id,
                                       FlPenerimaEFT = item.FlKategoriPenerima,
                                       AkPembekalId = null,
                                       SuPekerjaId = itemGanda.SuPekerjaId,
                                       SuAtletId = itemGanda.SuAtletId,
                                       SuJurulatihId = itemGanda.SuJurulatihId,
                                       NoPV = item.NoPV,
                                       NoKP = itemGanda.NoKp,
                                       NoAkaun = itemGanda.NoAkaun,
                                       Penerima = itemGanda.Nama,
                                       NoCekAtauEFT = itemGanda.NoCekAtauEFT,
                                       Tarikh = item.Tarikh,
                                       KodBank = itemGanda.JBank.KodEFT,
                                       Amaun = itemGanda.Amaun
                                   });
                                }

                            }

                        }
                    }
                    

                }

                // add to cart first
                PopulateCart(pv, caraBayar);

                return Json(new { result = "OK", table = pvTable });

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private void PopulateCart(List<AkPV> pv, JCaraBayar caraBayar)
        {
            int indek = 0;

            // individual
            foreach(var item in pv)
            {
                //checked if already jana PV in AkCimbEFT1
                bool isExistPV = _context.AkCimbEFT1.Where(b => b.AkPVId == item.Id && b.FlStatus != 0 && b.AkCimbEFT.FlHapus == 0).Any();


                if (item.IsGanda == false)
                {
                    if (isExistPV == true)
                    {
                        continue;
                    }

                    // check if have no akaun bank or not
                    bool isExistNoAkaun = _context.AkPV.Where(b => b.Id == item.Id && string.IsNullOrEmpty(b.NoAkaunBank)).Any();
                    if (isExistNoAkaun == true)
                    {
                        continue;
                    }

                    if (item.JCaraBayarId == caraBayar.Id)
                    {
                        indek++;
                        _cart.AddItem1(0,
                                        indek,
                                        item.Id,
                                        item.FlKategoriPenerima,
                                        item.AkPembekalId == null ? null : item.AkPembekalId,
                                        item.SuPekerjaId == null ? null : item.SuPekerjaId,
                                        null,
                                        null,
                                        item.Jumlah,
                                        item.NoCekAtauEFT,
                                        "",
                                        item.JBankId,
                                        1
                                        );
                    }
                    
                }
                else
                {
                    var pvGanda = item.AkPVGanda;

                    if (pvGanda != null)
                    {
                        foreach (var itemGanda in pvGanda)
                        {
                            // check if already succeed jana txt in previous data
                            if (!string.IsNullOrEmpty(itemGanda.NoCekAtauEFT) || !string.IsNullOrEmpty(itemGanda.TarCekAtauEFT?.ToString("dd/MM/yyyy")))
                            {
                                continue;
                            };

                            if (itemGanda.JCaraBayarId == caraBayar.Id)
                            {

                                indek++;
                                _cart.AddItem1(0,
                                indek,
                                item.Id,
                                item.FlKategoriPenerima,
                                null,
                                itemGanda.SuPekerjaId,
                                itemGanda.SuAtletId,
                                itemGanda.SuJurulatihId,
                                itemGanda.Amaun,
                                itemGanda.NoAkaun,
                                "",
                                itemGanda.JBankId,
                                1
                                );
                            }
                        }
                    }
                }   
            }
        }
        private string GetNoRujukan(DateTime tarJana, string year, string month)
        {
            string prefix = year + month;
            int x = 1;
            string noRujukan = prefix + "000";

            var LatestNoRujukan = _context.AkCimbEFT
                       .IgnoreQueryFilters()
                       .Where(x => x.TarJana.Year ==  tarJana.Year && x.TarJana.Month == tarJana.Month && x.NoPBI.Contains(prefix.ToUpper()))
                       .Max(x => x.NoPBI);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(7));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            return noRujukan;
        }

        //remove cart akCimbEFT1
        public JsonResult RemoveAkCimbEFT1(AkCimbEFT1 akCimbEFT1)
        {

            try
            {

                var ak1 = _cart.Lines1.Where(x => x.Indek == akCimbEFT1.Indek).FirstOrDefault();

                if (ak1 != null)
                {
                    _cart.RemoveItem1(akCimbEFT1.Indek);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //remove cart akCimbEFT1 end

        // POST: AkCimbEFT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PV002C")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkCimbEFT akCimbEFT, int AkBankId)
        {
            AkCimbEFT m = new AkCimbEFT();
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
            var noRujukan = GetNoRujukan(akCimbEFT.TarJana, akCimbEFT.TarJana.ToString("yyyy"), akCimbEFT.TarJana.ToString("MM"));
            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akCimbEFT != null && AkBankId != 0)
                {

                    m.NoPBI = noRujukan;
                    m.TarJana = akCimbEFT.TarJana;
                    m.TarBayar = DateTime.Now;
                    m.Jumlah = akCimbEFT.Jumlah;
                    m.NamaFail = akCimbEFT.NamaFail;
                    m.BilPV = akCimbEFT.BilPV;
                    m.FlHapus = 0;
                    m.FlKategori = akCimbEFT?.FlKategori;
                    m.FlStatus = akCimbEFT.FlStatus;
                    m.AkBankId = AkBankId;
                    m.SuPekerjaId = namaUser.SuPekerjaId;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;
                    m.SuPekerjaMasukId = pekerjaId;

                    m.AkCimbEFT1 = _cart.Lines1.ToArray();

                    await _akCimbEFTRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoPBI, m.NoPBI, 0, m.Jumlah, pekerjaId);
                    //insert applog end
                    
                    // update no EFT in akPV
                    foreach(var item in _cart.Lines1)
                    {
                        var akPV = await _akPVRepo.GetById(item.AkPVId);

                        if (akPV.IsGanda == false)
                        {
                            akPV.NoCekAtauEFT = noRujukan;
                            akPV.TarCekAtauEFT = DateTime.Now;
                        }
                        else
                        {
                            foreach (var itemGanda in akPV.AkPVGanda)
                            {
                                itemGanda.NoCekAtauEFT = noRujukan;
                                itemGanda.TarCekAtauEFT = DateTime.Now;
                            }
                        }
                        await _akPVRepo.Update(akPV);
                    }
                    // update no EFT in AkPV end

                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat EFT berjaya ditambah. No PBI adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            CartEmpty();
            ViewBag.NoPBI = noRujukan;
            ViewBag.NamaFail = noRujukan + ".txt";

            return View(akCimbEFT);
        }
        //public FileResult Download()
        //{
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(@"c:\folder\myfile.ext");
        //    string fileName = "myfile.ext";
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}

        [HttpPost]
        public async Task<JsonResult> JsonJanaTxt(int id)
        {
            try
            {
                var akCimbEFT = await _akCimbEFTRepo.GetByIdIncludeDeletedItems((int)id);

                List<string> txt = new List<string>();
                // batch header record
                var header = "0190210Majlis Sukan Negeri Kedah               "+akCimbEFT.TarBayar.ToString("ddMMyyyy")+"0000000000000000  ";
                // batch header record end
                txt.Add(header);
                // detail record
                foreach (var i in akCimbEFT.AkCimbEFT1)
                {
                    var jenisRekod = "02";

                    if (string.IsNullOrEmpty(i.JBank.KodEFT))
                    {
                        var error = "Ralat, Bank " + i.JBank.Nama + " yang tidak mempunyai Kod BNM.";
                        return Json(new { result = "Error", message = error });
                    }

                    var KodBNM = i.JBank.KodEFT.ToString().PadRight(7, '0');
                    var noAkaun = "";
                    var penerima = "";
                    var NoKP = "";

                    // FlPenerimaEFT = 0 ( Am / Lain - lain )
                    // FlPenerimaEFT = 1 ( pembekal )
                    // FlPenerimaEFT = 2 ( pekerja )
                    // FlPenerimaEFT = 3 ( pemegang panjar )
                    // FlPenerimaEFT = 4 ( jurulatih )
                    // FlPenerimaEFT = 5 ( atlet )
                    switch (i.FlPenerimaEFT)
                    {
                        case KategoriPenerima.Pembekal:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = "";
                            break;
                        case KategoriPenerima.Pekerja:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = i.AkPV.NoKP;
                            break;
                        case KategoriPenerima.Jurulatih:
                            noAkaun = i.SuJurulatih.NoAkaunBank;
                            penerima = i.SuJurulatih.Nama;
                            NoKP = i.SuJurulatih.NoKp;
                            break;
                        case KategoriPenerima.Atlet:
                            noAkaun = i.SuAtlet.NoAkaunBank;
                            penerima = i.SuAtlet.Nama;
                            NoKP = i.SuAtlet.NoKp;
                            break;
                        default:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = i.AkPV.NoKP;
                            break;

                    }

                    noAkaun = TruncateNumbers(noAkaun,16).PadRight(16);
                    penerima = TruncateText(penerima,40).PadRight(40).ToUpper();
                    string amaun = i.Amaun.ToString().Replace(".", "").PadLeft(11, '0');
                    string refNum = akCimbEFT.NoPBI.PadRight(30);
                    if (NoKP != null)
                    {
                        NoKP = TruncateNumbers(NoKP,20).PadRight(20);
                    } else
                    {
                        NoKP = "".PadRight(20);
                    }
                    
                    string description = i.Id.ToString().PadRight(20);

                    var nextLine = jenisRekod + KodBNM + noAkaun + penerima + amaun + refNum + NoKP + description;

                    txt.Add(nextLine);
                    
                }

                return Json(new { result = "OK", record = txt });


            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [Authorize(Policy = "PV002C")]
        public async Task<IActionResult> JanaTxt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _akCimbEFTRepo.GetByIdIncludeDeletedItems((int)id);


            //File and path you want to create and write to
            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
            //string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string downloadsPath = "C:\\";

            string txtFile = downloadsPath + "\\" + akCimbEFT.NamaFail;

            if (System.IO.File.Exists(txtFile))
            {
                System.IO.File.Delete(txtFile);
            }

            using (StreamWriter sw = new StreamWriter(txtFile))
            {
                // batch header record
                sw.WriteLine("0190210Majlis Sukan Negeri Kedah               "+akCimbEFT.TarBayar.ToString("ddMMyyyy")+"0000000000000000  ");
                // batch header record end

                // detail record
                foreach (var i in akCimbEFT.AkCimbEFT1)
                {
                    var jenisRekod = "02";

                    if (string.IsNullOrEmpty(i.JBank.KodEFT))
                    {
                        TempData[SD.Error] = "Ralat, Bank " + i.JBank.Nama + " yang tidak mempunyai Kod BNM.";
                        return RedirectToAction(nameof(Details),new { Id = akCimbEFT.Id });
                    }

                    var KodBNM = i.JBank.KodEFT.ToString().PadRight(7, '0');
                    var noAkaun = "";
                    var penerima = "";
                    var NoKP = "";

                    // FlPenerimaEFT = 0 ( Am / Lain - lain )
                    // FlPenerimaEFT = 1 ( pembekal )
                    // FlPenerimaEFT = 2 ( pekerja )
                    // FlPenerimaEFT = 3 ( pemegang panjar )
                    // FlPenerimaEFT = 4 ( jurulatih )
                    // FlPenerimaEFT = 5 ( atlet )
                    switch (i.FlPenerimaEFT)
                    {
                        case KategoriPenerima.Pembekal:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = "";
                            break;
                        case KategoriPenerima.Pekerja:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = i.AkPV.NoKP;
                            break;
                        case KategoriPenerima.Jurulatih:
                            noAkaun = i.SuJurulatih.NoAkaunBank;
                            penerima = i.SuJurulatih.Nama;
                            NoKP = i.SuJurulatih.NoKp;
                            break;
                        case KategoriPenerima.Atlet:
                            noAkaun = i.SuAtlet.NoAkaunBank;
                            penerima = i.SuAtlet.Nama;
                            NoKP = i.SuAtlet.NoKp;
                            break;
                        default:
                            noAkaun = i.AkPV.NoAkaunBank;
                            penerima = i.AkPV.Nama;
                            NoKP = i.AkPV.NoKP;
                            break;

                    }

                    noAkaun = noAkaun.PadRight(16);
                    penerima = penerima.PadRight(40).ToUpper();
                    string amaun = i.Amaun.ToString().Replace(".","").PadLeft(11,'0');
                    string refNum = akCimbEFT.NoPBI.PadRight(30);
                    NoKP = NoKP?.PadRight(20) ?? "".PadRight(20) ;
                    string description = i.Id.ToString().PadRight(20);

                    sw.WriteLine(jenisRekod + KodBNM + noAkaun + penerima + amaun + refNum + NoKP + description);

                }
                // detail record end
            }
            TempData[SD.Success] = "Maklumat EFT berjaya dijana ke fail TXT. Sila rujuk pada direktori 'Desktop' dengan nama fail " + akCimbEFT.NamaFail;
            return RedirectToAction(nameof(Index));
        }
        // GET: AkCimbEFT/Edit/5
        [Authorize(Policy = "PV002E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _akCimbEFTRepo.GetByIdIncludeDeletedItems((int)id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCartFromDb(akCimbEFT);
            return View(akCimbEFT);
        }

        private void PopulateCartFromDb(AkCimbEFT akCimbEFT)
        {
            List<AkCimbEFT1> akCimbEFT1 = _context.AkCimbEFT1
                .Include(b => b.JBank)
                .Include(b => b.AkPV)
                        .ThenInclude(b => b.SuProfil)
                .Include(b => b.AkPV)
                        .ThenInclude(b => b.SpPendahuluanPelbagai)
                .Include(b => b.AkPembekal)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuPekerja)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuPekerja)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuAtlet)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuAtlet)
                        .ThenInclude(b => b.JBank)
                .Include(b => b.SuJurulatih)
                        .ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuJurulatih)
                        .ThenInclude(b => b.JBank)
                .Where(b => b.AkCimbEFTId == akCimbEFT.Id)
                .OrderBy(b => b.Indek)
                .ToList();

            foreach (AkCimbEFT1 item in akCimbEFT1)
            {
                _cart.AddItem1(item.Id,
                                item.Indek,
                                item.AkPVId,
                                item.FlPenerimaEFT,
                                item.AkPembekalId == null ? null : item.AkPembekalId,
                                item.SuPekerjaId == null ? null : item.SuPekerjaId,
                                item.SuAtletId == null ? null : item.SuAtletId,
                                item.SuJurulatihId == null ? null : item.SuJurulatihId,
                                item.Amaun,
                                item.NoCek,
                                item.Catatan,
                                item.JBankId,
                                item.FlStatus
                                );
            }
        }

        //save cart akCimbEFT1
        public JsonResult SaveCartAkCimbEFT1(AkCimbEFT1 akCimbEFT1)
        {

            try
            {

                var akT2 = _cart.Lines1.Where(x => x.Indek == akCimbEFT1.Indek).FirstOrDefault();

                var user = _userManager.GetUserName(User);

                if (akT2 != null)
                {
                    _cart.RemoveItem1(akCimbEFT1.Indek);

                    _cart.AddItem1(0,
                                akT2.Indek,
                                akT2.AkPVId,
                                akT2.FlPenerimaEFT,
                                akT2.AkPembekalId == null ? null : akT2.AkPembekalId,
                                akT2.SuPekerjaId == null ? null : akT2.SuPekerjaId,
                                akT2.SuAtletId == null ? null : akT2.SuAtletId,
                                akT2.SuJurulatihId == null ? null : akT2.SuJurulatihId,
                                akT2.Amaun,
                                akT2.NoCek,
                                akT2.Catatan,
                                akT2.JBankId,
                                akCimbEFT1.FlStatus
                                );

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart akCimbEFT1 end

        // POST: AkCimbEFT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "PV002E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AkCimbEFT akCimbEFT)
        {
            if (id != akCimbEFT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                    AkCimbEFT dataAsal = await _akCimbEFTRepo.GetById(id);

                    // list of input that cannot be change
                    akCimbEFT.TarBayar = dataAsal.TarBayar;
                    akCimbEFT.TarJana = dataAsal.TarJana;
                    akCimbEFT.FlKategori = dataAsal.FlKategori;
                    akCimbEFT.NoPBI = dataAsal.NoPBI;
                    akCimbEFT.NamaFail = dataAsal.NamaFail;
                    akCimbEFT.SuPekerjaId = dataAsal.SuPekerjaId;
                    akCimbEFT.TarMasuk = dataAsal.TarMasuk;
                    akCimbEFT.UserId = dataAsal.UserId;
                    akCimbEFT.SuPekerjaMasukId = dataAsal.SuPekerjaMasukId;
                    // list of input that cannot be change end

                    foreach (AkCimbEFT1 item in dataAsal.AkCimbEFT1)
                    {
                        var model = _context.AkCimbEFT1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }

                    int berjaya = 0;
                    int gagal = 0;

                    foreach (var cart in _cart.Lines1)
                    {
                        switch (cart.FlStatus)
                        {
                            case 1:
                                berjaya++;
                                break;
                            case 2:
                                gagal++;
                                break;
                            default:
                                gagal++;
                                break;
                        }
                    }
                    var status = 0;
                    if (berjaya > 0)
                    {
                        status = 1;
                        if (gagal > 0)
                        {
                            status = 2;
                        }
                    } else
                    {
                        status=0;
                    }

                    akCimbEFT.FlStatus = status;

                    _context.Entry(dataAsal).State = EntityState.Detached;

                    akCimbEFT.AkCimbEFT1 = _cart.Lines1.ToList();

                    akCimbEFT.UserIdKemaskini = user.UserName;
                    akCimbEFT.TarKemaskini = DateTime.Now;
                    akCimbEFT.SuPekerjaKemaskiniId = pekerjaId;

                    _context.Update(akCimbEFT);

                    foreach (var cart in _cart.Lines1)
                    {

                        AkPV akPV = await _akPVRepo.GetById(cart.AkPVId);
                        if (akPV.IsGanda == false)
                        {
                            switch (cart.FlStatus)
                            {
                                case 1:
                                    akPV.NoCekAtauEFT = akCimbEFT.NoPBI;
                                    akPV.TarCekAtauEFT = akCimbEFT.TarJana;
                                    berjaya++;
                                    break;
                                case 2:
                                    akPV.NoCekAtauEFT = "";
                                    akPV.TarCekAtauEFT = null;
                                    gagal++;
                                    break;
                                default:
                                    akPV.NoCekAtauEFT = "";
                                    akPV.TarCekAtauEFT = null;
                                    gagal++;
                                    break;
                            }
                        }
                        else
                        {
                            var ganda = akPV.AkPVGanda.Where(b => b.NoAkaun == cart.NoCek).FirstOrDefault();

                            switch (cart.FlStatus)
                            {
                                case 1:
                                    ganda.NoCekAtauEFT = akCimbEFT.NoPBI;
                                    ganda.TarCekAtauEFT = akCimbEFT.TarJana;
                                    berjaya++;
                                    break;
                                case 2:
                                    ganda.NoCekAtauEFT = "";
                                    ganda.TarCekAtauEFT = null;
                                    gagal++;
                                    break;
                                default:
                                    ganda.NoCekAtauEFT = "";
                                    ganda.TarCekAtauEFT = null;
                                    gagal++;
                                    break;
                            }
                        }
                        

                        _context.Update(akPV);
                    }
                    //insert applog
                    await AddLogAsync("Ubah", "Ubah Data", akCimbEFT.NoPBI, id, akCimbEFT.Jumlah, pekerjaId);

                    //insert applog end

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkCimbEFTExists(akCimbEFT.Id))
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
            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Delete/5
        [Authorize(Policy = "PV002D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _akCimbEFTRepo.GetByIdIncludeDeletedItems((int)id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            PopulateTable(id);
            return View(akCimbEFT);
        }

        // POST: AkCimbEFT/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "PV002D")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _context.AkCimbEFT.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            //insert applog
            await AddLogAsync("Hapus", obj.NoPBI, obj.NoPBI, id, obj.Jumlah, pekerjaId);
            //insert applog end

            _context.AkCimbEFT.Remove(obj);

            var akCimbEft = await _akCimbEFTRepo.GetById(id);

            foreach (var item in akCimbEft.AkCimbEFT1)
            {
                var akCimbEFT1 = await _akCimbEFT1Repo.GetById(item.Id);

                akCimbEFT1.FlStatus = 0;

                await _akCimbEFT1Repo.Update(akCimbEFT1);
            }

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "PV002R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _akCimbEFTRepo.GetByIdIncludeDeletedItems(id);

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            // Rollback operation

            obj.FlHapus = 0;
            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            obj.SuPekerjaKemaskiniId = pekerjaId;

            _context.AkCimbEFT.Update(obj);

            // Rollback operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoPBI, (int)id, obj.Jumlah, pekerjaId);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkCimbEFTExists(int id)
        {
            return _context.AkCimbEFT.Any(e => e.Id == id);
        }
    }
}
