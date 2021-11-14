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
        public string Kod { get; set; }
        public string Nama { get; set; }
        public int JenisId { get; set; }
        public int ParasId { get; set; }
        public string DebitKredit { get; set; }
        public string UmumDetail { get; set; }
        public string Catatan1 { get; set; }
        public string Catatan2 { get; set; }

        //Relationship
        public KW KW { get; set; }
        public Jenis Jenis { get; set; }
        public Paras Paras { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun1 { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun2 { get; set; }
        //public ICollection<PO2> PO2 { get; set; }
    }
}
