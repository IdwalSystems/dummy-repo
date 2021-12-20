using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkJurnal1
    {
        public int Id { get; set; }
        [Display(Name = "No Rujukan")]
        public int AkJurnalId { get; set; }
        public int Indeks { get; set; }
        public int AkCartaId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }

        //Relationship
        public AkCarta AkCarta { get; set; }
        //public AkJurnal AkJurnal { get; set; }

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
