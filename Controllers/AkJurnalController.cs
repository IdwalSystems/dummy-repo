using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    public class AkJurnalController : Controller
    {
        public const string modul = "JU001";
        public const string namamodul = "Baucer Jurnal";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkJurnal, int, string> _akJurnalRepo;
        private readonly IRepository<JKW, int, string> _jKWRepo;
        private readonly ListViewIRepository<AkJurnal1, int> _akJurnal1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVot;
        private CartJurnal _cart;

        public AkJurnalController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkJurnal, int, string> akJurnalRepository,
            IRepository<JKW, int, string> jKWRepository,
            ListViewIRepository<AkJurnal1, int> akJurnal1Repository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IRepository<AbBukuVot, int, string> abBukuVotRepository,
            CartJurnal cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akJurnalRepo = akJurnalRepository;
            _jKWRepo = jKWRepository;
            _akJurnal1Repo = akJurnal1Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _abBukuVot = abBukuVotRepository;
            _cart = cart;
        }
        private string GetKod(AkJurnal data)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == data.JKWId);

            var kumpulanWang = kw.Kod;
            //var year = DateTime.Now.Year.ToString();
            var year = data.Tarikh.Year;
            string prefix = year +"/"+ kumpulanWang+"/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkJurnal
                .Where(x => x.NoJurnal.Substring(0,9) == prefix)
                .Max(x => x.NoJurnal);
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
        public JsonResult JsonGetKod(AkJurnal data)
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
                    result = GetKod(data);
                }
                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkCarta> cartaList = _context.AkCarta.Where(x=>x.JParas.Kod=="4").OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta = cartaList;

        }
        private void PopulateTable(int? id)
        {
            List<AkJurnal1> akJurnal1Table = _context.AkJurnal1
                .Include(b => b.AkCarta)
                .Where(b => b.AkJurnalId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akJurnal1 = akJurnal1Table;
        }
        private void PopulateCart(AkJurnal akJurnal)
        {
            List<AkJurnal1> akJurnal1Table = _context.AkJurnal1
                .Include(b => b.AkCarta)
                .Where(b => b.AkJurnalId == akJurnal.Id)
                .OrderBy(b => b.Id)
                .ToList();
            foreach (AkJurnal1 akJurnal1 in akJurnal1Table)
            {
                _cart.AddItem1(
                    akJurnal1.AkJurnalId, 
                    akJurnal1.Indeks, 
                    akJurnal1.AkCartaId, 
                    akJurnal1.Debit, 
                    akJurnal1.Kredit
                    );
            }
        }
        // GET: AkJurnal
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchKw,
            string searchColumn)
        {
            //populate search option

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            List<SelectListItem> kwSelect = new();
            kwSelect.Add(new SelectListItem() { Text = "-- Pilih Kumpulan Wang --", Value = "" });
            foreach (var q in kwList)
            {
                kwSelect.Add(new SelectListItem() { Text = q.Kod + " - " + q.Perihal, Value = q.Kod });
            }
            if (!String.IsNullOrEmpty(searchKw))
            {
                ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", searchKw);
            }
            else
            {
                ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", "");
            }

            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No Jurnal", Value = "NoJurnal" });
            columnList.Add(new SelectListItem() { Text = "Kumpulan Wang", Value = "KW" });
            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akJurnal = await _akJurnalRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(searchKw)||
                (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoJurnal")
                    {
                        akJurnal = akJurnal.Where(s => s.NoJurnal.ToUpper().Contains(searchString.ToUpper()));
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
                        akJurnal = akJurnal.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                if (!String.IsNullOrEmpty(searchKw))
                {
                    if (searchColumn == "KW")
                    {
                        akJurnal = akJurnal.Where(s => s.JKW.Kod == searchKw);
                    }
                    ViewBag.SearchKw = new SelectList(kwSelect, "Value", "Text", searchKw);
                }
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }
            return View(akJurnal);
        }

        // GET: AkJurnal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _akJurnalRepo.GetById((int)id);
            akJurnal.JKW = await _jKWRepo.GetById(akJurnal.JKWId);

            if (akJurnal == null)
            {
                return NotFound();
            }
            PopulateList();
            PopulateTable(id);
            return View(akJurnal);
        }

        // GET: AkJurnal/Create
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            return View();
        }

        // POST: AkJurnal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkJurnal akJurnal, int JKWId, decimal JumDebit, decimal JumKredit)
        {
            AkJurnal m = new AkJurnal();
            var user = await _userManager.GetUserAsync(User);

            decimal debit = 0;
            decimal kredit = 0;
            foreach (var q in _cart.Lines1.ToArray())
            {
                debit += q.Debit;
                kredit += q.Kredit;
            };

            if(debit == kredit)
            {
                if (ModelState.IsValid)
                {
                    string noRujukan = GetKod(akJurnal);
                    if (akJurnal != null && JKWId != 0)
                    {
                        m.JKWId = akJurnal.JKWId;
                        m.NoJurnal = noRujukan;
                        m.Tarikh = akJurnal.Tarikh;
                        m.JumDebit = debit;
                        m.JumKredit = kredit;
                        m.Catatan1 = akJurnal.Catatan1;
                        m.Catatan2 = akJurnal.Catatan2;
                        m.Catatan3 = akJurnal.Catatan3;
                        m.Catatan4 = akJurnal.Catatan4;
                        m.Posting = akJurnal.Posting;
                        m.Cetak = akJurnal.Cetak;
                        m.Batal = akJurnal.Batal;
                        m.AkJurnal1 = _cart.Lines1.OrderBy(x=>x.Indeks).ToList();
                        m.UserId = user.UserName;
                        m.TarMasuk = DateTime.Now;

                        await _akJurnalRepo.Insert(m);
                        await AddLogAsync("Tambah", noRujukan, kredit);
                        await _context.SaveChangesAsync();

                        CartEmpty();
                        TempData[SD.Success] = "Maklumat berjaya ditambah. No jurnal adalah " + noRujukan;
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                TempData[SD.Error] = "Pastikan jumlah debit = jumlah kredit";
            }

            PopulateList();
            return View(akJurnal);
        }

        // GET: AkJurnal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _akJurnalRepo.GetById((int)id);
            if (akJurnal.Posting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            akJurnal.JKW = await _jKWRepo.GetById(akJurnal.JKWId);

            if (akJurnal == null)
            {
                return NotFound();
            }

            CartEmpty();
            PopulateList();
            PopulateTable(id);
            PopulateCart(akJurnal);
            return View(akJurnal);
        }

        // POST: AkJurnal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkJurnal akJurnal)
        {
            if (id != akJurnal.Id)
            {
                return NotFound();
            }
            if (akJurnal.Posting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }

            decimal debit = 0, kredit = 0;
            var akj1 = _cart.Lines1;
            foreach (var q in akj1)
            {
                debit += q.Debit;
                kredit += q.Kredit;
            };
            if (debit==kredit)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.GetUserAsync(User);
                        AkJurnal akJurnalAsal = await _akJurnalRepo.GetById(id);
                        akJurnalAsal.AkJurnal1 = _akJurnal1Repo.GetAll(id).Result.ToList();
                        foreach(AkJurnal1 item in akJurnalAsal.AkJurnal1)
                        {
                            var model = _context.AkJurnal1.FirstOrDefault(q => q.Id == item.Id);
                            if(model != null)
                            {
                                _context.Remove(model);
                            }
                        }
                        _context.Entry(akJurnalAsal).State = EntityState.Detached;

                        akJurnal.AkJurnal1 = _cart.Lines1.OrderBy(q => q.Indeks).ToList();
                        akJurnal.UserIdKemaskini = user.UserName;
                        akJurnal.TarKemaskini = DateTime.Now;

                        _context.Update(akJurnal);
                        await AddLogAsync("Ubah", akJurnal.NoJurnal, kredit);
                        await _context.SaveChangesAsync();
                        TempData[SD.Success] = "Maklumat berjaya diubah. No jurnal - " + akJurnal.NoJurnal;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkJurnalExists(akJurnal.Id))
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
            }
            else
            {
                TempData[SD.Error] = "Pastikan jumlah debit = jumlah kredit";
            }
            PopulateList();
            PopulateTable(id);
            return View(akJurnal);
        }

        // GET: AkJurnal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _context.AkJurnal
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);

            PopulateTable(id);
            if (akJurnal == null)
            {
                return NotFound();
            }

            return View(akJurnal);
        }

        // POST: AkJurnal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akJurnal = await _context.AkJurnal.FindAsync(id);
            if (akJurnal.Posting == 1)
            {
                TempData[SD.Error] = "Akses tidak dibenarkan..!";
                return RedirectToAction(nameof(Index));
            }
            _context.AkJurnal.Remove(akJurnal);
            await AddLogAsync("Hapus", akJurnal.NoJurnal, akJurnal.JumKredit);
            await _context.SaveChangesAsync();
            TempData[SD.Success] = "Data berjaya dihapuskan..!";
            return RedirectToAction(nameof(Index));
        }

        private bool AkJurnalExists(int id)
        {
            return _context.AkJurnal.Any(e => e.Id == id);
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

        public JsonResult SaveAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal debit = 0;
                decimal kredit = 0;
                var data = Json(new { });
                if (akJurnal1 != null)
                {
                    _cart.AddItem1(
                        akJurnal1.AkJurnalId,
                        akJurnal1.Indeks, 
                        akJurnal1.AkCartaId, 
                        akJurnal1.Debit, 
                        akJurnal1.Kredit
                        );
                }
                List<AkJurnal1> list = new();
                list = _cart.Lines1.ToList();
                foreach (AkJurnal1 l in list)
                {
                    debit += l.Debit;
                    kredit += l.Kredit;
                }
                data = Json(new { debit = debit, kredit = kredit });
                return Json(new { result = "OK", record = data});
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult RemoveAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal debit = 0;
                decimal kredit = 0;
                var data = Json(new { });
                if (akJurnal1 != null)
                {
                    _cart.RemoveItem1(akJurnal1.AkCartaId,akJurnal1.Indeks);
                }
                List<AkJurnal1> list = new();
                list = _cart.Lines1.ToList();
                foreach (AkJurnal1 l in list)
                {
                    debit += l.Debit;
                    kredit += l.Kredit;
                }
                data = Json(new { debit = debit, kredit = kredit });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> InsertUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal debit = 0;
                decimal kredit = 0;
                var data = Json(new { });
                if (akJurnal1 != null || akJurnal1.Debit != 0 || akJurnal1.Kredit !=0)
                {
                    var akCarta = _context.AkCarta.FirstOrDefault(x => x.Id == akJurnal1.AkCartaId);
                    akJurnal1.AkCarta = akCarta;
                    await _akJurnal1Repo.Insert(akJurnal1);

                    AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                    debit = akJurnal.JumDebit + akJurnal1.Debit;
                    kredit = akJurnal.JumKredit + akJurnal1.Kredit;
                    akJurnal.JumDebit = debit;
                    akJurnal.JumKredit = kredit;

                    await _akJurnalRepo.Update(akJurnal);
                    await _context.SaveChangesAsync();
                }
                data = Json(new { debit = debit, kredit = kredit });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> RemoveUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                decimal debit = 0;
                decimal kredit = 0;
                var data = Json(new { });
                if (akJurnal1 != null)
                {
                    var akJ1 = await _context.AkJurnal1.FirstOrDefaultAsync(
                        x => x.AkCartaId == akJurnal1.AkCartaId 
                        && x.AkJurnalId == akJurnal1.AkJurnalId
                        && x.Id == akJurnal1.Id);
                    _context.AkJurnal1.Remove(akJ1);

                    AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                    debit = akJurnal.JumDebit - akJ1.Debit;
                    kredit = akJurnal.JumKredit - akJ1.Kredit;
                    akJurnal.JumDebit = debit;
                    akJurnal.JumKredit = kredit;

                    await _akJurnalRepo.Update(akJurnal);
                    await _context.SaveChangesAsync();
                }
                data = Json(new { debit = debit, kredit = kredit });
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> UpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                AkJurnal1 data = await _akJurnal1Repo.GetBy2Id(akJurnal1.AkJurnalId, akJurnal1.Id);
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> SaveUpdateAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                _cart.Clear1();

                AkJurnal1 akJ1 = await _akJurnal1Repo.GetById(akJurnal1.Id);
                akJ1.Debit = akJurnal1.Debit;
                akJ1.Kredit = akJurnal1.Kredit;
                _context.AkJurnal1.Update(akJ1);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCart1(AkJurnal1 akJurnal1)
        {
            try
            {
                AkJurnal data = await _context.AkJurnal
                    .Include(x => x.AkJurnal1)
                    .ThenInclude(x=> x.AkCarta)
                    .FirstOrDefaultAsync(x => x.Id == akJurnal1.AkJurnalId);

                List<AkJurnal1> akJ1 = data.AkJurnal1.ToList();

                foreach (AkJurnal1 item in akJ1)
                {
                    _cart.AddItem1(item.AkJurnalId, item.Indeks, item.AkCartaId, item.Debit, item.Kredit);
                }

                decimal debit = 0;
                decimal kredit = 0;
                foreach (var item in akJ1)
                {
                    debit += item.Debit;
                    kredit += item.Kredit;
                }
                AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                akJurnal.JumDebit = debit;
                akJurnal.JumKredit = kredit;

                await _akJurnalRepo.Update(akJurnal);
                await _context.SaveChangesAsync();

                return Json(new { result = "OK", data = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAnItemCartAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                AkJurnal1 data = _cart.Lines1.Where(x => x.AkCartaId == akJurnal1.AkCartaId&& x.Indeks == akJurnal1.Indeks).FirstOrDefault();
                return Json(new { result = "OK", record = data });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult SaveCartAkJurnal1(AkJurnal1ViewModel akJurnal1)
        {
            try
            {
                var akJ1 = _cart.Lines1.Where(x => x.AkCartaId == akJurnal1.AkCartaId).FirstOrDefault();

                if (akJ1 != null)
                {
                    _cart.RemoveItem1(akJurnal1.AkCartaId, akJurnal1.IndeksLama);
                    _cart.AddItem1(
                        akJurnal1.AkJurnalId,
                        akJurnal1.IndeksBaru,
                        akJurnal1.AkCartaId,
                        akJurnal1.Debit,
                        akJurnal1.Kredit
                        );
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public JsonResult GetAllItemCartAkJurnal1(AkJurnal1 akJurnal1)
        {
            try
            {
                List<AkJurnal1> data = _cart.Lines1.OrderBy(x=>x.Indeks).ToList();
                foreach (AkJurnal1 item in data)
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


        private async Task AddLogAsync(string operasi, string rujukan, decimal jumlah)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.Jumlah = jumlah;

            if (operasi == "Tambah")
            {
                appLog.LgModule = modul + "C";
                appLog.LgOperation = "Tambah";
                appLog.LgNote = modul + " " + namamodul + " - Tambah";
            }
            else if (operasi == "Hapus")
            {
                appLog.LgModule = modul + "D";
                appLog.LgOperation = "Hapus";
                appLog.LgNote = modul + " " + namamodul + " - Hapus";
            }
            else if (operasi == "Ubah")
            {
                appLog.LgModule = modul + "E";
                appLog.LgOperation = "Ubah";
                appLog.LgNote = modul + " " + namamodul + " - Ubah";
            }
            else if (operasi == "Posting")
            {
                appLog.LgModule = modul + "T";
                appLog.LgOperation = "Posting";
                appLog.LgNote = modul + " " + namamodul + " - Posting";
            }
            else if (operasi == "UnPosting")
            {
                appLog.LgModule = modul + "UT";
                appLog.LgOperation = "UnPosting";
                appLog.LgNote = modul + " " + namamodul + " - UnPosting";
            }
            else if (operasi == "Cetak")
            {
                appLog.LgModule = modul + "P";
                appLog.LgOperation = "Cetak";
                appLog.LgNote = modul + " " + namamodul + " - Cetak";
            }
            await _appLog.Insert(appLog);
        }

        public async Task<IActionResult> Posting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkJurnal akJurnal = await _context.AkJurnal.Include(x => x.AkJurnal1).FirstOrDefaultAsync(x => x.Id == id);
                List<AkJurnal1> akJ1 = akJurnal.AkJurnal1.OrderBy(x=>x.Indeks).ToList();

                var akAkaun = await _context.AkAkaun.Where(x => x.NoRujukan == "JR/"+akJurnal.NoJurnal).FirstOrDefaultAsync();
                if (akAkaun != null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";
                }
                else
                {
                    //posting operation start here
                    //insert into akAkaun
                    int currentIdx = 0;
                    decimal currentDebit = 0;
                    foreach (AkJurnal1 debit1 in akJ1.Where(z => z.Debit > 0))
                    {
                        currentDebit = debit1.Debit;
                        foreach (AkJurnal1 kredit1 in akJ1.Where(z => z.Kredit > 0&&z.Indeks>currentIdx&&currentDebit>0))
                        {
                            AkAkaun akADebit = new AkAkaun();
                            akADebit.NoRujukan = "JR/" + akJurnal.NoJurnal;
                            akADebit.JKWId = akJurnal.JKWId;
                            akADebit.Tarikh = akJurnal.Tarikh;
                            akADebit.AkCartaId1 = debit1.AkCartaId;
                            akADebit.Debit = kredit1.Kredit;
                            akADebit.AkCartaId2 = kredit1.AkCartaId;
                            akADebit.Kredit = 0;
                            try
                            {
                                await _akAkaunRepo.Insert(akADebit);
                                currentDebit -= kredit1.Kredit;
                            }
                            catch
                            {
                                TempData[SD.Error] = "Data gagal dikemaskini ke lejar.";
                            }
                            finally
                            {
                                akADebit = new AkAkaun();
                                akADebit.NoRujukan = "JR/" + akJurnal.NoJurnal;
                                akADebit.JKWId = akJurnal.JKWId;
                                akADebit.Tarikh = akJurnal.Tarikh;
                                akADebit.AkCartaId1 = kredit1.AkCartaId;
                                akADebit.Debit = 0;
                                akADebit.AkCartaId2 = debit1.AkCartaId;
                                akADebit.Kredit = kredit1.Kredit;
                                await _akAkaunRepo.Insert(akADebit);
                                currentIdx = kredit1.Indeks;
                            }
                        };
                    };

                    foreach(AkJurnal1 keVot in akJ1)
                    {
                        if (GetJenisObjek(keVot.AkCartaId) == "B")
                        {
                            AbBukuVot vot = new()
                            {
                                Rujukan = "JR/" + akJurnal.NoJurnal,
                                JKWId = akJurnal.JKWId,
                                Tarikh = akJurnal.Tarikh,
                                VotId = keVot.AkCartaId,
                                Penerima = akJurnal.Catatan1.Substring(0, akJurnal.Catatan1.Length<200? akJurnal.Catatan1.Length:200),
                                Debit = keVot.Debit,
                                Kredit = keVot.Kredit,
                                Tahun = akJurnal.Tarikh.Year.ToString()
                            };
                            await _abBukuVot.Insert(vot);
                        }
                    }

                    //update posting status in akTerima
                    akJurnal.Posting = 1;
                    await _akJurnalRepo.Update(akJurnal);
                    await AddLogAsync("Posting", akJurnal.NoJurnal, akJurnal.JumKredit);

                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = "Data berjaya dikemaskini ke lejar.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnPosting(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                AkJurnal akJurnal = await _context.AkJurnal
                    .Include(x => x.AkJurnal1)
                    .FirstOrDefaultAsync(x => x.Id == id);

                //AbBukuVot abBukuVot = await _context.AbBukuVot.Include(x=>x.)

                List<AkAkaun> akAkaun = _context.AkAkaun.Where(x => x.NoRujukan == "JR/"+akJurnal.NoJurnal).ToList();
                if (akAkaun == null)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Data belum dikemaskini ke lejar.";
                }
                else
                {
                    //unposting operation start here
                    //delete data from akAkaun
                    foreach (AkAkaun item in akAkaun)
                    {
                        await _akAkaunRepo.Delete(item.Id);
                    }

                    //update posting status in akTerima
                    akJurnal.Posting = 0;
                    //akJurnal.TarikhPosting = null;
                    //akTerima.TarikhPosting = null;
                    await _akJurnalRepo.Update(akJurnal);

                    await AddLogAsync("UnPosting", akJurnal.NoJurnal, akJurnal.JumKredit);
                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Data berjaya batal kemaskini dari lejar.";
                    //unposting operation end
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PrintPdf(int id)
        {
            AkJurnal akJurnal = await _context.AkJurnal
                .Include(x=>x.JKW)
                .Include(x=>x.AkJurnal1).ThenInclude(x=>x.AkCarta)
                .FirstOrDefaultAsync(x => x.Id == id);
            JurnalPrintModel data = new JurnalPrintModel();
            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            CompanyDetails company = new CompanyDetails();
            data.Username = namaUser.Nama;
            data.AkJurnal = akJurnal;
            data.CompanyDetail = company;

            //update cetak -> 1
            akJurnal.Cetak = 1;
            await _akJurnalRepo.Update(akJurnal);
            await AddLogAsync("Cetak", akJurnal.NoJurnal, akJurnal.JumKredit);
            await _context.SaveChangesAsync();

            return new ViewAsPdf("JurnalPrintPdf", data)
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //CustomSwitches = "--footer-center \"  Tarikh: " +
                //    DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                //    " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        private string GetJenisObjek(int id)
        {
            return _context.AkCarta.Include(x => x.JJenis).FirstOrDefault(x => x.Id == id).JJenis.Kod;
        }
    }
}
