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
    public class AkPOLaras : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Rujukan")]
        public string NoRujukan { get; set; }
        [DisplayName("Tarikh")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [MaxLength(4)]
        [DisplayName("Tahun Belanjawan")]
        public string Tahun { get; set; }
        public string Tajuk { get; set; }
        //field end

        //flag
        [DisplayName("Status Batal")]
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
        public int AkPOId { get; set; }
        public AkPO AkPO { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        public ICollection<AkPOLaras2> AkPOLaras2 { get; set; }
        public ICollection<AkPOLaras1> AkPOLaras1 { get; set; }

        //relationship end

    }
}
