using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkTerima3
    {
        //field
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        [DisplayName("No Inbois Dikeluarkan")]
        public int? AkInvoisId { get; set; }
        public AkInvois AkInvois { get; set; }
        public AkTerima AkTerima { get; set; }
        //relationship end
    }
}
