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
    public class SpPendahuluanPelbagai : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string NoPermohonan { get; set; }
        [DisplayName("Jenis Permohonan")]
        public int JenisPermohonan { get; set; }
        public bool Penyertaan { get; set; }
        public bool Pertandingan { get; set; }
        public bool Pengelolaan { get; set; }
        public bool ProgramBinaan { get; set; }
        public string Tarikh { get; set; }
        public string Aktiviti { get; set; }
        public string Tempat { get; set; }

        public DateTime TarSedia { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumKeseluruhan { get; set; }

        public string Penyokong { get; set; }
        public int StatusSokong { get; set; }
        public DateTime TarSokong { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumSokong { get; set; }

        public string Pelulus { get; set; }
        public int StatusLulus { get; set; }
        public DateTime TarLulus { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal JumLulus { get; set; }
        [DisplayName("Tarikh Posting")]
        public DateTime? TarikhPosting { get; set; }

        public int FlPosting { get; set; }
        public int FlCetak { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }

        //relationship
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        public int JSukanId { get; set; }
        public JSukan JSukan { get; set; }
        public int JTahapAktivitiId { get; set; }
        public JTahapAktiviti JTahapAktiviti { get; set; }
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
        public ICollection<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; }
        public ICollection<SpPendahuluanPelbagai2> SpPendahuluanPelbagai2 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }

    }
}
