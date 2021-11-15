using System;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class SiModul
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string FuncId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FuncName { get; set; }
    }
}