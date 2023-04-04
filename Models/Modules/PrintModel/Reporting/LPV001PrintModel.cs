using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPV001PrintModel :AkPV
    {
        public string Username { get; set; } = String.Empty;
        public IEnumerable<AkPV> AkPV { get; set; }
        public string KodLaporan { get; set; } = String.Empty;
        public string TarikhDari { get; set; } = String.Empty;
        public string TarikhHingga { get; set; } = String.Empty;
        public string Tajuk { get; set; } = String.Empty;
        public string KodKw { get; set; } = String.Empty;
        public string AkaunBank { get; set; } = String.Empty;
        public string PerihalAkaunBank { get; set; } = String.Empty;
        public string PerihalKw { get; set; } = String.Empty;
        public decimal JumlahDebit { get; set; }
        public CompanyDetails CompanyDetail { get; set; }

    }
}
