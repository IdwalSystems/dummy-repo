using System.ComponentModel.DataAnnotations.Schema;
using MSNK.Models.Operations;
using System.Collections.Generic;

namespace MSNK.Models.Modules
{
    public class JKonfigPerubahanEkuitiBaris
    {
        public int Id { get; set; }
        public int JKonfigPerubahanEkuitiId { get; set; }
        public EnBarisPerubahanEkuiti EnBaris { get; set; }
        public JKonfigPerubahanEkuiti JKonfigPerubahanEkuiti { get; set; }
        public EnJenisOperasi EnJenisOperasi { get; set; }
        public bool IsPukal { get; set; }
        public string EnJenisCartaList { get; set; }
        [NotMapped]
        public List<string> SelectedEnJenisCartaList { get; set; }
        public bool IsKecuali { get; set; }

        public string KodList { get; set; }

        public string SetKodList { get; set; }
        [NotMapped]
        public List<int> SelectedKodList { get; set; }
        [NotMapped]
        public string BarisDescription { get; set; }
        [NotMapped]
        public string FormulaDescription { get; set; }

    }
}