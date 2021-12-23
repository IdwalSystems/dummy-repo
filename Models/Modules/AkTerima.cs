using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }      
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        public decimal Jumlah { get; set; }
        [DisplayName("Kod Pembayar")]
        [MaxLength(20)]
        public string KodPembayar { get; set; }
        [DisplayName("No KP")]
        [MaxLength(15)]
        public string NoKp { get; set; }
        [Required(ErrorMessage = "Nama Diperlukan")]
        [MaxLength(100)]
        public string Nama { get; set; }
        [MaxLength(100)]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        public string Alamat3 { get; set; }
        [MaxLength(5)]
        public string Poskod { get; set; }
        [MaxLength(100)]
        public string Bandar { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        //field end

        //flag
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        //flag end

        //Relationship
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        [Required(ErrorMessage = "Negeri Diperlukan.")]
        [DisplayName("Negeri")]
        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        [Required(ErrorMessage = "Kod Bank Diperlukan")]
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
        //relationship end

        //log
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
