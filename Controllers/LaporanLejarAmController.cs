using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.PrintModel.Reporting;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanLejarAmController : Controller
    {
        public const string modul = "LPL001";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LaporanLejarAmController(
            ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context=context;
            _userManager=userManager;
        }

        public IActionResult Index(
            int searchKW,
            int searchCarta1,
            int searchCarta2,
            string searchDateFrom,
            string searchDateTo)
        {
            PopulateList(searchKW, searchCarta1, searchCarta2, searchDateFrom, searchDateTo);
            return View();
        }

        private void PopulateList(int searchKW, int searchCarta1, int searchCarta2, string searchDateFrom, string searchDateTo)
        {

            ViewData["searchDateFrom"] = searchDateFrom;
            ViewData["searchDateTo"] = searchDateTo;

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            List<JKW> kwSelect = new List<JKW>
            {
                new JKW() { Id = 0, Kod = "SEMUA", Perihal = "" }
            };
            foreach (var q in kwList)
            {
                
                kwSelect.Add(new JKW() { Id = q.Id, Kod = q.Kod, Perihal = q.Perihal });
            }

            ViewBag.Kw = kwSelect;

            List<AkCarta> cartaList1 = _context.AkCarta.Include(b => b.JParas)
                .Where(b => b.JParas.Kod == "4")
                .OrderBy(b => b.Kod)
                .ToList();

            List<AkCarta> carta1Select = new List<AkCarta>();
            foreach (var q in cartaList1)
            {
                carta1Select.Add(new AkCarta() { Id = q.Id, Kod = q.Kod, Perihal = q.Perihal });
            }

            ViewBag.Carta1 = carta1Select;

            List<AkCarta> carta2Select = new List<AkCarta>();
            foreach (var q in cartaList1)
            {
                carta2Select.Add(new AkCarta() { Id = q.Id, Kod = q.Kod, Perihal = q.Perihal });
            }

            ViewBag.Carta2 = carta2Select;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(string kodLaporan, ReportFormModel model)
        {
            var pdfName = model.kodLaporan;
            if (model.JKWId != null)
            {
                JKW kw = await _context.JKW.FirstOrDefaultAsync(k => k.Id == model.JKWId);
                model.JKW = kw;
            }
            else
            {
                TempData[SD.Error] = "Sila pilih Kump. Wang.";
                PopulateList((int)model.JKWId, model.IdDari, model.IdHingga, model.tarikhDari, model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            }

            LPL001PrintModel printModel = new LPL001PrintModel();

            if (kodLaporan != null)
            {
                List<AkAkaun> akAkaun = await _context.AkAkaun
                .Include(b => b.JKW)
                .Include(b => b.AkCarta1)
                .Include(b => b.AkCarta2)
                .OrderBy( b => b.Tarikh)
                .ToListAsync();

                List<AkAkaun> akAkaunList = new();
                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();

                if (model.tarikhDari != "" && model.tarikhHingga != "")
                {
                    date1 = DateTime.Parse(model.tarikhDari);
                    date2 = DateTime.Parse(model.tarikhHingga).AddHours(23.99);

                    printModel.ParamTarikh = Convert.ToDateTime(model.tarikhDari).ToString("dd/MM/yyyy") + " -> " + Convert.ToDateTime(model.tarikhHingga).ToString("dd/MM/yyyy");
                }
                else
                {
                    TempData[SD.Error] = "Sila isi julat tarikh.";
                    PopulateList((int)model.JKWId, model.IdDari, model.IdHingga, model.tarikhDari, model.tarikhHingga);
                    return RedirectToAction(nameof(Index));
                }

                // filter kw
                if (model.JKWId != 0)
                {
                    akAkaun = akAkaun.Where(q => q.JKWId == model.JKWId).ToList();
                    printModel.ParamKW = model.JKW.Kod + " - " + model.JKW.Perihal;
                }
                else
                {
                    printModel.ParamKW = "SEMUA";
                }
                //
                
                //
                // filter range 
                if (model.IdDari != 0 && model.IdHingga != 0)
                {
                    AkCarta carta1 = await _context.AkCarta.FirstOrDefaultAsync(c => c.Id == model.IdDari);
                    if (carta1 == null) return RedirectToAction(nameof(Index));

                    AkCarta carta2 = await _context.AkCarta.FirstOrDefaultAsync(c => c.Id == model.IdHingga);
                    if (carta2 == null) return RedirectToAction(nameof(Index));

                    printModel.ParamCarta = carta1.Kod + " - " + carta1.Perihal + " -> " + carta2.Kod + " - " + carta2.Perihal;

                    Tuple<string, string> range = Tuple.Create(carta1.Kod, carta2.Kod);
                    akAkaun = akAkaun.Where(s =>
                        range.Item1.CompareTo(s.AkCarta1.Kod.Substring(0, range.Item1.Length)) <= 0 &&
                        s.AkCarta1.Kod.Substring(0, range.Item2.Length).CompareTo(range.Item2) <= 0)
                        .OrderBy(x => x.AkCarta1.Kod).ToList();

                    foreach (var i in akAkaun.Where(q => q.Tarikh<date1))
                    {
                        
                        akAkaunList.Add(new AkAkaun()
                        {
                            JKWId = i.JKWId,
                            Tarikh = date1,
                            AkCartaId1 = i.AkCartaId1,
                            AkCarta1 = i.AkCarta1,
                            NoRujukan = "Baki Awal",
                            Debit = i.Debit,
                            Kredit = i.Kredit,
                            JKW = i.JKW
                        });
                    };

                    akAkaunList = akAkaunList.GroupBy(x => new { x.JKWId, x.AkCartaId1, x.AkCartaId2 })
                        .Select( a => new AkAkaun
                        {
                            JKWId = a.First().JKWId,
                            AkCartaId1 = a.First().AkCartaId1,
                            Tarikh = a.First().Tarikh,
                            NoRujukan = a.First().NoRujukan,
                            Debit = a.Sum(x => x.Debit),
                            Kredit = a.Sum(x => x.Kredit),
                            JKW = a.First().JKW,
                            AkCarta1 = a.First().AkCarta1
                        }).OrderBy(x => x.AkCarta1.Kod).ToList();
                }
                else
                {
                    TempData[SD.Error] = "Sila isi julat kod akaun";
                    PopulateList((int)model.JKWId, model.IdDari, model.IdHingga, model.tarikhDari, model.tarikhHingga);
                    return RedirectToAction(nameof(Index));
                }
                //

                // filter tarikh
                akAkaun = akAkaun.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                //

                foreach (var i in akAkaun)
                {
                    akAkaunList.Add(new AkAkaun()
                    {
                        JKWId = i.JKWId,
                        AkCartaId1=i.AkCartaId1,
                        Tarikh = i.Tarikh,
                        AkCartaId2=i.AkCartaId2,
                        Id=i.Id,
                        NoRujukan=i.NoRujukan,
                        Debit=i.Debit,
                        Kredit=i.Kredit,
                        JKW=i.JKW,
                        AkCarta1 = i.AkCarta1,
                        AkCarta2 = i.AkCarta2
                    });
                }

                List<AkAkaun> akAkaunGroupedByCarta1 = akAkaunList;

                printModel.AkAkaun = akAkaunGroupedByCarta1;

                List<AkAkaunGroupViewModel> akAkaunGroupViewModels = new List<AkAkaunGroupViewModel>();

                foreach (var item in akAkaunGroupedByCarta1)
                {
                    akAkaunGroupViewModels.Add(new AkAkaunGroupViewModel
                    {
                        Tarikh = item.Tarikh,
                        SearchObjek = item.AkCarta1.Kod + " - " + item.AkCarta1.Perihal,
                        Objek = item.AkCarta2?.Kod + " - " + item.AkCarta2?.Perihal,
                        NoRujukan = item.NoRujukan,
                        Debit = item.Debit,
                        Kredit = item.Kredit,
                        Baki = 0
                    });
                }

                var user = await _userManager.GetUserAsync(User);
                var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

                printModel.Username = namaUser.Nama;

                printModel.KodLaporan = model.kodLaporan;

                CompanyDetails company = new CompanyDetails();
                printModel.CompanyDetails = company;

                dynamic dyModel = new ExpandoObject();
                dyModel.AkAkaunGrouped = akAkaunGroupViewModels.GroupBy(b => b.SearchObjek);
                dyModel.printModel = printModel;

                return new ViewAsPdf(pdfName, dyModel,
                new ViewDataDictionary(ViewData) { 
                { "NamaSyarikat", company.NamaSyarikat },
                { "AlamatSyarikat1", company.AlamatSyarikat1 },
                { "AlamatSyarikat2", company.AlamatSyarikat2 },
                { "AlamatSyarikat3", company.AlamatSyarikat3 }
            })
                {
                    PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 10 },
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };

            }
            else
            {
                PopulateList((int)model.JKWId, model.IdDari, model.IdHingga, model.tarikhDari, model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            };

        }

    }
}
