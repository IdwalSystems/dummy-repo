using System;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class Negeri
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(2)]
        public string Kod { get; set; }
        [Required]
        [MaxLength(100)]
        public string Perihal { get; set; }
    }
}