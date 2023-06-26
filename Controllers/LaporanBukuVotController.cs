using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.PrintModel.Reporting;
using MSNK.Models.Modules.ViewModel;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanBukuVotController : Controller
    {
        public const string modul = "LPB001";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _cache;

        public LaporanBukuVotController(
            ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager,
            IMemoryCache cache)
        {
            _context = applicationDbContext;
            _userManager = userManager;
            _cache = cache;

        }
        public IActionResult Index(int searchKW,
            int searchBahagian,
            string searchTahun,
            int searchCarta1,
            int searchCarta2,
            int searchPenyemak,
            int searchPelulus,
            string searchDateFrom,
            string searchDateTo)
        {

            PopulateList(searchKW, searchBahagian, searchTahun, searchCarta1, searchCarta2, searchPenyemak, searchPelulus, searchDateFrom, searchDateTo);
            return View();
        }

        private void PopulateList(int searchKW,
                                  int SearchBahagian,
                                  string searchTahun,
                                  int searchCarta1,
                                  int searchCarta2,
                                  int searchPenyemak,
                                  int searchPelulus,
                                  string searchDateFrom,
                                  string searchDateTo)
        {
            if (String.IsNullOrEmpty(searchTahun))
                searchTahun = DateTime.Now.ToString("yyyy");

            ViewData["searchDateFrom"] = searchDateFrom;
            ViewData["searchDateTo"] = searchDateTo;

            ViewData["searchTahun"] = searchTahun;

            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            List<JKW> kwSelect = new List<JKW>();

            foreach (var q in kwList)
            {

                kwSelect.Add(new JKW() { Id = q.Id, Kod = q.Kod, Perihal = q.Perihal });
            }

            ViewBag.Kw = kwSelect;

            List<JBahagian> bahagianList = _context.JBahagian.OrderBy(b => b.Kod).ToList();
            List<JBahagian> bahagianSelect = new List<JBahagian>{
                new JBahagian() { Id = 0, Kod = "SEMUA", Perihal = "" }
            };

            foreach (var q in bahagianList)
            {

                bahagianSelect.Add(new JBahagian() { Id = q.Id, Kod = q.Kod, Perihal = q.Perihal });
            }

            ViewBag.Bahagian = bahagianSelect;

            List<JPenyemak> penyemakList = _context.JPenyemak.Include(b => b.SuPekerja).OrderBy(b => b.SuPekerja.Nama).Where(b => b.IsLaporanBukuVot == true).ToList();
            List<SuPekerja> penyemakSelect = new List<SuPekerja>();

            foreach (var q in penyemakList)
            {

                penyemakSelect.Add(new SuPekerja() { Id = q.Id, NoGaji = q.SuPekerja.NoGaji, Nama = q.SuPekerja.Nama });
            }

            ViewBag.Penyemak = penyemakSelect;

            List<JPelulus> pelulusList = _context.JPelulus.Include(b => b.SuPekerja).OrderBy(b => b.SuPekerja.Nama).Where(b => b.IsLaporanBukuVot == true).ToList();
            List<SuPekerja> pelulusSelect = new List<SuPekerja>();

            foreach (var q in pelulusList)
            {

                pelulusSelect.Add(new SuPekerja() { Id = q.Id, NoGaji = q.SuPekerja.NoGaji, Nama = q.SuPekerja.Nama });
            }

            ViewBag.Pelulus = pelulusSelect;

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
                PopulateList((int)model.JKWId,
                             (int)model.JBahagianId,
                             model.Tahun,
                             model.IdDari,
                             model.IdHingga,
                             (int)model.JPenyemakId,
                             (int)model.JPelulusId,
                             model.tarikhDari,
                             model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            }

            if (model.JBahagianId != null || model.JBahagianId == 0)
            {
                JBahagian bahagian = await _context.JBahagian.FirstOrDefaultAsync(k => k.Id == model.JBahagianId);
                model.JBahagian = bahagian;
            }
            else
            {
                TempData[SD.Error] = "Sila pilih Bahagian";
                PopulateList((int)model.JKWId,
                             (int)model.JBahagianId,
                             model.Tahun,
                             model.IdDari,
                             model.IdHingga,
                             (int)model.JPenyemakId,
                             (int)model.JPelulusId,
                             model.tarikhDari,
                             model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            }

            if (model.tarikhDari == "" && model.tarikhHingga == "")
            {
                TempData[SD.Error] = "Sila isi julat tarikh.";
                PopulateList((int)model.JKWId,
                             (int)model.JBahagianId,
                             model.Tahun,
                             model.IdDari,
                             model.IdHingga,
                             (int)model.JPenyemakId,
                             (int)model.JPelulusId,
                             model.tarikhDari,
                             model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            }

            if (model.IdDari == 0 && model.IdHingga == 0)
            {
                TempData[SD.Error] = "Sila isi julat kod akaun";
                PopulateList((int)model.JKWId,
                             (int)model.JBahagianId,
                             model.Tahun,
                             model.IdDari,
                             model.IdHingga,
                             (int)model.JPenyemakId,
                             (int)model.JPelulusId,
                             model.tarikhDari,
                             model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            }


            LPB001PrintModel printModel = new LPB001PrintModel();

            if (kodLaporan != null)
            {
                var abBukuVot = await _context.AbBukuVot
                .Include(x => x.Vot).Include(x => x.JKW).Include(x => x.JBahagian)
                .OrderBy(b => b.JBahagian.Kod).ThenBy(b => b.Vot.Kod).ThenBy(b => b.Tarikh)
                .ToListAsync();

                List<AbBukuVotDetailViewModel> abBukuVotList = new List<AbBukuVotDetailViewModel>();
                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();

                date1 = DateTime.Parse(model.tarikhDari);
                date2 = DateTime.Parse(model.tarikhHingga).AddHours(23.99);

                printModel.ParamTarikh = Convert.ToDateTime(model.tarikhDari).ToString("dd/MM/yyyy") + " -> " + Convert.ToDateTime(model.tarikhHingga).ToString("dd/MM/yyyy");

                // filter kw
                if (model.JKWId != 0)
                {
                    abBukuVot = abBukuVot.Where(q => q.JKWId == model.JKWId).ToList();
                    printModel.ParamKW = model.JKW.Kod + " - " + model.JKW.Perihal;
                }
                else
                {
                    printModel.ParamKW = "SEMUA";
                }
                //

                // filter bahagian
                if (model.JBahagianId != 0)
                {
                    abBukuVot = abBukuVot.Where(q => q.JBahagianId == model.JBahagianId).ToList();
                    printModel.ParamBahagian = model.JBahagian.Kod + " - " + model.JBahagian.Perihal;
                }
                else
                {
                    printModel.ParamBahagian = "SEMUA";
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
                    abBukuVot = abBukuVot.Where(s =>
                        range.Item1.CompareTo(s.Vot.Kod.Substring(0, range.Item1.Length)) <= 0 &&
                        s.Vot.Kod.Substring(0, range.Item2.Length).CompareTo(range.Item2) <= 0).ToList();

                }

                // filter tarikh
                abBukuVot = abBukuVot.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                //

                foreach (var i in abBukuVot)
                {
                    abBukuVotList.Add(new AbBukuVotDetailViewModel()
                    {
                        Id = i.Id,
                        JKW = i.JKW.Kod + " - " + i.JKW.Perihal,
                        JBahagian = i.JBahagian.Kod,
                        Vot = i.Vot.Kod + " - " + i.Vot.Perihal,
                        Tarikh = i.Tarikh,
                        Kod = i.Kod,
                        Nama = i.Penerima,
                        NoRujukan = i.Rujukan,
                        Debit = i.Debit,
                        Kredit = i.Kredit,
                        Tanggungan = i.Tanggungan,
                        Liabiliti = i.Liabiliti,
                        Baki = i.Baki
                    });
                }

                var user = await _userManager.GetUserAsync(User);
                var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

                printModel.Username = namaUser.Nama;

                //penyemak
                var penyemak = await _context.JPenyemak.Include(b => b.SuPekerja).FirstOrDefaultAsync(b => b.Id == model.JPenyemakId);

                printModel.Penyemak = penyemak?.SuPekerja?.Nama ?? "";
                printModel.JawatanPenyemak = penyemak?.SuPekerja?.Jawatan ?? "";

                //pelulus
                var pelulus = await _context.JPelulus.Include(b => b.SuPekerja).FirstOrDefaultAsync(b => b.Id == model.JPelulusId);

                printModel.Pelulus = pelulus?.SuPekerja?.Nama ?? "";
                printModel.JawatanPelulus = pelulus?.SuPekerja?.Jawatan ?? "";

                printModel.KodLaporan = model.kodLaporan;

                CompanyDetails company = new CompanyDetails();
                printModel.CompanyDetails = company;

                var GroupedData = abBukuVotList.GroupBy(b => new { JBahagian = b.JBahagian, Vot = b.Vot });

                dynamic dyModel = new ExpandoObject();
                
                dyModel.AbBukuVotGrouped = GroupedData;
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
                PopulateList((int)model.JKWId,
                             (int)model.JBahagianId,
                             model.Tahun,
                             model.IdDari,
                             model.IdHingga,
                             (int)model.JPenyemakId,
                             (int)model.JPelulusId,
                             model.tarikhDari,
                             model.tarikhHingga);
                return RedirectToAction(nameof(Index));
            };

        }
        [HttpPost]
        public async Task<JsonResult> ExportExcel(
        string kodLaporan, ReportFormModel model)
        {
            var pdfName = model.kodLaporan;
            if (model.JKWId != null)
            {
                JKW kw = await _context.JKW.FirstOrDefaultAsync(k => k.Id == model.JKWId);
                model.JKW = kw;
            }
            else
            {
                var message = "Sila pilih Kump. Wang.";
                return Json(new { FileGuid = "error", FileName = message });
            }

            if (model.JBahagianId != null || model.JBahagianId == 0)
            {
                JBahagian bahagian = await _context.JBahagian.FirstOrDefaultAsync(k => k.Id == model.JBahagianId);
                model.JBahagian = bahagian;
            }
            else
            {
                var message = "Sila pilih Bahagian";
                return Json(new { FileGuid = "error", FileName = message });
            }

            if (model.tarikhDari == "" && model.tarikhHingga == "")
            {
                var message = "Sila isi julat tarikh.";
                return Json(new { FileGuid = "error", FileName = message });
            }

            if (model.IdDari == 0 && model.IdHingga == 0)
            {
                var message = "Sila isi julat kod akaun";
                return Json(new { FileGuid = "error", FileName = message });
            }


            LPB001PrintModel printModel = new LPB001PrintModel();

            if (kodLaporan != null)
            {
                var abBukuVot = await _context.AbBukuVot
                .Include(x => x.Vot).Include(x => x.JKW).Include(x => x.JBahagian)
                .OrderBy(b => b.JBahagian.Kod).ThenBy(b => b.Vot.Kod).ThenBy(b => b.Tarikh)
                .ToListAsync();

                List<AbBukuVotDetailViewModel> abBukuVotList = new List<AbBukuVotDetailViewModel>();
                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();

                date1 = DateTime.Parse(model.tarikhDari);
                date2 = DateTime.Parse(model.tarikhHingga).AddHours(23.99);

                printModel.ParamTarikh = Convert.ToDateTime(model.tarikhDari).ToString("dd/MM/yyyy") + " -> " + Convert.ToDateTime(model.tarikhHingga).ToString("dd/MM/yyyy");

                // filter kw
                if (model.JKWId != 0)
                {
                    abBukuVot = abBukuVot.Where(q => q.JKWId == model.JKWId).ToList();
                    printModel.ParamKW = model.JKW.Kod + " - " + model.JKW.Perihal;
                }
                else
                {
                    printModel.ParamKW = "SEMUA";
                }
                //

                // filter bahagian
                if (model.JBahagianId != 0)
                {
                    abBukuVot = abBukuVot.Where(q => q.JBahagianId == model.JBahagianId).ToList();
                    printModel.ParamBahagian = model.JBahagian.Kod + " - " + model.JBahagian.Perihal;
                }
                else
                {
                    printModel.ParamBahagian = "SEMUA";
                }
                //

                //
                // filter range 
                if (model.IdDari != 0 && model.IdHingga != 0)
                {
                    AkCarta carta1 = await _context.AkCarta.FirstOrDefaultAsync(c => c.Id == model.IdDari);
                    if (carta1 == null) return Json(new { FileGuid = "error", FileName = "kod tidak wujud." });

                    AkCarta carta2 = await _context.AkCarta.FirstOrDefaultAsync(c => c.Id == model.IdHingga);
                    if (carta2 == null) return Json(new { FileGuid = "error", FileName = "Kod tidak wujud." });

                    printModel.ParamCarta = carta1.Kod + " - " + carta1.Perihal + " -> " + carta2.Kod + " - " + carta2.Perihal;

                    Tuple<string, string> range = Tuple.Create(carta1.Kod, carta2.Kod);
                    abBukuVot = abBukuVot.Where(s =>
                        range.Item1.CompareTo(s.Vot.Kod.Substring(0, range.Item1.Length)) <= 0 &&
                        s.Vot.Kod.Substring(0, range.Item2.Length).CompareTo(range.Item2) <= 0).ToList();

                }

                // filter tarikh
                abBukuVot = abBukuVot.Where(x => x.Tarikh >= date1
                            && x.Tarikh <= date2).ToList();
                //

                foreach (var i in abBukuVot)
                {
                    abBukuVotList.Add(new AbBukuVotDetailViewModel()
                    {
                        Id = i.Id,
                        JKW = i.JKW.Kod + " - " + i.JKW.Perihal,
                        JBahagian = i.JBahagian.Kod,
                        Vot = i.Vot.Kod + " - " + i.Vot.Perihal,
                        Tarikh = i.Tarikh,
                        Kod = i.Kod,
                        Nama = i.Penerima,
                        NoRujukan = i.Rujukan,
                        Debit = i.Debit,
                        Kredit = i.Kredit,
                        Tanggungan = i.Tanggungan,
                        Liabiliti = i.Liabiliti,
                        Baki = i.Baki
                    });
                }

                var user = await _userManager.GetUserAsync(User);
                var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

                printModel.Username = namaUser.Nama;

                //penyemak
                var penyemak = await _context.JPenyemak.Include(b => b.SuPekerja).FirstOrDefaultAsync(b => b.Id == model.JPenyemakId);

                printModel.Penyemak = penyemak?.SuPekerja?.Nama ?? "";
                printModel.JawatanPenyemak = penyemak?.SuPekerja?.Jawatan ?? "";

                //pelulus
                var pelulus = await _context.JPelulus.Include(b => b.SuPekerja).FirstOrDefaultAsync(b => b.Id == model.JPelulusId);

                printModel.Pelulus = pelulus?.SuPekerja?.Nama ?? "";
                printModel.JawatanPelulus = pelulus?.SuPekerja?.Jawatan ?? "";

                printModel.KodLaporan = model.kodLaporan;

                CompanyDetails company = new CompanyDetails();
                printModel.CompanyDetails = company;

                printModel.abBukuVotDetailList = abBukuVotList.OrderBy(b => b.JKW).ThenBy(b => b.JBahagian).ThenBy(b => b.Vot).ToList();

                var GroupedData = abBukuVotList.GroupBy(b => new { JBahagian = b.JBahagian, Vot = b.Vot });

                // Generate a new unique identifier against which the file can be stored
                string handle = string.Format("attachment;" +kodLaporan+ ".xlsx;", string.IsNullOrEmpty(kodLaporan) ? Guid.NewGuid().ToString() : WebUtility.UrlEncode(kodLaporan));

                List<DataTable> excelDataList = new List<DataTable>();

                if (GroupedData != null)
                {
                    foreach ( var group in GroupedData )
                    {
                            var excelData = GetExcelDataLPB00101(group);
                            excelDataList.Add(excelData);
                    }
                }
                
                RunWorkBookLPB00101(printModel, printModel.CompanyDetails, excelDataList, handle);


                // Note we are returning a filename as well as the handle
                return Json(new { FileGuid = handle, FileName = kodLaporan + ".xlsx" });

            }
            else
            {
                // Note we are returning a filename as well as the handle
                return Json(new { FileGuid = "error", FileName = kodLaporan + ".xlsx" });

            }
        }
        private DataTable GetExcelDataLPB00101(IGrouping<object,AbBukuVotDetailViewModel> model)
        {
            DataTable dt = new DataTable();
            var tableName = "";
            foreach (var item in model)
            {
                tableName = item.JBahagian + "_" + item.Vot.Substring(0,6);
            }
            dt.TableName = tableName;
            dt.Columns.Add("Bil", typeof(int));
            dt.Columns.Add("Tarikh", typeof(DateTime));
            dt.Columns.Add("Nama", typeof(string));
            dt.Columns.Add("No Rujukan", typeof(string));
            dt.Columns.Add("Debit RM", typeof(decimal));
            dt.Columns.Add("Kredit RM", typeof(decimal));
            dt.Columns.Add("Tanggungan RM", typeof(decimal));
            dt.Columns.Add("Liabiliti RM", typeof(decimal));
            dt.Columns.Add("Baki RM", typeof(decimal));

            if (model != null)
            {

                var bil = 1;
                decimal baki = 0;
                decimal jumDebit = 0;
                decimal jumKredit = 0;
                decimal jumTanggung = 0;
                decimal jumLiabiliti = 0;

                foreach (var item in model)
                {
                    jumDebit += item.Debit;
                    jumKredit += item.Kredit;
                    jumTanggung += item.Tanggungan;
                    jumLiabiliti += item.Liabiliti;

                    baki += item.Kredit - item.Debit - item.Tanggungan;

                    DataRow workRow = dt.NewRow();
                    workRow[0] = bil;
                    workRow[1] = item.Tarikh;
                    workRow[2] = item.Nama?.ToUpper() ?? "";
                    workRow[3] = item.NoRujukan ?? "";
                    workRow[4] = item.Debit;
                    workRow[5] = item.Kredit;
                    workRow[6] = item.Tanggungan;
                    workRow[7] = item.Liabiliti;
                    workRow[8] = baki;

                    dt.Rows.Add(workRow);
                    bil++;
                }

            }

            return dt;
        }

        private void RunWorkBookLPB00101(LPB001PrintModel printModel, CompanyDetails company, List<DataTable> excelDataList, string handle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (excelDataList != null && excelDataList.Count > 0)
                {
                    foreach (var excelData in excelDataList)
                    {
                        var ws = wb.AddWorksheet(excelData.TableName);

                        ws.Cell("A1").Value = company.NamaSyarikat;
                        ws.Cell("A1").Style.Font.Bold = true;
                        ws.Cell("A2").Value = "SENARAI LEJAR BUKU VOT " + printModel.ParamCarta + " BAGI KW : " + printModel.ParamKW;
                        ws.Cell("A3").Value = "BAHAGIAN : " + printModel.ParamBahagian;
                        ws.Cell("A4").Value = "DARI TARIKH : " + printModel.ParamTarikh;

                        ws.ColumnWidth = 9;
                        ws.Cell("A6").InsertTable(excelData)
                            .Theme = XLTableTheme.TableStyleMedium1;

                        ws.Column(2)
                            .Style.DateFormat.Format = "dd/MM/yyyy hh:mm:ss";
                        ws.Column(2).AdjustToContents();
                        ws.Column(3).AdjustToContents();
                        ws.Column(4).AdjustToContents();
                        ws.Column(5)
                           .Style.NumberFormat.Format = " #,##0.00";
                        ws.Column(5).AdjustToContents();
                        ws.Column(6)
                           .Style.NumberFormat.Format = " #,##0.00";
                        ws.Column(6).AdjustToContents();
                        ws.Column(7)
                           .Style.NumberFormat.Format = " #,##0.00";
                        ws.Column(7).AdjustToContents();
                        ws.Column(8)
                           .Style.NumberFormat.Format = " #,##0.00";
                        ws.Column(8).AdjustToContents();
                        ws.Column(9)
                           .Style.NumberFormat.Format = " #,##0.00";
                        ws.Column(9).AdjustToContents();
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    //return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportModel.KodLaporan + ".xlsx");

                    //This is an equivalent to tempdata, but requires manual cleanup
                    _cache.Set(handle, ms.ToArray(),
                                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    //(I'd recommend you revise the expiration specifics to suit your application)

                }

            }
        }
    }
}
