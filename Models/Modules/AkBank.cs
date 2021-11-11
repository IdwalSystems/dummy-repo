using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class AkBank
    {
        public int Id { get; set; }
        public ICollection<KW> KW { get; set; } 
        public ICollection<Bank> Banks { get; set; } 
        [MaxLength(4)]
        public string Kod { get; set; }
        [MaxLength(100)]
        public string Nama { get; set; }
    }
}