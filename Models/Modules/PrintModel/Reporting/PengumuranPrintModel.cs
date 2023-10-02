using MSNK.Models.Administration;
using MSNK.Models.Operations;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class PengumuranPrintModel
    {
        public List<Pengumuran> Pengumuran { get; set; }
        public string Tajuk1 { get; set; }
        public string Tajuk2 { get; set; }
        public string KodLaporan { get; set; }
        public string Username { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
