using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkNotaMinta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public string NoRujukan { get; set; }
        public string Tajuk { get; set; }
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        //flag
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        //flag end

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
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public int AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public ICollection<AkNotaMinta1> AkNotaMinta1 { get; set; }
        public ICollection<AkNotaMinta2> AkNotaMinta2 { get; set; }

    }
}
