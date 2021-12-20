using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkTerima1
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int AkCartaId { get; set; }
        public decimal Amaun { get; set; }

        
        
        //Relationship
        public AkCarta AkCarta { get; set; }
        //public AkTerima AkTerima { get; set; }

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
    }
}