using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkNotaMinta : AppLogHelper
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
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        //flag end

        // untuk kewangan
        [DisplayName("No Siri")]
        public string NoSiri{ get; set; }
        [DisplayName("No CAS")] // no PO
        public string NoCAS { get; set; }
        [DisplayName("Tarikh Seksyen Kewangan")]
        public DateTime? TarikhSeksyenKewangan { get; set; }
        // untuk kewangan end


        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        [DisplayName("Kod Pembekal")]
        public int AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public ICollection<AkNotaMinta1> AkNotaMinta1 { get; set; }
        public ICollection<AkNotaMinta2> AkNotaMinta2 { get; set; }
        public ICollection<AkPO> AkPO { get; set; }

    }
}
