using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkJurnal1
    {
        public int Id { get; set; }
        public int AkJurnalId { get; set; }
        [MaxLength(20)]
        [Display(Name = "No Rujukan")]
        public string NoRujukan { get; set; }
        public int Indeks { get; set; }
        public int AkCartaId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }

        //Relationship
        public AkCarta AkCarta { get; set; }
        //public AkJurnal AkJurnal { get; set; }
    }
}
