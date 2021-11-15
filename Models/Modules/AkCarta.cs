using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkCarta
    {
        public int JKWId { get; set; }

        public int Id { get; set; }
        [MaxLength(6)]
        public string Kod { get; set; }
        [MaxLength(100)]
        public string Nama { get; set; }
        public int JJenisId { get; set; }
        public int JParasId { get; set; }
        [MaxLength(1)]
        public string DebitKredit { get; set; }
        [MaxLength(1)]
        public string UmumDetail { get; set; }
        [MaxLength(100)]
        public string Catatan1 { get; set; }
        [MaxLength(100)]
        public string Catatan2 { get; set; }

        //Relationship
        public JKW JKW { get; set; }
        public JJenis JJenis { get; set; }
        public JParas JParas { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun1 { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun2 { get; set; }
        public ICollection<AkPO2> AkPO2 { get; set; }
    }
}
