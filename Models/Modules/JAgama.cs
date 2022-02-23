using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JAgama : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //relationship
        public ICollection<SuPekerja> SuPekerja { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
