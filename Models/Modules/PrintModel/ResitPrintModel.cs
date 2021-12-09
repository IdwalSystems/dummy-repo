using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.PrintModel
{
    public class ResitPrintModel
    {
        public string LogoSyarikat { get; set; }
        public string NamaSyarikat { get; set; }
        public string AlamatSyarikat1 { get; set; }
        public string AlamatSyarikat2 { get; set; }
        public string AlamatSyarikat3 { get; set; }
        public string TelSyarikat { get; set; }
        public JNegeri Negeri { get; set; }
        public AkTerima AkTerima { get; set; }
    }
}
