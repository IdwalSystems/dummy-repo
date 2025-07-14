using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class SuProfil : AppLogHelper, ISoftDelete

    {
        public int Id { get; set; }
        [DisplayName("No Rujukan")]
        [Required(ErrorMessage = "No Rujukan diperlukan")]
        public string NoRujukan { get; set; }
        [MaxLength(2)]
        [Required(ErrorMessage = "Bulan diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Bulan { get; set; }
        [MaxLength(4)]
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        public string Tahun { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        //Kategori
        //  0 = Atlet 
        //  1 = Jurulatih
        //Kategori END
        [DisplayName("Kategori")]
        public int FlKategori { get; set; }
        //relationship
        [Display(Name = "Kod Akaun")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kod Akaun")]
        [Required(ErrorMessage = "Kod Akaun diperlukan")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [Display(Name = "Kumpulan Wang")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        [Display(Name = "Bahagian")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public ICollection<SuProfil1> SuProfil1 { get; set; }
        //relationship end

        //flag
        [DisplayName("Status Batal")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        //flag end
    }
}
