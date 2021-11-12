using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class PO2
    {
        public int Id { get; set; }
        public int NoPO { get; set; }
        public int KodKW { get; set; }
        public int KodAkCarta { get; set; }
        public decimal Amaun { get; set; }

        public PO PO { get; set; }
        public ICollection<KW> KW { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
    }
}
