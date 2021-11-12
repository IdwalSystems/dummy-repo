using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class Pembekal
    {
        public int Id { get; set; }
        public string NamaSykt { get; set; }
        public string NoPendaftaran { get; set; }
        public string Alamat1 { get; set; }
        public string Alamat2 { get; set; }
        public string Alamat3 { get; set; }
        public string Poskod { get; set; }
        public string Bandar { get; set; }
        public string KodNegeri { get; set; }
        public string Telefon1 { get; set; }
        public string Email { get; set; }
        public string AkaunBank { get; set; }
        public string KodBank { get; set; }

        public Negeri Negeri { get; set; }
        public Bank Bank { get; set; }
        public ICollection<PO> PO { get; set; }

    }
}
