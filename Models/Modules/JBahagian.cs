using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JBahagian : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Perihal Diperlukan")]
        public string Perihal { get; set; }
        public JKW JKW { get; set; }
        [DisplayName("Kumpulan Wang")]
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan")]
        public int JKWId { get; set; }
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }

        public ICollection<AbWaran> AbWaran { get; set; }
    }
}
