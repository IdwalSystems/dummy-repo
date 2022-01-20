using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SpPermohonanAktiviti2
    {
        public int Id { get; set; }
        public int BilAtl { get; set; }
        public int BilJul { get; set; }
        public int BilPeg { get; set; }
        public int BilTek { get; set; }
        public int BilUru { get; set; }

        public int JumL { get; set; }
        public int JumP { get; set; }

        public int SpPermohonanAktivitiId { get; set; }
        public JJantina JJantina { get; set; }
        public int JJantinaId { get; set; }
    }
}
