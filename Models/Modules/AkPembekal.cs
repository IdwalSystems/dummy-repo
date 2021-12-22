using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPembekal
    {
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
        [Required(ErrorMessage = "Negeri Diperlukan.")]
        [Display(Name = "Negeri")]
        public int JNegeriId { get; set; }
        [Required(ErrorMessage = "Nombor Telefon Diperlukan."), Phone(ErrorMessage = "Nombor Telefon Tidak Sah."), MaxLength(30)]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Emel Diperlukan."), EmailAddress(ErrorMessage = "Emel Tidak Sah."), MaxLength(100)]
        public string Emel { get; set; }
        [Required(ErrorMessage = "Nombor Akaun Bank Diperlukan."), MaxLength(20)]
        [Display(Name = "No Akaun Bank")]
        public string AkaunBank { get; set; }
        [Required(ErrorMessage = "Nama Bank Diperlukan.")]
        [Display(Name = "Bank")]
        public int JBankId { get; set; }

        //Relationship

        [Display(Name = "Negeri")]
        public JNegeri JNegeri { get; set; }
        [Display(Name = "Bank")]
        public JBank JBank { get; set; }
        public ICollection<AkPO> AkPO { get; set; }
        public ICollection<AkBelian> AkBelian { get; set; }

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
