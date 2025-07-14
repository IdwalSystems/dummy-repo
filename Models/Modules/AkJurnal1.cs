using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkJurnal1
    {
        public int Id { get; set; }
        [Display(Name = "No Rujukan")]
        public int AkJurnalId { get; set; }
        public AkJurnal AkJurnal { get; set; }
        public int Indeks { get; set; }
        [DisplayName("Bahagian")]
        public int? JBahagianDebitId { get; set; }
        public JBahagian JBahagianDebit { get; set; }
        
        [DisplayName("Kod Objek")]
        public int? AkCartaDebitId { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianKreditId { get; set; }
        public JBahagian JBahagianKredit { get; set; }

        [DisplayName("Kod Objek")]
        public int? AkCartaKreditId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        //public decimal Debit { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        //public decimal Kredit { get; set; }

        //Relationship
        public AkCarta AkCartaDebit { get; set; }
        public AkCarta AkCartaKredit { get; set; }
        //public AkJurnal AkJurnal { get; set; }

    }
}
