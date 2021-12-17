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
    public class AkBelianController : Controller
    {

        public const string modul = "TG002";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkBelian, int> _akBelianRepo;
        private readonly IRepository<AkPembekal, int> _akPembekalRepo;
        private readonly IRepository<JKW, int> _kwRepo;
        private readonly IRepository<AkPO, int> _akPORepo;
        private readonly ListViewIRepository<AkBelian1, int> _akBelian1Repo;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private CartBelian _cart;

        public AkBelianController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkBelian, int> akBelian,
            IRepository<AkPembekal, int> akPembekal,
            IRepository<JKW, int> kwRepo,
            IRepository<AkPO, int> akPORepo,
            ListViewIRepository<AkBelian1, int> akBelian1Repository,
            CartBelian cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akBelianRepo = akBelian;
            _akPembekalRepo = akPembekal;
            _kwRepo = kwRepo;
            _akPORepo = akPORepo;
            _akBelian1Repo = akBelian1Repository;
            _cart = cart;
        }

        // GET: AkBelian
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            var akBelian = await _akBelianRepo.GetAll();

            //var akBelian = await _context.AkBelian.ToListAsync();

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

            List<AkBelianViewModel> viewModel = new List<AkBelianViewModel>();

            foreach (AkBelian item in akBelian)
            {
                var namaSykt = "";
                var alamat1 = "";

                if(item.AkPOId == null)
                {
                    namaSykt = item.AkPembekal.NamaSykt;
                    alamat1 = item.AkPembekal.Alamat1;
                }
                else
                {
                    namaSykt = item.AkPO.AkPembekal.NamaSykt;
                    alamat1 = item.AkPO.AkPembekal.Alamat1;
                }

                viewModel.Add( new AkBelianViewModel
                    {
                        Id = item.Id,
                        Tahun = item.Tahun,
                        NoInbois = item.NoInbois,
                        Tarikh = item.Tarikh,
                        Jumlah = item.Jumlah,
                        NamaSykt = namaSykt,
                        Alamat1 = alamat1,
                        FlCetak = item.FlCetak,
                        FlBatal = item.FlBatal,
                        FlPosting = item.FlPosting
                    }
                );    
            }

            return View(viewModel);
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

            List<AkPembekal> akPembekalList = _context.AkPembekal
                .Include(b => b.JBank)
                .OrderBy(b => b.KodSykt).ToList();
            ViewBag.AkPembekal = akPembekalList;

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
            //CartEmpty();
            return View();
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

        // function  json tbl objek
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

        public JsonResult SaveAkBelian1(AkBelian1 akBelian1)
        {

            try
            {
                if (akBelian1 != null)
                {
                    _cart.AddItem1(akBelian1.AkBelianId,
                                    akBelian1.Amaun,
                                    akBelian1.AkCartaId);

                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        // function json tbl objek end

        // POST: AkBelian/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkBelian akBelian, int JKWId, int AkPOId, int AkPembekalId)
        {
            AkBelian m = new AkBelian();
            var noRujukan = "IN/" + akBelian.NoInbois;

            var akPo = await _akPORepo.GetById(AkPOId);

            if (ModelState.IsValid)
            {
                if (akBelian != null && JKWId != 0)
                {
                    
                    m.JKWId = JKWId;
                    m.Tahun = akBelian.Tahun;
                    m.NoInbois = noRujukan;
                    m.Tarikh = akBelian.Tarikh;
                    m.Jumlah = akBelian.Jumlah;
                    m.FlCetak = 0;
                    m.FlPosting = 0;
                    m.FlBatal = 0;
                    if (akPo != null)
                    {
                        m.FlTanggungan = "1";
                        m.AkPOId = AkPOId;
                        m.AkPembekalId = akPo.AkPembekalId;
                    }
                    else
                    {
                        m.FlTanggungan = "0";
                        m.AkPembekalId = AkPembekalId;
                    }

                    //m.UserId = username;
                    //m.TarMasuk = DateTime.Now;
                    //m.TarKemaskini = akTerima.TarKemaskini;

                    m.AkBelian1 = _cart.Lines1.ToArray();
                    //m.AkTerima2 = _cart.Lines2.ToArray();

                    await _akBelianRepo.Insert(m);

                    //insert applog
                    var user = await _userManager.GetUserAsync(User);

                    AppLog appLog = new AppLog();

                    appLog.UserId = user.UserName;
                    appLog.LgModule = modul + "C";
                    appLog.LgOperation = "Tambah";
                    appLog.LgNote = modul + " Inbois Pembekal - Tambah";
                    appLog.NoRujukan = noRujukan;
                    appLog.Jumlah = akBelian.Jumlah;

                    await _appLog.Insert(appLog);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    //CartEmpty();
                    TempData[SD.Success] = "Maklumat berjaya ditambah. No rujukan pendaftaran adalah " + akBelian.NoInbois;
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
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
