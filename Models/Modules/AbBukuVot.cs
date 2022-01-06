using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AbBukuVot
    {
        public int Id { get; set; }
        public string Tahun { get; set; }
        public int JKWId { get; set; }
        public DateTime Tarikh { get; set; }
        public string Kod { get; set; }
        public string Penerima { get; set; }
        public string Vot { get; set; }
        public int AkCartaId { get; set; }
        public string Rujukan { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public decimal Tanggungan { get; set; }
        public decimal Tbs { get; set; }
        public decimal Baki { get; set; }
        public decimal Rizab { get; set; }
        public decimal Liabiliti { get; set; }

        //relationship
        public JKW JKW { get; set; }
        public AkCarta AkCarta { get; set; }

        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
    }
}
