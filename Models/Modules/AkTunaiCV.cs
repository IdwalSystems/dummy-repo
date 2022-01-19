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
        public int AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public int KategoriPenerima { get; set; }
        public string Tahun { get; set; }
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public string NoCV { get; set; }
        public DateTime Tarikh { get; set; }
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public int? AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public string Penerima { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Almat3 { get; set; }
        public string Catatan { get; set; }
        public int AkBankId { get; set; }
        public AkBank AkBank { get; set; }
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
