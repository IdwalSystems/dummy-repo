using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class POPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public string Jawatan { get; set; }
        public JNegeri Negeri { get; set; }
        public AkPO AkPO { get; set; }
        public AkPO1 AkPO1 { get; set; }
        public AkPO2 AkPO2 { get; set; }
        public AkPembekal AkPembekal{ get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }

}
