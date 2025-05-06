using MSNK.Models.Operations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class _AbPenyataAlirTunai
    {
        public int JKonfigPenyataBarisId1 { get; set; }
        public int JKonfigPenyataBarisId2 { get; set; }
        public int Susunan { get; set; }
        public string Perihal { get; set; }
        public string Tahun { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun1 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun2 { get; set; }
        public EnKategoriTajuk EnKategoriTajuk { get; set; }
        public EnKategoriJumlah EnKategoriJumlah { get; set; }
    }
}
