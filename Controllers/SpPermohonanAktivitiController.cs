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
    public class SpPermohonanAktivitiController : Controller


    {
        private readonly IRepository<SpPermohonanAktiviti, int, string> _spPermohonanAktivitiRepo;
        private readonly ListViewIRepository<SpPermohonanAktiviti1, int> _spPermohonanAktiviti1Repo;
        private readonly ListViewIRepository<SpPermohonanAktiviti2, int> _spPermohonanAktiviti2Repo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JSukan, int, string> _sukanRepo;
        private readonly IRepository<JTahapAktiviti, int, string> _tahapAktivitiRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly ApplicationDbContext _context;
        //   private CartPeserta _cart;

        public SpPermohonanAktivitiController(
           ApplicationDbContext context,
           IRepository<SpPermohonanAktiviti, int, string> SpPermohonanAktivitiRepository,
           ListViewIRepository<SpPermohonanAktiviti1, int> SpPermohonanAktiviti1Repository,
           ListViewIRepository<SpPermohonanAktiviti2, int> SpPermohonanAktiviti2Repository,
           IRepository<JNegeri, int, string> negeriRepository,
           IRepository<JSukan, int, string> sukanRepository,
           IRepository<JTahapAktiviti, int, string> tahapAktivitiRepository,
           IRepository<AkCarta, int, string> akCartaRepository,
           IRepository<JKW, int, string> kwRepository

           //     CartPeserta cart
           )
        {
            _spPermohonanAktivitiRepo = SpPermohonanAktivitiRepository;
            _spPermohonanAktiviti1Repo = SpPermohonanAktiviti1Repository;
            _spPermohonanAktiviti2Repo = SpPermohonanAktiviti2Repository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _context = context;
            _negeriRepo = negeriRepository;
            _sukanRepo = sukanRepository;
            _tahapAktivitiRepo = tahapAktivitiRepository;
            //   _cart = cart;
        }

        // GET: SpPermohonanAktiviti
        public async Task<IActionResult> Index(
             string searchString,
             string searchDate1,
             string searchDate2,
             string searchColumn)
        {
            var spPermohonanAktiviti = await _spPermohonanAktivitiRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoPermohonan")
                    {
                        spPermohonanAktiviti = spPermohonanAktiviti.Where(s => s.NoPermohonan.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        spPermohonanAktiviti = spPermohonanAktiviti.Where(x => x.TarSedia >= date1
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

            return View(spPermohonanAktiviti);
        }

        // GET: SpPermohonanAktiviti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPermohonanAktiviti = await _spPermohonanAktivitiRepo.GetById((int)id);
            var kw = await _kwRepo.GetById(spPermohonanAktiviti.JKWId);
            spPermohonanAktiviti.JKW = kw;
            if (spPermohonanAktiviti == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(spPermohonanAktiviti);
        }

        public async Task<IActionResult> Lulus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var negeri = await _negeriRepo.GetById((int)id);
            //var spPermohonanAktiviti = await _context.SpPermohonanAktiviti
            //    .Include(s => s.JNegeri)
            //    .Include(s => s.JSukan)
            //    .Include(s => s.JTahapAktiviti)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var spPermohonanAktiviti = await _context.SpPermohonanAktiviti
                .Include(s => s.JNegeri)
                .Include(s => s.JSukan)
                .FirstOrDefaultAsync(m => m.Id == id);
            spPermohonanAktiviti.JTahapAktiviti = await _tahapAktivitiRepo.GetById(spPermohonanAktiviti.JTahapId);
            if (spPermohonanAktiviti == null)
            {
                return NotFound();
            }

            return View(spPermohonanAktiviti);
        }

        // GET: SpPermohonanAktiviti/Create
        public IActionResult Create()
        {
            PopulateList();
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod");
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id");
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id");
            return View();
        }

        // POST: SpPermohonanAktiviti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ppn,Penyertaan,Pertandingan,Pengelolaan,ProgramBinaan,JNegeriId,JSukanId,Tarikh,Aktiviti,Tempat,JTahapId,Penyedia,TarSedia,JumKeseluruhan,Penyokong,StatusSokong,TarSokong,JumSokong,Pelulus,StatusLulus,TarLulus,JumLulus,FlPosting,FlCetak,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SpPermohonanAktiviti spPermohonanAktiviti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spPermohonanAktiviti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", spPermohonanAktiviti.JNegeriId);
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id", spPermohonanAktiviti.JSukanId);
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id", spPermohonanAktiviti.JTahapId);
            PopulateList();
            return View(spPermohonanAktiviti);
        }

        // GET: SpPermohonanAktiviti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPermohonanAktiviti = await _context.SpPermohonanAktiviti.FindAsync(id);
            if (spPermohonanAktiviti == null)

            {
                return NotFound();
            }

            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", spPermohonanAktiviti.JNegeriId);
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id", spPermohonanAktiviti.JSukanId);
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id", spPermohonanAktiviti.JTahapId);
            return View(spPermohonanAktiviti);
        }

        // POST: SpPermohonanAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ppn,Penyertaan,Pertandingan,Pengelolaan,ProgramBinaan,JNegeriId,JSukanId,Tarikh,Aktiviti,Tempat,JTahapId,Penyedia,TarSedia,JumKeseluruhan,Penyokong,StatusSokong,TarSokong,JumSokong,Pelulus,StatusLulus,TarLulus,JumLulus,FlPosting,FlCetak,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SpPermohonanAktiviti spPermohonanAktiviti)
        {
            if (id != spPermohonanAktiviti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spPermohonanAktiviti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpPermohonanAktivitiExists(spPermohonanAktiviti.Id))
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
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", spPermohonanAktiviti.JNegeriId);
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id", spPermohonanAktiviti.JSukanId);
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id", spPermohonanAktiviti.JTahapId);
            return View(spPermohonanAktiviti);
        }

        // GET: SpPermohonanAktiviti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spPermohonanAktiviti = await _context.SpPermohonanAktiviti
                .Include(s => s.JNegeri)
                .Include(s => s.JSukan)
                .Include(s => s.JTahapAktiviti)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spPermohonanAktiviti == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(spPermohonanAktiviti);
        }

        // POST: SpPermohonanAktiviti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spPermohonanAktiviti = await _context.SpPermohonanAktiviti.FindAsync(id);
            _context.SpPermohonanAktiviti.Remove(spPermohonanAktiviti);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JNegeri> negeriList = _context.JNegeri.OrderBy(b => b.Kod).ToList();
            ViewBag.JNegeri = negeriList;

            List<JSukan> sukanList = _context.JSukan.OrderBy(b => b.Id).ToList();
            ViewBag.JSukan = sukanList;

            List<JTahapAktiviti> tahapAktivitiList = _context.JTahapAktiviti.OrderBy(b => b.Id).ToList();
            ViewBag.JTahapAktiviti = tahapAktivitiList;
        }

        private void PopulateTable(int? id)
        {

            List<SpPermohonanAktiviti1> spPermohonanAktiviti1Table = _context.SpPermohonanAktiviti1
                .Include(b => b.AkCarta)
                .Where(b => b.SpPermohonanAktivitiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = spPermohonanAktiviti1Table;

            List<SpPermohonanAktiviti2> spPermohonanAktiviti2Table = _context.SpPermohonanAktiviti2
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPermohonanAktivitiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO2 = spPermohonanAktiviti2Table;
        }

        //private void PopulateCart(SpPermohonanAktiviti spPermohonanAktiviti)
        //{
        //    List<SpPermohonanAktiviti2> spPermohonanAktiviti2Table = _context.SpPermohonanAktiviti2
        //        //.Include(b => b.JJantina)
        //        .Where(b => b.SpPermohonanAktivitiId == spPermohonanAktiviti.Id)
        //        .OrderBy(b => b.Id)
        //        .ToList();

        //    foreach (SpPermohonanAktiviti2 spPermohonanAktiviti2 in spPermohonanAktiviti2Table)
        //    {
        //        _cart.AddItem2 (spPermohonanAktiviti2.Id,
        //                       spPermohonanAktiviti2.BilAtlL,
        //                       spPermohonanAktiviti2.BilJulL,
        //                       spPermohonanAktiviti2.BilPegL,
        //                       spPermohonanAktiviti2.BilTekL,
        //                       spPermohonanAktiviti2.BilUruL,
        //                       spPermohonanAktiviti2.BilAtlP,
        //                       spPermohonanAktiviti2.BilJulP,
        //                       spPermohonanAktiviti2.BilPegP,
        //                       spPermohonanAktiviti2.BilTekP,
        //                       spPermohonanAktiviti2.BilUruP,
        //                       spPermohonanAktiviti2.JumL,
        //                       spPermohonanAktiviti2.JumP,
        //                       spPermohonanAktiviti2.JumAtl,
        //                       spPermohonanAktiviti2.JumJul,
        //                       spPermohonanAktiviti2.JumPeg,
        //                       spPermohonanAktiviti2.JumTek,
        //                       spPermohonanAktiviti2.JumUru);
        //    }
        //}

        //public JsonResult GetAnItemCartSpPermohonanAktiviti2(SpPermohonanAktiviti2 spPermohonanAktiviti)
        //{

        //    try
        //    {
        //        SpPermohonanAktiviti2 data = _cart.Lines1.Where(x => x.Id == spPermohonanAktiviti.Id).FirstOrDefault();

        //        return Json(new { result = "OK", record = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "ERROR", message = ex.Message });
        //    }
        //}
        //// get an item from cart spPermohonanAktiviti1 end

        ////save cart spPermohonanAktiviti1
        //public JsonResult SaveCartSpPermohonanAktiviti2(SpPermohonanAktiviti2 spPermohonanAktiviti2)
        //{

        //    try
        //    {

        //        var akP1 = _cart.Lines1.Where(x => x.Id == spPermohonanAktiviti2.Id).FirstOrDefault();

        //        //var user = _userManager.GetUserName(User);

        //        if (akP1 != null)
        //        {
        //            _cart.RemoveItem2(spPermohonanAktiviti2.Id);

        //            _cart.AddItem2(spPermohonanAktiviti2.Id,
        //                       spPermohonanAktiviti2.BilAtlL,
        //                       spPermohonanAktiviti2.BilJulL,
        //                       spPermohonanAktiviti2.BilPegL,
        //                       spPermohonanAktiviti2.BilTekL,
        //                       spPermohonanAktiviti2.BilUruL,
        //                       spPermohonanAktiviti2.BilAtlP,
        //                       spPermohonanAktiviti2.BilJulP,
        //                       spPermohonanAktiviti2.BilPegP,
        //                       spPermohonanAktiviti2.BilTekP,
        //                       spPermohonanAktiviti2.BilUruP,
        //                       spPermohonanAktiviti2.JumL,
        //                       spPermohonanAktiviti2.JumP,
        //                       spPermohonanAktiviti2.JumAtl,
        //                       spPermohonanAktiviti2.JumJul,
        //                       spPermohonanAktiviti2.JumPeg,
        //                       spPermohonanAktiviti2.JumTek,
        //                       spPermohonanAktiviti2.JumUru);
        //        }

        //        return Json(new { result = "OK" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "ERROR", message = ex.Message });
        //    }
        //}
        ////save cart spPermohonanAktiviti1 end

        //// get all item from cart spPermohonanAktiviti1
        //public JsonResult GetAllItemCartSpPermohonanAktiviti2(SpPermohonanAktiviti2 spPermohonanAktiviti)
        //{

        //    try
        //    {
        //        List<SpPermohonanAktiviti2> data = _cart.Lines1.ToList();

        //        foreach (SpPermohonanAktiviti2 item in data)
        //        {
        //         //   var jJantina = _context.JJantina.Find(item.Id);

        //          //  item.JJantina = jJantina;
        //        }

        //        return Json(new { result = "OK", record = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = "ERROR", message = ex.Message });
        //    }
        //}
        //// get all item from cart spPermohonanAktiviti1 end

        private bool SpPermohonanAktivitiExists(int id)
        {
            return _context.SpPermohonanAktiviti.Any(e => e.Id == id);
        }

        //public async Task<IActionResult> PrintPdf(int id)
        //{
        //spPermohonanAktiviti spPermohonanAktiviti = await _spPermohonanARepo.GetById(id);

        //var jumlahDalamPerkataan = ("Ringgit Malaysia " + Tools.JumlahDalamPerkataan(spPermohonanAktiviti.Jumlah)).ToUpper();

        //var user = await _userManager.GetUserAsync(User);

        //POPrintModel data = new POPrintModel();

        //CompanyDetails company = new CompanyDetails();
        //data.CompanyDetail = company;
        //data.spPermohonanAktiviti = spPermohonanAktiviti;
        ////data.spPermohonanAktiviti.JNegeri = negeri;
        //data.JumlahDalamPerkataan = jumlahDalamPerkataan;
        //data.Username = user.UserName;

        ////update cetak -> 1
        //spPermohonanAktiviti.FlCetak = 1;
        //await _spPermohonanAktivitiRepo.Update(spPermohonanAktiviti);

        ////insert applog
        //AppLog appLog = new AppLog();

        //appLog.UserId = user.UserName;
        //appLog.LgModule = modul + "P";
        //appLog.LgOperation = "Cetak";
        //appLog.LgNote = modul + " Pesanan Tempatan - Cetak";
        //appLog.NoRujukan = spPermohonanAktiviti.NoPO;
        //appLog.Jumlah = spPermohonanAktiviti.Jumlah;

        //await _appLog.Insert(appLog);
        ////insert applog end

        //await _context.SaveChangesAsync();

        //var actionPDF = new ViewAsPdf("htmlpage")
        //{
        //    FileName = "abc" + ".pdf",
        //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
        //};

        //return new ViewAsPdf("POPrintPdf", data)
        //{
        //    PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
        //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
        //    //CustomSwitches = "--footer-center \"  Tarikh: " +
        //    //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
        //    //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
        //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
        //};
        //}
    }
}

