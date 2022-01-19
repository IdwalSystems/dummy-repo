using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiRuncit
    {
        public int Id { get; set; }
        public string KaunterPanjar { get; set; }
        public string Catatan { get; set; }

        //relationship
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTunaiPemegang> AkTunaiPemegang { get; set; }

        // log
        public string UserId { get; set; }
        [DisplayName("Tarikh Masuk")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarMasuk { get; set; }
        public string UserIdKemaskini { get; set; }
        [DisplayName("Tarikh Kemaskini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TarKemaskini { get; set; } = DateTime.Now;

    }
}
