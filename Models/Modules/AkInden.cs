using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkInden : AppLogHelper, ISoftDelete, ICancel
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Inden")]
        public string NoInden { get; set; }
        [DisplayName("Tarikh")]
        [Required(ErrorMessage = "Tarikh diperlukan")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Bekalkan Sebelum / Pada")]
        public DateTime? TarikhBekalan { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        [DisplayName("Tahun Bel.")]
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
        public string SebabHapus { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        // untuk cek Inden tersebut ada di kewangan atau tidak
        [DefaultValue(true)]
        public bool IsInKewangan { get; set; }
        //flag end

        //relationship
        [Required(ErrorMessage = "Kod Pembekal diperlukan")]
        [DisplayName("Kod Pembekal")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Pembekal")]
        public int AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }

        [DisplayName("Bahagian")]
        [Required(ErrorMessage = "Bahagian diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [DisplayName("No Nota Minta")]
        public int? AkNotaMintaId { get; set; }
        public AkNotaMinta AkNotaMinta { get; set; }
        public JKW JKW { get; set; }
        public ICollection<AkInden2> AkInden2 { get; set; }
        public ICollection<AkInden1> AkInden1 { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }

        //relationship end
    }
}
