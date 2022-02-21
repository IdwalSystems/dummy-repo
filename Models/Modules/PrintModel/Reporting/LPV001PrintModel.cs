using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPV001PrintModel :AkPV
    {
        public string Username { get; set; }
        public IEnumerable<AkPV> AkPV { get; set; }
        public string KodLaporan { get; set; }
        public string TarikhDari { get; set; }
        public string TarikhHingga { get; set; }
        public string Tajuk { get; set; }
        public string KodKw { get; set; }
        public string PerihalKw { get; set; }
        public decimal JumlahDebit { get; set; }
        public CompanyDetails CompanyDetail { get; set; }

    }
}
