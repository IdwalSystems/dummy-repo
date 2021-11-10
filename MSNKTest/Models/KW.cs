using System;
using System.ComponentModel.DataAnnotations;

namespace MSNKTest.Models
{
    public class KW
    {
        public int Id { get; set; }
        [MaxLength(3)]
        public string KOD { get; set; }
        [MaxLength(100)]
        public string PERIHAL { get; set; }
    }
}