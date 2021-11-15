using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTerima
    {
        public int KodKW { get; set; }
        public int Id { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        [MaxLength(20)]
        public string NoRujukan { get; set; }
        public DateTime Tarikh { get; set; }
        public decimal Jumlah { get; set; }
        public int KodAkBank { get; set; }    //akBank
        [MaxLength(1)]
        public string FlCetak { get; set; }
        [MaxLength(1)]
        public string FlPosting { get; set; }
        [MaxLength(1)]
        public string FlBatal { get; set; }
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
        public int KodNegeri { get; set; }
        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(400)]
        public string Sebab { get; set; }
        public string UserIdMasuk { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemasKini { get; set; }
        public DateTime TarKemasKini { get; set; }

        public KW KW { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
        public AkBank AkBank { get; set; }
        public Negeri Negeri { get; set; }

    }
}
