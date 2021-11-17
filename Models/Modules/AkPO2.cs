using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPO2
    {
        public int Id { get; set; }
        public int AkPOId { get; set; }
        public int JKWId { get; set; }
        public int AkCartaId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amaun { get; set; }

        //Relationship
        public AkPO AkPO { get; set; }
        public JKW JKW { get; set; }
        public AkCarta AkCarta { get; set; }
    }
}
