using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Infrastructure;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Roles = "SuperAdmin,Supervisor")]
    public class AbPenyataAlirTunaiController : Controller
    {
        public const string modul = "JD0015";
        public const string namamodul = "Penyata Alir Tunai";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CustomIRepository<string, int> _custom;
        private readonly UserService _userService;

        public AbPenyataAlirTunaiController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            CustomIRepository<string, int> custom,
            UserService userService)
        {
            _context = context;
            _userManager = userManager;
            _custom = custom;
            _userService = userService;
        }
        public IActionResult Index(PenyataFormModel form)
        {
            List<_AbPenyataAlirTunai> penyataAlirTunai = new List<_AbPenyataAlirTunai>();

            PopulateSelectList(form.Tahun1, form.Tahun2);

            return View(penyataAlirTunai.OrderBy(p => p.Susunan));
        }
        private void PopulateSelectList(string tahun1, string tahun2)
        {
            // populate tahun
            if (string.IsNullOrWhiteSpace(tahun1))
                ViewData["Tahun1"] = DateTime.Now.Year.ToString();
            else
                ViewData["Tahun1"] = tahun1;

            // populate tahun
            if (string.IsNullOrWhiteSpace(tahun2))
                ViewData["Tahun2"] = DateTime.Now.AddYears(-1).Year.ToString();
            else
                ViewData["Tahun2"] = tahun2;
        }

        public async Task<JsonResult> GetPenyata(string tahun1, string tahun2)
        {
            try
            {
                if (tahun1 == null)
                {
                    tahun1 = DateTime.Now.Year.ToString();
                }

                if (tahun2 == null)
                {
                    tahun2 = DateTime.Now.AddYears(-1).Year.ToString();
                }

                List<_AbPenyataAlirTunai> penyataAlirTunai = new List<_AbPenyataAlirTunai>();
                JKonfigPenyata konfigPenyata = new JKonfigPenyata();
                if (tahun1 != null && tahun2 != null)
                {
                    // get JKonfigPenyata (include JKonfigPenyataBaris, include JKonfigPenyataBarisFormula) by tahun, modul
                    konfigPenyata = await _context.JKonfigPenyata.Include(p => p.JKonfigPenyataBaris).FirstOrDefaultAsync(p => p.Kod == modul && (p.Tahun == tahun1));
                    //penyataAlirTunai = await _custom.GetAbPenyataAlirTunaiComparedByYears(modul, tahun1, tahun2);

                    
                    if (konfigPenyata != null)
                    {
                        if (konfigPenyata.JKonfigPenyataBaris != null && konfigPenyata.JKonfigPenyataBaris.Any())
                        {
                            foreach (var baris in konfigPenyata.JKonfigPenyataBaris)
                            {
                                penyataAlirTunai.Add(new _AbPenyataAlirTunai
                                {
                                    JKonfigPenyataBarisId1 = baris.Id,
                                    Susunan = baris.Susunan,
                                    Perihal = baris.Perihal,
                                    Amaun1 = 0,
                                    Amaun2 = 0,
                                    Tahun = konfigPenyata.Tahun,
                                    EnKategoriTajuk = baris.EnKategoriTajuk,
                                    EnKategoriJumlah = baris.EnKategoriJumlah
                                });
                            }
                        }
                        
                    }
                    // get JKonfigPenyata (include JKonfigPenyataBaris, include JKonfigPenyataBarisFormula) by tahun, modul
                    konfigPenyata = await _context.JKonfigPenyata.Include(p => p.JKonfigPenyataBaris).FirstOrDefaultAsync(p => p.Kod == modul && (p.Tahun == tahun2));
                    //penyataAlirTunai = await _custom.GetAbPenyataAlirTunaiComparedByYears(modul, tahun1, tahun2);


                    if (konfigPenyata != null)
                    {
                        if (konfigPenyata.JKonfigPenyataBaris != null && konfigPenyata.JKonfigPenyataBaris.Any())
                        {
                            foreach (var baris in konfigPenyata.JKonfigPenyataBaris)
                            {
                                if (penyataAlirTunai.Any(p => p.Perihal == baris.Perihal))
                                {
                                    foreach (var item in penyataAlirTunai)
                                    {
                                        if (item.Perihal == baris.Perihal)
                                        {
                                            item.JKonfigPenyataBarisId2 = baris.Id;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    penyataAlirTunai.Add(new _AbPenyataAlirTunai
                                    {
                                        JKonfigPenyataBarisId2 = baris.Id,
                                        Susunan = baris.Susunan,
                                        Perihal = baris.Perihal,
                                        Amaun1 = 0,
                                        Amaun2 = 0,
                                        Tahun = konfigPenyata.Tahun,
                                        EnKategoriTajuk = baris.EnKategoriTajuk,
                                        EnKategoriJumlah = baris.EnKategoriJumlah
                                    });
                                }
                                
                            }
                        }

                    }
                }

                penyataAlirTunai = penyataAlirTunai.GroupBy(p => new { p.Perihal }).Select(l => new _AbPenyataAlirTunai
                {
                    JKonfigPenyataBarisId1 = l.First().JKonfigPenyataBarisId1,
                    JKonfigPenyataBarisId2 = l.First().JKonfigPenyataBarisId2,
                    Susunan = l.First().Susunan,
                    Perihal = l.First().Perihal,
                    Amaun1 = l.Sum(p => p.Amaun1),
                    Amaun2 = l.Sum(p => p.Amaun2),
                    Tahun = l.First().Tahun,
                    EnKategoriTajuk = l.First().EnKategoriTajuk,
                    EnKategoriJumlah = l.First().EnKategoriJumlah
                }).OrderBy(p => p.Susunan).ToList();

                return Json(new { result = "OK", record = penyataAlirTunai });

            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        private decimal CalculateBalance(List<AkAkaun> akAkaunList, EnJenisOperasi enJenisOperasi)
        {
            decimal localBalance = 0;

            foreach (var akaun in akAkaunList)
            {
                decimal debitKreditDifference = akaun.AkCarta1!.DebitKredit == "D" ? akaun.Debit - akaun.Kredit : akaun.Kredit - akaun.Debit;

                localBalance += enJenisOperasi == EnJenisOperasi.Tambah ? debitKreditDifference : -debitKreditDifference;
            }

            return localBalance;
        }

        [HttpPost]
        public async Task<JsonResult> GetPenyataAmount(int Id1, int Id2, string Tahun1, string Tahun2)
        {
            try
            {
                
                List<JKonfigPenyataBaris> barisList = await _context.JKonfigPenyataBaris
                    .Include(b => b.JKonfigPenyata).Include(b => b.JKonfigPenyataBarisFormula)
                    .Where(b => b.Id == Id1 || b.Id == Id2)
                    .ToListAsync();

                List<_AbPenyataAlirTunai> penyataAlirTunai = new List<_AbPenyataAlirTunai>();

                if (barisList != null && barisList.Any())
                {
                    penyataAlirTunai = await _custom.GetAbPenyataAlirTunaiComparedByJKonfigPenyataBarisId(barisList, Tahun1, Tahun2);
                }

                return Json(new { result = "OK", record = penyataAlirTunai });

            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", message = ex.Message });
            }
        }

        public async Task<IActionResult> PrintPDF(PenyataFormModel form)
        {
            List<_AbPenyataAlirTunai> penyataAlirTunai = new List<_AbPenyataAlirTunai>();

            if (form.Tahun1 == null)
            {
                form.Tahun1 = DateTime.Now.Year.ToString();
            }

            if (form.Tahun2 == null)
            {
                form.Tahun2 = DateTime.Now.AddYears(-1).Year.ToString();
            }

            PopulateSelectList(form.Tahun1, form.Tahun2);

            if (form.Tahun1 != null && form.Tahun2 != null)
            {
                penyataAlirTunai = await _custom.GetAbPenyataAlirTunaiComparedByYears(modul, form.Tahun1, form.Tahun2);

                var jkw = await _context.JKW.FindAsync(1);

                var company = await _userService.GetCompanyDetails();

                return new ViewAsPdf("AbPenyataAlirTunaiPDF", penyataAlirTunai.OrderBy(p => p.Susunan),
                    new ViewDataDictionary(ViewData)
                    {
                        { "NamaKW", jkw ?.Kod + " - " + jkw?.Perihal },
                        { "Tahun", form.Tahun1 },
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
            else
            {
                PopulateSelectList(form.Tahun1 ?? DateTime.Now.Year.ToString(), form.Tahun2 ?? DateTime.Now.AddYears(-1).ToString());

                TempData[SD.Error] = "Kump. Wang bagi tahun tersebut tidak wujud.";

                return View(penyataAlirTunai);
            }
        }
    }
}
