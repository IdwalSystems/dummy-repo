using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class SiModul
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        [DisplayName("Id")]
        public string FuncId { get; set; }
        [Required]
        [DisplayName("Perihal")]
        public string FuncName { get; set; }
        [DisplayName("Model")]
        public string Model { get; set; }
        [DisplayName("Model Asal")]
        public string FuncIdOld { get; set; }
    }
}