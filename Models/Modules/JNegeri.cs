using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JNegeri
    {
        //field
        public int Id { get; set; }
        [Required]
        [MaxLength(2)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }
        //field end

        //Relationship
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
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