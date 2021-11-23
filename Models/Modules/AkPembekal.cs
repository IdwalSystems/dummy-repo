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
        [MaxLength(5)]
        public string KodSykt { get; set; }//A0000
        [MaxLength(100)]
        public string NamaSykt { get; set; }
        [MaxLength(20)]
        public string NoPendaftaran { get; set; }
        [MaxLength(100)]
        public string Alamat1 { get; set; }
        [MaxLength(100)]
        public string Alamat2 { get; set; }
        [MaxLength(100)]
        public string Alamat3 { get; set; }
        [MaxLength(5)]
        public string Poskod { get; set; }//nvarchar
        [MaxLength(100)]
        public string Bandar { get; set; }
        public int JNegeriId { get; set; }
        [MaxLength(30)]
        public string Telefon1 { get; set; }
        [MaxLength(100)]
        public string Emel { get; set; }
        [MaxLength(20)]
        public string AkaunBank { get; set; }
        public int JBankId { get; set; }

        //Relationship
        public JNegeri JNegeri { get; set; }
        public JBank JBank { get; set; }
        public ICollection<AkPO> AkPO { get; set; }

    }
}
