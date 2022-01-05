using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Login.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Emel diperlukan")]
        [EmailAddress]
        public string Emel { get; set; }

        [Required(ErrorMessage = "Katalaluan diperlukan")]
        [DataType(DataType.Password)]
        public string Katalaluan { get; set; }

        [Display(Name = "Ingat saya?")]
        public bool IngatSaya { get; set; }
    }
}
