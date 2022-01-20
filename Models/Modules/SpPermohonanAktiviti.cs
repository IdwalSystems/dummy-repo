using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPermohonanAktiviti
    {
        public int Id { get; set; }
        public string NoPermohonan { get; set; }
        public string Ppn { get; set; }
        public int Penyertaan { get; set; }
        public int Pertandingan { get; set; }
        public int Pengelolaan { get; set; }
        public int ProgramBinaan { get; set; }
        public int JNegeriId { get; set; }
        public int JSukanId { get; set; }
        public string Tarikh { get; set; }
        public string Aktiviti { get; set; }
        public string Tempat { get; set; }
        public int JTahapId { get; set; }

        public int JumAtl { get; set; }
        public int JumJul { get; set; }
        public int JumPeg { get; set; }
        public int JumTek { get; set; }
        public int JumUru { get; set; }

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
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public JNegeri JNegeri { get; set; }
        public JSukan JSukan { get; set; }
        public JTahapAktiviti JTahapAktiviti { get; set; }
        public ICollection<SpPermohonanAktiviti1> SpPermohonanAktiviti1 { get; set; }
        public ICollection<SpPermohonanAktiviti2> SpPermohonanAktiviti2 { get; set; }


        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        // log end
    }
}
