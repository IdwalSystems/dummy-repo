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
    public class AkPV : AppLogHelper, ISoftDelete, ICancel, IRecon
    {
        // note:
        // FlJenisBaucer = 0 ( Am )
        // FlJenisBaucer = 1 ( Inbois )
        // FlJenisBaucer = 2 ( Gaji )
        // FlJenisBaucer = 3 ( Pendahuluan )
        // FlJenisBaucer = 4 ( Rekupan )
        // FlJenisBaucer = 5 ( Tambah Had Panjar )
        // FlJenisBaucer = 6 ( Profil Atlet / Jurulatih )
        // ..
        // FlKategoriPenerima = 0 ( Am / Lain - lain )
        // FlKategoriPenerima = 1 ( pembekal )
        // FlKategoriPenerima = 2 ( pekerja )
        // FlKategoriPenerima = 3 ( pemegang panjar )
        // FlKategoriPenerima = 4 ( jurulatih )
        // FlKategoriPenerima = 5 ( atlet )
        // ..

        //field
        public int Id { get; set; }
        [DisplayName("Tahun")]
        [Required(ErrorMessage = "Tahun diperlukan")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        [MaxLength(4)]
        public string Tahun { get; set; }
        [Required(ErrorMessage = "Tarikh diperlukan")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Tarikh { get; set; }
        public DateTime? TarikhPosting { get; set; }
        [DisplayName("No Rujukan")]
        [MaxLength(20)]
        public string NoPV { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        [DisplayName("No KP")]
        public string NoKP { get; set; }
        [Required(ErrorMessage = "Nama diperlukan")]
        public string Nama { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Telefon { get; set; }
        [DisplayName("No Akaun Bank")]
        public string NoAkaunBank { get; set; }
        [EmailAddress(ErrorMessage = "Emel tidak sah"), MaxLength(100)]
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
        public DateTime? TarBatal { get; set; }
        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        [DisplayName("Jenis Baucer")]
        public int FlJenisBaucer { get; set; }
        [DisplayName("Kategori Penerima")]
        public int FlKategoriPenerima { get; set; }
        public bool denganTanggungan { get; set; }
        public bool IsGanda { get; set; }
        public bool IsAKB { get; set; }
        //flag end

        //untuk kelulusan
        [DisplayName("Penyemak")]
        public int? JPenyemakId { get; set; }
        public JPenyemak JPenyemak { get; set; }
        [DisplayName("Status Semak")]
        public int FlStatusSemak { get; set; }
        public DateTime? TarSemak { get; set; }
        [DisplayName("Pelulus")]
        public int? JPelulusId { get; set; }
        public JPelulus JPelulus { get; set; }
        [DisplayName("Status Lulus")]
        public int FlStatusLulus { get; set; }
        public DateTime? TarLulus { get; set; }
        //untuk kelulusan end 

        //relationship
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }
        
        // kod akaun bank pembayar
        [Required(ErrorMessage = "Kod Bank diperlukan")]
        [DisplayName("Kod Bank")]
        public int AkBankId { get; set; }

        // jenis bank penerima
        [DisplayName("Bank Penerima")]
        public int? JBankId { get; set; }
        [DisplayName("Kod Pembekal")]
        public int? AkPembekalId { get; set; }
        [DisplayName("Kod Anggota")]
        public int? SuPekerjaId { get; set; }
        [DisplayName("Cara Bayaran")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Cara Bayar")]
        public int? JCaraBayarId { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public int? AkTunaiRuncitId { get; set; }
        [DisplayName("No Permohonan Aktiviti")]
        public int? SpPendahuluanPelbagaiId { get; set; }
        [DisplayName("No Profil Atlet / Jurulatih")]
        public int? SuProfilId { get; set; }

        [DisplayName("Bahagian")]
        [Required(ErrorMessage = "Bahagian diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        public JKW JKW { get; set; }
        
        public AkBank AkBank { get; set; }
        public JBank JBank { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public JCaraBayar JCaraBayar { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public SpPendahuluanPelbagai SpPendahuluanPelbagai { get; set; }
        public SuProfil SuProfil { get;set; }
        public ICollection<AkPV1> AkPV1 { get; set; }
        public ICollection<AkPV2> AkPV2 { get; set; }
        public ICollection<AkPVGanda> AkPVGanda { get; set; }
        public ICollection<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public ICollection<AkPadananPenyata> AkPadananPenyata { get; set; }
        public int FlTunai { get; set; }
        public DateTime? TarTunai { get; set; }

        //relationship end

    }
}
