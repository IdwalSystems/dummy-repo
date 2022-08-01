using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkPenyataPemungut2
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPenyataPemungutId { get; set; }
        public AkPenyataPemungut AkPenyataPemungut { get; set; }
        public int AkTerima2Id { get; set; }
        public AkTerima2 AkTerima2 { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
    }
}
