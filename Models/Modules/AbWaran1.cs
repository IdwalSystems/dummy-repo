using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AbWaran1
    {
        //field
        public int Id { get; set; }
        public int AbWaranId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        [DisplayName("Tambah/Kurang")]
        [MaxLength(1)]
        public string TK { get; set; }
        //field end

        //relationship
        [DisplayName("Kod Objek")]
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }

        //relationship end
    }
}
