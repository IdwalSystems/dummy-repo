using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPV1
    {
        //field
        public int Id { get; set; }
        public int AkPVId { get; set; }
        
        public decimal Amaun { get; set; }
        //field end

        //relationship
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        //relationship end

        //log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        //log end
    }
}
