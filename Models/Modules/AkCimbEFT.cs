using MSNK.Models.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkCimbEFT : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string NoPBI { get; set; }
        public DateTime TarJana { get; set; }
        public DateTime TarBayar { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; } 
        public string NamaFail { get; set; }
        public string BilPV { get; set; }
        public string FlKategori { get; set; }
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }

        // note:
        // AkBank - Akaun Bank Pembayar
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }

        // FlStatus = 0 -> Tolak / Gagal keseluruhan
        // FlStatus = 1 -> Berjaya keseluruhan
        // FlStatus = 2 -> Ada yang ditolak, ada yang berjaya
        public int FlStatus { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public ICollection<AkCimbEFT1> AkCimbEFT1 { get; set; }
    }
}
