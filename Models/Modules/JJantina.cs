using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JJantina
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //relationship
        //relationship end

        // log
        public string UserId { get; set; }
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        public DateTime TarKemaskini { get; set; } = DateTime.Now;
        //log end
    }
}
