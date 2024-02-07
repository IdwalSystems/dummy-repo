using MSNK.Models.Modules.Bases;
using MSNK.Models.Operations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MSNK.Models.Modules
{
    public class JKonfigPerubahanEkuiti : GenericFields
    {
        public int Id { get; set; }
        [Display(Name = "Lajur Jadual")]
        public EnJenisLajurJadualPerubahanEkuiti EnLajurJadual { get; set; }
        [Display(Name = "Kump. Wang")]
        public int? JKWId { get; set; }
        public JKW JKW { get; set; }
        public string Tahun { get; set; }
        
        public ICollection<JKonfigPerubahanEkuitiBaris> JKonfigPerubahanEkuitiBaris { get; set; }
        
    }
}
