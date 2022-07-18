using MSNK.Models.Administration;

namespace MSNK.Models.Modules.PrintModel
{
    public class IndenPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public string Jawatan { get; set; }
        public JNegeri Negeri { get; set; }
        public AkInden AkInden { get; set; }
        public AkInden1 AkInden1 { get; set; }
        public AkInden2 AkInden2 { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
