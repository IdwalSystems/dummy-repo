using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JKW
    {
        //field
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }
        //field end


        //Relationship
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkAkaun> AkAkaun { get; set; }
        public ICollection<AkPO> AkPO { get; set; }
        public ICollection<AkPOLaras> AkPOLaras { get; set; }
        public ICollection<AkJurnal> AkJurnal { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AbBukuVot> AbBukuVot { get; set; }
        public ICollection<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public ICollection<AkNotaMinta> AkNotaMinta { get; set; }
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