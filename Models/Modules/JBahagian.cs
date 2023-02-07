using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JBahagian : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Perihal Diperlukan")]
        public string Perihal { get; set; }
        public JKW JKW { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        public int JKWId { get; set; }
        public JPTJ JPTJ { get; set; }
        [DisplayName("PTJ")]
        public int? JPTJId { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }

        public ICollection<AbWaran> AbWaran { get; set; }
        public ICollection<AbWaran1> AbWaran1 { get; set; }
        public ICollection<AbBukuVot> abBukuVot { get; set; }
        public ICollection<AkAkaun> AkAkaun { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }
        public ICollection<AkJurnal> AkJurnal { get; set; }
        public ICollection<AkJurnal1> AkJurnal1 { get; set; }
        public ICollection<AkNotaMinta> AkNotaMinta { get; set; }
        public ICollection<AkPO> AkPO { get; set; }
        public ICollection<AkInden> AkInden { get; set; }
        public ICollection<AkPOLaras> AkPOLaras { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        public ICollection<AkTunaiLejar> AkTunaiLejar { get; set; }
        public ICollection<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public ICollection<SpPendahuluanPelbagai> SpPendahuluanPelbagai { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<SuProfil> SuProfil { get; set; }
        public ICollection<AkInvois> AkInvois { get; set; }
        public ICollection<AkPenyataPemungut1> AkPenyataPemungut1 { get; set; }
        public ICollection<AkNotaDebitKreditBelian> AkNotaDebitKreditBelian { get; set; }
    }
}
