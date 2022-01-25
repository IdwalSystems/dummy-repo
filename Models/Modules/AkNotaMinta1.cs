using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkNotaMinta1
    {
        //field
        public int Id { get; set; }
        public int AkNotaMintaId { get; set; }
        [DisplayName("Kod Objek")]
        public int AkCartaId { get; set; }
        [DisplayName("Amaun RM")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        public AkCarta AkCarta { get; set; }
        //relationship end
    }
}
