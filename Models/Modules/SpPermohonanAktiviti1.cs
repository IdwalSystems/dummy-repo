using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPermohonanAktiviti1
    {
        public int Id { get; set; }
        public int AkCartaId { get; set; }
        public string Perihal { get; set; }
        public decimal Kadar { get; set; }
        public int Bil { get; set; }
        public decimal Bln { get; set; }
        public int SpPermohonanAktivitiId { get; set; }

        //relationship
        public AkCarta AkCarta { get; set; }


        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        // log end
    }
}
