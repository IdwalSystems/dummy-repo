using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkCarta
    {
        public int KWId { get; set; }

        public int id { get; set; }
        [MaxLength(6)]
        public string Kod { get; set; }
        [MaxLength(100)]
        public string Nama { get; set; }
        public int KodJenis { get; set; }
        public int KodParas { get; set; }
        [MaxLength(1)]
        public string DebitKredit { get; set; }
        [MaxLength(1)]
        public string UmumDetail { get; set; }
        [MaxLength(100)]
        public string Catatan1 { get; set; }
        [MaxLength(100)]
        public string Catatan2 { get; set; }

        public KW KW { get; set; }
        public Jenis Jenis { get; set; }
        public Paras Paras { get; set; }
        public ICollection<AkTerima1>AkTerima1 { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkAkaun> AkAkaun { get; set; }
        public ICollection<PO2> PO2 { get; set; }
    }
}
