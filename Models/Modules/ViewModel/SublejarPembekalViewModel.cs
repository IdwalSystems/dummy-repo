using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules.ViewModel
{
    public class SublejarPembekalViewModel
    {
        public int Id { get; set; }
        public DateTime Tarikh { get; set; }
        public string Rujukan { get; set; }
        [DisplayName("Bayaran RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Bayaran { get; set; }
        [DisplayName("Hutang RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Hutang { get; set; }
        [DisplayName("Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
    }
}
