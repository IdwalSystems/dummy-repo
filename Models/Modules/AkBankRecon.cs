using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkBankRecon : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        [MaxLength(2)]
        public string Bulan { get; set; }
        [DisplayName("Baki Penyata RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BakiPenyata { get; set; }
        [Required(ErrorMessage = "Akaun Bank Diperlukan")]
        [DisplayName("Bank")]
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }
        [DisplayName("Muat Naik")]
        public int FlMuatNaik { get; set; }
        [DisplayName("Tarikh Muat Naik")]
        public DateTime? TarMuatNaik { get; set; }
        public bool IsKunci { get; set; }
        [DisplayName("Tarikh Kunci")]
        public DateTime? TarKunci { get; set; }
        public ICollection<AkBankReconPenyataBank> AkBankReconPenyataBank { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
    }
}
