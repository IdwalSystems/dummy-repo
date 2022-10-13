using MSNK.Models.Administration;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel
{
    public class AppLogPrintModel
    {
        public List<ApplicationUser> AppUser { get; set; }
        public List<AppLog> AppLog { get; set; }
        public CompanyDetails CompanyDetail { get; set; }
    }
}
