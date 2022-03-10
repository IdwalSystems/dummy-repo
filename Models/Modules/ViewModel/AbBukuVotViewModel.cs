using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AbBukuVotViewModel
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        public string KW { get; set; }
        public int JKWId { get; set; }
        public string Bahagian { get; set; }
        public int? JBahagianId { get; set; }
        public string KodAkaun { get; set; }
        public string Perihal { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public decimal Tanggungan { get; set; }
        public decimal Liabiliti { get; set; }
        public decimal Baki { get; set; }
    }
}
