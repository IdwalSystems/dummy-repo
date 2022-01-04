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
        [Display(Name = "KW")]
        public int JKWId { get; set; }
        [Display(Name = "Carta 1")]
        public int AkCartaId1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Tarikh { get; set; }
        [Display(Name = "Carta 2")]
        public int AkCartaId2 { get; set; }
        public int Id { get; set; }
        [Display(Name = "No Rujukan")]
        [MaxLength(40)]
        public string NoRujukan { get; set; }
        [Display(Name = "Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [Display(Name = "Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }

        //Relationship

        [Display(Name = "KW")]
        public JKW JKW { get; set; }
        [Display(Name = "Carta 1")]
        public virtual AkCarta AkCarta1 { get; set; }
        [Display(Name = "Carta 2")]
        public virtual AkCarta AkCarta2 { get; set; }
        //public ICollection<AkTerima> AkTerima { get; set; } 
    }
}
