using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima
    {
        public int Id { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        public int JKWId { get; set; }
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        public DateTime Tarikh { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Jumlah { get; set; }
        public int AkBankId { get; set; }
        public int FlCetak { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }
        [MaxLength(20)]
        public string KodPembayar { get; set; }
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
        public int JNegeriId { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        [MaxLength(15)]
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        [MaxLength(15)]
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; }
        
        //Relationship
        public JKW JKW { get; set; }
        public JNegeri JNegeri { get; set; }
        public AkBank AkBank { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
    }
}
