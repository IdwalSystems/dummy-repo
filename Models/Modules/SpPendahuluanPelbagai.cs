using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPendahuluanPelbagai
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

        public string Penyedia { get; set; }
        public DateTime TarSedia { get; set; }
        public decimal JumKeseluruhan { get; set; }

        public string Penyokong { get; set; }
        public int StatusSokong { get; set; }
        public DateTime TarSokong { get; set; }
        public decimal JumSokong { get; set; }

        public string Pelulus { get; set; }
        public int StatusLulus { get; set; }
        public DateTime TarLulus { get; set; }
        public decimal JumLulus { get; set; }

        public int FlPosting { get; set; }
        public int FlCetak { get; set; }

        //relationship
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public int JNegeriId { get; set; }
        public JNegeri JNegeri { get; set; }
        public int JSukanId { get; set; }
        public JSukan JSukan { get; set; }
        public int JTahapAktivitiId { get; set; }
        public JTahapAktiviti JTahapAktiviti { get; set; }
        public ICollection<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; }
        public ICollection<SpPendahuluanPelbagai2> SpPendahuluanPelbagai2 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkTerima> AkTerima { get; set; }


        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        // log end
    }
}
