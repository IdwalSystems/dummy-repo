using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPV2
    {
        //field
        public int Id { get; set; }
        public int AkPVId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        //field end

        //flag
        public bool HavePO { get; set; }
        //flage end

        //Relationship
        [DisplayName("No Inbois Pembekal")]
        public int? AkBelianId { get; set; }
        public AkBelian AkBelian { get; set; }
        public AkPV AkPV { get; set; }
        //relationship end

    }
}
