using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JSukan : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public string Perihal { get; set; }
        [DisplayName("Elit")]
        public bool IsElit { get; set; }
        [DisplayName("Pembangunan")]
        public bool IsPembangunan { get; set; }

        //relationship
        public ICollection<SpPendahuluanPelbagai> SpPermohonanAktiviti { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
