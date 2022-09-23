using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class WaranPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public string TandatanganSedia { get; set; }
        public AbWaran AbWaran { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
