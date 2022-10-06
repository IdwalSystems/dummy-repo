using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class ResitPrintModel: AkTerima
    {
        public string JumlahDalamPerkataan { get; set; }
        public string username { get; set; }
        public string penyedia { get; set; }
        public JNegeri Negeri { get; set; }
        public AkTerima AkTerima { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }

}
