using System;
using System.ComponentModel.DataAnnotations;

namespace MSNKTest.Models
{
    public class Bank
    {
        public int Id { get; set; }
        [MaxLength(4)]
        public string KOD { get; set; }
        [MaxLength(100)]
        public string NAMA { get; set; }
    }
}