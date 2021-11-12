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
        public int KodBank { get; set; }    //akBank
        [MaxLength(1)]
        public string FlCetak { get; set; }
        [MaxLength(1)]
        public string FlPosting { get; set; }
        [MaxLength(1)]
        public string FlBatal { get; set; }
        public string KodPembayar { get; set; }
        public string NoKp { get; set; }
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public int Poskod { get; set; }
        public string Bandar { get; set; }
        public int KodNegeri { get; set; }
        public string Tel { get; set; }
        public string Emel { get; set; }
        public string Sebab { get; set; }
        public string UserIdMasuk { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemasKini { get; set; }
        public DateTime TarKemasKini { get; set; }

        public KW KW { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
        public AkBank AkBank { get; set; }

    }
}
