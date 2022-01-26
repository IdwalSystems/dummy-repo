using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Administration
{
    public class CompanyDetails
    {
        public string NamaSyarikat { get; set; }
        public string AlamatSyarikat1 { get; set; }
        public string AlamatSyarikat2 { get; set; }
        public string AlamatSyarikat3 { get; set; }
        public string TelSyarikat { get; set; }
        public string FaksSyarikat { get; set; }
        public string EmelSyarikat { get; set; }

        public CompanyDetails()
        {
            NamaSyarikat = "Majlis Sukan Negeri Kedah";
            AlamatSyarikat1 = "Kompleks Sukan Muadzam Shah";
            AlamatSyarikat2 = "Lebuhraya Sultan Abdul Halim";
            AlamatSyarikat3 = "05300 Alor Setar, KEDAH DARUL AMAN";
            TelSyarikat = "04-7027441 / 7470 ";
            FaksSyarikat = "04-7027442";
        }
    }
}
