using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JKW : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(3)]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Perihal Diperlukan")]
        [MaxLength(100)]
        public string Perihal { get; set; }
        //field end


        //Relationship
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkAkaun> AkAkaun { get; set; }
        public ICollection<AkPO> AkPO { get; set; }
        public ICollection<AkInden> AkInden { get; set; }
        public ICollection<AkPOLaras> AkPOLaras { get; set; }
        public ICollection<AkJurnal> AkJurnal { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AbBukuVot> AbBukuVot { get; set; }
        public ICollection<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public ICollection<AkNotaMinta> AkNotaMinta { get; set; }
        public ICollection<AbWaran> AbWaran { get; set; }
        public ICollection<SuProfil> SuProfil { get; set; }
        public ICollection<JPTJ> JPTJ { get; set; }
        public ICollection<AkInvois> AkInvois { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //soft delete end
    }
}