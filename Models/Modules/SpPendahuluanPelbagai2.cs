using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPendahuluanPelbagai2
    {
        public int Id { get; set; }
        public int SpPendahuluanPelbagaiId { get; set; }
        public int Indek { get; set; }
        public int Baris { get; set; }
        public string Perihal { get; set; }
        public decimal Kadar { get; set; }
        public decimal Bil { get; set; }
        public decimal Bulan { get; set; }
        public decimal Jumlah { get; set; }

    }
}
