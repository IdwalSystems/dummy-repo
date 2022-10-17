using System;
using System.Collections.Generic;

namespace MSNK.Models.Modules.ViewModel
{
    public class AbBukuTunaiViewModel
    {
        public DateTime? TarMasuk { get; set; }
        public string NamaAkaunMasuk { get; set; }
        public string NoRujukanMasuk { get; set; }
        public decimal AmaunMasuk { get; set; }
        public decimal JumlahMasuk { get; set; }
        public DateTime? TarKeluar { get; set; }
        public string NamaAkaunKeluar { get; set; }
        public string NoRujukanKeluar { get; set; }
        public decimal AmaunKeluar { get; set; }
        public decimal JumlahKeluar { get; set; }
        public int KeluarMasuk { get; set; }

    }
}
