using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPermohonanAktiviti2
    {
        public int Id { get; set; }

        public int BilAtlL { get; set; }
        public int BilJulL { get; set; }
        public int BilPegL { get; set; }
        public int BilTekL { get; set; }
        public int BilUruL { get; set; }

        public int BilAtlP { get; set; }
        public int BilJulP { get; set; }
        public int BilPegP { get; set; }
        public int BilTekP { get; set; }
        public int BilUruP { get; set; }

        public int JumL { get; set; }
        public int JumP { get; set; }
        public int JumAtl { get; set; }
        public int JumJul { get; set; }
        public int JumPeg { get; set; }
        public int JumTek { get; set; }
        public int JumUru { get; set; }

        public int SpPermohonanAktivitiId { get; set; }


    // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        // log end
    }
}
