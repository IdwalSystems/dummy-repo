using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string NoPO { get; set; }
        public DateTime Tarikh { get; set; }
        public DateTime TarikhPosting { get; set; }
        public int AkPembekalId { get; set; }
        public decimal Jumlah { get; set; }
        [MaxLength(1)]
        public string Posting { get; set; }
        public int JKWId { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        [MaxLength(1)]
        public string Batal { get; set; }

        public AkPembekal AkPembekal { get; set; }
        public JKW JKW { get; set; }
        public ICollection<AkPO1> AkPO1 { get; set; }
        public ICollection<AkPO2> AkPO2 { get; set; }
    }
}
