using MSNK.Models.Administration;

namespace MSNK.Models.Modules.PrintModel
{
    public class InvoisPrintModel
    {
        public string JumlahDalamPerkataan { get; set; }
        public decimal JumlahPerihal { get; set; }
        public string TarikhKewangan { get; set; }
        public string username { get; set; }
        public AkInvois akInvois { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
