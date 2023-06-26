using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanBaucerController : Controller
    {
        public const string modul = "LPV001";

        private readonly ApplicationDbContext _context;
        private readonly AppLogIRepository<AppLog, int> _appLog;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<AkPV, int, string> _akPVRepo;
        private readonly IRepository<AkBank, int, string> _akBankRepo;
        private readonly IRepository<JKW, int, string> _kwRepo;
        private readonly IRepository<JNegeri, int, string> _negeriRepo;
        private readonly ListViewIRepository<AkPV1, int> _akPV1Repo;
        private readonly IRepository<AkCarta, int, string> _akCartaRepo;
        private readonly ListViewIRepository<AkPV2, int> _akPV2Repo;
        private readonly IRepository<AkAkaun, int, string> _akAkaunRepo;
        private readonly IMemoryCache _cache;

        public LaporanBaucerController(
            ApplicationDbContext context,
            AppLogIRepository<AppLog, int> appLog,
            UserManager<IdentityUser> userManager,
            IRepository<AkPV, int, string> akPVRepository,
            ListViewIRepository<AkPV1, int> akPV1Repository,
            ListViewIRepository<AkPV2, int> akPV2Repository,
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
            _akPVRepo = akPVRepository;
            _akPV1Repo = akPV1Repository;
            _akPV2Repo = akPV2Repository;
            _akCartaRepo = akCartaRepository;
            _akAkaunRepo = akAkaunRepository;
            _cache = cache;

        }
        public IActionResult Index()
        {
            ViewBag.AkBank = _context.AkBank.Include(b => b.AkCarta).ToList();
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
            LPV001PrintModel reportModel = await PrepareData(kodLaporan, tarikhDari, tarikhHingga, status, AkBankId, susunan);

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
            LPV001PrintModel reportModel = await PrepareData(kodLaporan, tarikhDari, tarikhHingga, status, AkBankId, susunan);

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            if (kodLaporan == "LPV00101")
            {
                var excelData = GetExcelDataLPV00101(reportModel);
                RunWorkBookLPV00101(reportModel, reportModel.CompanyDetail, excelData, handle);
            }
            else
            {
            }

            // Note we are returning a filename as well as the handle
            return Json(new { FileGuid = handle, FileName = kodLaporan + ".xlsx" });

        }

        private DataTable GetExcelDataLPV00101(LPV001PrintModel reportModel)
        {
            DataTable dt = new DataTable();
            dt.TableName =  "Laporan Terimaan";
            dt.Columns.Add("Bil", typeof(int));
            dt.Columns.Add("Tarikh", typeof(DateTime));
            dt.Columns.Add("No Baucer", typeof(string));
            dt.Columns.Add("Penerima", typeof(string));
            dt.Columns.Add("Kod Akaun", typeof(string));
            dt.Columns.Add("Perihalan Akaun", typeof(string));
            dt.Columns.Add("Cara Bayar", typeof(string));
            dt.Columns.Add("No Cek", typeof(string));
            dt.Columns.Add("Tarikh Cek", typeof(string));
            dt.Columns.Add("Amaun RM", typeof(decimal));
            dt.Columns.Add("Sebab Hapus", typeof(string));

            if (reportModel.AkPV != null)
            {

                var bil = 1;
                foreach (var item in reportModel.AkPV)
                {
                    if (item.AkPV1 != null)
                    {
                        foreach (var item1 in item.AkPV1)
                        {
                            dt.Rows.Add(bil,
                                        item.Tarikh,
                                        item.NoPV.Substring(3),
                                        item.Nama?.ToUpper() ?? "",
                                        item1.AkCarta?.Kod,
                                        item1.AkCarta?.Perihal,
                                        item.JCaraBayar?.Perihal,
                                        item.NoCekAtauEFT ?? "-",
                                        item.TarCekAtauEFT?.ToString("dd/MM/yyyy") ?? "",
                                        item1.Amaun,
                                        item.SebabHapus?.ToUpper() ?? "");
                        }
                    }
                    bil++;
                }
            }
            return dt;
        }

        private void RunWorkBookLPV00101(LPV001PrintModel reportModel, CompanyDetails company, DataTable excelData, string handle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.AddWorksheet("Laporan Baucer");

                ws.Cell("A1").Value = company.NamaSyarikat;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = reportModel.Tajuk +  reportModel.JKW.Kod + " - " + reportModel.JKW.Perihal;
                ws.Cell("A3").Value = "Bagi Tarikh : " + @Convert.ToDateTime(reportModel.TarikhDari).ToString("dd/MM/yyyy") + "->" + @Convert.ToDateTime(reportModel.TarikhHingga).ToString("dd/MM/yyyy");

                ws.ColumnWidth = 11;
                ws.Cell("A5").InsertTable(excelData)
                    .Theme = XLTableTheme.TableStyleMedium1;

                var rowNum = 1;
                foreach (DataRow row in excelData.Rows)
                {

                    if (!string.IsNullOrWhiteSpace(row[10].ToString()))
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
                ws.Column(9).AdjustToContents();
                ws.Column(10)
                   .Style.NumberFormat.Format = " #,##0.00";
                ws.Column(10).AdjustToContents();
                ws.Column(11).AdjustToContents();


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

        private async Task<LPV001PrintModel> PrepareData(
            string kodLaporan,
            string tarikhDari,
            string tarikhHingga,
            int status,
            int AkBankId,
            int susunan)
        {
            var pdfName = kodLaporan;
            JKW kW = _context.JKW.Where(x => x.Kod == "100").FirstOrDefault();
            AkBank akBank = _context.AkBank.FirstOrDefault(b => b.Id == AkBankId);

            LPV001PrintModel reportModel = new LPV001PrintModel();

            if (kodLaporan == "LPV00101")
            {

                reportModel.Tajuk = "Laporan Daftar Baucer Kump Wang :";

                IEnumerable<AkPV> akT = _context.AkPV
                    .IgnoreQueryFilters()
                    .Include(b => b.JKW)
                    .Include(b => b.JCaraBayar)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.SuPekerja)
                    .Include(b => b.AkPV1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2).ThenInclude(b => b.AkBelian).ThenInclude(b => b.AkPO)
                    .ToList();

                // date condition
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);
                akT = akT.Where(x => x.Tarikh >= date1
                      && x.Tarikh <= date2)
                    .ToList();
                //date condition end

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
                    case 3:
                        akT = akT.Where(x => x.FlHapus == 1).ToList();
                        break;
                    // semua
                    default:
                        break;
                }
                //status condition end

                // akaun bank
                akT = akT.Where(x => x.AkBankId == AkBankId).ToList();
                // akaun bank end

                //susunan condition
                if (susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoPV).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoPV).ToList();
                }
                //susunan condition end

                decimal jumlahDebit = 0;

                reportModel.AkPV = akT;

                foreach (AkPV item in reportModel.AkPV)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlahDebit += 0;
                    }
                    else
                    {
                        jumlahDebit += item.Jumlah;
                    }

                }
                reportModel.JumlahDebit = jumlahDebit;
            }
            else if (kodLaporan == "LPV00102")
            {
                reportModel.Tajuk = "Laporan Daftar Baucer Kump Wang :";

                IEnumerable<AkPV> akT = _context.AkPV
                    .Include(b => b.JKW)
                    .Include(b => b.JCaraBayar)
                    .Include(b => b.AkBank).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.SuPekerja)
                    .Include(b => b.AkPV1).ThenInclude(b => b.AkCarta)
                    .Include(b => b.AkPV2).ThenInclude(b => b.AkBelian).ThenInclude(b => b.AkPO).ThenInclude(b => b.AkNotaMinta)
                    .Where(b => b.denganTanggungan == true)
                    .ToList();

                // date condition
                DateTime date1 = DateTime.Parse(tarikhDari);
                DateTime date2 = DateTime.Parse(tarikhHingga).AddHours(23.99);
                akT = akT.Where(x => x.Tarikh >= date1
                      && x.Tarikh <= date2)
                    .ToList();
                //date condition end

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
                    // semua
                    default:
                        break;
                }
                //status condition end

                // akaun bank
                akT = akT.Where(x => x.AkBankId == AkBankId).ToList();
                // akaun bank end

                //susunan condition
                if (susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoPV).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoPV).ToList();
                }
                //susunan condition end

                decimal jumlahDebit = 0;

                reportModel.AkPV = akT;

                foreach (AkPV item in reportModel.AkPV)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlahDebit += 0;
                    }
                    else
                    {
                        jumlahDebit += item.Jumlah;
                    }

                }
                reportModel.JumlahDebit = jumlahDebit;
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


    }
}

