using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima1
    {
        public int RujukanAkTerima { get; set; }
        public int KodAkCarta { get; set; }
        public int Id { get; set; }
        public decimal Amaun { get; set; }

        public AkTerima AkTerima { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }

    }
}
