using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkTerima2
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int JCaraBayarId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amaun { get; set; }
        [MaxLength(10)]
        public string NoCek { get; set; }
        [MaxLength(1)]
        public string JenisCek { get; set; }
        [MaxLength(4)]
        public string KodBankCek { get; set; }
        [MaxLength(100)]
        public string TempatCek { get; set; }
        [MaxLength(30)]
        public string NoSlip { get; set; }
        public DateTime TarSlip { get; set; }

        

        //Relationship
        public JCaraBayar JCaraBayar { get; set; }
        //public AkTerima AkTerima { get; set; }
    }
}