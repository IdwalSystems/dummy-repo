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
        public string Tahun { get; set; }
        public int KWId { get; set; }
        public string NoRujukan { get; set; }
        public DateTime Tarikh { get; set; }
        public decimal Jumlah { get; set; }
        public int AkBankId { get; set; }
        public int FlCetak { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }
        public string KodPembayar { get; set; }
        public string NoKp { get; set; }
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Poskod { get; set; }
        public string Bandar { get; set; }
        public int NegeriId { get; set; }
        public string Tel { get; set; }
        public string Emel { get; set; }
        public string Sebab { get; set; }
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; }
        
        //Relationship
        public KW KW { get; set; }
        public Negeri Negeri { get; set; }
        public AkBank AkBank { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkTerima2> AkTerima2 { get; set; }
    }
}
