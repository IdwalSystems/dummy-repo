using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPR0012PrintModel : AkTerima
    {
        public IEnumerable<AkTerima> AkTerima { get; set; }
        public string KodLaporan { get; set; }
        public string RingkasanKodObjek { get; set; }
        public string RingkasanNamaObjek { get; set; }
        public string TarikhDari { get; set; }
        public string TarikhHingga { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
