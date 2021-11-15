using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class PO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string NoPO { get; set; }
        public DateTime Tarikh { get; set; }
        public DateTime TarikhPosting { get; set; }
        public int PembekalId { get; set; }
        public decimal Jumlah { get; set; }
        [MaxLength(1)]
        public string Posting { get; set; }
        public int KWId { get; set; }
        [MaxLength(4)]
        public string Tahun { get; set; }
        [MaxLength(1)]
        public string Batal { get; set; }

        public Pembekal Pembekal { get; set; }
        public KW KW { get; set; }
        public ICollection<PO1> PO1 { get; set; }
        public ICollection<PO2> PO2 { get; set; }
    }
}
