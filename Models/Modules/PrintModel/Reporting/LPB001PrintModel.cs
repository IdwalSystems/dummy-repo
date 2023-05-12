using MSNK.Models.Administration;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPB001PrintModel : AbBukuVot
    {
        public string Username { get; set; }
        public IEnumerable<AbBukuVot> AbBukuVot { get; set; }
        public string KodLaporan { get; set; }
        public string ParamTajuk { get; set; }
        public string ParamCarta { get; set; }
        public string ParamKW { get; set; }
        public string ParamBahagian { get; set; }
        public string ParamTarikh { get; set; }
        public CompanyDetails CompanyDetails { get; set; }
    }
}
