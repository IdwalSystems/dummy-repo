using MSNK.Models.Administration;
using System.Collections;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPL001PrintModel : AkAkaun
    {
        public string Username { get; set; }
        public IEnumerable<AkAkaun> AkAkaun { get; set; }
        public string KodLaporan { get; set; }
        public string ParamTajuk { get; set; }
        public string ParamCarta { get; set; }
        public string ParamKW { get; set; }
        public string ParamTarikh { get; set; }
        public CompanyDetails CompanyDetails { get; set; }

    }
}
