using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkCimbEFT1
    {
        // note:
        // FlJenisBaucer = 0 ( Am )
        // FlJenisBaucer = 1 ( Inbois )
        // FlJenisBaucer = 2 ( Gaji )
        // FlJenisBaucer = 3 ( Pendahuluan )
        // FlJenisBaucer = 4 ( Rekupan )
        // FlJenisBaucer = 5 ( Tambah Had Panjar )
        // FlJenisBaucer = 6 ( Profil Atlet / Jurulatih )
        // ..
        // FlPenerimaEFT = 0 ( Am / Lain - lain )
        // FlPenerimaEFT = 1 ( pembekal )
        // FlPenerimaEFT = 2 ( pekerja )
        // FlPenerimaEFT = 3 ( pemegang panjar )
        // FlPenerimaEFT = 4 ( jurulatih )
        // FlPenerimaEFT = 5 ( atlet )
        public int Id { get; set; }
        public int AkCimbEFTId { get; set; }
        public AkCimbEFT AkCimbEFT { get; set; }
        public int AkPVId { get; set; }
        public AkPV AkPV { get; set; }
        public int FlPenerimaEFT { get; set; }
        public int? PenerimaId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public SuJurulatih SuJurulatih { get; set; }
        public SuAtlet SuAtlet { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        public string NoCek { get; set; }
        public string Catatan { get; set; }

        // note:
        // AkBank - Akaun Bank Penerima
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }

        // FlStatus = 0 -> Tolak / Gagal
        // FlStatus = 1 -> Berjaya
        public int? FlStatus { get; set; }


    }
}
