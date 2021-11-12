using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkCarta
    {
        public int KWId { get; set; }

        public int id { get; set; }
        public KW KW { get; set; }
        public string Kod { get; set; }
        public string Nama { get; set; }
        public string Jenis { get; set; }
        public string Paras { get; set; }
        public string DebitKredit { get; set; }
        public string UmumDetail { get; set; }
        public string Catatan1 { get; set; }
        public string Catatan2 { get; set; }
    }
}
