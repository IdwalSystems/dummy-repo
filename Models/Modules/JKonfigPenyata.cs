using MSNK.Models.Modules.Bases;
using System.Collections.Generic;


namespace MSNK.Models.Modules
{
    public class JKonfigPenyata : GenericFields
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public string Perihal { get; set; }
        public string Tahun { get; set; }
        public ICollection<JKonfigPenyataBaris> JKonfigPenyataBaris { get; set; }

    }
}
