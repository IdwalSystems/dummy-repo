using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPO : AppLogHelper
    {
        //field
        public int Id { get; set; }
        [MaxLength(50)]
        [DisplayName("No. Pesanan Tempatan")]
        public string NoPO { get; set; }
        [DisplayName("Tarikh")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        [MaxLength(4)]
        [DisplayName("Tahun Belanjawan")]
        public string Tahun { get; set; }
        public DateTime TempohSiap { get; set; }
        public DateTime TarikhSiap { get; set; }
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
        [DisplayName("Kod Pembekal")]
        public int AkPembekalId { get; set; }
        [DisplayName("Nama Pembekal")]
        public AkPembekal AkPembekal { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
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
