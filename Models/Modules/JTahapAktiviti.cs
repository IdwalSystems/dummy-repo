using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JTahapAktiviti
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //relationship
        public ICollection<SpPermohonanAktiviti> SpPermohonanAktiviti { get; set; }

        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        // log end
    }
}
