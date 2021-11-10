using System;
using System.ComponentModel.DataAnnotations;

namespace MSNKTest.Models
{
    public class Modul
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public string FUNCID { get; set; }
        [MaxLength(100)]
        public string FUNCNAME { get; set; }
    }
}