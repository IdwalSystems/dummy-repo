using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiLejar
    {
        public int Id { get; set; }
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public int AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public int AkTunaiCVId { get; set; }
        public AkTunaiCV AkTunaiCV { get; set; }
        public DateTime Tarikh { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public decimal Baki { get; set; }
        public string Rekup { get; set; }

    }
}
