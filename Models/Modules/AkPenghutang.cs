using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkPenghutang : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [MaxLength(5)]
        [Display(Name = "Kod Syarikat")]
        public string KodSykt { get; set; }//A0000
        [Required(ErrorMessage = "Nama Syarikat Diperlukan."), MaxLength(100)]
        [Display(Name = "Nama Syarikat")]
        public string NamaSykt { get; set; }
        [Required(ErrorMessage = "Nombor Pendaftaran Syarikat Diperlukan."), MaxLength(20)]
        [Display(Name = "No Pendaftaran")]
        public string NoPendaftaran { get; set; }
        [Required(ErrorMessage = "Alamat Diperlukan."), MaxLength(100)]
        [Display(Name = "Alamat 1")]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Alamat 2")]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Alamat 3")]
        public string Alamat3 { get; set; }
        [Required(ErrorMessage = "Poskod Diperlukan."), MaxLength(5), RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor Sahaja.")]
        public string Poskod { get; set; }//nvarchar
        [Required(ErrorMessage = "Bandar Diperlukan."), MaxLength(100)]
        public string Bandar { get; set; }

        [Display(Name = "No Telefon")]
        public string Telefon1 { get; set; }
        public string Emel { get; set; }
        [Required(ErrorMessage = "Nombor Akaun Bank Diperlukan."), MaxLength(20)]
        [Display(Name = "No Akaun Bank")]
        public string AkaunBank { get; set; }
        //field end

        //Relationship
        [Required(ErrorMessage = "Negeri Diperlukan.")]
        [Display(Name = "Negeri")]
        public int JNegeriId { get; set; }
        [Display(Name = "Negeri")]
        public JNegeri JNegeri { get; set; }
        [Required(ErrorMessage = "Nama Bank Diperlukan.")]
        [Display(Name = "Bank")]
        public int JBankId { get; set; }
        [Display(Name = "Bank")]
        public JBank JBank { get; set; }
        public ICollection<AkInvois> AkInvois { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        //relationship end

        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
    }
}
