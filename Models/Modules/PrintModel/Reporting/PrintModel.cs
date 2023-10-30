using MSNK.Models.Administration;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class PrintModel
    {
        public string Tajuk1 { get; set; }
        public string Tajuk2 { get; set; }
        public string KodLaporan { get; set; }
        public string Username { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
