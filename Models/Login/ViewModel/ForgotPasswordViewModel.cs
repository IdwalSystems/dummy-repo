using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Login.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Emel diperlukan")]
        [EmailAddress]
        public string Emel { get; set; }
    }
}
