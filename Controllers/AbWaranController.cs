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

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]
    public class AbWaranController : Controller
    {
        public const string modul = "BJ001";
        public const string namamodul = "Waran";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AbWaran, int, string> _abWaranRepo;
        private readonly ListViewIRepository<AbWaran1, int> _abWaran1Repo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JBahagian, int, string> _jBahagianRepo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<AbBukuVot, int, string> _abBukuVotRepo;
        private CartWaran _cart;

        public AbWaranController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AbWaran, int, string> abWaranRepo,
            ListViewIRepository<AbWaran1, int> abWaran1Repo,
            IRepository<JKW, int, string> jkwRepo,
            IRepository<JBahagian, int, string> jBahagianRepo,
            IRepository<AkCarta, int, string> akCartaRepo,
            IRepository<AbBukuVot, int, string> abBukuVotRepo,
            CartWaran cart)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _abWaranRepo = abWaranRepo;
            _abWaran1Repo = abWaran1Repo;
            _kwRepo = jkwRepo;
            _jBahagianRepo = jBahagianRepo;
            _akCartaRepo = akCartaRepo;
            _abBukuVotRepo = abBukuVotRepo;
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

        // GET: AbWaran
        [Authorize(Policy = "BJ001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No PV", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Tahun", Value = "Tahun" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var abWaran = await _abWaranRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                abWaran = await _abWaranRepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        abWaran = abWaran.Where(s => s.NoRujukan.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Tahun")
                    {
                        abWaran = abWaran.Where(s => s.Tahun.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        abWaran = abWaran.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "Tarikh");
            }

            return View(abWaran);
        }

        // GET: AbWaran/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _context.AbWaran
                .Include(a => a.JBahagian)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abWaran == null)
            {
                return NotFound();
            }

            return View(abWaran);
        }

        // GET: AbWaran/Create
        public IActionResult Create()
        {
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod");
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod");
            return View();
        }

        // POST: AbWaran/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoRujukan,Tahun,Tarikh,TarikhPosting,Jumlah,FlJenisWaran,FlHapus,TarHapus,FlPosting,FlCetak,JKWId,JBahagianId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AbWaran abWaran)
        {
            if (ModelState.IsValid)
            {
                _context.Add(abWaran);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", abWaran.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", abWaran.JKWId);
            return View(abWaran);
        }

        // GET: AbWaran/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _context.AbWaran.FindAsync(id);
            if (abWaran == null)
            {
                return NotFound();
            }
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", abWaran.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", abWaran.JKWId);
            return View(abWaran);
        }

        // POST: AbWaran/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoRujukan,Tahun,Tarikh,TarikhPosting,Jumlah,FlJenisWaran,FlHapus,TarHapus,FlPosting,FlCetak,JKWId,JBahagianId,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AbWaran abWaran)
        {
            if (id != abWaran.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abWaran);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbWaranExists(abWaran.Id))
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
            ViewData["JBahagianId"] = new SelectList(_context.JBahagian, "Id", "Kod", abWaran.JBahagianId);
            ViewData["JKWId"] = new SelectList(_context.JKW, "Id", "Kod", abWaran.JKWId);
            return View(abWaran);
        }

        // GET: AbWaran/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abWaran = await _context.AbWaran
                .Include(a => a.JBahagian)
                .Include(a => a.JKW)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abWaran == null)
            {
                return NotFound();
            }

            return View(abWaran);
        }

        // POST: AbWaran/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abWaran = await _context.AbWaran.FindAsync(id);
            _context.AbWaran.Remove(abWaran);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbWaranExists(int id)
        {
            return _context.AbWaran.Any(e => e.Id == id);
        }
    }
}
