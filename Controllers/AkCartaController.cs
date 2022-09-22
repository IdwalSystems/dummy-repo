using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel;
using Rotativa.AspNetCore;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin , Supervisor")]
    public class AkCartaController : Controller
    {
        public const string modul = "AK001";
        public const string namamodul = "Carta Akaun";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserService _userService;

        public AkCartaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<JKW, int, string> kwRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IWebHostEnvironment webHostEnvironment,
            UserService userService)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
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
            appLog.LgNote = namamodul + " - " + nota ;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = pekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        // GET: AkCarta
        public async Task<IActionResult> Index()
        {
            var akCarta = await _akCartaRepo.GetAll();
            return View(akCarta);
        }

        // GET: AkCarta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCarta = await _akCartaRepo.GetById((int)id);

            if (akCarta == null)
            {
                return NotFound();
            }

            return View(akCarta);
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.Kw = kwList;

            List<JJenis> jenisList = _context.JJenis.OrderBy(b => b.Kod).ToList();
            ViewBag.Jenis = jenisList;

            List<JParas> parasList = _context.JParas.OrderBy(b => b.Kod).ToList();
            ViewBag.Paras = parasList;
        }

        // GET: AkCarta/Create
        public IActionResult Create()
        {
            PopulateList();
            return View();
        }

        // POST: AkCarta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AkCarta akCarta, int JKWId, int JJenisId, int JParasId, bool IsBajet)
        {
            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            string paras = _context.JParas.FirstOrDefault(q => q.Id == JParasId).Kod;
            int kodman = Convert.ToInt32(akCarta.Kod.Substring(1, 1));
            int kodsen = Convert.ToInt32(akCarta.Kod.Substring(2, 1));
            int kodhyaku = Convert.ToInt32(akCarta.Kod.Substring(3, 1));
            int kodju = Convert.ToInt32(akCarta.Kod.Substring(4));
            string prefix = akCarta.Kod.Substring(0, 1);
            bool check = false;
            bool check2 = false;

            if (paras == "1")
            {
                if (kodman > 0 && kodsen == 0 && kodhyaku == 0&& kodju==0)
                {
                    check = true;
                }
            }
            else if (paras == "2")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku == 0 && kodju == 0)
                {
                    check = true;
                }
            }
            else if (paras == "3")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku > 0 && kodju == 0)
                {
                    check = true;
                }
            }
            else if (paras == "4")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku > 0 && kodju > 0 )
                {
                    check = true;
                }
            }

            if(paras == "4")
            {
                check2 = CheckKod(prefix + (kodman * 10000 + kodsen * 1000 + kodhyaku * 100));
            }
            else if (paras == "3")
            {
                check2 = CheckKod(prefix + (kodman * 10000 + kodsen * 1000));
            }
            else if (paras == "2")
            {
                check2 = CheckKod(prefix + (kodman * 10000));
            }
            else if (paras == "1")
            {
                check2 = true;
            }

            ///////---------------------------------------------------
            if (!check)
            {
                TempData[SD.Error] = "Maklumat gagal ditambah. Kod Carta " + akCarta.Kod + " tidak sesuai untuk Paras " + paras + ". ";
            }
            else if (!check2)
            {
                int parasatas = Convert.ToInt32(paras)-1;
                TempData[SD.Error] = "Maklumat gagal ditambah. Pastikan Paras " + parasatas + " telah wujud. ";
            }
            else if (CheckKod(akCarta.Kod))
            {
                TempData[SD.Error] = "Maklumat gagal ditambah. Kod Carta " + akCarta.Kod + " sudah digunakan. ";
            }
            else
            {
                AkCarta akC = new AkCarta();
                if (ModelState.IsValid)
                {
                    if (akCarta != null && JKWId != 0)
                    {
                        akC.JKWId = JKWId;
                        akC.Kod = akCarta.Kod;
                        akC.JJenisId = JJenisId;
                        akC.Perihal = akCarta.Perihal?.ToUpper() ?? null;
                        akC.JParasId = JParasId;
                        akC.DebitKredit = akCarta.DebitKredit;
                        akC.UmumDetail = akCarta.UmumDetail;
                        akC.Baki = akCarta.Baki;
                        akC.Catatan1 = akCarta.Catatan1?.ToUpper() ?? null;
                        akC.Catatan2 = akCarta.Catatan2;
                        akC.IsBajet = akCarta.IsBajet;
                        akC.UserId = user.UserName;
                        akC.TarMasuk = DateTime.Now;
                        akC.SuPekerjaMasukId = pekerjaId;
                        try {
                            await _akCartaRepo.Insert(akC);
                            //insert applog
                            await AddLogAsync("Tambah", akC.Kod, akC.Kod, 0, akC.Baki, pekerjaId);
                            //insert applog end
                            await _akCartaRepo.Save();
                        }
                        catch { }
                        finally
                        {
                            //add bakiawal into akAkaun
                            if (akCarta.Baki != 0)
                            {
                                AkAkaun aka = new AkAkaun()
                                {
                                    JKWId = JKWId,
                                    AkCartaId1 = _context.AkCarta.FirstOrDefault(x => x.Kod == akCarta.Kod).Id,
                                    Tarikh = DateTime.Parse("2021-12-31"),
                                    NoRujukan = "BAKI AWAL",
                                    Debit = (akCarta.Baki>0)? akCarta.Baki:0,
                                    Kredit = (akCarta.Baki < 0) ? akCarta.Baki : 0
                                };
                                _context.AkAkaun.Add(aka);
                            }
                        }
                        TempData[SD.Success] = "Maklumat berjaya ditambah. Kod Carta adalah " + akCarta.Kod;

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            PopulateList();
            return View(akCarta);
        }

        // GET: AkCarta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PopulateList();
            var akCarta = await _akCartaRepo.GetById((int)id);

            if (akCarta == null)
            {
                return NotFound();
            }
            
            return View(akCarta);
        }

        // POST: AkCarta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AkCarta akCarta, int JKWId, int JJenisId, int JParasId)
        {
            if (id != akCarta.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            string paras = _context.JParas.FirstOrDefault(q => q.Id == JParasId).Kod;
            int kodman = Convert.ToInt32(akCarta.Kod.Substring(1, 1));
            int kodsen = Convert.ToInt32(akCarta.Kod.Substring(2, 1));
            int kodhyaku = Convert.ToInt32(akCarta.Kod.Substring(3, 1));
            int kodju = Convert.ToInt32(akCarta.Kod.Substring(4));
            string prefix = akCarta.Kod.Substring(0, 1);
            bool check = false;
            bool check2 = false;

            if (paras == "1")
            {
                if (kodman > 0 && kodsen == 0 && kodhyaku == 0 && kodju == 0)
                {
                    check = true;
                }
            }
            else if (paras == "2")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku == 0 && kodju == 0)
                {
                    check = true;
                }
            }
            else if (paras == "3")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku > 0 && kodju == 0)
                {
                    check = true;
                }
            }
            else if (paras == "4")
            {
                if (kodman > 0 && kodsen > 0 && kodhyaku > 0 && kodju > 0)
                {
                    check = true;
                }
            }

            if (paras == "4")
            {
                check2 = CheckKod(prefix + (kodman * 10000 + kodsen * 1000 + kodhyaku * 100));
            }
            else if (paras == "3")
            {
                check2 = CheckKod(prefix + (kodman * 10000 + kodsen * 1000));
            }
            else if (paras == "2")
            {
                check2 = CheckKod(prefix + (kodman * 10000));
            }
            else if (paras == "1")
            {
                check2 = true;
            }

            ///////---------------------------------------------------
            if (!check)
            {
                TempData[SD.Error] = "Maklumat gagal ditambah. Kod Carta " + akCarta.Kod + " tidak sesuai untuk Paras " + paras + ". ";
            }
            else if (!check2)
            {
                int parasatas = Convert.ToInt32(paras) - 1;
                TempData[SD.Error] = "Maklumat gagal ditambah. Pastikan Paras " + parasatas + " telah wujud. ";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //AkJurnal akJurnal = await _akJurnalRepo.GetById(akJurnal1.AkJurnalId);

                        //debit = akJurnal.JumDebit + akJurnal1.Debit;
                        //kredit = akJurnal.JumKredit + akJurnal1.Kredit;
                        //akJurnal.JumDebit = debit;
                        //akJurnal.JumKredit = kredit;

                        //await _akJurnalRepo.Update(akJurnal);
                        //await _context.SaveChangesAsync();

                        AkCarta carta = await _akCartaRepo.GetById(akCarta.Id);

                        carta.JKWId = JKWId;
                        carta.JJenisId = JJenisId;
                        carta.Perihal = akCarta.Perihal;
                        carta.JParasId = JParasId;
                        carta.UmumDetail = akCarta.UmumDetail;
                        carta.DebitKredit = akCarta.DebitKredit;
                        carta.Baki = akCarta.Baki;
                        carta.Catatan1 = akCarta.Catatan1;
                        carta.Catatan2 = akCarta.Catatan2;
                        carta.IsBajet = akCarta.IsBajet;
                        carta.UserIdKemaskini = user.UserName;
                        carta.TarKemaskini = DateTime.Now;
                        carta.SuPekerjaKemaskiniId = pekerjaId;

                        try
                        {
                            await _akCartaRepo.Update(carta);
                        }
                        catch { }
                        finally
                        {
                            if (akCarta.Baki != 0)
                            {
                                var checkAka = _context.AkAkaun.Where(x => x.AkCarta1.Kod == carta.Kod && x.NoRujukan == "BAKI AWAL").FirstOrDefault();
                                if (checkAka != null)
                                {
                                    checkAka.Debit = (akCarta.Baki > 0) ? akCarta.Baki : 0;
                                    checkAka.Kredit = (akCarta.Baki < 0) ? (akCarta.Baki*-1) : 0;
                                    _context.AkAkaun.Update(checkAka);
                                }
                                else
                                {
                                    AkAkaun aka = new AkAkaun()
                                    {
                                        JKWId = JKWId,
                                        AkCartaId1 = _context.AkCarta.FirstOrDefault(x => x.Kod == akCarta.Kod).Id,
                                        AkCartaId2 = null,
                                        Tarikh = DateTime.Parse("2021-12-31"),
                                        NoRujukan = "BAKI AWAL",
                                        Debit = (akCarta.Baki > 0) ? akCarta.Baki : 0,
                                        Kredit = (akCarta.Baki < 0) ? (akCarta.Baki * -1) : 0
                                    };
                                    _context.AkAkaun.Add(aka);
                                }
                            }
                        }
                        //insert applog
                        await AddLogAsync("Ubah", carta.Kod, carta.Kod, id, carta.Baki, pekerjaId);
                        //insert applog end

                        await _context.SaveChangesAsync();
                        TempData[SD.Success] = "Data berjaya diubah..!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AkCartaExists(akCarta.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    PopulateList();
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateList();
            return View(akCarta);
        }

        // GET: AkCarta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var akCarta = await _akCartaRepo.GetById((int)id);

            if (akCarta == null)
            {
                return NotFound();
            }

            return View(akCarta);
        }

        // POST: AkCarta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var akCarta = await _context.AkCarta
                .Include(a=>a.JKW)
                .Include(a=>a.JJenis)
                .Include(a=>a.JParas)
                .Include(a=>a.AkAkaun1)
                .Include(a=>a.AkAkaun2)
                .Include(a=>a.AkBank)
                .Include(a=>a.AkBelian1)
                .Include(a => a.AkJurnal1)
                .Include(a => a.AkPO1)
                .Include(a => a.AkTerima1)
                .FirstOrDefaultAsync(m => m.Id == id);

            string kodCarta = akCarta.Kod;

            var user = await _userManager.GetUserAsync(User);
            int? pekerjaId = _context.applicationUsers.Where(b => b.Id == user.Id).FirstOrDefault().SuPekerjaId;

            akCarta.UserIdKemaskini = user.UserName;
            akCarta.TarKemaskini = DateTime.Now;
            akCarta.SuPekerjaKemaskiniId = pekerjaId;

            if (
                akCarta.AkAkaun1.Count > 0||
                akCarta.AkAkaun2.Count > 0||
                akCarta.AkBank.Count>0||
                akCarta.AkBelian1.Count>0||
                akCarta.AkJurnal1.Count>0||
                akCarta.AkPO1.Count>0||
                akCarta.AkTerima1.Count>0
                )
            {
                TempData[SD.Error] = kodCarta + " - " + akCarta.Perihal + " gagal dipadam. Maklumat digunakan dalam sistem. ";
                return RedirectToAction(nameof(Index));
            };

            decimal decimalMaxKodCarta = Convert.ToDecimal(kodCarta.Substring(1));
            if(akCarta.JParas.Kod == "1") 
            {
                decimalMaxKodCarta = (decimalMaxKodCarta / 10000) + 1;
                decimalMaxKodCarta = (Math.Floor(decimalMaxKodCarta) * 10000) - 1;
                string maxKodCarta = kodCarta.Substring(0, 1) + decimalMaxKodCarta.ToString();
                var allCarta = await _akCartaRepo.GetAll();
                allCarta = allCarta
                    .Where(x => x.Kod.CompareTo(kodCarta) >= 0 && x.Kod.CompareTo(maxKodCarta) <= 0)
                    .OrderBy(x => x.Kod).ToList();
                if (allCarta.Count() == 1)
                {
                    _context.AkCarta.Remove(akCarta);
                    //insert applog
                    await AddLogAsync("Hapus", akCarta.Kod + " Paras 1", akCarta.Kod, 0, akCarta.Baki, pekerjaId);
                    //insert applog end
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = kodCarta + " - " + akCarta.Perihal + " berjaya dipadam.";
                }
                else if (allCarta.Count() > 1)
                {
                    TempData[SD.Error] = kodCarta + " - " + akCarta.Perihal + " gagal dipadam.";
                }
                else 
                {
                    TempData[SD.Error] = "Something went wrong!!!";
                };
            }
            else if (akCarta.JParas.Kod == "2")
            {
                decimalMaxKodCarta = (decimalMaxKodCarta / 1000) + 1;
                decimalMaxKodCarta = (Math.Floor(decimalMaxKodCarta) * 1000) - 1;
                string maxKodCarta = kodCarta.Substring(0, 1) + decimalMaxKodCarta.ToString();
                var allCarta = await _akCartaRepo.GetAll();
                allCarta = allCarta
                    .Where(x => x.Kod.CompareTo(kodCarta) >= 0 && x.Kod.CompareTo(maxKodCarta) <= 0)
                    .OrderBy(x => x.Kod).ToList();
                if (allCarta.Count() == 1)
                {
                    _context.AkCarta.Remove(akCarta);
                    //insert applog
                    await AddLogAsync("Hapus", akCarta.Kod + " Paras 2", akCarta.Kod, 0, akCarta.Baki, pekerjaId);
                    //insert applog end
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = kodCarta + " - " + akCarta.Perihal + " berjaya dipadam.";
                }
                else if (allCarta.Count() > 1)
                {
                    TempData[SD.Error] = kodCarta + " - " + akCarta.Perihal + " gagal dipadam.";
                }
                else
                {
                    TempData[SD.Error] = "Something went wrong!!!";
                };
            }
            else if (akCarta.JParas.Kod == "3")
            {
                decimalMaxKodCarta = (decimalMaxKodCarta / 100) + 1;
                decimalMaxKodCarta = (Math.Floor(decimalMaxKodCarta) * 100) - 1;
                string maxKodCarta = kodCarta.Substring(0, 1) + decimalMaxKodCarta.ToString();
                var allCarta = await _akCartaRepo.GetAll();
                allCarta = allCarta
                    .Where(x => x.Kod.CompareTo(kodCarta) >= 0 && x.Kod.CompareTo(maxKodCarta) <= 0)
                    .OrderBy(x => x.Kod).ToList();
                if (allCarta.Count() == 1)
                {
                    _context.AkCarta.Remove(akCarta);
                    //insert applog
                    await AddLogAsync("Hapus", akCarta.Kod + " Paras 3", akCarta.Kod, 0, akCarta.Baki, pekerjaId);
                    //insert applog end
                    await _context.SaveChangesAsync();
                    TempData[SD.Success] = kodCarta + " - " + akCarta.Perihal + " berjaya dipadam.";
                }
                else if (allCarta.Count() > 1)
                {
                    TempData[SD.Error] = kodCarta + " - " + akCarta.Perihal + " gagal dipadam.";
                }
                else
                {
                    TempData[SD.Error] = "Something went wrong!!!";
                };
            }
            else if(akCarta.JParas.Kod == "4")
            {
                _context.AkCarta.Remove(akCarta);
                //insert applog
                await AddLogAsync("Hapus", akCarta.Kod + " Paras 4", akCarta.Kod, 0, akCarta.Baki, pekerjaId);
                //insert applog end
                await _context.SaveChangesAsync();
                TempData[SD.Success] = kodCarta + " - " + akCarta.Perihal + " berjaya dipadam.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AkCartaExists(int id)
        {
            return _context.AkCarta.Any(e => e.Id == id);
        }

        private bool CheckKod(string kod)
        {
            return _context.AkCarta.Any(e => e.Kod == kod);
        }

        // printing List of Carta
        [AllowAnonymous]
        public async Task<IActionResult> PrintCarta()
        {
            IEnumerable<AkCarta> akCarta = await _akCartaRepo.GetAll();

            var company = await _userService.GetCompanyDetails();
            //string customSwitches = "--page-offset 0 --footer-center [page] / [toPage] --footer-font-size 6";

            return new ViewAsPdf("ListCartaPrintPDF",akCarta,
                new ViewDataDictionary(ViewData) {
                    { "NamaSyarikat", company.NamaSyarikat },
                    { "AlamatSyarikat1", company.AlamatSyarikat1 },
                    { "AlamatSyarikat2", company.AlamatSyarikat2 },
                    { "AlamatSyarikat3", company.AlamatSyarikat3 }
                })
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }
        // printing List of Carta end
    }
}
