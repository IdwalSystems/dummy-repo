using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiRuncit : AppLogHelper
    {
        public int Id { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public string KaunterPanjar { get; set; }
        public string Catatan { get; set; }

        //relationship
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTunaiPemegang> AkTunaiPemegang { get; set; }
        public ICollection<AkTunaiLejar> AkTunaiLejar { get; set; }
        public ICollection<AkPV> AkPV { get; set; }



    }
}
