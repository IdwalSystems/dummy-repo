using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkPembekal
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
        public int JNegeriId { get; set; }
        [MaxLength(30)]
        public string Telefon1 { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(50)]
        public string AkaunBank { get; set; }
        public int AkBankId { get; set; }

        //Relationship
        public JNegeri JNegeri { get; set; }
        public AkBank AkBank { get; set; }
        public ICollection<AkPO> AkPO { get; set; }

    }
}
