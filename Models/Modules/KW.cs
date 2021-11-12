using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class KW
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }

        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkCarta> AkCarta { get; set; }
    }
}