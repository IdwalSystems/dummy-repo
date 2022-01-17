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
using MSNK.Models.Modules.IRepository;

namespace MSNK.Controllers
{
    [Authorize(Roles ="Admin , Supervisor")]
    public class AkCartaController : Controller
    {
        public const string modul = "JU001";
        public const string namamodul = "JU001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkCarta, int> _akCartaRepo;
        private readonly IRepository<JKW, int> _kwRepo;

        public AkCartaController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<JKW, int> kwRepository,
            IRepository<AkCarta, int> akCartaRepository)
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _akCartaRepo = akCartaRepository;
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
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;

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
        public async Task<IActionResult> Create(AkCarta akCarta, int JKWId, int JJenisId, int JParasId)
        {
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
                        akC.Perihal = akCarta.Perihal;
                        akC.JParasId = JParasId;
                        akC.DebitKredit = akCarta.DebitKredit;
                        akC.UmumDetail = akCarta.UmumDetail;
                        akC.Baki = akCarta.Baki;
                        akC.Catatan1 = akCarta.Catatan1;
                        akC.Catatan2 = akCarta.Catatan2;
                        try {
                            await _akCartaRepo.Insert(akC);
                            await _akCartaRepo.Save();
                        }
                        catch { }
                        finally
                        {
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
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;
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
            var kw = await _kwRepo.GetById(akCarta.JKWId);
            akCarta.JKW = kw;
            var jenis = _context.JJenis.FirstOrDefault(b => b.Id == akCarta.JJenisId);
            akCarta.JJenis = jenis;
            var paras = _context.JParas.FirstOrDefault(b => b.Id == akCarta.JParasId);
            akCarta.JParas = paras;

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

        private async Task AddLogAsync(string operasi, string rujukan, decimal jumlah)
        {
            var user = await _userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.Jumlah = jumlah;

            if(operasi == "Tambah")
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
            else if (operasi == "TambahObjek")
            {
                appLog.LgModule = modul + "EC";
                appLog.LgOperation = "Tambah";
                appLog.LgNote = modul + " " + namamodul + " - Tambah Objek";
            }
            else if (operasi == "HapusObjek")
            {
                appLog.LgModule = modul + "ED";
                appLog.LgOperation = "Hapus";
                appLog.LgNote = modul + " " + namamodul + " - Hapus Objek";
            }
            else if (operasi == "UbahObjek")
            {
                appLog.LgModule = modul + "EE";
                appLog.LgOperation = "Ubah";
                appLog.LgNote = modul + " " + namamodul + " - Ubah Objek";
            }
            else if (operasi == "TambahPerihal")
            {
                appLog.LgModule = modul + "EC";
                appLog.LgOperation = "Tambah";
                appLog.LgNote = modul + " " + namamodul + " - Tambah Perihal";
            }
            else if(operasi == "HapusPerihal")
            {
                appLog.LgModule = modul + "ED";
                appLog.LgOperation = "Hapus";
                appLog.LgNote = modul + " " + namamodul + " - Hapus Perihal";
            }
            else if (operasi == "UbahPerihal")
            {
                appLog.LgModule = modul + "EE";
                appLog.LgOperation = "Ubah";
                appLog.LgNote = modul + " " + namamodul + " - Ubah Perihal";
            }
            else if (operasi=="Posting")
            {
                appLog.LgModule = modul + "T";
                appLog.LgOperation = "Posting";
                appLog.LgNote = modul + " " + namamodul + " - Posting";
            }
            else if (operasi=="UnPosting")
            {
                appLog.LgModule = modul + "UT";
                appLog.LgOperation = "UnPosting";
                appLog.LgNote = modul + " " + namamodul + " - UnPosting";
            }
            else if (operasi=="Cetak") 
            {
                appLog.LgModule = modul + "P";
                appLog.LgOperation = "Cetak";
                appLog.LgNote = modul +" "+ namamodul + " - Cetak";
            }
            await _appLog.Insert(appLog);
        }
    }
}
