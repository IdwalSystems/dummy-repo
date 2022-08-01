using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkTerima1
    {
        //field
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public AkTerima AkTerima { get; set; }
        [DisplayName("Kod Objek")]
        public int AkCartaId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end  
        
        //Relationship
        public AkCarta AkCarta { get; set; }
        //relationship end

    }
}