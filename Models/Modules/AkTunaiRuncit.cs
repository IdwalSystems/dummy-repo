using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkTunaiRuncit : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [DisplayName("Kod Kaunter Panjar")]
        public string KaunterPanjar { get; set; }
        public string Catatan { get; set; }
        [DisplayName("Had Maksimum RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal HadMaksimum { get; set; }

        //relationship
        [DisplayName("Kumpulan Wang")]
        public int JKWId { get; set; }
        public JKW JKW { get; set; }

        [DisplayName("Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }

        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public ICollection<AkTunaiPemegang> AkTunaiPemegang { get; set; }
        public ICollection<AkTunaiLejar> AkTunaiLejar { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkJurnal> AkJurnal { get; set; }

        //flag
        [DisplayName("Cetak")]
        [DefaultValue("0")]
        public int FlCetak { get; set; }
        [DisplayName("Posting")]
        [DefaultValue("0")]
        public int FlPosting { get; set; }
        [DisplayName("Batal")]
        [DefaultValue("0")]
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //flag end



    }
}
