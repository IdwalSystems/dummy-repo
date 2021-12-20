using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JBank
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(12)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nama { get; set; }
        public string KodEFT { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
        //public ICollection<AkTerima2> AkTerima2 { get; set; }

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
    }
}