using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    public class AkBelianController : Controller
    {
        public const string modul = "TG002";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int> _akBelianRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkPO, int> _akPORepo;

        public AkBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int> akBelian,
            IRepository<JKW, int> kwRepo,
            IRepository<AkPO, int> akPORepo
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelian;
            _kwRepo = kwRepo;
            _akPORepo = akPORepo;
        }

        // GET: AkBelian
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            var akBelian = await _akBelianRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akBelian = akBelian.Where(s => s.NoInbois.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akBelian = akBelian.Where(s => s.AkPO.AkPembekal.NamaSykt.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akBelian = akBelian.Where(x => x.Tarikh >= date1
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
            return View(akBelian);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<AkPO> akPOList = _context.AkPO
                .Include(b => b.AkPembekal).ThenInclude(b=>b.JBank)
                .Include(b => b.JKW)
                .Include(b => b.AkPO1).ThenInclude(b=> b.AkCarta)
                .Include(b => b.AkPO2)
                .OrderBy(b => b.Tarikh).ToList();
            ViewBag.AkPO = akPOList;

            List<AkCarta> akCartaList = _context.AkCarta.Include(b => b.JKW).OrderBy(b => b.Kod).ToList();
            ViewBag.AkCarta = akCartaList;

        }

        // GET: AkBelian/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _context.AkBelian
                .Include(a => a.AkPO)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akBelian == null)
            {
                return NotFound();
            }

            return View(akBelian);
        }

        // GET: AkBelian/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkBelian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tahun,Tarikh,TarikhPosting,NoInbois,JKWId,AkPOId,Jumlah,FlCetak,FlPosting,FlBatal")] AkBelian akBelian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akBelian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akBelian.AkPOId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akBelian.JKWId);
            return View(akBelian);
        }

        // GET: AkBelian/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _context.AkBelian.FindAsync(id);
            if (akBelian == null)
            {
                return NotFound();
            }
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akBelian.AkPOId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akBelian.JKWId);
            return View(akBelian);
        }

        // POST: AkBelian/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tahun,Tarikh,TarikhPosting,NoInbois,JKWId,AkPOId,Jumlah,FlCetak,FlPosting,FlBatal")] AkBelian akBelian)
        {
            if (id != akBelian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akBelian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkBelianExists(akBelian.Id))
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
            ViewData["AkPOId"] = new SelectList(_context.AkPO, "Id", "Id", akBelian.AkPOId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", akBelian.JKWId);
            return View(akBelian);
        }

        // GET: AkBelian/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akBelian = await _context.AkBelian
                .Include(a => a.AkPO)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akBelian == null)
            {
                return NotFound();
            }

            return View(akBelian);
        }

        // POST: AkBelian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akBelian = await _context.AkBelian.FindAsync(id);
            _context.AkBelian.Remove(akBelian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkBelianExists(int id)
        {
            return _context.AkBelian.Any(e => e.Id == id);
        }
    }
}
