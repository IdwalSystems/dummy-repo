using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class PO1
    {
        public int Id { get; set; }
        public int NoPO { get; set; }
        public int Indek { get; set; }
        public string Bil { get; set; }
        public string NoStok { get; set; }
        public string Perihal { get; set; }
        public decimal Kuantiti { get; set; }
        public string Unit { get; set; }
        public decimal Harga { get; set; }
        public decimal Amaun { get; set; }

        public PO PO { get; set; }
    }
}
