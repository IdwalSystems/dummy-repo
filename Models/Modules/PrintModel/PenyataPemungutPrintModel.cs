using MSNK.Models.Administration;

namespace MSNK.Models.Modules.PrintModel
{
    public class PenyataPemungutPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public AkPenyataPemungut AkPenyataPemungut { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
