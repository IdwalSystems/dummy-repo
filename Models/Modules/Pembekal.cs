using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class Pembekal
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string KodSykt { get; set; }
        [MaxLength(100)]
        public string NamaSykt { get; set; }
        [MaxLength(50)]
        public string NoPendaftaran { get; set; }
        [MaxLength(100)]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        public string Alamat3 { get; set; }
        [MaxLength(5)]
        public string Poskod { get; set; }
        [MaxLength(100)]
        public string Bandar { get; set; }
        public int KodNegeri { get; set; }
        [MaxLength(30)]
        public string Telefon1 { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string AkaunBank { get; set; }
        public int KodBank { get; set; }

        public Negeri Negeri { get; set; }
        public Bank Bank { get; set; }
        public ICollection<PO> PO { get; set; }

    }
}
