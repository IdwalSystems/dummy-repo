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
    public class AkPOLaras : AppLogHelper, ISoftDelete, ICancel
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Rujukan")]
        public string NoRujukan { get; set; }
        [DisplayName("Tarikh")]
        [Required(ErrorMessage = "Tarikh diperlukan")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        public string Tahun { get; set; }
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
        //flag end

        //relationship
        [DisplayName("No Pesanan Tempatan")]
        [Required(ErrorMessage = "No Pesanan Tempatan diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih No Pesanan Tempatan")]
        public int AkPOId { get; set; }
        public AkPO AkPO { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        [Required(ErrorMessage = "Bahagian diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        public ICollection<AkPOLaras2> AkPOLaras2 { get; set; }
        public ICollection<AkPOLaras1> AkPOLaras1 { get; set; }
        

        //relationship end

    }
}
