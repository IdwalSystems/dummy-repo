using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima2
    {
        public int RujukanAkTerima { get; set; }
        public int KodCaraBayar { get; set; }
        public decimal Amaun { get; set; }
        [MaxLength(10)]
        public string NoCek { get; set; }
        [MaxLength(1)]
        public string JenisCek { get; set; }
        public int KodBankCek { get; set; }
        [MaxLength(100)]
        public string TempatCek { get; set; }
        [MaxLength(30)]
        public string NoSlip { get; set; }
        public DateTime TarSlip { get; set; }

        public AkTerima AkTerima { get; set; }
        public CaraBayar CaraBayar { get; set; }
        public Bank Bank { get; set; }
    }
}
