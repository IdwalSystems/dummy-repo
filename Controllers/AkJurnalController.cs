using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    public class AkJurnalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<AkJurnal, int> _akJurnalRepo;
        private readonly IRepository<JKW, int> _jKWRepo;
        private readonly ListViewIRepository<AkJurnal1, int> _akJurnal1Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private CartJurnal _cart;

        public AkJurnalController(
            ApplicationDbContext context,
            IRepository<AkJurnal, int> akJurnalRepository,
            IRepository<JKW, int> jKWRepository,
            ListViewIRepository<AkJurnal1, int> akJurnal1Repository,
            IRepository<AkCarta, int> akCartaRepository,
            CartJurnal cart
            )
        {
            _context = context;
            _akJurnalRepo = akJurnalRepository;
            _jKWRepo = jKWRepository;
            _akJurnal1Repo = akJurnal1Repository;
            _akCartaRepo = akCartaRepository;
            _cart = cart;
        }
        private string GetKod(int kod)
        {
            var kw = _context.JKW.FirstOrDefault(x => x.Id == kod);

            var kumpulanWang = kw.Kod;
            var year = DateTime.Now.Year.ToString();
            string prefix = year +"/"+ kumpulanWang+"/";
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkJurnal.Max(x => x.NoJurnal);
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
        public JsonResult JsonGetKod(string data)
        {
            try
            {
                var result = "";
                if (data == null || data == "")
                {
                    result = "";
                }
                else
                {
                    result = GetKod(int.Parse(data));
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

            List<AkCarta> cartaList = _context.AkCarta.OrderBy(b => b.Kod).ToList();
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
        public async Task<IActionResult> Index()
        {
            var akJunal = await _akJurnalRepo.GetAll();
            return View(akJunal);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkJurnal akJurnal, int JKWId, decimal JumDebit, decimal JumKredit)
        {
            AkJurnal m = new AkJurnal();
            var username = User.FindFirstValue(ClaimTypes.Name).Substring(0, 15);

            if (ModelState.IsValid)
            {
                string noRujukan = GetKod(akJurnal.JKWId);
                if (akJurnal != null && JKWId != 0)
                {
                    m.JKWId = akJurnal.JKWId;
                    m.NoJurnal = noRujukan;
                    m.Tarikh = akJurnal.Tarikh;
                    m.JumDebit = JumDebit;
                    m.JumKredit = JumKredit;
                    m.Catatan1 = akJurnal.Catatan1;
                    m.Catatan2 = akJurnal.Catatan2;
                    m.Catatan3 = akJurnal.Catatan3;
                    m.Catatan4 = akJurnal.Catatan4;
                    m.Posting = akJurnal.Posting;
                    m.Cetak = akJurnal.Cetak;
                    m.Batal = akJurnal.Batal;
                    m.UserId = username;
                    m.TarMasuk = DateTime.Now;
                    m.AkJurnal1 = _cart.Lines1.ToArray();

                    await _akJurnalRepo.Insert(m);
                    await _context.SaveChangesAsync();

                    CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No jurnal adalah " + noRujukan;
                    return RedirectToAction(nameof(Index));
                }
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkJurnal akJurnal)
        {
            if (id != akJurnal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akJurnal);
                    await _context.SaveChangesAsync();
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
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
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
            _context.AkJurnal.Remove(akJurnal);
            await _context.SaveChangesAsync();
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
                    _cart.RemoveItem1(akJurnal1.AkCartaId);
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
    }
}
