using MSNK.Models.Administration;

namespace MSNK.Models.Modules.PrintModel
{
    public class IndenLarasPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public string Username { get; set; }
        public AkIndenLaras AkIndenLaras { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
