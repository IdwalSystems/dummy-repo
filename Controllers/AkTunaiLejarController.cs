using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor, User")]

    public class AkTunaiLejarController : Controller
    {
        public const string modul = "DF004";
        public const string namamodul = "Daftar P. Tunai Runcit";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly CustomIRepository<string, int> _customRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTunaiRuncit, int, string> _akTunaiRuncitRepo;

        public AkTunaiLejarController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            CustomIRepository<string, int> customRepo,
            UserManager<IdentityUser> userManager,
            IRepository<AkTunaiRuncit, int, string> akTunaiRuncitRepository
            )
        {
            _context = context;
            _appLog = appLog;
            _customRepo = customRepo;
            _userManager = userManager;
            _akTunaiRuncitRepo = akTunaiRuncitRepository;
        }

        private async Task AddLogAsync(
           string operasi,
           string nota,
           string rujukan,
           int idRujukan,
           decimal jumlah,
           int? pekerjaId)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = pekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        public async Task<IActionResult> Index()
        {
            var akTunaiRuncit = new List<AkTunaiRuncit>().AsEnumerable();

            if (User.IsInRole("SuperAdmin"))
            {
                akTunaiRuncit = await _akTunaiRuncitRepo.GetAllIncludeDeletedItems();
            }
            else
            {
                akTunaiRuncit = await _akTunaiRuncitRepo.GetAll(null);
            }

            List<AkTunaiRuncitViewModel> viewModel = new List<AkTunaiRuncitViewModel>();

            foreach (AkTunaiRuncit item in akTunaiRuncit)
            {
                decimal baki = await _customRepo.GetBalanceFromKaunterPanjar("BAKI AWAL", item.Id);

                viewModel.Add(new AkTunaiRuncitViewModel
                {
                    Id = item.Id,
                    KodKW = item.JKW.Kod,
                    KodRujukan = item.KaunterPanjar,
                    KodAkaun = item.AkCarta.Kod,
                    Perihal = item.AkCarta.Perihal,
                    BakiLejarPanjar = baki,
                    FlHapus = item.FlHapus
                });
            }
            return View(viewModel);
        }

        // GET: AkTunaiRuncit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);

            if (akTunaiRuncit == null)
            {
                return NotFound();
            }

            PopulateList();
            PopulateTable(id);

            return View(akTunaiRuncit);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            ViewBag.JBahagian = bahagianList;

            List<AkCarta> akCartaList = _context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            ViewBag.AkCarta = akCartaList;

            List<SuPekerja> suPekerjaList = _context.SuPekerja
                .OrderBy(b => b.NoGaji).ToList();
            ViewBag.SuPekerja = suPekerjaList;


        }

        private void PopulateTable(int? id)
        {
            List<AkTunaiPemegang> akTunaiPemegangTable = _context.AkTunaiPemegang
                .Include(b => b.SuPekerja)
                .Where(b => b.AkTunaiRuncitId == id)
                .OrderBy(b => b.Id)
                .ToList();
            ViewBag.akTunaiPemegang = akTunaiPemegangTable;

            // baki awal
            List<AkTunaiLejar> tunaiLejar = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == id && b.Rekup == "BAKI AWAL")
                .OrderBy(b => b.Tarikh)
                .ToList();

            // rekupan
            List<AkTunaiLejar> tunaiLejarRekup = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == id && b.Rekup != "BAKI AWAL" && !string.IsNullOrEmpty(b.Rekup))
                .OrderBy(b => b.Rekup).ThenBy(b => b.Tarikh)
                .ToList();

            tunaiLejar.AddRange(tunaiLejarRekup);
            // belum rekup
            List<AkTunaiLejar> tunaiLejarBelumRekup = _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncit.Id == id && string.IsNullOrEmpty(b.Rekup))
                .OrderBy(b => b.Tarikh)
                .ToList();

            tunaiLejar.AddRange(tunaiLejarBelumRekup);

            ViewBag.akTunaiLejar = tunaiLejar.OrderBy(b => b.Tarikh);

        }
        // rekup function
        [Authorize(Policy = "DF004E")]
        public async Task<IActionResult> Rekup(int? id, string tarikhDari, string tarikhHingga)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;
                var akTunaiRuncit = await _akTunaiRuncitRepo.GetById((int)id);
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);

                // check if date 2 less than date 1
                if (date2 < date1)
                {
                    //duplicate id error
                    TempData[SD.Error] = "Tarikh Hingga tidak boleh kurang dari Tarikh Dari.";
                    return RedirectToAction(nameof(Index));
                }
                // check end
                // step:
                //1. cari latest no rekup.
                //2. define running number untuk no rekup.
                //3. ambil latest no baucer dan list of tunai keluar yang tiada no rekup(range tarikhDari -> tarikhHingga) ikut input user.
                //4. update latest no rekup untuk list of (3) ikut running number (2)

                // 1
                // cari latest no rekup
                var LatestTunaiLejarRekup = _context.AkTunaiLejar
                    .Include(b => b.AkTunaiRuncit)
                    .Where(b => b.AkTunaiRuncit.Id == id && !string.IsNullOrEmpty(b.Rekup) && !b.NoRujukan.Contains("BAKI AWAL"))
                    .OrderByDescending(b => b.Rekup).ThenByDescending(b => b.Tarikh)
                    .FirstOrDefault();

                // 2
                // define running number 
                var year = date1.Year.ToString();
                string prefix = year + "/";
                int x = 1;
                string noRekup = prefix + "0000";

                // kalau tiada
                if (LatestTunaiLejarRekup == null || LatestTunaiLejarRekup.NoRujukan == "BAKI AWAL")
                {
                    // cari baki awal (sebab tak pernah buat rekupan lagi)
                    //LatestTunaiLejarRekup = await _context.AkTunaiLejar
                    //    .Include(b => b.AkTunaiRuncit)
                    //    .Where(b => b.AkTunaiRuncit.Id == id && b.Rekup == "BAKI AWAL")
                    //    .OrderByDescending(b => b.Rekup).ThenByDescending(b => b.Tarikh)
                    //    .FirstOrDefaultAsync();

                    noRekup = string.Format("{0:" + prefix + "0000}", x);
                }
                else
                {
                    x = int.Parse(LatestTunaiLejarRekup.Rekup.Substring(5));
                    x++;
                    noRekup = string.Format("{0:" + prefix + "0000}", x);
                }
                // 1 & 2 end


                List<AkTunaiLejar> tunaiLejarBelumRekup = await _context.AkTunaiLejar
                    .Include(b => b.AkTunaiRuncit)
                    .Where(b => b.AkTunaiRuncit.Id == id && string.IsNullOrEmpty(b.Rekup) &&
                    b.Tarikh >= date1 && b.Tarikh <= date2)
                    .OrderBy(b => b.Tarikh)
                    .ToListAsync();

                if (tunaiLejarBelumRekup.Count == 0)
                {

                    //duplicate id error
                    TempData[SD.Error] = "Tiada tunai keluar untuk direkup.";

                }
                else
                {
                    decimal jumlahRekupan = 0;
                    //unposting operation start here
                    //delete data from akTunaiLejar
                    foreach (AkTunaiLejar item in tunaiLejarBelumRekup)
                    {
                        jumlahRekupan = jumlahRekupan + item.Kredit;
                        //var tunaiLejar = await _context.AkTunaiLejar.Where(b => b.Id == item.Id).FirstOrDefaultAsync();
                        item.Rekup = noRekup;
                        _context.Update(item);
                    }

                    //update posting status in akTunaiCV
                    akTunaiRuncit.UserIdKemaskini = user.UserName;
                    akTunaiRuncit.TarKemaskini = DateTime.Now;
                    akTunaiRuncit.SuPekerjaKemaskiniId = pekerjaId;

                    await _akTunaiRuncitRepo.Update(akTunaiRuncit);

                    //insert applog
                    await AddLogAsync("Rekup", "Rekup Data", akTunaiRuncit.KaunterPanjar + " - No Rekup : " + noRekup, (int)id, jumlahRekupan, pekerjaId);
                    //insert applog end

                    await _context.SaveChangesAsync();

                    TempData[SD.Success] = "Rekupan berjaya. No Rekup yang berdaftar adalah " + noRekup;
                    //unposting operation end
                }


            }

            return RedirectToAction(nameof(Index));

        }
        // rekup function end

        // printing Rekupan Tunai Runcit
        [Authorize(Policy = "DF004D")]
        public async Task<IActionResult> PrintPdf(int id, string kodKaunter, string rekup)
        {
            if (rekup == null)
            {
                TempData[SD.Error] = "Tiada pilihan rekupan";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var userManager = _userManager.GetUserAsync(User);

                var user = _context.applicationUsers.Include(x => x.SuPekerja).FirstOrDefault(x => x.Email == userManager.Result.Email);

                int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

                var rekupanKreditList = (from tblTunaiLejar in _context.AkTunaiLejar
                                       .Include(x => x.AkTunaiRuncit)
                                       .Where(x => x.AkTunaiRuncitId == id && x.Rekup == rekup && x.Kredit > 0).ToList()
                                   join tblTunaiCV in _context.AkTunaiCV
                                       .Include(x => x.AkTunaiCV1).ThenInclude(x => x.AkCarta).ToList()
                                   on tblTunaiLejar.NoRujukan equals tblTunaiCV.NoCV into tblTunaiLejarTblTunaiCV
                                   from tbl_1 in tblTunaiLejarTblTunaiCV.DefaultIfEmpty()
                                   select new
                                   {
                                       Tarikh = tblTunaiLejar.Tarikh,
                                       Butiran = tbl_1?.Penerima ?? string.Empty,
                                       NoRujukan = tblTunaiLejar.NoRujukan,
                                       Debit = tblTunaiLejar.Debit,
                                       Kredit = tblTunaiLejar.Kredit,
                                       Baki = tblTunaiLejar.Baki
                                   }).OrderBy(x => x.Tarikh).ToList();

                var rekupanDebitList = (from tblTunaiLejar in _context.AkTunaiLejar
                                       .Include(x => x.AkTunaiRuncit)
                                       .Where(x => x.AkTunaiRuncitId == id && (x.Rekup == rekup || x.Rekup == "BAKI AWAL") && x.Debit > 0).ToList()
                                         join tblAkPV in _context.AkPV
                                             .ToList()
                                         on tblTunaiLejar.NoRujukan equals tblAkPV.NoPV into tblTunaiLejarTblAkPV
                                         from tbl_1 in tblTunaiLejarTblAkPV.DefaultIfEmpty()
                                         select new
                                         {
                                             Tarikh = tblTunaiLejar.Tarikh,
                                             Butiran = tbl_1?.Perihal ?? string.Empty,
                                             NoRujukan = tblTunaiLejar.NoRujukan,
                                             Debit = tblTunaiLejar.Debit,
                                             Kredit = tblTunaiLejar.Kredit,
                                             Baki = tblTunaiLejar.Baki
                                         }).OrderBy(x => x.Tarikh).ToList();


                RekupTunaiRuncitPrintModel data = new RekupTunaiRuncitPrintModel();

                List<Rekupan> rekupans = new List<Rekupan>();

                decimal maksRekup = 0;
                if (rekupanDebitList.Count > 1)
                {
                    foreach (var item in rekupanDebitList)
                    {
                        if (item.NoRujukan.Contains("BAKI AWAL"))
                        {
                            maksRekup = item.Debit;
                            continue;
                        }

                        rekupans.Add(
                            new Rekupan
                            {
                                Tarikh = item.Tarikh,
                                Butiran = item.Butiran,
                                NoRujukan = item.NoRujukan,
                                Debit = item.Debit,
                                Kredit = item.Kredit,
                                Baki = maksRekup
                            }
                            );
                    }
                }
                else
                {
                    foreach (var item in rekupanDebitList)
                    {
                        maksRekup = item.Debit;
                        rekupans.Add(
                            new Rekupan
                            {
                                Tarikh = item.Tarikh,
                                Butiran = item.Butiran,
                                NoRujukan = item.NoRujukan,
                                Debit = item.Debit,
                                Kredit = item.Kredit,
                                Baki = item.Baki
                            }
                            );
                    }
                }
                

                foreach (var item in rekupanKreditList)
                {
                    rekupans.Add(
                        new Rekupan
                        {
                            Tarikh = item.Tarikh,
                            Butiran = item.Butiran,
                            NoRujukan = item.NoRujukan,
                            Debit = item.Debit,
                            Kredit = item.Kredit,
                            Baki = item.Baki
                        }
                        );
                }

                data.RekupanList = rekupans;

                CompanyDetails company = new CompanyDetails();
                data.CompanyDetail = company;
                if (User.IsInRole("SuperAdmin"))
                {
                    data.Penyedia = user.UserName;
                }
                else
                {
                    data.Penyedia = user.SuPekerja.Nama;
                }
                data.NoRekup = rekup;
                data.MaksRekupan = maksRekup;
                //string customSwitches = string.Format("--header-spacing \"-12\" " +
                                       //"--header-font-size \"10\" " +
                                       //"--footer-center \"[page]/[toPage]\" " +
                                       //"--footer-font-size \"7\" --footer-spacing 1");

                //insert applog
                await AddLogAsync("Cetak", "Cetak Rekupan", "Kod Kaunter Panjar : " + kodKaunter + ", No Rekup : " + rekup, id, 0, pekerjaId);

                //insert applog end
                await _context.SaveChangesAsync();

                //return View("TunaiRuncitPrintPdf");
                return new ViewAsPdf("TunaiRuncitPrintPdf", data)
                {
                    PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    //CustomSwitches = customSwitches,
                    CustomSwitches = "--footer-center \"  Tarikh: " +
                        DateTime.Now.Date.ToString("dd/MM/yyyy") + "            Mukasurat: [page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
            }

        }
        // printing Rekupan Tunai Runcit end

        //function get latest date rekup (noPV) in tunai lejar
        public async Task<JsonResult> GetLastDateRekupInTunaiLejar(int id)
        {
            try
            {
                // cari baucer yang tak direkup lagi paling latest
                var result = await _context.AkTunaiLejar
                .Include(b => b.AkTunaiRuncit)
                .Where(b => b.AkTunaiRuncitId == id && string.IsNullOrEmpty(b.Rekup) && b.NoRujukan.Contains("PV"))
                .OrderByDescending(b => b.Tarikh)
                .FirstOrDefaultAsync();

                var tarikh = DateTime.Now.ToString("yyyy-MM-dd");

                if (result == null)
                {
                    result = await _context.AkTunaiLejar
                                    .Include(b => b.AkTunaiRuncit)
                                    .Where(b => b.AkTunaiRuncit.Id == id && b.Rekup == "BAKI AWAL")
                                    .OrderBy(b => b.Tarikh)
                                    .FirstOrDefaultAsync();

                    if (result == null)
                    {
                        return Json(new { result = "ERROR" });
                    }
                    tarikh = result.Tarikh.ToString("yyyy-MM-dd");
                }
                else
                {
                    if (result.NoRujukan.Contains("PV", StringComparison.OrdinalIgnoreCase))
                    {
                        tarikh = result.Tarikh.ToString("yyyy-MM-dd");
                    }
                }

               

                return Json(new { result = "OK", tarikh = tarikh, record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        //function get latest date rekup (noPV) in tunai lejar end

        // get list of no rekup based on AkTunaiRuncitId
        public JsonResult GetListOfNoRekup(int id)
        {
            try
            {
                // cari baucer yang tak direkup lagi paling latest
                var result = (from tbl1 in _context.AkTunaiLejar
                            .Where(x => x.AkTunaiRuncitId == id && x.Rekup != "BAKI AWAL" && !string.IsNullOrEmpty(x.Rekup)).ToList()
                              select new
                              {
                                  tbl1.Rekup
                              }).GroupBy(x => x.Rekup).Select(x => x.First());

                return Json(new { result = "OK", record = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }

        }
        // get list of no rekup based on AkTunaiRuncitId end
    }
}
