using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Login.ViewModel
{
    public class AppInfoViewModel : EditImageViewModel
    {
        public int Id { get; set; }
        [DisplayName("Kod Sistem")]
        public string KodSistem { get; set; }
        [DisplayName("Tarikh Versi")]
        public DateTime TarVersi { get; set; }
        [DisplayName("Nama Syarikat")]
        public string NamaSyarikat { get; set; }
        [DisplayName("No Daftar")]
        public string NoPendaftaran { get; set; }
        [DisplayName("Alamat")]
        public string AlamatSyarikat1 { get; set; }
        [DisplayName("")]
        public string AlamatSyarikat2 { get; set; }
        [DisplayName("")]
        public string AlamatSyarikat3 { get; set; }
        public string Bandar { get; set; }
        [DisplayName("Poskod")]
        public string Poskod { get; set; }
        public string Daerah { get; set; }
        public string Negeri { get; set; }
        [DisplayName("No Tel")]
        public string TelSyarikat { get; set; }
        [DisplayName("No Faks")]
        public string FaksSyarikat { get; set; }
        [EmailAddress]
        [DisplayName("Emel")]
        public string EmelSyarikat { get; set; }
        [DisplayName("Tarikh Mula")]
        public DateTime TarMula { get; set; }
        [DisplayName("Logo")]
        public string LogoSyarikat { get; set; }
    }
}
