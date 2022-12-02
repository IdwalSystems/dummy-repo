using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkNotaDebitKreditBelian1
    {
        //field
        public int Id { get; set; }
        public int AkNotaDebitKreditBelianId { get; set; }
        [DisplayName("Kod Akaun")]
        public int AkCartaId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //Relationship
        public AkCarta AkCarta { get; set; }
        //relationship end
    }
}
