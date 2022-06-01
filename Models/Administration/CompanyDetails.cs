using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Administration
{
    public class CompanyDetails
    {
        public string KodSistem { get; set; }
        public DateTime TarVersi { get; set; }
        public string NamaSyarikat { get; set; }
        public string NoPendaftaran { get; set; }
        public string AlamatSyarikat1 { get; set; }
        public string AlamatSyarikat2 { get; set; }
        public string AlamatSyarikat3 { get; set; }
        public string Bandar { get; set; }
        public string Poskod { get; set; }
        public string Daerah { get; set; }
        public string Negeri { get; set; }
        public string TelSyarikat { get; set; }
        public string FaksSyarikat { get; set; }
        public string EmelSyarikat { get; set; }
        public DateTime TarMula { get; set; }
        public string LogoSyarikat { get; set; }

        
        public CompanyDetails()
        {
            KodSistem = "SPMB";
            NamaSyarikat = "Majlis Sukan Negeri Kedah";
            NoPendaftaran = "";
            AlamatSyarikat1 = "Kompleks Sukan Muadzam Shah";
            AlamatSyarikat2 = "Lebuhraya Sultan Abdul Halim";
            AlamatSyarikat3 = "05300 Alor Setar, KEDAH DARUL AMAN";
            Bandar = "";
            Poskod = "05300";
            Daerah = "Alor Setar";
            Negeri = "Kedah Darul Aman";
            TelSyarikat = "04-7027441 / 7470 ";
            FaksSyarikat = "04-7027442";
            EmelSyarikat = "msnk@kedah.gov.my";
            LogoSyarikat = "MainLogo_Syarikat.png";

        }
    }
}
