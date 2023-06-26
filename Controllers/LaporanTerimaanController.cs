using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Modules.PrintModel.Reporting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanTerimaanController : Controller
    {
        public const string modul = "LPR001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkTerima, int, string> _akTerimaRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkTerima1, int> _akTerima1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkTerima2, int> _akTerima2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IMemoryCache _cache;

        public LaporanTerimaanController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkTerima, int, string> akTerimaRepository,
            ListViewIRepository<AkTerima1, int> akTerima1Repository,
            ListViewIRepository<AkTerima2, int> akTerima2Repository,
            IRepository<AkBank, int, string> akBankRepository,
            IRepository<JKW, int, string> kwRepository,
            IRepository<JNegeri, int, string> negeriRepository,
            IRepository<AkCarta, int, string> akCartaRepository,
            IRepository<AkAkaun, int, string> akAkaunRepository,
            IMemoryCache cache
            )
        {
            _context = context;
            _appLog = appLog;
            _userManager = userManager;
            _kwRepo = kwRepository;
            _negeriRepo = negeriRepository;
            _akBankRepo = akBankRepository;
            _akTerimaRepo = akTerimaRepository;
            _akTerima1Repo = akTerima1Repository;
            _akTerima2Repo = akTerima2Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            // list of bank penerima
            List<AkBank> akBankList = await _context.AkBank.Include(b => b.JBank).OrderBy(b => b.Kod).ToListAsync();
            ViewBag.AkBank = akBankList;

            ViewBag.AkBank = akBankList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(
            string kodLaporan,
            string tarikhDari,
            string tarikhHingga,
            int status,
            int AkBankId,
            int susunan)
        {
            LPR001PrintModel reportModel = await PrepareData(kodLaporan, tarikhDari, tarikhHingga, status, AkBankId, susunan);

            dynamic dyModel = new ExpandoObject();

            dyModel.reportModel = reportModel;

            return new ViewAsPdf(kodLaporan, dyModel,
                new ViewDataDictionary(ViewData)
                {
                    { "Tajuk", reportModel.Tajuk },
                    { "NamaSyarikat", reportModel.CompanyDetail.NamaSyarikat },
                    { "AlamatSyarikat1", reportModel.CompanyDetail.AlamatSyarikat1 },
                    { "AlamatSyarikat2", reportModel.CompanyDetail.AlamatSyarikat2 },
                    { "AlamatSyarikat3", reportModel.CompanyDetail.AlamatSyarikat3 }
                })
            {
                PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                            " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        [HttpPost]
        public async Task<JsonResult> ExportExcel(
            string kodLaporan,
            string tarikhDari,
            string tarikhHingga,
            int status,
            int AkBankId,
            int susunan)
        {
            LPR001PrintModel reportModel = await PrepareData(kodLaporan, tarikhDari, tarikhHingga, status, AkBankId, susunan);

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            if (kodLaporan == "LPR00102")
            {
                var excelData = GetExcelDataLPR00102(reportModel);
                RunWorkBookLPR00102(reportModel, reportModel.CompanyDetail, excelData, handle);
            }
            else 
            {
                var excelData = GetExcelDataLPR00103(reportModel);
                var excelDataRingkasan = GetExcelDataLPR00103Ringkasan(reportModel);
                RunWorkBookLPR00103(reportModel, reportModel.CompanyDetail, excelData,excelDataRingkasan, handle);
            }

            // Note we are returning a filename as well as the handle
            return Json(new { FileGuid = handle, FileName = kodLaporan + ".xlsx" });

        }


        private async Task<LPR001PrintModel> PrepareData(string kodLaporan,
            string tarikhDari,
            string tarikhHingga,
            int status,
            int AkBankId,
            int susunan)
        {
            var pdfName = kodLaporan;
            JKW kW = _context.JKW.Where(x => x.Kod == "100").FirstOrDefault();

            AkBank akBank = new AkBank();
            if (AkBankId != 0)
            {
                akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);
            }
            else
            {
                akBank.NoAkaun = "SEMUA";
            }


            LPR001PrintModel reportModel = new LPR001PrintModel();

            if (kodLaporan == "LPR00102" || kodLaporan == "LPR00103")
            {
                if (kodLaporan == "LPR00102")
                {
                    reportModel.Tajuk = "Laporan Daftar Resit Terperinci Mengikut Pecahan Cara Bayar Kump Wang :";
                }
                else
                {
                    reportModel.Tajuk = "Laporan Daftar Resit Terperinci Mengikut Pecahan Kod Akaun Kump Wang :";
                }
                IEnumerable<AkTerima> akT = _context.AkTerima
                    .IgnoreQueryFilters()
                    .Include(b => b.JKW)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.JNegeri)
                    .Include(b => b.AkTerima1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkTerima2).ThenInclude(b => b.JCaraBayar)
                    //.Where(b=> b.JKWId == 1)
                    .ToList();

                // date condition
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);
                akT = akT.Where(x => x.Tarikh >= date1
                      && x.Tarikh <= date2)
                    .ToList();
                //date condition end

                //bank penerima condition
                if (AkBankId != 0)
                {
                    akT = akT.Where(c => c.AkBankId == AkBankId).ToList();
                }
                else
                {
                }
                //susunan condition end

                //status condition
                switch (status)
                {
                    // belum posting
                    case 1:
                        akT = akT.Where(x => x.FlPosting == 0).ToList();
                        break;
                    // sudah posting
                    case 2:
                        akT = akT.Where(x => x.FlPosting == 1).ToList();
                        break;
                    // batal

                    case 3:
                        akT = akT.Where(x => x.FlHapus == 1).ToList();
                        break;
                    // semua
                    default:
                        break;
                }
                //status condition end

                //susunan condition
                if (susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoRujukan).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoRujukan).ToList();
                }
                //susunan condition end

                decimal debit = 0;
                decimal kredit = 0;
                decimal amaunUrusniaga = 0;

                reportModel.AkTerima = akT;
                foreach (AkTerima item in reportModel.AkTerima)
                {
                    if (item.FlHapus == 1)
                    {
                        debit += 0;
                    }
                    else
                    {
                        debit += item.Jumlah;
                    }


                    foreach (AkTerima1 item1 in item.AkTerima1)
                    {
                        if (item.FlHapus == 1)
                        {
                            kredit += 0;
                        }
                        else
                        {
                            kredit += item1.Amaun;
                        }
                    }
                    foreach (AkTerima2 item2 in item.AkTerima2)
                    {
                        if (item.FlHapus == 1)
                        {
                            amaunUrusniaga += 0;
                        }
                        else
                        {
                            amaunUrusniaga += item2.Amaun;
                        }
                    }

                }
                reportModel.AmaunUrusniaga = amaunUrusniaga;
                reportModel.Kredit = kredit;
                reportModel.Debit = debit;

                //Ringkasan Cara bayar
                var RingkasanCaraBayar = (from tbl1 in _context.AkTerima2.Include(x => x.JCaraBayar).ToList()
                                          join tbl in akT.ToList()
                                          on tbl1.AkTerimaId equals tbl.Id into tbl1Tbl
                                          from tbl1_tbl in tbl1Tbl.DefaultIfEmpty()
                                          select new
                                          {
                                              CaraBayar = tbl1.JCaraBayar.Perihal
                                          }).GroupBy(x => x.CaraBayar).Select(group => new
                                          {
                                              Metric = group.Key,
                                              Count = group.Count()
                                          }).OrderBy(x => x.Metric).ToList();

                IEnumerable<RingkasanPrintModel> ringkasanCaraBayarResult = RingkasanCaraBayar.Select(l => new RingkasanPrintModel
                {
                    Perihal = l.Metric,
                    Kuantiti = l.Count.ToString(),
                }).ToList();

                //Ringkasan Cara bayar end
                //Ringkasan Debit group by kod Bank AkTerima
                var DebitLines = (from tbl in akT.ToList()
                                  select new
                                  {
                                      kodAkaun = tbl.AkBank.AkCarta.Kod,
                                      Perihal = tbl.AkBank.AkCarta.Perihal,
                                      Debit = tbl.Jumlah

                                  }).GroupBy(x => x.kodAkaun).ToList();

                IEnumerable<RingkasanPrintModel> debitResult = DebitLines.Select(l => new RingkasanPrintModel
                {
                    KodAkaun = l.First().kodAkaun,
                    Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                    Debit = l.Sum(c => c.Debit).ToString(),
                    Kredit = "0.00"
                }).ToList();

                //Ringkasan Debit group by kod Bank AkTerima end
                // Ringkasan kredit group by Kod Objek AkTerima1
                var kreditLines = (from tbl1 in _context.AkTerima1.Include(x => x.AkCarta).ToList()
                                   join tbl in akT.ToList()
                                   on tbl1.AkTerimaId equals tbl.Id into tbl1Tbl
                                   from tbl1_tbl in tbl1Tbl.DefaultIfEmpty()
                                   select new
                                   {
                                       kodAkaun = tbl1.AkCarta.Kod,
                                       Perihal = tbl1.AkCarta.Perihal,
                                       Kredit = tbl1.Amaun

                                   }).GroupBy(x => x.kodAkaun).ToList();

                IEnumerable<RingkasanPrintModel> kreditResult = kreditLines.Select(l => new RingkasanPrintModel
                {
                    KodAkaun = l.First().kodAkaun,
                    Perihal = l.Select(x => x.Perihal).FirstOrDefault(),
                    Debit = "0.00",
                    Kredit = l.Sum(c => c.Kredit).ToString()
                }).ToList();

                IEnumerable<RingkasanPrintModel> result = debitResult.Concat(kreditResult);

                reportModel.LPR00102_1 = ringkasanCaraBayarResult;
                reportModel.LPR00103_1 = result;

                // ringkasan kredit group by Kod Objek AkTerima1 end
            }
            else
            {

            }


            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser.Nama;

            reportModel.KodLaporan = kodLaporan;
            reportModel.TarikhDari = tarikhDari;
            reportModel.TarikhHingga = tarikhHingga;
            reportModel.JKW = kW;
            reportModel.AkBank = akBank;
            CompanyDetails company = new CompanyDetails();
            reportModel.CompanyDetail = company;
            return reportModel;
        }
        private void RunWorkBookLPR00102(LPR001PrintModel reportModel,CompanyDetails company, DataTable excelData, string handle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.AddWorksheet("Laporan Terimaan");

                ws.Cell("A1").Value = company.NamaSyarikat;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = reportModel.Tajuk +  reportModel.JKW.Kod + " - " + reportModel.JKW.Perihal;
                ws.Cell("A3").Value = "Bagi Tarikh : " + @Convert.ToDateTime(reportModel.TarikhDari).ToString("dd/MM/yyyy") + "->" + @Convert.ToDateTime(reportModel.TarikhHingga).ToString("dd/MM/yyyy");

                ws.ColumnWidth = 10;
                ws.Cell("A5").InsertTable(excelData)
                    .Theme = XLTableTheme.TableStyleMedium1;

                var rowNum = 1;
                foreach (DataRow row in excelData.Rows)
                {

                    if (!string.IsNullOrWhiteSpace(row[9].ToString()))
                    {
                        ws.Row(rowNum + 5).CellsUsed().Style.Fill.BackgroundColor = XLColor.PastelRed;
                        ws.Row(rowNum + 5).CellsUsed().Style.Font.FontColor = XLColor.White;
                    }
                    rowNum++;
                }
                ws.Column(2)
                    .Style.DateFormat.Format = "dd/MM/yyyy hh:mm:ss";
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                ws.Column(4).AdjustToContents();
                ws.Column(5).AdjustToContents();
                ws.Column(6).AdjustToContents();
                ws.Column(7).AdjustToContents();
                ws.Column(8).AdjustToContents();
                ws.Column(9)
                    .Style.NumberFormat.Format = " #,##0.00";
                ws.Column(9).AdjustToContents();
                ws.Column(10).AdjustToContents();


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

        private string RunWorkBookLPR00103(LPR001PrintModel reportModel, CompanyDetails company, DataTable excelData, DataTable excelDataRingkasan, string handle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                
                var ws = wb.AddWorksheet("Laporan Terimaan");

                ws.Cell("A1").Value = company.NamaSyarikat;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = reportModel.Tajuk +  reportModel.JKW.Kod + " - " + reportModel.JKW.Perihal;
                ws.Cell("A3").Value = "Bagi Tarikh : " + @Convert.ToDateTime(reportModel.TarikhDari).ToString("dd/MM/yyyy") + "->" + @Convert.ToDateTime(reportModel.TarikhHingga).ToString("dd/MM/yyyy");

                ws.ColumnWidth = 9;
                ws.Cell("A5").InsertTable(excelData)
                    .Theme = XLTableTheme.TableStyleMedium1;

                var rowNum = 1;
                foreach (DataRow row in excelData.Rows)
                {
                    
                    if (!string.IsNullOrWhiteSpace(row[8].ToString()))
                    {
                                ws.Row(rowNum + 5).CellsUsed().Style.Fill.BackgroundColor = XLColor.PastelRed;
                        ws.Row(rowNum + 5).CellsUsed().Style.Font.FontColor = XLColor.White;
                    }
                    rowNum++;
                }
                ws.Column(2)
                    .Style.DateFormat.Format = "dd/MM/yyyy hh:mm:ss";
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                ws.Column(4).AdjustToContents();
                ws.Column(5).AdjustToContents();
                ws.Column(6).AdjustToContents();
                ws.Column(7)
                    .Style.NumberFormat.Format = " #,##0.00";
                ws.Column(7).AdjustToContents();
                ws.Column(8)
                    .Style.NumberFormat.Format = " #,##0.00";
                ws.Column(8).AdjustToContents();
                ws.Column(9).AdjustToContents();

                // worksheet ringkasan
                var ws2 = wb.AddWorksheet("Ringkasan");

                ws2.Cell("A1").Value = company.NamaSyarikat;
                ws2.Cell("A1").Style.Font.Bold = true;
                ws2.Cell("A2").Value = reportModel.Tajuk +  reportModel.JKW.Kod + " - " + reportModel.JKW.Perihal;
                ws2.Cell("A3").Value = "Bagi Tarikh : " + @Convert.ToDateTime(reportModel.TarikhDari).ToString("dd/MM/yyyy") + "->" + @Convert.ToDateTime(reportModel.TarikhHingga).ToString("dd/MM/yyyy");
                ws2.Cell("A4").Value = "Ringkasan : ";

                ws2.ColumnWidth = 4;
                ws2.Cell("A6").InsertTable(excelDataRingkasan)
                    .Theme = XLTableTheme.TableStyleMedium1;

                ws2.Column(2).AdjustToContents();
                ws2.Column(3)
                    .Style.NumberFormat.Format = " #,##0.00";
                ws2.Column(3).AdjustToContents();
                ws2.Column(4)
                    .Style.NumberFormat.Format = " #,##0.00";
                ws2.Column(4).AdjustToContents();

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    //return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportModel.KodLaporan + ".xlsx");

                    //This is an equivalent to tempdata, but requires manual cleanup
                    _cache.Set(handle, ms.ToArray(),
                                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    //(I'd recommend you revise the expiration specifics to suit your application)

                }
                return handle;
            }
        }
        private DataTable GetExcelDataLPR00102(LPR001PrintModel reportModel)
        {
            DataTable dt = new DataTable();
            dt.TableName =  "Laporan Terimaan";
            dt.Columns.Add("Bil", typeof(int));
            dt.Columns.Add("Tarikh", typeof(DateTime));
            dt.Columns.Add("No Resit", typeof(string));
            dt.Columns.Add("Pembayar", typeof(string));
            dt.Columns.Add("Cara Bayar", typeof(string));
            dt.Columns.Add("No Cek/Dok.", typeof(string));
            dt.Columns.Add("No Slip", typeof(string));
            dt.Columns.Add("Tar Slip", typeof(string));
            dt.Columns.Add("Amaun RM", typeof(decimal));
            dt.Columns.Add("Sebab Hapus", typeof(string));

            if (reportModel.AkTerima != null)
            {

                var bil = 1;
                foreach (var item in reportModel.AkTerima)
                {
                    
                    if (item.AkTerima2 != null)
                    {
                        
                        foreach (var item2 in item.AkTerima2)
                        {
                            dt.Rows.Add(bil,
                                               item.Tarikh,
                                               item.NoRujukan.Substring(3),
                                               item.Nama?.ToUpper() ?? "",
                                               item2.JCaraBayar.Perihal,
                                               item2.NoCek,
                                               item2.NoSlip,
                                               item2.TarSlip?.ToString("dd/MM/yyyy") ?? "-",
                                               item2.Amaun,
                                               item.SebabHapus?.ToUpper() ?? "");

                        }
                    }
                    bil++;
                }
            }

            return dt;

        }

        private DataTable GetExcelDataLPR00103(LPR001PrintModel reportModel)
        {
            DataTable dt = new DataTable();
            dt.TableName =  "Laporan Terimaan";
            dt.Columns.Add("Bil", typeof(int));
            dt.Columns.Add("Tarikh", typeof(DateTime));
            dt.Columns.Add("No Resit", typeof(string));
            dt.Columns.Add("Pembayar", typeof(string));
            dt.Columns.Add("Kod Akaun Debit", typeof(string));
            dt.Columns.Add("Kod Akaun Kredit", typeof(string));
            dt.Columns.Add("Debit RM", typeof(decimal));
            dt.Columns.Add("Kredit RM", typeof(decimal));
            dt.Columns.Add("Sebab Hapus", typeof(string));

            if (reportModel.AkTerima != null)
            {

                var bil = 1;
                foreach (var item in reportModel.AkTerima)
                {

                    if (item.AkTerima1 != null)
                    {

                        foreach (var item1 in item.AkTerima1)
                        {
                            dt.Rows.Add(bil,
                                               item.Tarikh,
                                               item.NoRujukan.Substring(3),
                                               item.Nama?.ToUpper() ?? "",
                                               item.AkBank?.AkCarta?.Kod + " - " + item.AkBank?.AkCarta?.Perihal,
                                               item1.AkCarta?.Kod + " - " +  item1.AkCarta?.Perihal,
                                               item.Jumlah,
                                               item1.Amaun,
                                               item.SebabHapus?.ToUpper() ?? "");

                        }
                    }
                    bil++;
                }
            }

            return dt;

        }

        private DataTable GetExcelDataLPR00103Ringkasan(LPR001PrintModel reportModel)
        {
            DataTable dt = new DataTable();
            dt.TableName =  "Ringkasan";
            dt.Columns.Add("Kod", typeof(string));
            dt.Columns.Add("Nama Objek", typeof(string));
            dt.Columns.Add("Debit RM", typeof(decimal));
            dt.Columns.Add("Kredit RM", typeof(decimal));

            if (reportModel.LPR00103_1 != null)
            {

                foreach (var item in reportModel.LPR00103_1)
                {
                    dt.Rows.Add(item.KodAkaun,
                                item.Perihal,
                                item.Debit,
                                item.Kredit);
                }
            }

            return dt;

        }
    }

}
