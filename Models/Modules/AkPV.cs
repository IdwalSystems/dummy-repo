using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPV
    {
        //field
        public int Id { get; set; }
        [DisplayName("Tahun Belanjawan")]
        [Required(ErrorMessage = "Tahun Diperlukan.")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh Diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoPV { get; set; }
        [DisplayName("Jumlah RM")]
        public decimal Jumlah { get; set; }
        [DisplayName("No KP")]
        public string NoKP { get; set; }
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Telefon { get; set; }
        [DisplayName("No Akaun Bank")]
        public string NoAkaunBank { get; set; }
        public string Emel { get; set; }
        [DisplayName("No Cek / EFT / JomPAY")]
        [MaxLength(10)]
        public string NoCekAtauEFT { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Tarikh Cek / EFT / JomPAY")]
        public DateTime? TarCekAtauEFT { get; set; }
        [MaxLength(400)]
        [DefaultValue("")]
        public string Perihal { get; set; }
        [DisplayName("No Rekup")]
        public string NoRekup { get; set; }
        //field end

        //flag
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        [DisplayName("Jenis Baucer")]
        // Am = 0; Rekupan = 1; Akruan = 2;
        public int FlJenisBaucer { get; set; }
        // Am = 0; Pembekal = 1; Pekerja = 2;
        public int FlKategoriPenerima { get; set; }
        public bool denganTanggungan { get; set; }
        //flag end

        //relationship
        [Required(ErrorMessage = "Jenis Kumpulan Wang Diperlukan.")]
        [DisplayName("Jenis Kumpulan Wang")]
        public int JKWId { get; set; }
        
        [Required(ErrorMessage = "Kod Bank Diperlukan.")]
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }
        [DisplayName("Kod Pembekal")]
        public int? AkPembekalId { get; set; }
        [DisplayName("Kod Pekerja")]
        public int? SuPekerjaId { get; set; }
        [Required(ErrorMessage = "Cara Bayaran Diperlukan.")]
        [DisplayName("Cara Bayaran")]
        public int JCaraBayarId { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public int? AkTunaiRuncitId { get; set; }
        [DisplayName("No Permohonan Aktiviti")]
        public int? SpPendahuluanPelbagaiId { get; set; }
        public JKW JKW { get; set; }
        
        public AkBank AkBank { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public JCaraBayar JCaraBayar { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        public ICollection<AkPV1> AkPV1 { get; set; }
        public ICollection<AkPV2> AkPV2 { get; set; }
        //relationship end

        //log 
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
