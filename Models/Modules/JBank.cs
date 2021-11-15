using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JBank
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(12)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nama { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        //public ICollection<AkTerima2> AkTerima2 { get; set; }
        //public ICollection<Pembekal> Pembekal { get; set; }
    }
}