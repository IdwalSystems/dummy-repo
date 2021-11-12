using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima2
    {
        public int RujukanAkTerima { get; set; }
        public int KodCaraBayar { get; set; }
        public decimal Amaun { get; set; }
        public string NoCek { get; set; }
        public string JenisCek { get; set; }
        public int KodBankCek { get; set; }
        public string TempatCek { get; set; }
        public string NoSlip { get; set; }
        public DateTime TarSlip { get; set; }

        public AkTerima AkTerima { get; set; }
        public CaraBayar CaraBayar { get; set; }
        public Bank Bank { get; set; }
    }
}
