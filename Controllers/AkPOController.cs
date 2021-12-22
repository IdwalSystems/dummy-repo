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
    [Authorize]
    public class AkPOController : Controller
    {
        public const string modul = "TG001";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPO, int> _akPORepo;
        private readonly ListViewIRepository<AkPO1, int> _akPO1Repo;
        private readonly ListViewIRepository<AkPO2, int> _akPO2Repo;
        private readonly IRepository<AkPembekal, int> _akpembekalRepo;
        private readonly IRepository<AkBank, int> _akBankRepo;
        private readonly IRepository<JBank, int> _jbankRepo;
        private readonly IRepository<JNegeri, int> _negeriRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkAkaun, int> _akAkaunRepo;
        private CartPO _cart;

        public AkPOController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IRepository<AkPO, int> AkPORepository,
            ListViewIRepository<AkPO1, int> AkPO1Repository,
            ListViewIRepository<AkPO2, int> AkPO2Repository,
            IRepository<AkPembekal, int> AkPembekalRepository,
            IRepository<AkBank, int> akBankRepository,
            IRepository<JBank, int> JBankRepository,
            IRepository<JNegeri, int> negeriRepository,
            IRepository<JKW, int> kwRepository,
            IRepository<AkAkaun, int> akAkaunRepository,
            CartPO cart
            )
        {
            _context = context;
            _userManager = userManager;
            _akPORepo = AkPORepository;
            _akPO1Repo = AkPO1Repository;
            _akPO2Repo = AkPO2Repository;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akpembekalRepo = AkPembekalRepository;
            _akBankRepo = akBankRepository;
            _jbankRepo = JBankRepository;
            _akAkaunRepo = akAkaunRepository;
            _cart = cart;
        }

        // GET: AkPO
        public async Task<IActionResult> Index()
        {
            var akPO = await _akPORepo.GetAll();
            return View(akPO);
        }

        // GET: AkPO/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;
            if (akPO == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akPO);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPembekal> PembekalList = _context.AkPembekal.OrderBy(b => b.Id).ToList();
            ViewBag.AkPembekal = PembekalList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<AkBank> akBankList = _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToList();
            ViewBag.AkBank = akBankList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW).OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta = akCartaList;

        }

        private void PopulateTable(int? id)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = akPO1Table;

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.AkCarta)
                .Where(b => b.AkPOId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO2 = akPO2Table;
        }
        private void PopulateCart(AkPO akPO)
        {
            List<AkPO1> akPO1Table = _context.AkPO1
                .Include(b => b.AkCarta)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO1 akPO1 in akPO1Table)
            {
                _cart.AddItem1(akPO1.AkPOId,
                                akPO1.AkCartaId,
                               akPO1.Amaun
                               );
            }

            List<AkPO2> akPO2Table = _context.AkPO2
                //.Include(b => b.JPerihal)
                .Where(b => b.AkPOId == akPO.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkPO2 akPO2 in akPO2Table)
            {
                _cart.AddItem2(akPO2.AkPOId,
                               akPO2.Indek,
                               akPO2.Baris,
                               akPO2.Bil,
                               akPO2.NoStok,
                               akPO2.Perihal,
                               akPO2.Kuantiti,
                               akPO2.Unit,
                               akPO2.Harga,
                               akPO2.Amaun);
            }
        }

        // GET: AkPO/Create
        public IActionResult Create()
        {
            PopulateList();
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "KodSykt", "NamaSykt");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: AkPO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkPO akPO, int JKWId)
        {

            AkPO m = new AkPO();
            var pembekal = _context.AkPembekal.FirstOrDefault(x => x.Id == akPO.AkPembekalId);

            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            // get latest no rujukan running number  
            //var kw = _context.JKW.FirstOrDefault(x => x.Id == akPO.JKWId);

            //var kumpulanWang = kw.Kod;
            //var year = DateTime.Now.Year.ToString();
            //var month = DateTime.Now.Month.ToString();
            //string prefix = "RR/IB" + kumpulanWang + year;
            //int x = 1;
            //string noRujukan = prefix + "000000";

            //var LatestNoRujukan = _context.AkPO.Max(x => x.NoPO);
            //if (LatestNoRujukan == null)
            //{
            //    noRujukan = string.Format("{0:" + prefix + "000000}", x);
            //}
            //else
            //{
            //    x = int.Parse(LatestNoRujukan.Substring(12));
            //    x++;
            //    noRujukan = string.Format("{0:" + prefix + "000000}", x);
            //}

            // get latest no rujukan running number end

            if (ModelState.IsValid)
            {
                if (akPO != null && JKWId != 0)
                {

                    m.JKWId = JKWId;
                    m.NoPO = akPO.NoPO;
                    m.Tarikh = akPO.Tarikh;
                    m.TarikhPosting = akPO.TarikhPosting;
                    m.AkPembekal = pembekal;
                    m.Jumlah = akPO.Jumlah;
                    m.FlPosting = akPO.FlPosting;
                    m.Tahun = akPO.Tahun;
                    m.FlBatal = akPO.FlBatal;

                    m.AkPO1 = _cart.Lines1.ToArray();
                    m.AkPO2 = _cart.Lines2.ToArray();

                    await _akPORepo.Insert(m);
                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat Pesanan Tempatan berjaya ditambah";
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akPO);
        }

        // GET: AkPO/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _akPORepo.GetById((int)id);
            var kw = await _kwRepo.GetById(akPO.JKWId);
            akPO.JKW = kw;
            if (akPO == null)
            {
                return NotFound();
            }
            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akPO);
            return View(akPO);
        }

        // POST: AkPO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoPO,Tarikh,TarikhPosting,AkPembekalId,Jumlah,Posting,JKWId,Tahun,Batal")] AkPO akPO)
        {
            if (id != akPO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akPO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkPOExists(akPO.Id))
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
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "Id", akPO.AkPembekalId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akPO.JKWId);
            return View(akPO);
        }

        // GET: AkPO/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akPO = await _context.AkPO
                .Include(a => a.AkPembekal)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akPO == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akPO);
            return View(akPO);
        }

        // POST: AkPO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akPO = await _context.AkPO.FindAsync(id);
            _context.AkPO.Remove(akPO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkPOExists(int id)
        {
            return _context.AkPO.Any(e => e.Id == id);
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
                _cart.Clear1();
                _cart.Clear2();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {
                    _cart.AddItem1(akPO1.AkPOId,
                                akPO1.AkCartaId,
                                    akPO1.Amaun);

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {
                    _cart.AddItem2(akPO2.AkPOId,
                         akPO2.Indek,
                         akPO2.Baris,
                         akPO2.Bil,
                         akPO2.NoStok,
                         akPO2.Perihal,
                         akPO2.Kuantiti,
                         akPO2.Unit,
                         akPO2.Harga,
                         akPO2.Amaun);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {

                    _cart.RemoveItem1(akPO1.AkCartaId);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {

                    _cart.RemoveItem2(akPO2.Indek);
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // Ubah PO1
        public async Task<JsonResult> UpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                AkPO1 data = await _akPO1Repo.GetBy2Id(akPO1.AkPOId, akPO1.AkCartaId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null || akPO1.Amaun != 0)
                {
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akPO1.AkCartaId);
                    akPO1.AkCarta = akCarta;
                    await _akPO1Repo.Insert(akPO1);

                    decimal total = 0;

                    AkPO akPO = await _akPORepo.GetById(akPO1.AkPOId);

                    total = akPO.Jumlah + akPO1.Amaun;

                    akPO.Jumlah = total;

                    await _akPORepo.Update(akPO);
                    await _context.SaveChangesAsync();

                }


                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                if (akPO1 != null)
                {
                    var akT1 = await _context.AkPO1.FirstOrDefaultAsync(x => x.AkCartaId == akPO1.AkCartaId && x.AkPOId == akPO1.AkPOId);
                    _context.AkPO1.Remove(akT1);

                    decimal total = 0;

                    AkPO akPO = await _akPORepo.GetById(akPO1.AkPOId);

                    total = akPO.Jumlah - akT1.Amaun;

                    akPO.Jumlah = total;

                    await _akPORepo.Update(akPO);

                    await _context.SaveChangesAsync();

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkPO1(AkPO1 akPO1)
        {

            try
            {
                _cart.Clear1();

                AkPO1 akT1 = await _akPO1Repo.GetById(akPO1.Id);
                akT1.Amaun = akPO1.Amaun;
                _context.AkPO1.Update(akT1);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart1(AkPO1 akPO1)
        {
            try
            {
                AkPO data = await _context.AkPO.Include(x => x.AkPO1).ThenInclude(x => x.AkCarta).FirstOrDefaultAsync(x => x.Id == akPO1.AkPOId);

                List<AkPO1> akT1 = data.AkPO1.ToList();

                foreach (AkPO1 item in akT1)
                {
                    _cart.AddItem1(item.AkPOId, item.AkCartaId,item.Amaun);
                }


                decimal total = 0;
                foreach (var item in akT1)
                {
                    total += item.Amaun;
                }
                AkPO akPO = await _akPORepo.GetById(akPO1.AkPOId);

                akPO.Jumlah = total;

                await _akPORepo.Update(akPO);
                await _context.SaveChangesAsync();


                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        // Ubah AkPO1 End

        // Ubah AkPO2
        public async Task<JsonResult> UpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                AkPO2 data = await _akPO2Repo.GetById(akPO2.AkPOId);

                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null || akPO2.Amaun != 0)
                {
                    //var jCaraBayar = _context.JCaraBayar.FirstOrDefault(x => x.Id == akTerima2.JCaraBayarId);
                    //akTerima2.JCaraBayar = jCaraBayar;
                    await _akPO2Repo.Insert(akPO2);

                    await _context.SaveChangesAsync();
                }




                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                if (akPO2 != null)
                {
                    var akT2 = await _context.AkPO2.FirstOrDefaultAsync(x => x.Indek == akPO2.Indek && x.AkPOId == akPO2.AkPOId);
                    _context.AkPO2.Remove(akT2);

                    await _context.SaveChangesAsync();

                }



                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkPO2(AkPO2 akPO2)
        {

            try
            {
                _cart.Clear2();

                AkPO2 akT2 = await _akPO2Repo.GetById(akPO2.Id);

                akT2.Amaun = akPO2.Amaun;
                akT2.Indek = akPO2.Indek;
                akT2.Bil = akPO2.Bil;
                akT2.Baris = akPO2.Baris;
                akT2.NoStok = akPO2.NoStok;
                akT2.Perihal = akPO2.Perihal;
                akT2.Kuantiti = akPO2.Kuantiti;
                akT2.Unit = akPO2.Unit;
                akT2.Harga = akPO2.Harga;
                akT2.Amaun = akPO2.Amaun;

                _context.AkPO2.Update(akT2);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart2(AkPO2 akPO2)
        {
            try
            {
                AkPO data = await _context.AkPO.Include(x => x.AkPO2).ThenInclude(x => x.Perihal).FirstOrDefaultAsync(x => x.Id == akPO2.AkPOId);

                List<AkPO2> akT2 = data.AkPO2.ToList();

                foreach (AkPO2 item in akT2)
                {
                    _cart.AddItem2(akPO2.AkPOId,
                         akPO2.Indek,
                         akPO2.Baris,
                         akPO2.Bil,
                         akPO2.NoStok,
                         akPO2.Perihal,
                         akPO2.Kuantiti,
                         akPO2.Unit,
                         akPO2.Harga,
                         akPO2.Amaun);
                }

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }
        //Ubah AkPO2 end

        [HttpPost]
        public JsonResult GetMaklumat(AkPembekal akPembekal)
        {
            try
            {
                var result = _context.AkPembekal.Where(b => b.Id == akPembekal.Id).Include(x => x.JBank).FirstOrDefault();

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }

        // Fungsi Posting
        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkPO akPO = await _context.AkPO.Include(x => x.AkPO1).ThenInclude(x => x.AkCarta).FirstOrDefaultAsync(x => x.Id == id);

                List<AkPO1> akT1 = akPO.AkPO1.ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == akPO.NoPO).FirstOrDefaultAsync();
                if (akAkaun != null)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";

                }
                else
                {
                    //posting operation start here
                    //insert into akAkaun
                    AkAkaun akADebit = new AkAkaun();
                    foreach (AkPO1 item in akT1)
                    {
                        akADebit.NoRujukan = akPO.NoPO;
                        akADebit.JKWId = akPO.JKWId;
                        //akADebit.AkCartaId1 = akPO.AkBankId;
                        akADebit.AkCartaId2 = item.AkCartaId;
                        akADebit.Tarikh = akPO.Tarikh;
                        akADebit.Debit = item.Amaun;
                    }
                    await _akAkaunRepo.Insert(akADebit);

                    //update posting status in akPO
                    akPO.FlPosting = 1;
                    await _akPORepo.Update(akPO);

                    //insert applog

                    //AppLog appLog = new AppLog();

                    //appLog.UserId = user.UserName;
                    //appLog.LgModule = modul + "C";
                    //appLog.LgOperation = "Tambah";
                    //appLog.LgNote = modul + " Pesanan Tempatan - Tambah";
                    //appLog.NoRujukan = AkPO.NoPO;
                    //appLog.Jumlah = akPO.Jumlah;

                    //await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();


                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // posting function end

    }
}
