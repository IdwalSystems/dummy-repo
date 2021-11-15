using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkAkaun
    {
        public int KWId { get; set; }
        public int AkCartaId1 { get; set; }
        public DateTime Tarikh { get; set; }
        public int AkCartaId2 { get; set; }
        public int Id { get; set; }
        [MaxLength(40)]
        public string NoRujukan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }

        //Relationship
        public KW KW { get; set; }
        public virtual AkCarta AkCarta1 { get; set; }
        public virtual AkCarta AkCarta2 { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; } 
    }
}
