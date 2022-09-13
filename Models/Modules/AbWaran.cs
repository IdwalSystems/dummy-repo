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
    public class AbWaran : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "No Rujukan diperlukan")]
        [DisplayName("No Rujukan")]
        public string NoRujukan { get; set; }
        [MaxLength(4)]
        [DisplayName("Tahun")]
        [Required(ErrorMessage = "Tahun diperlukan")]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh diperlukan")]
        public DateTime Tarikh { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Jumlah { get; set; }

        //flag
        // WPA = 0(asal); WPT = 1 (tambah/tarik balik); WPP = 2(pindah);
        public int FlJenisWaran { get; set; }
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
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW {get;set;}
        [Required(ErrorMessage = "Bahagian diperlukan")]
        [DisplayName("Bahagian")]
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public ICollection<AbWaran1> AbWaran1 { get; set; }
        //relationship end
    }
}
