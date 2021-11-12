using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkAkaun
    {
        public int KWId { get; set; }
        public int KodObjek1 { get; set; }
        public DateTime Tarikh { get; set; }
        public int KodObjek2 { get; set; }
        public int Id { get; set; }
        public string NoRujukan { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }

        public KW KW { get; set; }
        public AkCarta AkCarta { get; set; }
    }
}
