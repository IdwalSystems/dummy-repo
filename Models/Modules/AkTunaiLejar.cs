using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiLejar
    {
        public int Id { get; set; }
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [DisplayName("No Rujukan")]
        public string NoRujukan { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public int AkTunaiRuncitId { get; set; }
        public AkTunaiRuncit AkTunaiRuncit { get; set; }
        public DateTime Tarikh { get; set; }
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [DisplayName("Debit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }
        [DisplayName("Kredit RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Kredit { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
        public string Rekup { get; set; }

    }
}
