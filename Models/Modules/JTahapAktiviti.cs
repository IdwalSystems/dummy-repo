using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JTahapAktiviti : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //relationship
        public ICollection<SpPendahuluanPelbagai> SpPermohonanAktiviti { get; set; }

        //soft delete
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
