using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkTerima1
    {
        //field
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int AkCartaId { get; set; }
        public decimal Amaun { get; set; }
        //field end  
        
        //Relationship
        public AkCarta AkCarta { get; set; }
        //relationship end

    }
}