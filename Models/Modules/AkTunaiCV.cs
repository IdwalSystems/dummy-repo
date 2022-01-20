using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiCV
    {
        public int Id { get; set; }
        [DisplayName("Kod Kaunter")]
        public int AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public int KategoriPenerima { get; set; }
        public string Tahun { get; set; }
        [DisplayName("No CV")]
        public string NoCV { get; set; }
        public DateTime Tarikh { get; set; }
        [DisplayName("Kod Pekerja")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
        [DisplayName("Kod Pembekal")]
        public int? AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public string Penerima { get; set; }
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Catatan { get; set; }
        public decimal Jumlah { get; set; }
        public ICollection<AkTunaiCV1> AkTunaiCV1 { get; set; }
        public int FlPosting { get; set; }
        public int FlCetak { get; set; }
        public int FlBatal { get; set; }
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
    }
}
