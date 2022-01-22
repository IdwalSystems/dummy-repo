using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiPemegang
    {
        public int Id { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public int AkTunaiRuncitId { get; set; }
        [DisplayName("Kod Anggota")]
        public int SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
    }
}
