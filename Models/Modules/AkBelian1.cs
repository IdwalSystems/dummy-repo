using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkBelian1
    {
        public int Id { get; set; }
        public int AkBelianId { get; set; }
        public int AkCartaId { get; set; }
        public decimal Amaun { get; set; }

        //Relationship
        public AkCarta AkCarta { get; set; }
    }
}
