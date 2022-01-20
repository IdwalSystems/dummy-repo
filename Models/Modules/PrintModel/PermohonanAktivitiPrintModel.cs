using MSNK.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class PermohonanAktivitiPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public JNegeri Negeri { get; set; }
        public SpPermohonanAktiviti SpPermohonanAktiviti { get; set; }
        public SpPermohonanAktiviti1 SpPermohonanAktiviti1 { get; set; }
        public SpPermohonanAktiviti2 SpPermohonanAktiviti2 { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }

}
