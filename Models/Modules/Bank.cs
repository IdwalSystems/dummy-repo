using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class Bank
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(4)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nama { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
    }
}