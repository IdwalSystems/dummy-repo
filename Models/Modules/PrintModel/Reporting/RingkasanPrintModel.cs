using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class RingkasanPrintModel
    {
        public RingkasanPrintModel() { }

        public string Bahagian { get; set; } = String.Empty;
        public string KodAkaun { get; set; } = String.Empty;
        public string Perihal { get; set; } = String.Empty;
        public string Debit { get; set; } = String.Empty;
        public string Kredit { get; set; } = String.Empty;
        public decimal DebitDecimal { get; set; }
        public decimal KreditDecimal { get; set; }
        public string Kuantiti { get; set; } = String.Empty;
    }
}
