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

    [Authorize(Roles = "SuperAdmin,Supervisor,User")]
    public class AkCimbEFTController : Controller
    {

        public const string modul = "PV002";
        public const string namamodul = "Biz Channel";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkCimbEFT, int, string> _akCimbEFTRepo;
        private readonly ListViewIRepository<AkCimbEFT1, int> _akCimbEFT1Repo;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private CartCimbEFT _cart;

        public AkCimbEFTController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkCimbEFT, int, string> akCimbEFTRepo,
            ListViewIRepository<AkCimbEFT1, int> akCimbEFT1Repo,
            IRepository<AkPV, int, string> akPVRepo,
            IRepository<AkBank, int, string> akBankRepo,
            CartCimbEFT cart
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _akCimbEFTRepo = akCimbEFTRepo;
            _akCimbEFT1Repo = akCimbEFT1Repo;
            _akPVRepo = akPVRepo;
            _akBankRepo = akBankRepo;
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

        // GET: AkCimbEFT
        [Authorize(Policy = "PV002")]
        public async Task<IActionResult> Index(
            string searchString,
            string searchDate1,
            string searchDate2,
            string searchColumn)
        {
            List<SelectListItem> columnList = new();
            columnList.Add(new SelectListItem() { Text = "Tar Jana", Value = "TarJana" });
            columnList.Add(new SelectListItem() { Text = "No PBI", Value = "NoPBI" });
            columnList.Add(new SelectListItem() { Text = "Nama Fail", Value = "Nama" });

            var akCimbEFT = await _akCimbEFTRepo.GetAll();

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Supervisor"))
            {
                akCimbEFT = await _akCimbEFTRepo.GetAllIncludeDeletedItems();
            }

            if (!String.IsNullOrEmpty(searchString) || (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2)))
            {
                // searching with '%like%' condition
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchColumn == "NoPBI")
                    {
                        akCimbEFT = akCimbEFT.Where(s => s.NoPBI.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    else if (searchColumn == "Nama")
                    {
                        akCimbEFT = akCimbEFT.Where(s => s.SuPekerja.Nama.ToUpper().Contains(searchString.ToUpper())).ToList();
                    }


                    ViewBag.SearchData1 = searchString;

                }

                // searching with '%like%' condition end

                // searching with date range condition
                if (!String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(searchDate2))
                {
                    if (searchColumn == "TarJana")
                    {
                        DateTime date1 = DateTime.Parse(searchDate1);
                        DateTime date2 = DateTime.Parse(searchDate2).AddHours(23.99);
                        akCimbEFT = akCimbEFT.Where(x => x.TarJana >= date1
                            && x.TarJana <= date2).ToList();
                    }
                    ViewBag.SearchData1 = searchDate1;
                    ViewBag.SearchData2 = searchDate2;
                }

                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", searchColumn);
            }
            // searching with date range condition end
            else
            {
                ViewBag.SearchColumn = new SelectList(columnList, "Value", "Text", "TarJana");
            }

            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT
                .Include(a => a.AkBank)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Create
        [Authorize(Policy = "PV002C")]
        public IActionResult Create()
        {
            PopulateList();
            CartEmpty();
            var noRujukan = GetNoRujukan(DateTime.Now, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

            ViewBag.NoPBI = noRujukan;
            ViewBag.NamaFail = noRujukan + ".txt";

            return View();
        }

        public JsonResult CartEmpty()
        {
            try
            {
                ViewBag.akCimbEFT1 = new List<int>();
                _cart.Clear1();

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        private void PopulateList()
        {
            List<AkBank> bankList = _context.AkBank
                .Include(x => x.JKW)
                .Include(x => x.JBank)
                .Include(x => x.JBahagian)
                .Include(x => x.AkCarta)
                .ToList();

            ViewBag.AkBank = bankList;

        }

        [HttpPost]
        public JsonResult JsonGetKod(DateTime tarJana, string noPBI)
        {
            try
            {
                if (noPBI != "Invalid date")
                {
                    noPBI = GetNoRujukan(tarJana, tarJana.ToString("yyyy"), tarJana.ToString("MM"));

                    return Json(new { result = "OK", record = noPBI });
                }
                else
                {
                    return Json(new { result = "OK", record = "" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> JsonGetBaucer(DateTime tarDari, DateTime tarHingga)
        {
            try
            {
                // find all pv within date range where it is not jenis baucer panjar or jenis baucer gaji and posting = 1
                // switch by jenis baucer, differentiate by individu or gandaan
                // case individual : 
                    // get all information on AkPV only
                // case Gandaan :
                    // get all information from SuProfil
                // return
                    // 1. data
                    // 2. type of data (Individual or Gandaan) 

                // get all PV where posting = 1
                List<AkPV> pv = await _context.AkPV
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.AkTunaiRuncit).ThenInclude(b => b.AkCarta)
                    .Include(b => b.SpPendahuluanPelbagai).ThenInclude(b => b.AkCarta)
                    .Include(b => b.SuProfil)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.SuProfil)
                        .ThenInclude(b => b.SuProfil1).ThenInclude(b => b.SuAtlet).ThenInclude(b => b.JBank)
                    .Include(b => b.SuProfil)
                        .ThenInclude(b => b.SuProfil1).ThenInclude(b => b.SuJurulatih).ThenInclude(b => b.JBank)
                    .Include(b => b.AkPembekal).ThenInclude(x => x.JBank)
                    .Include(b => b.SuPekerja).ThenInclude(x => x.JBank)
                    .Include(b => b.AkBank).ThenInclude(b => b.JBank)
                    .Include(b => b.JCaraBayar)
                    .Include(b => b.AkPV1)
                        .ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2)
                        .ThenInclude(b => b.AkBelian)
                            .ThenInclude(b => b.AkPO)
                    .Where(b => b.FlPosting == 1)
                    .OrderBy(b => b.NoPV)
                    .ToListAsync();

                // get all PV within date range
                pv = pv.Where(x => x.Tarikh >= tarDari
                    && x.Tarikh <= tarHingga.AddHours(23.99)).ToList();

                // get all PV where it is not jenis baucer panjar or jenis baucer gaji
                pv = pv.Where(x => x.FlJenisBaucer != 2 
                                || x.FlJenisBaucer != 4 
                                || x.FlJenisBaucer != 5)
                                .ToList();

                return Json(new { result = "OK", record = pv });

            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        private string GetNoRujukan(DateTime tarJana, string year, string month)
        {
            string prefix = year + month;
            int x = 1;
            string noRujukan = prefix + "000";

            var LatestNoRujukan = _context.AkCimbEFT
                       .IgnoreQueryFilters()
                       .Where(x => x.TarJana.Year ==  tarJana.Year && x.TarJana.Month == tarJana.Month && x.NoPBI.Contains(prefix.ToUpper()))
                       .Max(x => x.NoPBI);

            if (LatestNoRujukan == null)
            {
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            else
            {
                x = int.Parse(LatestNoRujukan.Substring(5));
                x++;
                noRujukan = string.Format("{0:" + prefix + "000}", x);
            }
            return noRujukan;
        }

        // POST: AkCimbEFT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoPBI,TarJana,TarBayar,Jumlah,NamaFail,BilPV,FlKategori,SuPekerjaId,AkBankId,FlStatus,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkCimbEFT akCimbEFT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(akCimbEFT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT.FindAsync(id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // POST: AkCimbEFT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoPBI,TarJana,TarBayar,Jumlah,NamaFail,BilPV,FlKategori,SuPekerjaId,AkBankId,FlStatus,FlHapus,TarHapus,UserId,TarMasuk,UserIdKemaskini,TarKemaskini")] AkCimbEFT akCimbEFT)
        {
            if (id != akCimbEFT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(akCimbEFT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AkCimbEFTExists(akCimbEFT.Id))
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
            ViewData["AkBankId"] = new SelectList(_context.AkBank, "Id", "Id", akCimbEFT.AkBankId);
            ViewData["SuPekerjaId"] = new SelectList(_context.SuPekerja, "Id", "Emel", akCimbEFT.SuPekerjaId);
            return View(akCimbEFT);
        }

        // GET: AkCimbEFT/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCimbEFT = await _context.AkCimbEFT
                .Include(a => a.AkBank)
                .Include(a => a.SuPekerja)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (akCimbEFT == null)
            {
                return NotFound();
            }

            return View(akCimbEFT);
        }

        // POST: AkCimbEFT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akCimbEFT = await _context.AkCimbEFT.FindAsync(id);
            _context.AkCimbEFT.Remove(akCimbEFT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AkCimbEFTExists(int id)
        {
            return _context.AkCimbEFT.Any(e => e.Id == id);
        }
    }
}
