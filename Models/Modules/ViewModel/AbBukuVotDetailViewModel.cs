using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules.ViewModel
{
    public class AbBukuVotDetailViewModel
    {
        public int Id { get; set; }
        public DateTime Tarikh { get; set; }
        [DisplayName("Kod")]
        public string Kod { get; set; }
        public string Nama { get; set; }
        [DisplayName("No Rujukan")]
        public string NoRujukan { get; set; }
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Tanggungan RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tanggungan { get; set; }
        [DisplayName("Liabiliti RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Liabiliti { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Baki { get; set; }
    }
}
