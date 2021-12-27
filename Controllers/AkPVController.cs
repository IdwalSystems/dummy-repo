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
            IRepository<AkPV, int> akPV,
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
            _akPVRepo = akPV;
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

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4" && b.Kod.Substring(0, 1) == "B")
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
        public async Task<IActionResult> Create(AkPV akPV, int JKWId, int AkPembekalId, int AkBankId, int JCaraBayarId)
        {
            AkPV m = new AkPV();
            var user = await _userManager.GetUserAsync(User);
            var pembekal = new AkPembekal();

            if (AkPembekalId != 0)
            {
                pembekal = _context.AkPembekal.Find(AkPembekalId);

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

            var akPV = await _context.AkPV.FindAsync(id);
            if (akPV == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPV.AkBankId);
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akPV.AkPembekalId);
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod", akPV.JCaraBayarId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPV.JKWId);
            return View(akPV);
        }

        // POST: AkPV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,Tarikh,TarikhTerima,TarikhPosting,NoPV,Jumlah,NoCekAtauEFT,TarCekAtauEFT,Perihal,FlCetak,FlPosting,FlBatal,JKWId,AkBankId,AkPembekalId,JCaraBayarId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkPV akPV)
        {
            if (id != akPV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akPV);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akPV.AkBankId);
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akPV.AkPembekalId);
            ViewData["JCaraBayarId"] = new SelectList(_context.JCaraBayar, "Id", "Kod", akPV.JCaraBayarId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPV.JKWId);
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
