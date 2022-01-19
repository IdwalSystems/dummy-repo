using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiCV1
    {
        //field
        public int Id { get; set; }
        public int AkTunaiCVId { get; set; }
        public int AkCartaId { get; set; }
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        public AkCarta AkCarta { get; set; }
        //relationship end
    }
}
