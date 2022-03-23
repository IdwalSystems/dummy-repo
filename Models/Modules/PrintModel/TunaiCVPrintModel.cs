using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class TunaiCVPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public JNegeri Negeri { get; set; }
        public AkTunaiCV AkTunaiCV { get; set; }
        public AkTunaiCV1 AkTunaiCV1 { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
