using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository2<AkJurnal1, int> _akJurnal1Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private CartJurnal _cart;

        public AkJurnalController(
            ApplicationDbContext context,
            IRepository<AkJurnal, int> akJurnalRepository,
            IRepository<JKW, int> jKWRepository,
            IRepository2<AkJurnal1, int> akJurnal1Repository,
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
            ViewBag.akTerima1 = akJurnal1Table;
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
            var akTerima = await _akJurnalRepo.GetAll();
            return View(akTerima);
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
        public async Task<IActionResult> Create(AkJurnal akJurnal, int JKWId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akJurnal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
            return View(akJurnal);
        }

        // GET: AkJurnal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akJurnal = await _context.AkJurnal.FindAsync(id);
            if (akJurnal == null)
            {
                return NotFound();
            }
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akJurnal.JKWId);
            return View(akJurnal);
        }

        // POST: AkJurnal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JKWId,NoJurnal,Tarikh,JumDebit,JumKredit,Catatan1,Catatan2,Catatan3,Catatan4,Posting,Cetak,Batal,UserId,TarikhMasuk")] AkJurnal akJurnal)
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
    }
}
