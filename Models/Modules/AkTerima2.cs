using System;
using System.Collections.Generic;

namespace MSNK.Models.Modules
{
    public class AkTerima2
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int CaraBayarId { get; set; }
        public decimal Amaun { get; set; }
        public string NoCek { get; set; }
        public string JenisCek { get; set; }
        public string KodBankCek { get; set; }
        public string TempatCek { get; set; }
        public string NoSlip { get; set; }
        public DateTime TarSlip { get; set; }

        

        //Relationship
        public CaraBayar CaraBayar { get; set; }
        public AkTerima AkTerima { get; set; }
    }
}