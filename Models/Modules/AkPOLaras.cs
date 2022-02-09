using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPOLaras 
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Pesanan Tempatan")]
        public string NoPOLaras { get; set; }
        [DisplayName("Tarikh")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        [MaxLength(4)]
        [DisplayName("Tahun Belanjawan")]
        public string Tahun { get; set; }
        public string Tajuk { get; set; }
        //field end

        //flag
        [DisplayName("Status Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        public int FlCetak { get; set; }
        //flag end

        //relationship
        [DisplayName("No Pesanan Tempatan")]
        public int AkPOId { get; set; }
        public AkPO AkPO { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public ICollection<AkPOLaras2> AkPOLaras2 { get; set; }
        public ICollection<AkPOLaras1> AkPOLaras1 { get; set; }

        //relationship end

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        //log end
    }
}
