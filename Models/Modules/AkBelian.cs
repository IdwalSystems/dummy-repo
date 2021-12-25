using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkBelian
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("No Rujukan")]
        [Required(ErrorMessage = "No Rujukan Diperlukan")]
        [MaxLength(20)]
        public string NoInbois { get; set; }
        public decimal Jumlah { get; set; }
        //field end

        //flag
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Dengan Pesanan Tempatan/Tanpa Pesanan Tempatan")]
        [DefaultValue("1")]
        public string FlPO { get; set; }
        //flag end

        //Relationship
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        [DisplayName("No Pesanan Tempatan")]
        public int? AkPOId { get; set; }
        [Required(ErrorMessage = "Kod Bank Diperlukan.")]
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }
        [Required(ErrorMessage = "Kod Pembekal Diperlukan.")]
        [DisplayName("Kod Pembekal")]
        public int AkPembekalId { get; set; }
        public JKW JKW { get; set; }
        public AkPO AkPO { get; set; }
        public AkBank AkBank { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public ICollection<AkBelian1> AkBelian1 { get; set; }
        public ICollection<AkBelian2> AkBelian2 { get; set; }
        //relationship end

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
