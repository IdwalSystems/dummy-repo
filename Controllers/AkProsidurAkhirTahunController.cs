using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "AK004")]
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class AkProsidurAkhirTahunController : Controller
    {
        public const string modul = "AK004";
        public const string namamodul = "Prosidur Akhir Tahun";
        private readonly ApplicationDbContext context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly IRepository<AkAkaun, int, string> akAkaunRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AkProsidurAkhirTahunController(ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            UserManager<IdentityUser> userManager)
        {
            this.context=context;
            this._appLog=appLog;
            this.akAkaunRepository=akAkaunRepository;
            this.userManager=userManager;
        }
        private async Task AddLogAsync(
            string operasi,
            string nota,
            string rujukan,
            int idRujukan,
            decimal jumlah,
            int? pekerjaId)
        {
            var user = await userManager.GetUserAsync(User);
            AppLog appLog = new AppLog();

            appLog.IdRujukan = idRujukan;
            appLog.UserId = user.UserName;
            appLog.NoRujukan = rujukan;
            appLog.LgNote = namamodul + " - " + nota;
            appLog.Jumlah = jumlah;
            appLog.SuPekerjaId = pekerjaId;

            await _appLog.Insert(appLog, modul, operasi);
        }

        public IActionResult Index()
        {
            ViewBag.Tahun = DateTime.Now.AddYears(-1).ToString("yyyy");
            ViewBag.JKW = context.JKW.ToList();
            ViewBag.AkCarta = context.AkCarta.Where(b => b.Kod.StartsWith("E") && b.UmumDetail == "D").OrderBy(b => b.Kod).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AK004T")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateProcedure(string tahun, int JKWId, int AkCartaEkuitiId)
        {
            // check if already generate procedure in stated year and kump wang
            if (ProcedureExists(tahun, JKWId))
            {
                TempData[SD.Error] = "Tahun dan Kump. Wang yang dimasukkan telahpun dibuat proses penutupan akaun!";
                return RedirectToAction(nameof(Index));
            }

            // user should not key in current year for generating procedure
            if (tahun == DateTime.Now.ToString("yyyy"))
            {
                TempData[SD.Error] = "Tahun semasa tidak dibenarkan untuk proses penutupan akaun!";
                return RedirectToAction(nameof(Index));
            }

            var jkw = await context.JKW.FirstOrDefaultAsync(b => b.Id == JKWId);
            if (jkw == null)
            {
                TempData[SD.Error] = "Kump Wang tidak wujud!";
                return RedirectToAction(nameof(Index));
            }

            var cartaEkuiti = await context.AkCarta.FirstOrDefaultAsync(b => b.Id == AkCartaEkuitiId);
            if (cartaEkuiti == null)
            {
                TempData[SD.Error] = "Kod Akaun tidak wujud!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                int? pekerjaId = context.applicationUsers.FirstOrDefault(b => b.Id == user.Id).SuPekerjaId;
                
                // 1.0 zerorize BELANJA into EKUITI
                // 1.1 get all kod akaun
                //  -- that starts with 'B' (Belanja)
                //  -- kump. wang == {JKWId}
                //  -- year(tarikh) == {tahun}
                //  from akaun [to be inserted into kod 'E']
                // 1.2 insert into akAkaun where Belanja =/= Ekuiti

                List<AkAkaun> listBelanja = context.AkAkaun
                    .Include(a => a.AkCarta1)
                    .ToList();

                listBelanja = listBelanja
                    .Where(a => a.AkCarta1.Kod.StartsWith("B")).ToList();

                listBelanja = listBelanja
                    .Where(a => a.JKWId == JKWId).ToList();

                listBelanja = listBelanja
                    .Where(a => a.Tarikh.ToString("yyyy") == tahun).ToList();

                listBelanja = listBelanja.GroupBy(b => b.AkCartaId1)
                    .Select(l => new AkAkaun{
                        JKWId = l.First().JKWId,
                        JBahagianId = l.First().JBahagianId,
                        AkCartaId1 = l.First().AkCartaId1,
                        AkCartaId2 = l.First().AkCartaId2,
                        Tarikh = l.First().Tarikh,
                        Tahun = l.First().Tahun,
                        Debit = 0,
                        Kredit = l.Sum(s => s.Debit-s.Kredit)
                }).ToList();

                foreach (var item in listBelanja)
                {
                    AkAkaun akaunKredit = new()
                    {
                        JKWId = JKWId,
                        JBahagianId = item.JBahagianId,
                        AkCartaId1 = item.AkCartaId1,
                        AkCartaId2 = AkCartaEkuitiId,
                        Tarikh = new DateTime(int.Parse(tahun), 12, 31),
                        Tahun = tahun,
                        NoRujukan = "ENDYEAR-" + tahun,
                        Debit =  0,
                        Kredit = item.Kredit
                    };

                    await akAkaunRepository.Insert(akaunKredit);

                    AkAkaun akaunDebit = new()
                    {
                        JKWId = JKWId,
                        JBahagianId = item.JBahagianId,
                        AkCartaId1 = AkCartaEkuitiId,
                        AkCartaId2 = item.AkCartaId1,
                        Tarikh = new DateTime(int.Parse(tahun), 12, 31),
                        Tahun = tahun,
                        NoRujukan = "ENDYEAR-" + tahun,
                        Debit =  item.Kredit,
                        Kredit = 0
                    };
                    await akAkaunRepository.Insert(akaunDebit);
                }
                // 2.0 zerorize HASIL into EKUITI
                // 2.1 get all kod akaun
                // -- that starts with 'H'
                // -- kump. wang == {JKWId}
                // -- year(tarikh) == {tahun}
                // (Hasil) from akaun [to be inserted into kod 'E']
                // 2.2 insert into akAkaun where Hasil =/= Ekuiti
                List<AkAkaun> listHasil = context.AkAkaun
                    .Include(a => a.AkCarta1)
                    .Where(a => a.AkCarta1.Kod.StartsWith("H")
                    && a.JKWId == JKWId)
                    .ToList();

                listHasil = listHasil
                    .Where(a => a.Tarikh.ToString("yyyy") == tahun)
                    .ToList();

                listHasil = listHasil.GroupBy(b => b.AkCartaId1)
                    .Select(l => new AkAkaun
                    {
                        JKWId = l.First().JKWId,
                        JBahagianId = l.First().JBahagianId,
                        AkCartaId1 = l.First().AkCartaId1,
                        AkCartaId2 = l.First().AkCartaId2,
                        Tarikh = l.First().Tarikh,
                        Tahun = l.First().Tahun,
                        Debit = 0,
                        Kredit = l.Sum(s => s.Kredit-s.Debit)
                    }).ToList();

                foreach (var item in listHasil)
                {
                    AkAkaun akaunKredit = new()
                    {
                        JKWId = JKWId,
                        JBahagianId = item.JBahagianId,
                        AkCartaId1 = item.AkCartaId1,
                        AkCartaId2 = AkCartaEkuitiId,
                        Tarikh = new DateTime(int.Parse(tahun), 12, 31),
                        Tahun = tahun,
                        NoRujukan = "ENDYEAR-" + tahun,
                        Debit =  item.Kredit,
                        Kredit = 0
                    };

                    await akAkaunRepository.Insert(akaunKredit);

                    AkAkaun akaunDebit = new()
                    {
                        JKWId = JKWId,
                        JBahagianId = item.JBahagianId,
                        AkCartaId1 = AkCartaEkuitiId,
                        AkCartaId2 = item.AkCartaId1,
                        Tarikh = new DateTime(int.Parse(tahun), 12, 31),
                        Tahun = tahun,
                        NoRujukan = "ENDYEAR-" + tahun,
                        Debit =  0,
                        Kredit = item.Kredit
                    };
                    await akAkaunRepository.Insert(akaunDebit);
                }
                //insert applog
                await AddLogAsync("Posting", "Janaan Prosidur Akhir Tahun - " + tahun + " bagi Kump Wang " + jkw.Kod + " menggunakan kod akaun " + cartaEkuiti.Kod , "ENDYEAR-"+tahun, 0, 0, pekerjaId);
                //insert applog end
                await context.SaveChangesAsync();

                TempData[SD.Success] = "Prosidur berjaya dijana..!";
                return RedirectToAction(nameof(Index));
            }

            TempData[SD.Error] = "Terdapat ralat semasa janaan!";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Authorize(Policy = "AK004UT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProcedure(string tahun, int JKWId)
        {
            var jkw = await context.JKW.FirstOrDefaultAsync(b => b.Id == JKWId);
            if (jkw == null)
            {
                TempData[SD.Error] = "Kump Wang tidak wujud!";
                return RedirectToAction(nameof(Index));
            }

            List<AkAkaun> akaun = context.AkAkaun.Where(x => x.NoRujukan == "ENDYEAR-" + tahun && x.JKWId == JKWId).ToList();

            if (akaun == null || akaun.Count() == 0)
            {
                TempData[SD.Error] = "Proses penutupan akaun belum dibuat pada tahun " +tahun+ " bagi Kump Wang " +jkw.Kod+ " !";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                int? pekerjaId = context.applicationUsers.FirstOrDefault(b => b.Id == user.Id).SuPekerjaId;

                foreach (var item in akaun)
                {
                    await akAkaunRepository.Delete(item.Id);
                }
                //insert applog
                await AddLogAsync("UnPosting", "Batal janaan Prosidur Akhir Tahun - " + tahun + " bagi Kump Wang " + jkw.Kod, "ENDYEAR-"+tahun, 0, 0, pekerjaId);
                //insert applog end
                await context.SaveChangesAsync();

                TempData[SD.Success] = "Prosidur berjaya dibatalkan..!";
                return RedirectToAction(nameof(Index));
            }
            TempData[SD.Error] = "Terdapat ralat semasa pembatalan janaan!";
            return RedirectToAction(nameof(Index));
        }
        private bool ProcedureExists(string tahun, int JKWId)
        {
            return context.AkAkaun.Any(e => e.NoRujukan.Contains("ENDYEAR-" + tahun));
        }
    }
}
