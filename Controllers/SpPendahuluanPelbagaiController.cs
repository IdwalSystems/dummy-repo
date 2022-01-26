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
    public class SpPendahuluanPelbagaiController : Controller


    {

        public const string modul = "SP001";

        private readonly IRepository<SpPendahuluanPelbagai, int, string> _spPendahuluanPelbagaiRepo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai1, int> _spPendahuluanPelbagai1Repo;
        private readonly ListViewIRepository<SpPendahuluanPelbagai2, int> _spPendahuluanPelbagai2Repo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly IRepository<JSukan, int, string> _sukanRepo;
        private readonly IRepository<JTahapAktiviti, int, string> _tahapAktivitiRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly ApplicationDbContext _context;
        //   private CartPeserta _cart;

        public SpPendahuluanPelbagaiController(
           ApplicationDbContext context,
           IRepository<SpPendahuluanPelbagai, int, string> SpPendahuluanPelbagaiRepository,
           ListViewIRepository<SpPendahuluanPelbagai1, int> SpPendahuluanPelbagai1Repository,
           ListViewIRepository<SpPendahuluanPelbagai2, int> SpPendahuluanPelbagai2Repository,
           IRepository<JNegeri, int, string> negeriRepository,
           IRepository<JSukan, int, string> sukanRepository,
           IRepository<JTahapAktiviti, int, string> tahapAktivitiRepository,
           IRepository<AkCarta, int, string> akCartaRepository,
           IRepository<JKW, int, string> kwRepository

           //     CartPeserta cart
           )
        {
            _spPendahuluanPelbagaiRepo = SpPendahuluanPelbagaiRepository;
            _spPendahuluanPelbagai1Repo = SpPendahuluanPelbagai1Repository;
            _spPendahuluanPelbagai2Repo = SpPendahuluanPelbagai2Repository;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _context = context;
            _negeriRepo = negeriRepository;
            _sukanRepo = sukanRepository;
            _tahapAktivitiRepo = tahapAktivitiRepository;
            //   _cart = cart;
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
            var kw = await _kwRepo.GetById(spPendahuluanPelbagai.JKWId);
            spPendahuluanPelbagai.JKW = kw;
            if (spPendahuluanPelbagai == null)
            {
                return NotFound();
            }
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
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
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
        public async Task<IActionResult> Create(SpPendahuluanPelbagai spPendahuluanPelbagai, int JKWId)
        {

            SpPendahuluanPelbagai m = new SpPendahuluanPelbagai();
            var tahap = _context.JTahapAktiviti.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JTahapId);
            var sukan = _context.JSukan.FirstOrDefault(x => x.Id == spPendahuluanPelbagai.JSukanId);
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            if (ModelState.IsValid)
            {
                if (spPendahuluanPelbagai != null && JKWId != 0)
                {

                    m.JKWId = JKWId;
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
                    m.FlPosting = 0;
                    //m.TarikhPosting = spPermohonanAktiviti.TarikhPosting;
                    //m.FlBatal = 0;
                    m.FlCetak = 0;
                    //m.UserId = user.UserName;
                    m.TarMasuk = DateTime.Now;

                    //m.AkPO1 = _cart.Lines1.ToArray();
                    //m.AkPO2 = _cart.Lines2.ToArray();

                    await _spPendahuluanPelbagaiRepo.Insert(m);

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

            var spPendahuluanPelbagai = await _context.SpPendahuluanPelbagai.FindAsync(id);
            if (spPendahuluanPelbagai == null)

            {
                return NotFound();
            }

            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", spPendahuluanPelbagai.JNegeriId);
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id", spPendahuluanPelbagai.JSukanId);
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id", spPendahuluanPelbagai.JTahapId);
            return View(spPendahuluanPelbagai);
        }

        // POST: SpPermohonanAktiviti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ppn,Penyertaan,Pertandingan,Pengelolaan,ProgramBinaan,JNegeriId,JSukanId,Tarikh,Aktiviti,Tempat,JTahapId,Penyedia,TarSedia,JumKeseluruhan,Penyokong,StatusSokong,TarSokong,JumSokong,Pelulus,StatusLulus,TarLulus,JumLulus,FlPosting,FlCetak,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] SpPendahuluanPelbagai spPendahuluanPelbagai)
        {
            if (id != spPendahuluanPelbagai.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spPendahuluanPelbagai);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["JNegeriId"] = new SelectList(_context.JNegeri, "Id", "Kod", spPendahuluanPelbagai.JNegeriId);
            ViewData["JSukanId"] = new SelectList(_context.JSukan, "Id", "Id", spPendahuluanPelbagai.JSukanId);
            ViewData["JTahapId"] = new SelectList(_context.JTahapAktiviti, "Id", "Id", spPendahuluanPelbagai.JTahapId);
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

            List<SpPendahuluanPelbagai1> spPermohonanAktiviti1Table = _context.SpPendahuluanPelbagai1
                .Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akPO1 = spPermohonanAktiviti1Table;

            List<SpPendahuluanPelbagai2> spPermohonanAktiviti2Table = _context.SpPendahuluanPelbagai2
                //.Include(b => b.AkCarta)
                .Where(b => b.SpPendahuluanPelbagaiId == id)
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

        private bool SpPendahuluanPelbagaiExists(int id)
        {
            return _context.SpPendahuluanPelbagai.Any(e => e.Id == id);
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

