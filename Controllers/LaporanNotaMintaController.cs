using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.FormModel;
using MSNK.Models.Modules.PrintModel.Reporting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Controllers
{
    [Authorize(Policy = "LP001")]
    public class LaporanNotaMintaController : Controller
    {
        public const string modul = "LPN001";

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _cache;

        public LaporanNotaMintaController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IMemoryCache cache
            )
        {
            _context = context;
            _userManager = userManager;
            _cache = cache;

        }
        public IActionResult Index()
        {
            PopulateList();
            return View();
        }

        private void PopulateList()
        {
            List<JKW> kwList = _context.JKW.OrderBy(b => b.Kod).ToList();
            ViewBag.JKw = kwList;

            List<JBahagian> bahagianList = _context.JBahagian.ToList();
            ViewBag.JBahagian = bahagianList;

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Print(string kodLaporan, ReportFormModel param)
        {
            LPN001PrintModel reportModel = await PrepareData(kodLaporan, param);

            string customSwitches = string.Format(" --header-html  \"{0}\" " +
                                   "--header-spacing \"-12\" " +
                                   "--header-font-size \"10\" " +
                                   "--footer-center \"[page]/[toPage]\" " +
                                   "--footer-font-size \"7\" --footer-spacing 1",
                                   Url.Action("Header", "LaporanNotaMinta",
                                   new
                                   {
                                       KodLaporan = reportModel.KodLaporan,
                                       ParamKodKw = reportModel.JKW.Kod,
                                       ParamPerihalKw = reportModel.JKW.Perihal,
                                       ParamBulan = reportModel.ParamBulan,
                                       ParamTahun = reportModel.ParamTahun,
                                       ParamTajuk = reportModel.Tajuk
                                   },
                                   "https"));
            return new ViewAsPdf(reportModel.KodLaporan, reportModel)
            {
                PageMargins = { Left = 10, Bottom = 15, Right = 15, Top = 15 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = customSwitches,
                //CustomSwitches = "--footer-center \"[page]/[toPage]\"" +
                //        " --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        [HttpPost]
        public async Task<JsonResult> ExportExcel(
                string kodLaporan,
                ReportFormModel param)
        {
            LPN001PrintModel reportModel = await PrepareData(kodLaporan, param);

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            var excelData = GetExcelDataLPN00101(reportModel);
            RunWorkBookLPN00101(reportModel, reportModel.CompanyDetail, excelData, handle);


            // Note we are returning a filename as well as the handle
            return Json(new { FileGuid = handle, FileName = kodLaporan + ".xlsx" });

        }
        private DataTable GetExcelDataLPN00101(LPN001PrintModel reportModel)
        {
            DataTable dt = new DataTable();
            dt.TableName =  "Laporan Daftar Bil";
            dt.Columns.Add("Bil", typeof(int));
            dt.Columns.Add("Tarikh", typeof(DateTime));
            dt.Columns.Add("Tarikh Sah", typeof(string));
            dt.Columns.Add("Tarikh Terima Kewangan", typeof(string));
            dt.Columns.Add("Nama Pembekal", typeof(string));
            dt.Columns.Add("No Siri", typeof(string));
            dt.Columns.Add("Amaun RM", typeof(decimal));
            dt.Columns.Add("No PO/Inden", typeof(string));
            dt.Columns.Add("Tarikh Baucer", typeof(string));
            dt.Columns.Add("No Baucer", typeof(string));
            dt.Columns.Add("Tarikh Sah Baucer", typeof(string));
            dt.Columns.Add("Tarikh Cek", typeof(string));
            dt.Columns.Add("No Cek", typeof(string));
            dt.Columns.Add("Di Kewangan", typeof(string));

            if (reportModel.AkNotaMinta != null)
            {

                var bil = 1;
                foreach (var item in reportModel.AkNotaMinta)
                {
                    DataRow workRow = dt.NewRow();
                    workRow[0] = bil;
                    workRow[1] = item.Tarikh;
                    workRow[2] = item.TarikhPosting?.ToString("dd/MM/yyyy") ?? "-";
                    workRow[3] = item.TarikhSeksyenKewangan?.ToString("dd/MM/yyyy") ?? "-";
                    workRow[4] = item.AkPembekal?.NamaSykt?.ToUpper() ?? "";
                    workRow[5] = item.NoSiri ?? "";
                    workRow[6] = item.Jumlah;

                    if (item.AkPO.Count > 0)
                    {
                        foreach (var itemPO in item.AkPO)
                        {
                            if (itemPO.FlHapus == 0)
                            {
                                workRow[7] = itemPO.NoPO ?? "-";
                            }
                            else
                            {
                                workRow[7] = "-";
                            }

                            if (itemPO.AkBelian.Count > 0)
                            {
                                foreach (var itemBelian in itemPO.AkBelian)
                                {
                                    if (itemBelian.AkPV2.Count > 0)
                                    {
                                        foreach (var itemPV2 in itemBelian.AkPV2)
                                        {
                                            if (itemPV2.AkPV != null && itemPV2.AkPV.FlHapus == 0)
                                            {
                                                workRow[8] = itemPV2.AkPV.Tarikh.ToString("dd/MM/yyyy");
                                                workRow[9] = itemPV2.AkPV.NoPV;
                                                workRow[10] = itemPV2.AkPV.TarikhPosting?.ToString("dd/MM/yyyy");
                                                workRow[11] = itemPV2.AkPV.TarCekAtauEFT?.ToString("dd/MM/yyyy");
                                                workRow[12] = itemPV2.AkPV.JCaraBayar?.Perihal ?? "-";
                                            }
                                        }
                                    }
                                }
                            }
                            if (itemPO.IsInKewangan)
                            {
                                workRow[13] = "1";
                            }
                            else
                            {
                                workRow[13] = "0";
                            }
                        }
                    }
                    else if (item.AkInden.Count > 0)
                    {
                        foreach (var itemInden in item.AkInden)
                        {
                            if (itemInden.FlHapus == 0)
                            {
                                workRow[7] = itemInden.NoInden ?? "-";
                            }
                            else
                            {
                                workRow[7] = "-";
                            }

                            if (itemInden.AkBelian.Count > 0)
                            {
                                foreach (var itemBelian in itemInden.AkBelian)
                                {
                                    if (itemBelian.AkPV2.Count > 0)
                                    {
                                        foreach (var itemPV2 in itemBelian.AkPV2)
                                        {
                                            if (itemPV2.AkPV != null && itemPV2.AkPV.FlHapus == 0)
                                            {
                                                workRow[8] = itemPV2.AkPV.Tarikh.ToString("dd/MM/yyyy");
                                                workRow[9] = itemPV2.AkPV.NoPV;
                                                workRow[10] = itemPV2.AkPV.TarikhPosting?.ToString("dd/MM/yyyy");
                                                workRow[11] = itemPV2.AkPV.TarCekAtauEFT?.ToString("dd/MM/yyyy");
                                                workRow[12] = itemPV2.AkPV.JCaraBayar?.Perihal ?? "-";
                                            }
                                        }
                                    }
                                }
                            }
                            if (itemInden.IsInKewangan)
                            {
                                workRow[13] = "1";
                            }
                            else
                            {
                                workRow[13] = "0";
                            }
                        }
                    }

                    dt.Rows.Add(workRow);
                }

            }
            return dt;
        }

        private void RunWorkBookLPN00101(LPN001PrintModel reportModel, CompanyDetails company, DataTable excelData, string handle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.AddWorksheet();

                ws.Cell("A1").Value = company.NamaSyarikat;
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A2").Value = reportModel.ParamTajuk + " " + reportModel.ParamKodKw + " - " + reportModel.ParamPerihalKw;
                ws.Cell("A3").Value = "Bagi Bulan : " + reportModel.ParamBulan + " / " +  reportModel.ParamTahun;

                ws.ColumnWidth = 14;
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
                ws.Column(7)
                   .Style.NumberFormat.Format = " #,##0.00";
                ws.Column(7).AdjustToContents();
                ws.Column(8).AdjustToContents();
                ws.Column(9).AdjustToContents();
                ws.Column(10).AdjustToContents();
                ws.Column(11).AdjustToContents();
                ws.Column(12).AdjustToContents();
                ws.Column(13).AdjustToContents();
                ws.Column(14).AdjustToContents();


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

        private async Task<LPN001PrintModel> PrepareData(
            string kodLaporan,
            ReportFormModel param)
        {
            var pdfName = param.kodLaporan;
            if (param.JKWId != null)
            {
                JKW kW = _context.JKW.Where(x => x.Id == param.JKWId).FirstOrDefault();
                param.JKW = kW;
            }

            LPN001PrintModel reportModel = new LPN001PrintModel();

            if (kodLaporan == "LPN00101")
            {
                reportModel.ParamTajuk = "Laporan Daftar Bil / Nota Minta Kump Wang :";

                IEnumerable<AkNotaMinta> akT = _context.AkNotaMinta
                    .IgnoreQueryFilters()
                    .Include(b => b.JKW)
                    .Include(b => b.JBahagian)
                    .Include(b => b.AkPembekal)
                    .Include(b => b.AkNotaMinta1)
                    .Include(b => b.AkNotaMinta2)
                    .Include(b => b.AkPO).ThenInclude(b => b.AkBelian).ThenInclude(b => b.AkPV2).ThenInclude(b => b.AkPV).ThenInclude(b => b.JCaraBayar)
                    .Include(b => b.AkInden).ThenInclude(b => b.AkBelian).ThenInclude(b => b.AkPV2).ThenInclude(b => b.AkPV).ThenInclude(b => b.JCaraBayar)
                    .ToList();

                // bulan & tahun condition
                akT = akT.Where(x => x.Tarikh.Month == param.bulanTahun.Month
                    && x.Tarikh.Year == param.bulanTahun.Year)
                    .ToList();
                //bulan & tahun condition end

                //status condition
                switch (param.status)
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

                //susunan condition
                if (param.susunan == 1)
                {
                    akT = akT.OrderBy(x => x.NoSiri).ToList();
                }
                else
                {
                    akT = akT.OrderBy(x => x.Tarikh).ThenBy(x => x.NoSiri).ToList();
                }
                //susunan condition end

                decimal jumlah = 0;

                reportModel.AkNotaMinta = akT;

                foreach (AkNotaMinta item in reportModel.AkNotaMinta)
                {
                    if (item.FlHapus == 1)
                    {
                        jumlah += 0;
                    }
                    else
                    {
                        jumlah += item.Jumlah;
                    }

                }
                reportModel.FormJumlah = jumlah;
            }

            var user = await _userManager.GetUserAsync(User);
            var namaUser = await _context.applicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);

            reportModel.Username = namaUser.Nama;

            reportModel.KodLaporan = param.kodLaporan;
            reportModel.ParamBulan = param.bulanTahun.ToString("MM");
            reportModel.ParamTahun = param.bulanTahun.ToString("yyyy");
            reportModel.JKW = param.JKW;
            CompanyDetails company = new CompanyDetails();
            reportModel.CompanyDetail = company;

            return reportModel;
        }

        [AllowAnonymous]
        public ActionResult Header(LPN001PrintModel reportModel)
        {
            return View(reportModel);
        }
    }
}
