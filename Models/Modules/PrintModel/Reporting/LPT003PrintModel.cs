using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPT003PrintModel : SpPendahuluanPelbagai
    {
        public string Username { get; set; }
        public IEnumerable<SpPendahuluanPelbagai> SpPendahuluanPelbagai { get; set; }
        public string KodLaporan { get; set; }
        public string ParamBulan { get; set; }
        public string ParamTahun { get; set; }
        public string ParamTajuk { get; set; }
        public string ParamKodKw { get; set; }
        public string ParamPerihalKw { get; set; }
        public decimal FormJumlah { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
