using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkBelian
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        public DateTime Tarikh { get; set; }
        public DateTime TarikhPosting { get; set; }
        public string NoInbois { get; set; }
        public int JKWId { get; set; }
        public int AkPOId { get; set; }
        public decimal Jumlah { get; set; }

        public int FlCetak { get; set; }
        public int FlPosting { get; set; }
        public int FlBatal { get; set; }

        //Relationship
        public AkPO AkPO { get; set; }
        public JKW JKW { get; set; }
        public ICollection<AkBelian1> AkBelian1 { get; set; }
        public ICollection<AkBelian2> AkBelian2 { get; set; }
    }
}
