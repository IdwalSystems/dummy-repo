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
    public class AkTunaiCVController : Controller
    {

        public const string modul = "TR002";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiCV, int, string> _akTunaiCVRepo;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<SuPekerja, int, string> _suPekerjaRepo;
        private readonly IRepository<AkPembekal, int, string> _akPembekalRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartTunaiCV _cart;

        public AkTunaiCVController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiCV, int, string> akTunaiCVRepository,
            IRepository<SuPekerja, int, string> suPekerjaRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkPembekal, int, string> akPembekalRepository,
            IRepository<AkBank, int, string> akBankRepository,
             CartTunaiCV cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akTunaiCVRepo = akTunaiCVRepository;
            _suPekerjaRepo = suPekerjaRepository;
            _kwRepo = kwRepository;
            _akPembekalRepo = akPembekalRepository;
            _akBankRepo = akBankRepository;
            _cart = cart;
        }
        // GET: AkTunaiCV
        [Authorize(Policy = "TR001")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tarikh", Value = "Tarikh" });
            columnList.Add(new SelectListItem() { Text = "No CV", Value = "NoRujukan" });
            columnList.Add(new SelectListItem() { Text = "Penerima", Value = "Nama" });

            if (!String.IsNullOrEmpty(searchColumn))
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "");
            }

            var akTunaiCV = await _akTunaiCVRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoRujukan")
                    {
                        akTunaiCV = akTunaiCV.Where(s => s.NoCV.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akTunaiCV = akTunaiCV.Where(s => s.Penerima.ToUpper().Contains(searchString.ToUpper())).ToList();
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
                        akTunaiCV = akTunaiCV.Where(x => x.Tarikh >= date1
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

            List<AkTunaiCVViewModel> viewModel = new List<AkTunaiCVViewModel>();

            foreach (AkTunaiCV item in akTunaiCV)
            {
                viewModel.Add(new AkTunaiCVViewModel
                {
                    Id = item.Id,
                    KW = item.AkTunaiRuncit.JKW.Kod,
                    AkTunaiRuncit = item.AkTunaiRuncit,
                    NoCV = item.NoCV,
                    Tarikh = item.Tarikh,
                    Jumlah = item.Jumlah,
                    Penerima = item.Penerima,
                    Catatan = item.Catatan
                });
            }
            return View(viewModel);
        }

        // GET: AkTunaiCV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV
                .Include(a => a.AkPembekal)
                .Include(a => a.AkTunaiRuncit)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }

            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Create
        [Authorize(Policy ="TR001C")]
        public IActionResult Create()
        {
            // get latest no rujukan running number  
            var kodKaunter = _context.AkTunaiRuncit.FirstOrDefault(x => x.KaunterPanjar == "10001");

            if (kodKaunter == null)
            {
                TempData[SD.Error] = "Tiada kaunter panjar yang berdaftar lagi. Sila berbuat demikian pada modul Pemegang Tunai Runcit";
                return RedirectToAction(nameof(Index));
            }
            var kaunter = kodKaunter.KaunterPanjar;
            var year = DateTime.Now.Year.ToString();
            string prefix = year + kaunter;
            int x = 1;
            string noRujukan = prefix + "000000";

            var LatestNoRujukan = _context.AkTunaiCV
                        .Where(x => x.Tahun == year && x.AkTunaiRuncit.KaunterPanjar == kodKaunter.KaunterPanjar)
                        .Max(x => x.NoCV);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(14));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000000}", x);
            }

            // get latest no rujukan running number end
            ViewBag.NoRujukan = noRujukan;
            PopulateList();
            CartEmpty();
            return View();
        }

        private void PopulateList()
        {
            List<AkTunaiRuncit> akTunaiRuncitList = _context.AkTunaiRuncit.OrderBy(b => b.KaunterPanjar).ToList();
            ViewBag.akTunaiRuncit = akTunaiRuncitList;

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akTunaiCV1 = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // POST: AkTunaiCV/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AkTunaiRuncitId,KategoriPenerima,Tahun,NoCV,Tarikh,SuPekerjaId,AkPembekalId,Penerima,Alamat1,Alamat2,Almat3,Catatan,Jumlah,FlPosting,FlCetak,FlBatal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiCV akTunaiCV)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akTunaiCV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // POST: AkTunaiCV/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AkTunaiRuncitId,KategoriPenerima,Tahun,NoCV,Tarikh,SuPekerjaId,AkPembekalId,Penerima,Alamat1,Alamat2,Almat3,Catatan,Jumlah,FlPosting,FlCetak,FlBatal,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkTunaiCV akTunaiCV)
        {
            if (id != akTunaiCV.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akTunaiCV);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkTunaiCVExists(akTunaiCV.Id))
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
            ViewData["AkPembekalId"] = new SelectList(_context.AkPembekal, "Id", "AkaunBank", akTunaiCV.AkPembekalId);
            ViewData["AkTunaiRuncitId"] = new SelectList(_context.AkTunaiRuncit, "Id", "Id", akTunaiCV.AkTunaiRuncitId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Id", akTunaiCV.SuPekerjaId);
            return View(akTunaiCV);
        }

        // GET: AkTunaiCV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiCV = await _context.AkTunaiCV
                .Include(a => a.AkPembekal)
                .Include(a => a.AkTunaiRuncit)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akTunaiCV == null)
            {
                return NotFound();
            }

            return View(akTunaiCV);
        }

        // POST: AkTunaiCV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akTunaiCV = await _context.AkTunaiCV.FindAsync(id);
            _context.AkTunaiCV.Remove(akTunaiCV);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkTunaiCVExists(int id)
        {
            return _context.AkTunaiCV.Any(e => e.Id == id);
        }
    }
}
