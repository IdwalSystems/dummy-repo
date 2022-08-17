using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkBankReconPenyataBank
    {
        public int Id { get; set; }
        public int AkBankReconId { get; set; }
        public AkBankRecon AkBankRecon { get; set; }
        public string NoAkaunBank { get; set; }
        public DateTime Tarikh { get; set; }
        public string KodTransaksi { get; set; }
        public string PerihalTransaksi { get; set; }
        public string NoDokumen { get; set; }
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Baki { get; set; }
        public int AkPadananPenyataId { get; set; }
        public AkPadananPenyata AkPadananPenyata { get; set; }
    }
}
