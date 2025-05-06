using MSNK.Models.Operations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class JKonfigPenyataBarisFormula
    {
        public int Id { get; set; }
        public int BarisBil { get; set; }
        public int JKonfigPenyataBarisId { get; set; }
        public JKonfigPenyataBaris? JKonfigPenyataBaris { get; set; }
        public EnJenisOperasi EnJenisOperasi { get; set; }
        public bool IsPukal { get; set; }
        public string EnJenisCartaList { get; set; }
        public bool IsKecuali { get; set; }
        public string KodList { get; set; }
        public string SetKodList { get; set; }
        [NotMapped]
        public string BarisDescription { get; set; }
        [NotMapped]
        public string FormulaDescription { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal AmaunTetap { get; set; }
        public bool IsLastYear { get; set; }
        public bool IsUntilYear { get; set; }
    }
}