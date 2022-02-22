using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class SuTanggunganPekerja
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Hubungan { get; set; }
        public string NoKP { get; set; }
        public int SuPekerjaId { get; set; }

        //relationship
        public SuPekerja SuPekerja { get; set; }
        //relationship end

    }
}
