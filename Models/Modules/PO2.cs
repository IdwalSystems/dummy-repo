using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class PO2
    {
        public int Id { get; set; }
        public int POId { get; set; }
        public int KWId { get; set; }
        public int AkCartaId { get; set; }
        public decimal Amaun { get; set; }

        //Relationship
        public PO PO { get; set; }
        public KW KW { get; set; }
        public AkCarta AkCarta { get; set; }
    }
}
