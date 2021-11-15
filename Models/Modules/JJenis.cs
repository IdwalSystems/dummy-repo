using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class JJenis
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public string Nama { get; set; }

        //Relationship
        public ICollection<AkCarta> AkCarta { get; set; }
    }
}
