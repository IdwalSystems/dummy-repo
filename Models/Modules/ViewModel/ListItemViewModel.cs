using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class ListItemViewModel
    {
        [NotMapped]
        public int id { get; set; }
        [NotMapped]
        public int indek { get; set; }
        [NotMapped]
        public string perihal { get; set; }
        [NotMapped]
        public bool isGanda { get; set; }
    }
}
