using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPR001PrintModel :AkTerima
    {
        public string Username { get; set; } = String.Empty;
        public IEnumerable<AkTerima> AkTerima { get; set; }
        public string KodLaporan { get; set; } = String.Empty;
        public string RingkasanKodObjek { get; set; } = String.Empty;
        public string RingkasanNamaObjek { get; set; } = String.Empty;
        public string TarikhDari { get; set; } = String.Empty;
        public string TarikhHingga { get; set; } = String.Empty;
        public string Tajuk { get; set; } = String.Empty;
        public string KodKw { get; set; } = String.Empty;
        public string PerihalKw { get; set; } = String.Empty;
        public string AkaunBank { get; set; } = String.Empty;
        public string PerihalAkaunBank { get; set; } = String.Empty;
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public decimal AmaunUrusniaga { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
        public IEnumerable<RingkasanPrintModel> LPR00102_1 { get; set; }
        public IEnumerable<RingkasanPrintModel> LPR00103_1 { get; set; }

    }
}
