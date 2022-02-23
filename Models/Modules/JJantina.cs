using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JJantina : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
