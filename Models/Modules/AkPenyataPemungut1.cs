using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkPenyataPemungut1
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPenyataPemungutId { get; set; }
        public AkPenyataPemungut AkPenyataPemungut { get; set; }
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
    }
}
