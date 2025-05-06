using System.ComponentModel.DataAnnotations.Schema;
using MSNK.Models.Operations;
using System.Collections.Generic;

namespace MSNK.Models.Modules
{
    public class JKonfigPenyataBaris
    {
        public int Id { get; set; }
        public int Bil { get; set; }
        public int JKonfigPenyataId { get; set; }
        public JKonfigPenyata JKonfigPenyata { get; set; }
        public EnKategoriTajuk EnKategoriTajuk { get; set; }
        public string? Perihal { get; set; }
        public int Susunan { get; set; }
        public bool IsFormula { get; set; }
        public EnKategoriJumlah EnKategoriJumlah { get; set; }
        public string JumlahSusunanList { get; set; }
        public ICollection<JKonfigPenyataBarisFormula> JKonfigPenyataBarisFormula { get; set; }

    }
}