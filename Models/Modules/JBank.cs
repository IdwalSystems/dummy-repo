using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Modules
{
    public class JBank : AppLogHelper, ISoftDelete
    {
        //field
        public int Id { get; set; }
        [Required(ErrorMessage = "Kod Diperlukan")]
        [MaxLength(12, ErrorMessage = "Input tidak boleh melebihi 12 aksara")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Nama Bank Diperlukan")]
        [DisplayName("Nama Bank")]
        [MaxLength(100, ErrorMessage = "Input tidak boleh melebihi 100 aksara")]
        public string Nama { get; set; }
        [DisplayName("Kod EFT")]
        [MaxLength(3, ErrorMessage = "Input tidak boleh melebihi 3 aksara")]
        [RegularExpression(@"^[\d+]*$", ErrorMessage = "Nombor sahaja dibenarkan")]
        //[Required(ErrorMessage = "Kod EFT Diperlukan")]
        public string KodEFT { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public ICollection<AkPembekal> AkPembekal { get; set; }
        public ICollection<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public ICollection<AkPV> AkPV { get; set; }
        public ICollection<AkPVGanda> AkPVGanda { get; set; }
        //field end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //soft delete end
    }
}