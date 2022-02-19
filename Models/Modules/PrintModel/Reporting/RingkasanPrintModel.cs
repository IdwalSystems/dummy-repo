using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class RingkasanPrintModel
    {
        public RingkasanPrintModel() { }

        public string KodAkaun { get; set; }
        public string Perihal { get; set; }
        public string Debit { get; set; }
        public string Kredit { get; set; }
        public string Kuantiti { get; set; }
    }
}
