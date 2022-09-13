using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPO : AppLogHelper, ISoftDelete, ICancel
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Pesanan Tempatan")]
        public string NoPO { get; set; }
        [DisplayName("Tarikh")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Bekalkan Sebelum / Pada")]
        public DateTime? TarikhBekalan { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [MaxLength(4)]
        [DisplayName("Tahun")]
        public string Tahun { get; set; }
        public DateTime TempohSiap { get; set; }
        public DateTime TarikhSiap { get; set; }
        public string Tajuk { get; set; }
        //field end

        //flag
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        public DateTime? TarBatal { get; set; }
        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        // untuk cek po tersebut ada di kewangan atau tidak
        [DefaultValue(true)]
        public bool IsInKewangan { get; set; }
        //flag end

        //relationship
        [DisplayName("Kod Pembekal")]
        public int AkPembekalId { get; set; }
        [DisplayName("Nama Pembekal")]
        public AkPembekal AkPembekal { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [DisplayName("No Nota Minta")]
        public int? AkNotaMintaId { get; set; }
        public AkNotaMinta AkNotaMinta { get; set; }
        public JKW JKW { get; set; }
        public ICollection<AkPO2> AkPO2 { get; set; }
        public ICollection<AkPO1> AkPO1 { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }
        public ICollection<AkPOLaras> AkPOLaras { get; set; }
        

        //relationship end

    }
}
