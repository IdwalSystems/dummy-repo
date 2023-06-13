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
    public class SpPendahuluanPelbagai : AppLogHelper, ISoftDelete, ICancel
    {
        public int Id { get; set; }
        [DisplayName("No Permohonan")]
        [Required(ErrorMessage = "No Permohonan diperlukan")]
        public string NoPermohonan { get; set; }

        // note :
        // JenisPermohonan = 1 -> Pembangunan Sukan & Atlet
        // JenisPermohonan = 2 -> Pentadbiran
        [DisplayName("Jenis Permohonan")]
        public int JenisPermohonan { get; set; }
        [DisplayName("Tarikh Aktiviti")]
        [Required(ErrorMessage = "Tarikh Aktiviti diperlukan")]
        public string Tarikh { get; set; }
        [DisplayName("Aktiviti/Kejohanan")]
        [Required(ErrorMessage = "Aktiviti/Kejohanan diperlukan")]
        public string Aktiviti { get; set; }
        [Required(ErrorMessage = "Tempat diperlukan")]
        public string Tempat { get; set; }

        public DateTime? TarSedia { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Jumlah RM")]
        public decimal JumKeseluruhan { get; set; }

        // untuk kelulusan
        [DisplayName("Penyokong")]
        public int? JPenyemakId { get; set; }
        public JPenyemak JPenyemak { get; set; }
        public int FlStatusSokong { get; set; }
        public DateTime? TarSokong { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumSokong { get; set; }
        [DisplayName("Pelulus")]
        public int? JPelulusId { get; set; }
        public JPelulus JPelulus { get; set; }
        public int FlStatusLulus { get; set; }
        public DateTime? TarLulus { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumLulus { get; set; }
        // untuk kelulusan end 

        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }

        public int FlPosting { get; set; }
        public int FlCetak { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlBatal { get; set; }
        public DateTime? TarBatal { get; set; }
        [DisplayName("Hapus")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }

        //relationship
        [DisplayName("Kod Objek/Vot")]
        public int? AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kump. Wang diperlukan")]
        [RegularExpression("[^0]+", ErrorMessage = "Sila pilih Kump. Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        [DisplayName("Negeri")]
        [Required(ErrorMessage = "Negeri diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Negeri")]
        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        [DisplayName("Sukan")]
        [Required(ErrorMessage = "Sukan diperlukan")]
        //[RegularExpression("[^0]+", ErrorMessage = "Sila pilih Sukan")]
        public int JSukanId { get; set; }
        public JSukan JSukan { get; set; }
        [DisplayName("Tahap")]
        [Required(ErrorMessage = "Sila pilih Tahap Aktiviti")]
        public int JTahapAktivitiId { get; set; }
        public JTahapAktiviti JTahapAktiviti { get; set; }
        [DisplayName("Nama Pemohon")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public ICollection<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; }
        public ICollection<SpPendahuluanPelbagai2> SpPendahuluanPelbagai2 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }
        
    }
}
