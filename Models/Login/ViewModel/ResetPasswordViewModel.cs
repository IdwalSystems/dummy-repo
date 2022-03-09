using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Models.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Login.ViewModel
{
    public class ResetPasswordViewModel
    {
         [Required(ErrorMessage = "Emel Diperlukan.")]
        [EmailAddress]
        [Display(Name = "Emel")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Katalaluan Diperlukan.")]
        [StringLength(100, ErrorMessage = "Minimum {2} aksara diperlukan untuk {0}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Katalaluan")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pengesahan Katalaluan")]
        [Required(ErrorMessage = "Pengesahan Katalaluan Diperlukan.")]
        [Compare("Password", ErrorMessage = "Katalaluan dan pengesahan katalaluan tidak sama")]
        public string ConfirmedPassword { get; set; }

        public string Code { get; set; }

    }
}
