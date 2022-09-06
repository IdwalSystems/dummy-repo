using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SuJurulatih : AppLogHelper, ISoftDelete

    {
        public int Id { get; set; }
        [DisplayName("Kod Jurulatih")]
        public string KodJurulatih { get; set; }
        [Required(ErrorMessage = "No Kad Pengenalan Diperlukan")]
        [DisplayName("No KP")]
        public string NoKp { get; set; }
        public string Nama { get; set; }
        [DisplayName("Alamat")]
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Poskod { get; set; }
        public string Bandar { get; set; }
        [DisplayName("Negeri")]
        [Required(ErrorMessage = "Negeri Diperlukan")]
        public int JNegeriId { get; set; }
        [DisplayName("No Telefon")]
        public string Telefon { get; set; }
        [EmailAddress(ErrorMessage = "Emel Tidak Sah."), MaxLength(100)]
        public string Emel { get; set; }
        [DisplayName("Status Aktif")]
        public int FlStatus { get; set; }
        [DisplayName("Tarikh Aktif")]
        public DateTime TarikhAktif { get; set; }
        [DisplayName("Tarikh Berhenti")]
        public DateTime? TarikhBerhenti { get; set; }
        [DisplayName("Nama Bank")]
        public int JBankId { get; set; }
        [DisplayName("Agama")]
        public int? JAgamaId { get; set; }
        [DisplayName("Bangsa")]
        public int? JBangsaId { get; set; }
        [DisplayName("Jawatan")]
        public string Jawatan { get; set; }
        [DisplayName("Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        [DisplayName("No Akaun Bank")]
        public string NoAkaunBank { get; set; }
        //[DisplayName("Profil Kategori")]
        //public int? JProfilKategoriId { get; set; }
        //public JProfilKategori JProfilKategori { get; set; }
        [DisplayName("JSM Bakat")]
        public bool IsJSMBakat { get; set; }
        [DisplayName("JSM Pelapis")]
        public bool IsJSMPelapis { get; set; }
        [DisplayName("SUKMA")]
        public bool IsSukma { get; set; }

        //relationship
        [DisplayName("Cara Bayar")]
        public JCaraBayar JCaraBayar { get; set; }
        [DisplayName("Sukan")]
        public JSukan JSukan { get; set; }
        [DisplayName("Sukan")]
        [Required(ErrorMessage = "Sukan Diperlukan")]
        public int JSukanId { get; set; }
        [DisplayName("Negeri")]
        public JNegeri JNegeri { get; set; }
        [DisplayName("Agama")]
        public JAgama JAgama { get; set; }
        [DisplayName("Nama Bank")]
        public JBank JBank { get; set; }
        [DisplayName("Bangsa")]
        public JBangsa JBangsa { get; set; }
        public ICollection<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public ICollection<AkPVGanda> AkPVGanda { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
