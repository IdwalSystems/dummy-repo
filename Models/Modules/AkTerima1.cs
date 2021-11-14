using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkTerima1
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int AkAkaunId { get; set; }
        public decimal Amaun { get; set; }

        
        
        //Relationship
        public AkAkaun AkAkaun { get; set; }
        public AkTerima AkTerima { get; set; }
    }
}