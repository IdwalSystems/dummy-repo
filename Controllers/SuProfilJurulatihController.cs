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
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class SuProfilJurulatihController : Controller
    {
        public const string modul = "SU002";
        public const string namamodul = "Profil Jurulatih";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<SuProfil, int, string> _suProfilRepo;
        private CartJurulatih _cart;

        public SuProfilJurulatihController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<SuProfil, int, string> suProfilRepository,
            CartJurulatih cart)
        {
            _context = context;
            _userManager = userManager;
            _appLog = appLog;
            _suProfilRepo = suProfilRepository;
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

        // GET: SuProfilJurulatih
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuProfil
                .Include(s => s.AkCarta).Include(s => s.JBahagian).Include(s => s.JKW)
                .Where(s => s.FlKategori == 1);

            if (User.IsInRole("SuperAdmin"))
            {
                applicationDbContext = applicationDbContext.IgnoreQueryFilters();
            }

            return View(await applicationDbContext.ToListAsync());
        }

        private void PopulateTableDetails(int? id)
        {
            List<SuProfil1> table1 = _context.SuProfil1
                .Include(b => b.JSukan)
                .Include(b => b.SuJurulatih)
                .Include(b => b.JCaraBayar)
                .Where(b => b.SuProfilId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.suProfil1 = table1;
        }

        // GET: SuProfilJurulatih/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _suProfilRepo.GetByIdIncludeDeletedItems((int)id);

            if (suProfil == null)
            {
                return NotFound();
            }

            PopulateTableDetails(id);
            return View(suProfil);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();
            ViewBag.AkCarta = akCartaList;

            List<JCaraBayar> carabayarList = _context.JCaraBayar.ToList();
            ViewBag.JCaraBayar = carabayarList;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.Profil1 = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        private void PopulateTableCreate(int status, int month, int year)
        {
            List<SuJurulatih> data = _context.SuJurulatih
                .Include(x => x.JSukan).Include(b => b.JCaraBayar)
                .Where(b => b.FlStatus == status)
                .OrderBy(x => x.JSukanId).ThenBy(x => x.Nama)
                .ToList();

            // check if already create for previous month
            string monthInStr = (month -1).ToString("D2");
            var yearInStr = year.ToString();
            if (month == 1)
            {
                monthInStr = "12";
                yearInStr = (year - 1).ToString();
            }


            var suProfil = _context.SuProfil
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                    .Where(c => c.Bulan == monthInStr && c.Tahun == yearInStr && c.FlKategori == 1)
                    .FirstOrDefault();

            List<SuProfil1> suProfil1Table = new List<SuProfil1>();

            decimal jumlahKeseluruhan = 0;

            if (suProfil != null)
            {
                List<SuProfil1> data2 = _context.SuProfil1
                .Include(x => x.JSukan).Include(x => x.SuJurulatih).ThenInclude(b => b.JCaraBayar)
                .Where(x => x.SuProfilId == suProfil.Id)
                .OrderBy(x => x.JSukanId).ThenBy(x => x.SuJurulatih.Nama)
                .ToList();


                foreach (var item in data2)
                {

                    var Jurulatih = _context.SuJurulatih.FirstOrDefault(b => b.Id == item.SuJurulatihId && b.FlStatus == 1);
                    if (Jurulatih != null)
                    {
                        jumlahKeseluruhan = jumlahKeseluruhan + item.Amaun;

                        suProfil1Table.Add(
                            new SuProfil1
                            {
                                SuJurulatih = item.SuJurulatih,
                                SuJurulatihId = item.SuJurulatihId,
                                JSukan = item.JSukan,
                                JSukanId = item.JSukanId,
                                JCaraBayar = item.SuJurulatih.JCaraBayar,
                                JCaraBayarId = item.SuJurulatih.JCaraBayarId,
                                NoCekEFT = "",
                                TarCekEFT = null,
                                Amaun = item.Amaun,
                                AmaunSebelum = item.Amaun,
                                Tunggakan = 0,
                                Jumlah = item.Amaun
                            });
                    }

                    if (data2.Count() < data.Count())
                    {
                        // check if have balance of Jurulatih not inserted
                        foreach (var item1 in data)
                        {
                            var containsJurulatih = data2.Any(d => d.SuJurulatihId == item1.Id);
                            // if there is, insert into table
                            if (containsJurulatih == false)
                            {
                                suProfil1Table.Add(
                                new SuProfil1
                                {
                                    SuJurulatih = item1,
                                    SuJurulatihId = item1.Id,
                                    JSukan = item1.JSukan,
                                    JSukanId = item1.JSukanId,
                                    JCaraBayar = item1.JCaraBayar,
                                    JCaraBayarId = item1.JCaraBayarId,
                                    NoCekEFT = "",
                                    TarCekEFT = null,
                                    Amaun = 0,
                                    AmaunSebelum = 0,
                                    Tunggakan = 0,
                                    Jumlah = 0
                                });
                            }
                        }
                    }
                    

                } 


            }
            else
            {
                foreach (var item in data)
                {
                    suProfil1Table.Add(
                        new SuProfil1
                        {
                            SuJurulatih = item,
                            SuJurulatihId = item.Id,
                            JSukan = item.JSukan,
                            JSukanId = item.JSukanId,
                            JCaraBayar = item.JCaraBayar,
                            JCaraBayarId = item.JCaraBayarId,
                            NoCekEFT = "",
                            TarCekEFT = null,
                            Amaun = 0,
                            AmaunSebelum = 0,
                            Tunggakan = 0,
                            Jumlah = 0
                        });
                }
            }


            ViewBag.suProfil1 = suProfil1Table;
            ViewBag.Jumlah = jumlahKeseluruhan;
        }


        private void PopulateTableFromCart()
        {
            List<SuProfil1> suProfil1Table = _cart.Lines1
                .ToList();

            foreach (SuProfil1 item in suProfil1Table)
            {
                var suJurulatih = _context.SuJurulatih.Find(item.SuJurulatihId);

                item.SuJurulatih = suJurulatih;

                var jSukan = _context.JSukan.Find(item.JSukanId);

                item.JSukan = jSukan;
            }

            suProfil1Table = suProfil1Table.OrderBy(x => x.JSukanId)
                .ThenBy(x => x.SuJurulatih.Nama).ToList();

            ViewBag.suProfil1 = suProfil1Table;
        }

        private void PopulateCartFromSuJurulatih(int month, int year)
        {
            List<SuJurulatih> suJurulatih = _context.SuJurulatih
                .Include(x => x.JSukan).Include(x => x.JCaraBayar)
                .Where(b => b.FlStatus == 1)
                .OrderBy(x => x.JSukanId)
                .ThenBy(x => x.Nama)
                .ToList();

            // check if already create for previous month
            string monthInStr = (month -1).ToString("D2");
            var yearInStr = year.ToString();
            if (month == 1)
            {
                monthInStr = "12";
                yearInStr = (year - 1).ToString();
            }


            var suProfil = _context.SuProfil
                .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b.JCaraBayar)
                .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                    .Where(c => c.Bulan == monthInStr && c.Tahun == yearInStr && c.FlKategori == 1)
                    .FirstOrDefault();

            if (suProfil != null)
            {
                List<SuProfil1> data2 = _context.SuProfil1
                .Include(x => x.JSukan).Include(x => x.SuJurulatih).ThenInclude(x => x.JCaraBayar)
                .Where(x => x.SuProfilId == suProfil.Id)
                .OrderBy(x => x.JSukanId).ThenBy(x => x.SuJurulatih.Nama)
                .ToList();

                foreach (SuProfil1 item in data2)
                {
                    var Jurulatih = _context.SuJurulatih.FirstOrDefault(b => b.Id == item.SuJurulatihId && b.FlStatus == 1);
                    if (Jurulatih != null)
                    {
                        _cart.AddItem1(0,
                                   item.SuJurulatihId,
                                   item.JSukanId,
                                   item.JCaraBayarId,
                                   "",
                                   null,
                                   item.Amaun,
                                   item.Amaun,
                                   0,
                                   item.Amaun);
                    }

                    if (data2.Count() < suJurulatih.Count())
                    {
                        // check if have balance of Jurulatih not inserted
                        foreach (var item1 in suJurulatih)
                        {
                            var containsJurulatih = data2.Any(d => d.SuJurulatihId == item1.Id);
                            // if there is, insert into table
                            if (containsJurulatih == false)
                            {
                                _cart.AddItem1(0,
                                       item1.Id,
                                       item1.JSukanId,
                                       item1.JCaraBayarId,
                                       "",
                                       null,
                                       0,
                                       0,
                                       0,
                                       0);

                            }
                        }
                    }
                    

                }
            }
            else
            {
                foreach (SuJurulatih item in suJurulatih)
                {
                    _cart.AddItem1(0,
                                   item.Id,
                                   item.JSukanId,
                                   item.JCaraBayarId,
                                   "",
                                   null,
                                   0,
                                   0,
                                   0,
                                   0);
                }
            }



        }

        // on change no PO controller
        [HttpPost]
        public JsonResult JsonGetKod(string year, string month, int bahagianId)
        {
            try
            {
                // get latest no rujukan running number 

                var bahagian = _context.JBahagian.Where(x => x.Id == bahagianId).FirstOrDefault();

                var kw = bahagian.JKWId;

                var result = "J" + bahagian.Kod + "/" + year + "/" + month;

                var IsExistNoRujukan = _context.SuProfil.Where(x => x.NoRujukan == result).FirstOrDefault();

                CartEmpty();
                PopulateCartFromSuJurulatih(int.Parse(month), int.Parse(year));

                List<SuJurulatih> data = _context.SuJurulatih
                .Include(x => x.JSukan)
                .Include(x => x.JCaraBayar)
                .Where(b => b.FlStatus == 1)
                .OrderBy(x => x.JSukanId).ThenBy(x => x.Nama)
                .ToList();

                // check if already create for previous month
                string monthInStr = (int.Parse(month) -1).ToString("D2");
                var yearInStr = year.ToString();
                if (int.Parse(month) == 1)
                {
                    monthInStr = "12";
                    yearInStr = (int.Parse(year) - 1).ToString();
                }


                var suProfil = _context.SuProfil
                    .Include(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.SuProfil1).ThenInclude(b => b.JCaraBayar)
                        .Where(c => c.Bulan == monthInStr && c.Tahun == yearInStr && c.FlKategori == 1)
                        .FirstOrDefault();

                List<SuProfil1> suProfil1Table = new List<SuProfil1>();

                if (suProfil != null)
                {
                    List<SuProfil1> data2 = _context.SuProfil1
                    .Include(x => x.JSukan).Include(x => x.SuJurulatih).ThenInclude(x => x.JCaraBayar)
                    .Where(x => x.SuProfilId == suProfil.Id)
                    .OrderBy(x => x.JSukanId).ThenBy(x => x.SuJurulatih.Nama)
                    .ToList();


                    foreach (var item in data2)
                    {
                        var Jurulatih = _context.SuJurulatih.FirstOrDefault(b => b.Id == item.SuJurulatihId && b.FlStatus == 1);

                        if (Jurulatih != null)
                        {
                            suProfil1Table.Add(
                            new SuProfil1
                            {
                                SuJurulatih = item.SuJurulatih,
                                SuJurulatihId = item.SuJurulatihId,
                                JSukan = item.JSukan,
                                JSukanId = item.JSukanId,
                                JCaraBayar = item.SuJurulatih.JCaraBayar,
                                JCaraBayarId = item.SuJurulatih.JCaraBayarId,
                                NoCekEFT = "",
                                TarCekEFT = null,
                                Amaun = item.Amaun,
                                AmaunSebelum = item.Amaun,
                                Tunggakan = 0,
                                Jumlah = item.Amaun
                            });
                        }
                        if (data2.Count() < data.Count())
                        {
                            // check if have balance of Jurulatih not inserted
                            foreach (var item1 in data)
                            {
                                var containsJurulatih = data2.Any(d => d.SuJurulatihId == item1.Id);
                                // if there is, insert into table
                                if (containsJurulatih == false)
                                {
                                    suProfil1Table.Add(
                                    new SuProfil1
                                    {
                                        SuJurulatih = item1,
                                        SuJurulatihId = item1.Id,
                                        JSukan = item1.JSukan,
                                        JSukanId = item1.JSukanId,
                                        JCaraBayar = item1.JCaraBayar,
                                        JCaraBayarId = item1.JCaraBayarId,
                                        NoCekEFT = "",
                                        TarCekEFT = null,
                                        Amaun = 0,
                                        AmaunSebelum = 0,
                                        Tunggakan = 0,
                                        Jumlah = 0
                                    });
                                }
                            }
                        }
                            

                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        suProfil1Table.Add(
                            new SuProfil1
                            {
                                SuJurulatih = item,
                                SuJurulatihId = item.Id,
                                JSukan = item.JSukan,
                                JSukanId = item.JSukanId,
                                JCaraBayar = item.JCaraBayar,
                                JCaraBayarId = item.JCaraBayarId,
                                NoCekEFT = "",
                                TarCekEFT = null,
                                Amaun = 0,
                                AmaunSebelum = 0,
                                Tunggakan = 0,
                                Jumlah = 0
                            });
                    }
                }

                if (IsExistNoRujukan == null)
                {
                    return Json(new { result = "OK", record = result, kw = kw, table = suProfil1Table });
                }
                else
                {
                    return Json(new { result = "error" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        // GET: SuProfilJurulatih/Create
        [Authorize(Policy = "SU002C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.ToString("MM");
            var bahagian = _context.JBahagian.Where(x => x.Id == 1).FirstOrDefault();

            ViewBag.NoRujukan = "J" + bahagian.Kod + "/" + year + "/" + month;

            PopulateList();
            CartEmpty();
            PopulateTableCreate(1, int.Parse(month), int.Parse(year));
            PopulateCartFromSuJurulatih(int.Parse(month), int.Parse(year));
            return View();
        }

        public JsonResult RemoveSuProfil1(SuProfil1 suProfil1)
        {

            try
            {
                if (suProfil1 != null)
                {

                    _cart.RemoveItem1(suProfil1.SuJurulatihId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // get an item from cart SuProfil1
        public JsonResult GetAnItemCartSuProfil1(SuProfil1 suProfil1)
        {

            try
            {
                SuProfil1 data = _cart.Lines1.
                    Where(x => x.SuJurulatihId == suProfil1.SuJurulatihId).FirstOrDefault();

                var suJurulatih = _context.SuJurulatih.FirstOrDefault(x => x.Id == data.SuJurulatihId);

                return Json(new { result = "OK", record = data, suJurulatih = suJurulatih });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get an item from cart SuProfil1 end

        //save cart SuProfil1
        public JsonResult SaveCartSuProfil1(
            SuProfil1 suProfil1)
        {
            try
            {

                var suP1 = _cart.Lines1.FirstOrDefault(x => x.SuJurulatihId == suProfil1.SuJurulatihId);

                var jSukanId = suP1.JSukanId;

                if (suP1 != null)
                {

                    _cart.RemoveItem1(suP1.SuJurulatihId);

                    _cart.AddItem1(suProfil1.SuProfilId,
                        suProfil1.SuJurulatihId,
                        jSukanId,
                        suProfil1.JCaraBayarId,
                        suProfil1.NoCekEFT,
                        suProfil1.TarCekEFT,
                        suProfil1.Amaun,
                        suProfil1.AmaunSebelum,
                        suProfil1.Tunggakan,
                        suProfil1.Jumlah);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //save cart suProfil1 end

        // get all item from cart suProfil1
        public JsonResult GetAllItemCartSuProfil1()
        {

            try
            {
                List<SuProfil1> data = _cart.Lines1.ToList();

                foreach (SuProfil1 item in data)
                {
                    var suJurulatih = _context.SuJurulatih.Find(item.SuJurulatihId);

                    item.SuJurulatih = suJurulatih;

                    var jSukan = _context.JSukan.Find(item.JSukanId);

                    item.JSukan = jSukan;

                    var jCaraBayar = _context.JCaraBayar.Find(item.JCaraBayarId);

                    item.JCaraBayar = jCaraBayar;
                }

                data = data.OrderBy(x => x.JSukanId)
                    .ThenBy(x => x.SuJurulatih.Nama).ToList();

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // get all item from cart SuProfil1 end

        // POST: SuProfilJurulatih/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SU002C")]
        public async Task<IActionResult> Create(SuProfil suProfil, int JKWId, int JBahagianId, int AkCartaId )
        {
            SuProfil m = new SuProfil();
            var IsExistNoRujukan = _context.SuProfil.Where(x => x.NoRujukan == suProfil.NoRujukan).FirstOrDefault();

            // get latest no rujukan running number 
            var year = DateTime.Now.Year.ToString();
            var month = DateTime.Now.ToString("MM");
            var bahagian = _context.JBahagian.Where(x => x.Id == 1).FirstOrDefault();

            // check if Tahun, Bulan ,JBahagianId, JKWId already exist or not 
            if (IsExistNoRujukan != null)
            {
                TempData[SD.Error] = "Data bagi Kump. Wang dan Bahagian telah wujud bagi Tahun dan Bulan ini.";
                PopulateList();
                CartEmpty();

                ViewBag.NoRujukan = "J" + bahagian.Kod + "/" + year + "/" + month;

                PopulateCartFromSuJurulatih(int.Parse(month), int.Parse(year));
                PopulateTableFromCart();
                return View(suProfil);
            }
            // check end

            var user = await _userManager.GetUserAsync(User);


            if (ModelState.IsValid)
            {
                if (suProfil != null && JKWId != 0 && JBahagianId != 0 && AkCartaId != 0)
                {
                    m.FlKategori = 1;
                    m.Tahun = suProfil.Tahun;
                    m.Bulan = suProfil.Bulan;
                    m.NoRujukan = suProfil.NoRujukan;
                    m.JKWId = JKWId;
                    m.JBahagianId = JBahagianId;
                    m.AkCartaId = AkCartaId;
                    m.Jumlah = suProfil.Jumlah;
                    m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    m.SuProfil1 = _cart.Lines1.ToArray();

                    await _suProfilRepo.Insert(m);

                    //insert applog
                    await AddLogAsync("Tambah", m.NoRujukan, m.NoRujukan, 0, suProfil.Jumlah);
                    //insert applog end
                    await _suProfilRepo.Save();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No Rujukan adalah " + m.NoRujukan;

                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateList();
            CartEmpty();
            PopulateCartFromSuJurulatih(int.Parse(month), int.Parse(year));
            PopulateTableFromCart();
            return View(suProfil);
        }

        // GET: SuProfilJurulatih/Edit/5
        [Authorize(Policy = "SU002E")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _suProfilRepo.GetById((int)id);

            if (suProfil == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTableDetails(id);
            PopulateCartFromDb(suProfil);
            return View(suProfil);
        }

        private void PopulateCartFromDb(SuProfil suProfil)
        {
            List<SuProfil1> table1 = _context.SuProfil1
                .Include(b => b.JSukan)
                .Include(b => b.SuJurulatih)
                .Where(b => b.SuProfilId == suProfil.Id)
                .OrderBy(b => b.Id)
                .ToList();

            foreach (SuProfil1 item in table1)
            {
                _cart.AddItem1(item.SuProfilId,
                                item.SuJurulatihId,
                                item.JSukanId,
                                item.JCaraBayarId,
                                item.NoCekEFT,
                                item.TarCekEFT,
                                item.Amaun,
                                item.AmaunSebelum,
                                item.Tunggakan,
                                item.Jumlah);
            }

        }

        // POST: SuProfilJurulatih/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "SU002E")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SuProfil suProfil, int JKWId, int JBahagianId, int AkCartaId)
        {
            if (id != suProfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    SuProfil dataAsal = await _suProfilRepo.GetById(id);

                    // list of input that cannot be change
                    suProfil.FlKategori = dataAsal.FlKategori;
                    suProfil.Tahun = dataAsal.Tahun;
                    suProfil.Bulan = dataAsal.Bulan;
                    suProfil.NoRujukan = dataAsal.NoRujukan;
                    //suProfil.AkCartaId = dataAsal.AkCartaId;
                    suProfil.JKWId = dataAsal.JKWId;
                    //suProfil.JBahagianId = dataAsal.JBahagianId;
                    suProfil.TarMasuk = dataAsal.TarMasuk;
                    suProfil.UserId = dataAsal.UserId;
                    suProfil.FlCetak = 0;
                    // list of input that cannot be change end

                    foreach (SuProfil1 item in dataAsal.SuProfil1)
                    {
                        var model = _context.SuProfil1.FirstOrDefault(b => b.Id == item.Id);
                        if (model != null)
                        {
                            _context.Remove(model);
                        }
                    }
                    decimal jumlahAsal = dataAsal.Jumlah;
                    _context.Entry(dataAsal).State = EntityState.Detached;

                    suProfil.SuProfil1 = _cart.Lines1.ToList();

                    suProfil.UserIdKemaskini = user.UserName;
                    suProfil.TarKemaskini = DateTime.Now;

                    _context.Update(suProfil);
                    // insert applog
                    if (jumlahAsal != suProfil.Jumlah)
                    {
                        await AddLogAsync("Ubah", "RM" + Convert.ToDecimal(jumlahAsal).ToString("#,##0.00") + " -> RM" +
                            Convert.ToDecimal(suProfil.Jumlah).ToString("#,##0.00"), suProfil.NoRujukan, id, suProfil.Jumlah);

                    }
                    else
                    {
                        await AddLogAsync("Ubah", "Ubah Data", suProfil.NoRujukan, id, suProfil.Jumlah);
                    }
                    //insert applog end


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuProfilExists(suProfil.Id))
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
            PopulateTableFromCart();
            return View(suProfil);
        }

        // GET: SuProfilJurulatih/Delete/5
        [Authorize(Policy = "SU002D")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suProfil = await _suProfilRepo.GetByIdIncludeDeletedItems((int)id);

            if (suProfil == null)
            {
                return NotFound();
            }

            PopulateTableDetails(id);
            return View(suProfil);
        }

        // POST: SuProfilJurulatih/Delete/5
        [Authorize(Policy = "SU002D")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _context.SuProfil.FindAsync(id);

            var user = await _userManager.GetUserAsync(User);

            obj.UserIdKemaskini = user.UserName;
            obj.TarKemaskini = DateTime.Now;
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            obj.FlCetak = 0;
            _context.SuProfil.Update(obj);

            //insert applog
            await AddLogAsync("Hapus", obj.NoRujukan, obj.NoRujukan, id, obj.Jumlah);
            //insert applog end

            _context.SuProfil.Remove(obj);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";

            return RedirectToAction(nameof(Index));
        }

        // POST: AkPV/Cancel/5
        [Authorize(Policy = "SU002R")]
        public async Task<IActionResult> RollBack(int id)
        {
            var obj = await _suProfilRepo.GetByIdIncludeDeletedItems(id);
            // check if already posting redirect back
            if (obj.FlPosting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            // check if already have same no rujukan
            if (SuProfilExistsByNoRujukan(obj.NoRujukan) == true)
            {
                TempData[SD.Error] = "No Rujukan bagi data ini telah wujud..!";
                return RedirectToAction(nameof(Index));
            }
            // check end

            // Batal operation

            obj.FlHapus = 0;
            obj.FlCetak = 0;
            _context.SuProfil.Update(obj);

            // Batal operation end

            //insert applog
            await AddLogAsync("Rollback", "Rollback Data", obj.NoRujukan, (int)id, obj.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dikembalikan..!";
            return RedirectToAction(nameof(Index));
        }

        // printing SuProfil
        [Authorize(Policy = "SU002P")]
        public async Task<IActionResult> PrintPdf(int id)
        {
            SuProfil obj = await _suProfilRepo.GetByIdIncludeDeletedItems(id);

            string jumlahDalamPerkataan;

            if (obj.Jumlah < 0)
            {
                jumlahDalamPerkataan = ("Kurangan Ringgit Malaysia " + Tools.JumlahDalamPerkataan(0 - obj.Jumlah)).ToUpper();
            }
            else
            {
                jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(obj.Jumlah)).ToUpper();
            }

            var user = await _userManager.GetUserAsync(User);

            SuProfilAtletPrintModel data = new SuProfilAtletPrintModel();

            CompanyDetails company = new CompanyDetails();
            data.CompanyDetail = company;
            data.SuProfil = obj;
            data.JumlahDalamPerkataan = jumlahDalamPerkataan;
            data.Username = user.UserName;

            dynamic dyModel = new ExpandoObject();
            dyModel.SuProfil = obj;
            dyModel.SuProfil1Grouped = obj.SuProfil1.GroupBy(p => p.JSukan.Perihal);
            dyModel.JumlahDalamPerkataan = jumlahDalamPerkataan;
            dyModel.Username = user.UserName;
            dyModel.CompanyDetail = company;

            //update cetak -> 1
            obj.FlCetak = 1;
            await _suProfilRepo.Update(obj);

            //insert applog
            await AddLogAsync("Cetak", "Cetak Data", obj.NoRujukan, id, obj.Jumlah);

            //insert applog end

            await _context.SaveChangesAsync();

            return new ViewAsPdf("SuProfilJurulatihPrintPdf", dyModel)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing SuProfil end

        // posting function
        [Authorize(Policy = "SU002T")]
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                SuProfil obj = await _suProfilRepo.GetById((int)id);

                //check for print
                if (obj.FlCetak == 0)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal diluluskan. Sila cetak data dahulu sebelum menjalani operasi ini.";
                    return RedirectToAction(nameof(Index));
                }
                //check for print end

                List<SuProfil1> suProfil1 = obj.SuProfil1.ToList();

                // check for zero amaun
                foreach (SuProfil1 item in suProfil1)
                {
                    if (item.Amaun == 0)
                    {
                        TempData[SD.Error] = "Data gagal diluluskan. Terdapat Amaun Yang Tidak Diisi.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                // check for zero amaun end

                //posting operation start here

                //update posting status in akPO
                obj.FlPosting = 1;
                obj.TarikhPosting = DateTime.Now;
                await _suProfilRepo.Update(obj);

                //insert applog
                await AddLogAsync("Posting", "Posting Data", obj.NoRujukan, (int)id, obj.Jumlah);

                //insert applog end

                await _context.SaveChangesAsync();

                TempData[SD.Success] = "Data berjaya diluluskan.";

                return RedirectToAction(nameof(Index));

            }
        }
        // posting function end

        // unposting function
        [Authorize(Policy = "SU002UT")]
        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                SuProfil obj = await _suProfilRepo.GetById((int)id);

                //check
                // dah ada baucer atau tidak
                foreach (var suProfil in obj.SuProfil1)
                {
                    var akPV = await _context.AkPV.Where(b => b.SuProfilId == id).FirstOrDefaultAsync();

                    if (akPV != null)
                    {
                        //duplicate id error
                        TempData[SD.Error] = "Batal kelulusan tidak dibenarkan. Terlibat dengan No PV " + akPV.NoPV;
                        return RedirectToAction(nameof(Index));
                    }
                }
                //


                //unposting operation start here

                //update posting status in akPOLaras
                obj.FlPosting = 0;
                obj.TarikhPosting = null;
                await _suProfilRepo.Update(obj);

                //insert applog
                await AddLogAsync("UnPosting", "UnPosting Data", obj.NoRujukan, (int)id, obj.Jumlah);

                //insert applog end

                await _context.SaveChangesAsync();

                TempData[SD.Success] = "Data berjaya batal kelulusan.";
                //unposting operation end

                return RedirectToAction(nameof(Index));
            }

        }
        // unposting function end

        private bool SuProfilExistsByNoRujukan(string noRujukan)
        {
            return _context.SuProfil.Where(e => e.NoRujukan == noRujukan).Any();
        }

        private bool SuProfilExists(int id)
        {
            return _context.SuProfil.Any(e => e.Id == id);
        }
    }
}
