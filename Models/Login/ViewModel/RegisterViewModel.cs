using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Login.ViewModel
{
    public class RegisterViewModel
    {
         [Required]
        [EmailAddress]
        [Display(Name = "Emel")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Katalaluan")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pengesahan Katalaluan")]
        [Compare("Password", ErrorMessage = "Katalaluan dan pengesahan katalaluan tidak sama")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Nama { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string RoleSelected { get; set; }

    }
}
