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
            AlamatSyarikat1 = "Stadium Sultan Abdul Halim";
            AlamatSyarikat2 = "Jalan Suka Menanti";
            AlamatSyarikat3 = "05150 Alor Setar,Kedah";
            TelSyarikat = "04-7303362 / 04-7303363";
            FaksSyarikat = "04-734262";
        }
    }
}
