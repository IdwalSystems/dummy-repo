using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPendahuluanPelbagai1
    {
        public int Id { get; set; }
        public int AkCartaId { get; set; }
        public string Perihal { get; set; }
        public decimal Kadar { get; set; }
        public int Bil { get; set; }
        public decimal Bln { get; set; }
        public decimal Jumlah { get; set; }
        public int SpPendahuluanPelbagaiId { get; set; }

        //relationship
        public AkCarta AkCarta { get; set; }
    }
}
