using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JParas :AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }

        //soft delete
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
