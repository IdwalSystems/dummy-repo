using MSNK.Models.Administration;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel
{
    public class SuProfilAtletPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public SuProfil SuProfil { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
