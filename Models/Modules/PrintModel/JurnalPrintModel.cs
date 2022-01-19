using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class JurnalPrintModel
    {
        public string Username { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public AkJurnal AkJurnal { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
        public string JumlahDebitDalamPerkataan { get; set; }
        public string JumlahKreditDalamPerkataan { get; set; }

    }
}
