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
        public int Id { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        [DisplayName("Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime Tarikh { get; set; }
        public decimal Jumlah { get; set; }
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Kod Pembayar")]
        [MaxLength(20)]
        public string KodPembayar { get; set; }
        [DisplayName("No KP")]
        [MaxLength(15)]
        public string NoKp { get; set; }
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
        [DisplayName("Negeri")]
        public int JNegeriId { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        [MaxLength(15)]
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        public DateTime TarMasuk { get; set; }
        [MaxLength(15)]
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        
        //Relationship
        public JKW JKW { get; set; }
        public JNegeri JNegeri { get; set; }
        public AkBank AkBank { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
    }
}
