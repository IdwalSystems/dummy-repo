using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkCartaViewModel
    {
        [Required]
        public int KWId { get; set; }
        [Required]
        public string Kod { get; set; }
        [Required]
        public string Nama { get; set; }
        [Required]
        public string Jenis { get; set; }
        [Required]
        public string Paras { get; set; }
        [Required]
        public string DebitKredit { get; set; }
        [Required]
        public string UmumDetail { get; set; }
        public string Catatan1 { get; set; }
        public string Catatan2 { get; set; }

    }
}
